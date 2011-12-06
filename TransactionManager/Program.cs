using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Execution;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.StateHolder;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Utilities.InputUtilities;
using DistributedDatabase.Core.Utilities.TransactionUtilities;

namespace DistributedDatabase.TransactionManager
{
    public static class Program
    {
        private static SiteList _siteList;
        private static SystemClock _systemClock;
        private static TransactionList _transactionList;
        private static String _fileName;
        private static List<ExecutionEntity> _executionPlan;

        private static void Main(string[] args)
        {
            _systemClock = new SystemClock();
            _siteList = new SiteList(_systemClock);
            _transactionList = new TransactionList(_systemClock);
            State.output = new List<string>();

            InitializeSites();

            GetFile();
            List<string> file = ReadFile();
            var errors = new List<String>();

            _executionPlan = InputParser.ReadInput(file, _transactionList, _siteList, _systemClock, ref errors);

            Debug.WriteLine("System starting...");

            if (errors.Count == 0)
            {
                //main execution loop
                foreach (ExecutionEntity currentEntity in _executionPlan)
                {
                    foreach (BaseAction tempAction in currentEntity.Actions)
                    {
                        //begin a transaction of any type
                        if (tempAction is BeginTransaction)
                        {
                            ((BeginTransaction)tempAction).Transaction.Status = TransactionStatus.Running;
                            ((BeginTransaction)tempAction).Transaction.StartTime = _systemClock.CurrentTick;
                            State.output.Add(tempAction.ActionName);
                        }

                        if (tempAction is Read)
                        {
                            var action = (Read)tempAction;
                            var availableLocations = _siteList.GetRunningSitesWithVariable(action.VariableId);

                            //if there are no sites to read from
                            if (availableLocations.Count() == 0)
                            {
                                TransactionUtilities.BlockTransaction(action.Transaction, action);
                                State.output.Add("Transaction " + action.Transaction.Id + " blocked due to unavailable sites to read from.");
                            }
                        }


                        if (tempAction is EndTransaction)
                        {
                            //check rereplication
                        }
                    }

                    _systemClock.Tick();
                }
            }
            else
            {
                Console.WriteLine("Execution stopped, errors encountered: ");
                foreach (String error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Gets the file name from the command line.
        /// </summary>
        public static void GetFile()
        {
            Console.WriteLine("Please enter the fully qualified file location: ");
            _fileName = Console.ReadLine();
        }

        /// <summary>
        /// Reads the file into memory.
        /// </summary>
        /// <returns></returns>
        public static List<String> ReadFile()
        {
            var reader = new FileReader(_fileName);
            return reader.ReadFile();
        }

        /// <summary>
        /// Initializes the sites.
        /// </summary>
        public static void InitializeSites()
        {
            for (int i = 1; i < 11; i++)
            {
                _siteList.AddSite(new Site(i, _siteList, _systemClock));
            }
        }
    }
}