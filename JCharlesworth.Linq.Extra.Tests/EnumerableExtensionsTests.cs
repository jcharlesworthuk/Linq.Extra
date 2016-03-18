// <copyright file="EnumerableExtensionsTests.cs">
// Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>James Charlesworth</author>
// <date>18th March 2016</date>
// <summary>Enumeration unit tests</summary>
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCharlesworth.Linq.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace JCharlesworth.Linq.Extra.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod()]
        public void PageTest()
        {
            var testData = Enumerable.Range(1, 20).ToArray();

            var page1 = testData.Page(1, 10).ToList();

            Assert.AreEqual(10, page1.Count);
            Assert.AreEqual(1, page1[0]);
            Assert.AreEqual(10, page1[9]);

            var page2 = testData.Page(2, 10).ToList();

            Assert.AreEqual(10, page2.Count);
            Assert.AreEqual(11, page2[0]);
            Assert.AreEqual(20, page2[9]);
       }

        [TestMethod()]
        public void EnumerateTest()
        {
            var testDataFn = Enumerable.Range(1, 20);
            int counter = 0;

            var testDataTransformFn = testDataFn.Select(x => { counter++; return x; });

            Assert.AreEqual(0, counter);

            testDataTransformFn.Enumerate();

            Assert.AreEqual(20, counter);
        }

        [TestMethod()]
        public void WithActionTest()
        {
            var testData = Enumerable.Range(1, 20).ToArray();
            List<int> actioned = new List<int>();

            testData.WithAction(x => actioned.Add(x)).Enumerate();

            Assert.AreEqual(20, actioned.Count);

            for (int i = 0; i < 20; i++)
                Assert.AreEqual(i + 1, actioned[i]);

        }

        [TestMethod()]
        public void InterweaveTest()
        {
            var testDataA = Enumerable.Range(1, 10).ToArray();
            var testDataB = Enumerable.Range(100, 110).ToArray();

            var interweaved = testDataA.Interweave(testDataB).ToList();

            Assert.AreEqual(1, interweaved[0]);
            Assert.AreEqual(100, interweaved[1]);
            Assert.AreEqual(2, interweaved[2]);
            Assert.AreEqual(101, interweaved[3]);
        }
        
        [TestMethod()]
        public void EveryOtherTest()
        {
            var testData = Enumerable.Range(1, 10).ToArray();

            var factorsOfTwo = testData.EveryOther().ToList();
            var factorsOfThree = testData.EveryOther(3).ToList();

            var expectedFactorsOfTwo = new int[] { 2, 4, 6, 8, 10 };
            var expectedFactorsOfThree = new int[] { 3, 6, 9 };

            Assert.AreEqual(expectedFactorsOfTwo.Length, factorsOfTwo.Count);

            for (int i = 0; i < expectedFactorsOfTwo.Length; i++)
                Assert.AreEqual(expectedFactorsOfTwo[i], factorsOfTwo[i]);


            Assert.AreEqual(expectedFactorsOfThree.Length, factorsOfThree.Count);

            for (int i = 0; i < expectedFactorsOfThree.Length; i++)
                Assert.AreEqual(expectedFactorsOfThree[i], factorsOfThree[i]);

        }

        [TestMethod()]
        public void ToCollectionTest()
        {
            var testDataFn = Enumerable.Range(1, 20);

            var collection = testDataFn.ToCollection();

            Assert.IsNotNull(collection);
            Assert.IsInstanceOfType(collection, typeof(Collection<int>));
            Assert.AreEqual(20, collection.Count);
        }

        [TestMethod()]
        public void NotNullTest()
        {
            var testDataFn = new string[] { "one", "two", null, "four" };

            var notNulls = testDataFn.NotNull().ToList();

            Assert.AreEqual(3, notNulls.Count);
            Assert.AreEqual("one", notNulls[0]);
            Assert.AreEqual("two", notNulls[1]);
            Assert.AreEqual("four", notNulls[2]);

        }

        [TestMethod()]
        public void DuplicatesTest()
        {
            var testValueDataFn = new int[] { 1, 2, 3, 3, 4, 5, 6, 6 };
            var duplicateValueTypes = testValueDataFn.Duplicates().ToList();

            Assert.AreEqual(2, duplicateValueTypes.Count);
            Assert.AreEqual(3, duplicateValueTypes[0]);
            Assert.AreEqual(6, duplicateValueTypes[1]);

            var testRefDataFn = new string[] { "one", "two", "two", "three", "four", "four" };
            var duplicateRefTypes = testRefDataFn.Duplicates(StringComparer.InvariantCultureIgnoreCase).ToList();

            Assert.AreEqual(2, duplicateRefTypes.Count);
            Assert.AreEqual("two", duplicateRefTypes[0]);
            Assert.AreEqual("four", duplicateRefTypes[1]);
        }

        [TestMethod()]
        public void FirstOrDefaultTest()
        {
            var testStringDataFn = Enumerable.Range(1, 20).Select(x => x.ToString());

            var result = testStringDataFn.FirstOrDefault("3", StringComparer.InvariantCultureIgnoreCase);

            Assert.AreEqual("3", result);

            var notFound = testStringDataFn.FirstOrDefault("null", StringComparer.InvariantCultureIgnoreCase);

            Assert.IsNull(notFound);
        }

        [TestMethod()]
        public void SingleOrDefaultTest()
        {
            var testStringDataFn = Enumerable.Range(1, 20).Select(x => x.ToString());

            var result = testStringDataFn.SingleOrDefault("3", StringComparer.InvariantCultureIgnoreCase);

            Assert.AreEqual("3", result);

            var notFound = testStringDataFn.SingleOrDefault("null", StringComparer.InvariantCultureIgnoreCase);

            Assert.IsNull(notFound);
        }

        [TestMethod()]
        public void LastOrDefaultTest()
        {
            var testStringDataFn = Enumerable.Range(1, 20).Select(x => x.ToString());

            var result = testStringDataFn.LastOrDefault("3", StringComparer.InvariantCultureIgnoreCase);

            Assert.AreEqual("3", result);

            var notFound = testStringDataFn.LastOrDefault("null", StringComparer.InvariantCultureIgnoreCase);

            Assert.IsNull(notFound);
        }

        [TestMethod()]
        public void SelectManyManyTest()
        {
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

            var flattenedStrings = testData.SelectManyMany(x => x.Children, y => y.Strings).ToList();

            string[] expected = new string[] { "one", "two", "three", "four" };

            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], flattenedStrings[i]);           
        }

        [TestMethod()]
        public void ExpandTest()
        {
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


            var expanded = testData.Expand(x => x.Children).ToList();

            Assert.AreSame(testData[0], expanded[0].Item1);
            Assert.AreSame(testData[0], expanded[1].Item1);
            Assert.AreSame(testData[1], expanded[2].Item1);
            Assert.AreSame(testData[1], expanded[3].Item1);

            Assert.AreEqual("one", expanded[0].Item2.String);
            Assert.AreEqual("two", expanded[1].Item2.String);
            Assert.AreEqual("three", expanded[2].Item2.String);
            Assert.AreEqual("four", expanded[3].Item2.String);
        }

        [TestMethod()]
        public void DistinctTest()
        {
            var testDataFn = new Child[] { new Child { Integer = 1 }, new Child { Integer = 1 }, new Child { Integer = 2 } };

            var distinct = testDataFn.Distinct(x => x.Integer).ToList();

            Assert.AreEqual(2, distinct.Count);

            Assert.AreEqual(1, distinct[0].Integer);
            Assert.AreEqual(2, distinct[1].Integer);
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