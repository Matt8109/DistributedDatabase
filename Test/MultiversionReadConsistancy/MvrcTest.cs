using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistributedDatabase.Test.MultiversionReadConsistancy
{
    /// <summary>
    /// Summary description for MvrcTest
    /// </summary>
    [TestClass]
    public class MvrcTest
    {
        public MvrcTest()
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
        public void CheckMvrcBasic()
        {
            var systemClock = new SystemClock();

            var variable = new Variable(1, systemClock);

            var transactionOne = new Transaction("T1", systemClock);
            var transactionTwo = new Transaction("T1", systemClock);

            transactionOne.IsReadOnly = true;
            transactionOne.StartTime = 1;

            variable.Set("x");
            variable.CommitValue();

            systemClock.Tick();
            systemClock.Tick();
            systemClock.Tick();

            variable.Set("y");
            variable.CommitValue();

            Assert.IsTrue(variable.GetValue(transactionOne).Equals("x"));
            Assert.IsTrue(variable.GetValue().Equals("y"));
            Assert.IsTrue(variable.GetValue(transactionTwo).Equals("y"));
        }

        [TestMethod]
        public void CheckMvrcSameStartTime()
        {
            var systemClock = new SystemClock();

            var variable = new Variable(1, systemClock);

            var transactionOne = new Transaction("T1", systemClock);
            var transactionTwo = new Transaction("T1", systemClock);

            transactionOne.IsReadOnly = true;
            transactionOne.StartTime = 0;

            variable.Set("x");
            variable.CommitValue();

            systemClock.Tick();
            systemClock.Tick();
            systemClock.Tick();

            variable.Set("y");
            variable.CommitValue();

            Assert.IsTrue(variable.GetValue(transactionOne).Equals("x"));
            Assert.IsTrue(variable.GetValue().Equals("y"));
            Assert.IsTrue(variable.GetValue(transactionTwo).Equals("y"));
        }

        [TestMethod]
        public void CheckMvrcLongerHistory()
        {
            var systemClock = new SystemClock();

            var variable = new Variable(1, systemClock);

            var transactionOne = new Transaction("T1", systemClock);
            var transactionTwo = new Transaction("T1", systemClock);

            transactionOne.IsReadOnly = true;

            variable.Set("a");
            variable.CommitValue();
            systemClock.Tick();

            variable.Set("b");
            variable.CommitValue();
            systemClock.Tick();

            variable.Set("c");
            variable.CommitValue();
            systemClock.Tick();

            variable.Set("d");
            variable.CommitValue();
            systemClock.Tick();

            //set but don't commit
            variable.Set("e");
            systemClock.Tick();

            transactionOne.StartTime = 0;
            Assert.IsTrue(variable.GetValue(transactionOne).Equals("a"));

            transactionOne.StartTime = 1;
            Assert.IsTrue(variable.GetValue(transactionOne).Equals("b"));

            transactionOne.StartTime = 2;
            Assert.IsTrue(variable.GetValue(transactionOne).Equals("c"));

            transactionOne.StartTime = 3;
            Assert.IsTrue(variable.GetValue(transactionOne).Equals("d"));

            Assert.IsTrue(variable.GetValue().Equals("d"));
            Assert.IsTrue(variable.GetValue(transactionTwo).Equals("d"));
        }
    }
}
