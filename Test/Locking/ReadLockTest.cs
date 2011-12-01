using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributedDatabase.Test.Locking
{
    /// <summary>
    /// Summary description for ReadLockTest
    /// </summary>
    [TestClass]
    public class ReadLockTest : TestBase
    {
        public ReadLockTest()
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
        public void TestAcquireReadLock()
        {
            var systemClock = new SystemClock();

            var tempVariable = new Variable("x", systemClock);

            var transactionOne = new Transaction("T1", systemClock);
            var transactionTwo = new Transaction("T2", systemClock);

            var result = tempVariable.GetReadLock(transactionOne);
            Assert.IsTrue(result.Contains(transactionOne));

            var result2 = tempVariable.GetReadLock(transactionTwo);
            Assert.IsTrue(result.Contains(transactionOne));
            Assert.IsTrue(result.Contains(transactionTwo));
        }

        [TestMethod]
        public void TestAcquireAndRemoveReadLock()
        {
            var systemClock = new SystemClock();

            var tempVariable = new Variable("x", systemClock);

            var transactionOne = new Transaction("T1", systemClock);
            var transactionTwo = new Transaction("T2", systemClock);

            var result = tempVariable.GetReadLock(transactionOne);
            Assert.IsTrue(result.Contains(transactionOne));

            var result2 = tempVariable.GetReadLock(transactionTwo);
            Assert.IsTrue(result.Contains(transactionOne));
            Assert.IsTrue(result.Contains(transactionTwo));

            tempVariable.RemoveReadLock(transactionOne);
            Assert.IsFalse(tempVariable.ReadLockHolders.Contains(transactionOne));

            tempVariable.RemoveReadLock(transactionTwo);
            Assert.IsFalse(tempVariable.ReadLockHolders.Contains(transactionTwo));
        }
    }
}
