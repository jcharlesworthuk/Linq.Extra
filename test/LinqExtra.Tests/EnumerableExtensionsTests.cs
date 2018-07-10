using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Shouldly;
using Xunit;

namespace LinqExtra
{
    public class EnumerableExtensionsTests
    {
        [InlineData(1, 10)]
        [InlineData(2, 10)]
        [InlineData(3, 5)]
        [Theory]
        public void PageTest(int pageNumber, int expectedCount)
        {
            // Arrange
            var source = Enumerable.Range(1, 25);
            const int pageSize = 10;

            // Act
            var page = source.Page(pageNumber, pageSize).ToArray();

            // Assert
            page.Length.ShouldBe(expectedCount);
            page.First().ShouldBe((pageNumber - 1) * pageSize + 1);
       }

        [Fact]
        public void EnumerateTest()
        {
            // Arrange
            var testDataFn = Enumerable.Range(1, 20);
            int counter = 0;
            var testDataTransformFn = testDataFn.Select(x => { counter++; return x; });

            // Act
            testDataTransformFn.Enumerate();
            
            // Assert
            counter.ShouldBe(20);
        }

        [Fact]
        public void WithActionTest()
        {
            // Arrange
            var testData = Enumerable.Range(1, 20).ToArray();
            List<int> actioned = new List<int>();

            // ACt
            testData.WithAction(x => actioned.Add(x)).Enumerate();

            // Assert
            actioned.Count.ShouldBe(20);
            for (int i = 0; i < 20; i++)
                actioned[i].ShouldBe(i + 1);
        }

        [Fact]
        public void InterweaveTest()
        {
            // Arrange
            var testDataA = Enumerable.Range(1, 10).ToArray();
            var testDataB = Enumerable.Range(100, 110).ToArray();

            // Act
            var interweaved = testDataA.Interweave(testDataB).ToList();

            // Assert
            interweaved[0].ShouldBe(1);
            interweaved[1].ShouldBe(100);
            interweaved[2].ShouldBe(2);
            interweaved[3].ShouldBe(101);
        }
        
        [Fact]
        public void EveryOtherTest()
        {
            // Arrange
            var testData = Enumerable.Range(1, 10).ToArray();
            var expectedFactorsOfTwo = new int[] { 2, 4, 6, 8, 10 };
            var expectedFactorsOfThree = new int[] { 3, 6, 9 };

            // ACt
            var factorsOfTwo = testData.EveryOther().ToList();
            var factorsOfThree = testData.EveryOther(3).ToList();


            // Assert
            expectedFactorsOfTwo.Length.ShouldBe(factorsOfTwo.Count);

            for (int i = 0; i < expectedFactorsOfTwo.Length; i++)
                expectedFactorsOfTwo[i].ShouldBe(factorsOfTwo[i]);

            expectedFactorsOfThree.Length.ShouldBe(factorsOfThree.Count);

            for (int i = 0; i < expectedFactorsOfThree.Length; i++)
                expectedFactorsOfThree[i].ShouldBe(factorsOfThree[i]);

        }

        [Fact]
        public void ToCollectionTest()
        {
            // Arrange
            var testDataFn = Enumerable.Range(1, 20);

            // Act
            var collection = testDataFn.ToCollection();

            // Assert
            collection.ShouldNotBeNull();
            collection.ShouldBeOfType<Collection<int>>();
            collection.Count.ShouldBe(20);
        }

        [Fact]
        public void NotNullTest()
        {
            // Arrange
            var testDataFn = new string[] { "one", "two", null, "four" };

            // Act
            var notNulls = testDataFn.NotNull().ToList();

            // Assert
            notNulls.Count.ShouldBe(3);
            notNulls[0].ShouldBe("one");
            notNulls[1].ShouldBe("two");
            notNulls[2].ShouldBe("four");

        }

        [Fact]
        public void DuplicatesTest()
        {
            // Arrange
            var testValueDataFn = new int[] { 1, 2, 3, 3, 4, 5, 6, 6 };
            var duplicateValueTypes = testValueDataFn.Duplicates().ToList();

            duplicateValueTypes.Count.ShouldBe(2);
            duplicateValueTypes[0].ShouldBe(3);
            duplicateValueTypes[1].ShouldBe(6);

            var testRefDataFn = new string[] { "one", "two", "two", "three", "four", "four" };

            // Act
            var duplicateRefTypes = testRefDataFn.Duplicates(StringComparer.InvariantCultureIgnoreCase).ToList();

            duplicateRefTypes.Count.ShouldBe(2);
            duplicateRefTypes[0].ShouldBe("two");
            duplicateRefTypes[1].ShouldBe("four");
        }

