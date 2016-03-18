using Microsoft.VisualStudio.TestTools.UnitTesting;
using JCharlesworth.Linq.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCharlesworth.Linq.Extra.Tests
{
    [TestClass()]
    public class DictionaryExtensionsTests
    {
        [TestMethod()]
        public void GetValueOrDefaultTest()
        {
            Dictionary<int, string> testDictionary = new Dictionary<int, string> { [1] = "one" , [2] = "two" };
            var found = testDictionary.GetValueOrDefault(1);

            Assert.AreEqual("one", found);

            var notFound = testDictionary.GetValueOrDefault(3);

            Assert.IsNull(notFound);
        }

        [TestMethod()]
        public void GetValueOrValueTest()
        {
            Dictionary<int, int> testDictionary = new Dictionary<int, int> { [1] = 101, [2] = 202 };

            var found = testDictionary.GetValueOr(1, 901);

            Assert.AreEqual(101, found);

            var substituted = testDictionary.GetValueOr(3, 903);

            Assert.AreEqual(903, substituted);
        }

        [TestMethod()]
        public void GetValueOrRefTest()
        {
            Dictionary<int, string> testDictionary = new Dictionary<int, string> { [1] = "one", [2] = "two" };

            var found = testDictionary.GetValueOr(1, () => "null");

            Assert.AreEqual("one", found);

            var substituted = testDictionary.GetValueOr(3, () => "three");

            Assert.AreEqual("three", substituted);
        }
    }
}