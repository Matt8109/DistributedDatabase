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
    /// Summary description for SiteFailLockTest
    /// </summary>
    [TestClass]
    public class SiteFailLockTest
    {
        public SiteFailLockTest()
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
        public void TestVariableResetOnFailure()
        {
            var systemClock = new SystemClock();
            var variable = new Variable("x", systemClock);

            var transactionOne = new Transaction("T1", systemClock);

            variable.GetReadLock(transactionOne);
            variable.GetWriteLock(transactionOne);

            Assert.IsTrue(variable.IsReadLocked);
            Assert.IsTrue(variable.IsWriteLocked);
            Assert.IsTrue(variable.ReadLockHolders.Contains(transactionOne));
            Assert.IsTrue(variable.WriteLockHolder==transactionOne);

            variable.ResetToComitted();

            Assert.IsFalse(variable.IsReadLocked);
            Assert.IsFalse(variable.IsWriteLocked);
            Assert.IsFalse(variable.ReadLockHolders.Contains(transactionOne));
            Assert.IsFalse(variable.WriteLockHolder == transactionOne);
        }
    }
}