        [InlineData("1", true)]
        [InlineData("3", true)]
        [InlineData("20", true)]
        [InlineData("null", false)]
        [Theory]
        public void FirstOrDefaultTest(string test, bool shouldExist)
        {
            // Arrange
            var testStringDataFn = Enumerable.Range(1, 20).Select(x => x.ToString()).ToArray();

            // Act
            var result = testStringDataFn.FirstOrDefault(test, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            if (shouldExist)
                result.ShouldBe(test);
            else
                result.ShouldBeNull();
        }

        [InlineData("1", true)]
        [InlineData("3", true)]
        [InlineData("20", true)]
        [InlineData("null", false)]
        [Theory]
        public void SingleOrDefaultTest(string test, bool shouldExist)
        {
            // Arrange
            var testStringDataFn = Enumerable.Range(1, 20).Select(x => x.ToString()).ToArray();

            // Act
            var result = testStringDataFn.SingleOrDefault(test, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            if (shouldExist)
                result.ShouldBe(test);
            else
                result.ShouldBeNull();
        }

        [InlineData("1", true)]
        [InlineData("3", true)]
        [InlineData("20", true)]
        [InlineData("null", false)]
        [Theory]
        public void LastOrDefaultTest(string test, bool shouldExist)
        {
            // Arrange
            var testStringDataFn = Enumerable.Range(1, 20).Select(x => x.ToString()).ToArray();

            // Act
            var result = testStringDataFn.LastOrDefault(test, StringComparer.InvariantCultureIgnoreCase);

            // Assert
            if (shouldExist)
                result.ShouldBe(test);
            else
                result.ShouldBeNull();
        }

        [Fact]
        public void SelectManyManyTest()
        {
            // Arrange
            var testData = new Parent[]
            {
                new Parent
                {
                    Children = new List<Child>
                    {
                        new Child { Strings = new List<string> { "one" } },
                        new Child { Strings = new List<string> { "two"} }
                    }
                },
                new Parent
                {
                    Children = new List<Child>
                    {
                        new Child { Strings = new List<string> { "three"} },
                        new Child { Strings = new List<string> { "four"} }
                    }
                }
            };
            string[] expected = new string[] { "one", "two", "three", "four" };
            
            // Act
            var flattenedStrings = testData.SelectManyMany(x => x.Children, y => y.Strings).ToList();
            
            // Assert
            for (int i = 0; i < expected.Length; i++)
                flattenedStrings[i].ShouldBe(expected[i]);           
        }

        [Fact]
        public void ExpandTest()
        {
            // Arrange
            var testData = new Parent[]
            {
                new Parent
                {
                    Children = new List<Child>
                    {
                        new Child { String = "one"},
                        new Child { String = "two"}
                    }
                },
                new Parent
                {
                    Children = new List<Child>
                    {
                        new Child { String = "three"},
                        new Child { String = "four"}
                    }
                }
            };

            // Act
            var expanded = testData.Expand(x => x.Children).ToList();

            // Assert
            expanded[0].Item1.ShouldBe(testData[0]);
            expanded[1].Item1.ShouldBe(testData[0]);
            expanded[2].Item1.ShouldBe(testData[1]);
            expanded[3].Item1.ShouldBe(testData[1]);

            expanded[0].Item2.String.ShouldBe("one");
            expanded[1].Item2.String.ShouldBe("two");
            expanded[2].Item2.String.ShouldBe("three");
            expanded[3].Item2.String.ShouldBe("four");
        }

        [Fact]
        public void DistinctTest()
        {
            // Arrange
            var testDataFn = new Child[] { new Child { Integer = 1 }, new Child { Integer = 1 }, new Child { Integer = 2 } };

            // Act
            var distinct = testDataFn.Distinct(x => x.Integer).ToList();

            // Assert
            distinct.Count.ShouldBe(2);
            distinct[0].Integer.ShouldBe(1);
            distinct[1].Integer.ShouldBe(2);
        }

        [Fact]
        public void ChunkTest()
        {
            // Arrange
            var source = Enumerable.Range(1, 25);
            const int chunkSize = 6;

            // Act
            var batches = source.Chunk(chunkSize).ToArray();

            // Assert
            batches.Length.ShouldBe(5);
            batches[0].Count().ShouldBe(chunkSize);
            batches[1].Count().ShouldBe(chunkSize);
            batches[2].Count().ShouldBe(chunkSize);
            batches[3].Count().ShouldBe(chunkSize);
            batches[4].Count().ShouldBe(1);

        }


        [Fact]
        public void IsNullOrEmptyTest_Null()
        {
            // Arrange
            IEnumerable<string> source = null;

            // Act
            var result = source.IsNullOrEmpty();

            // Assert
            result.ShouldBeTrue();
        }


        [Fact]
        public void IsNullOrEmptyTest_Empty()
        {
            // Arrange
            IEnumerable<string> source = new string[0];

            // Act
            var result = source.IsNullOrEmpty();

            // Assert
            result.ShouldBeTrue();
        }

        private class Parent
        {
            public IList<Child> Children { get; set; }
        }

        private class Child
        {
            public string String { get; set; }
            public IList<string> Strings { get; set; }
            public int Integer { get; set; }
        }
    }
}