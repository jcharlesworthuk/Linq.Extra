using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace LinqExtra
{
    public class DictionaryExtensionsTests
    {
        [InlineData(1, "one")]
        [InlineData(3, null)]
        [Theory]
        public void GetValueOrDefaultTest(int lookFor, string expected)
        {
            // Arrange
            var testDictionary = new Dictionary<int, string> { [1] = "one" , [2] = "two" };

            // Act
            var found = testDictionary.GetValueOrDefault(lookFor);

            // Assert
            found.ShouldBe(expected);
        }

        [InlineData(1, 101)]
        [InlineData(2, 202)]
        [InlineData(3, 999)]
        [Theory]
        public void GetValueOrValueTest(int lookFor, int expected)
        {
            // Arrange
            var testDictionary = new Dictionary<int, int> { [1] = 101, [2] = 202 };

            // Act
            var found = testDictionary.GetValueOr(lookFor, 999);

            // Assert
            found.ShouldBe(expected);
        }

        [InlineData(1, "one")]
        [InlineData(2, "two")]
        [InlineData(3, "default")]
        [Theory]
        public void GetValueOrRefTest(int lookFor, string expected)
        {
            // Arrange
            var testDictionary = new Dictionary<int, string> { [1] = "one", [2] = "two" };

            // Act
            var found = testDictionary.GetValueOr(lookFor, () => "default");

            // Assert
            found.ShouldBe(expected);
        }
    }
}