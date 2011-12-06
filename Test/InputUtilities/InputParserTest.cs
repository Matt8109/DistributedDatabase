using System.Collections.Generic;
using DistributedDatabase.Core.Utilities.InputUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributedDatabase.Test.InputUtilities
{
    /// <summary>
    /// Summary description for InputParser
    /// </summary>
    [TestClass]
    public class InputParserTest : TestBase
    {
        public InputParserTest() : base()
        {
            
        }
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void TestLineBreaker()
        {
            string testOne = "hello ; world   ";

            List<string> result = InputParser.BreakLine(testOne);

            Assert.IsTrue(result.Contains("hello"));
            Assert.IsTrue(result.Contains("world"));
            Assert.IsTrue(result.Count == 2);

            string testTwo = "hello";
            List<string> resultTwo = InputParser.BreakLine(testTwo);

            Assert.IsTrue(resultTwo.Contains("hello"));
            Assert.IsTrue(resultTwo.Count == 1);
        }
    }
}