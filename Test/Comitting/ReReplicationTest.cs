using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributedDatabase.Test.Comitting
{
    /// <summary>
    /// Summary description for ReReplicationTest
    /// </summary>
    [TestClass]
    public class ReReplicationTest : TestBase
    {
        public ReReplicationTest()
            : base()
        {            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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

        //[TestMethod]
        public void TestMethod1()
        {
            var systemClock = new SystemClock();
            var siteList = new SiteList(systemClock);

            var siteOne = new Site(1, siteList, systemClock);
            var siteTwo = new Site(2, siteList, systemClock);

            siteList.AddSite(siteOne);
            siteList.AddSite(siteTwo);

            var variableOne = new Variable(2, siteOne, systemClock);
            var variableTwo = new Variable(2, siteTwo, systemClock);

            var transaction = new Transaction("test", systemClock);

            siteOne.VariableList.Add(variableOne);
            siteTwo.VariableList.Add(variableTwo);

            Assert.IsTrue(variableOne.GetWriteLock(transaction).Contains(transaction));
            Assert.IsTrue(variableTwo.GetWriteLock(transaction).Contains(transaction));
            variableOne.Set("5");
            variableOne.CommitValue();

            variableOne.Set("4");

            var affectedTranasactions = siteOne.Fail();
            Assert.IsTrue(siteOne.IsFailed);

            siteOne.Recover();

            Assert.IsTrue(affectedTranasactions.Contains(transaction));
            Assert.IsTrue(
                transaction.AwaitingReReplication.Contains(new ValueSitePair() { Site = siteOne, Variable = variableOne }));
            Assert.IsFalse(siteOne.IsFailed);
        }
    }
}
