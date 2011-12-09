using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributedDatabase.Test.FailureAndRecovery
{
    /// <summary>
    /// Summary description for TestSiteWentDown
    /// </summary>
    [TestClass]
    public class TestSiteWentDown
    {
        public TestSiteWentDown()
        {
            //
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

        [TestMethod]
        public void TestWhenSiteGoesDownAfter()
        {
            var systemClock = new SystemClock();
            var siteList = new SiteList(systemClock);
            var site = new Site(1, siteList, systemClock);

            site.FailTimes.Add(new FailureRecoverPair() { StartTime = 2, EndTime = 3 });

            Assert.IsTrue(site.DidGoDown(new SiteAccessRecord() { Site = site, TimeStamp = 1 }));
        }

        [TestMethod]
        public void TestWhenSiteGoesDownSameTime()
        {
            var systemClock = new SystemClock();
            var siteList = new SiteList(systemClock);
            var site = new Site(1, siteList, systemClock);

            site.FailTimes.Add(new FailureRecoverPair() { StartTime = 1, EndTime = 3 });

            Assert.IsTrue(site.DidGoDown(new SiteAccessRecord() { Site = site, TimeStamp = 1 }));
        }

        [TestMethod]
        public void TestSiteWentDownButCameUpBefore()
        {
            var systemClock = new SystemClock();
            var siteList = new SiteList(systemClock);
            var site = new Site(1, siteList, systemClock);

            site.FailTimes.Add(new FailureRecoverPair() { StartTime = 1, EndTime = 2 });

            Assert.IsFalse(site.DidGoDown(new SiteAccessRecord() { Site = site, TimeStamp = 3 }));
        }
    }
}
