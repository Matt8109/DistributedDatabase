﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DistributedDatabase.Core;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.ConflictResolution;
using DistributedDatabase.Core.Entities.Execution;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.StateHolder;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;
using DistributedDatabase.Core.Utilities.InputUtilities;
using DistributedDatabase.Core.Utilities.TransactionUtilities;
using DistributedDatabase.Core.Utilities.VariableUtilities;

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
                        ProcessEntity(tempAction);
                    }


                    ProcessPausedTransactions();


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

            foreach (String outpt in State.output)
                Console.WriteLine(outpt);
            Console.ReadLine();
        }

        private static void ProcessPausedTransactions()
        {
            var blockedTransactions =
                _transactionList.Transactions.Where(x => x.Status == TransactionStatus.Blocked).ToList();

            foreach (Transaction blocked in blockedTransactions)
            {
                blocked.Status = TransactionStatus.Running;
                var action = blocked.QueuedCommands.Dequeue();
                ProcessEntity(action);
            }

        }

        private static void ProcessEntity(BaseAction tempAction)
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
                ReadValue(tempAction);
            }

            if (tempAction is Write)
            {
                WriteValue(tempAction);
            }

            if (tempAction is Dump)
            {
                var action = (Dump)tempAction;
                if (action.DumpFull)
                {
                    foreach (Site tempSite in _siteList.Sites)
                    {
                        var outputString = "Site: " + tempSite.Id + " - ";
                        foreach (Variable tempvar in tempSite.VariableList)
                        {
                            outputString = outputString + tempvar.Id + ":" + tempvar.GetValue() + " ";
                        }
                        State.output.Add("Dump - " + outputString);
                    }
                } else if (action.DumpObject.Substring(0,1).ToLower().Equals("x"))
                {
                    //print out variable at each site
                    var locations = _siteList.FindVariable(VariableUtilities.VariableIdToInt(action.DumpObject).ToString());
                    var outputString = "Variable " + VariableUtilities.VariableIdToInt(action.DumpObject).ToString() + " - ";

                    foreach (Site tempsite in locations)
                    {
                        //var site = 
                    }
                }
            }

            if (tempAction is Fail)
            {
                var action = (Fail)tempAction;
                action.Site.Fail();
                State.output.Add("Site failure:" + action.Site.Id);
            }

            if (tempAction is Recover)
            {
                var action = (Recover)tempAction;
                action.Site.Fail();
                State.output.Add("Site Recovery:" + action.Site.Id);
            }


            if (tempAction is EndTransaction)
            {
                var action = (EndTransaction)tempAction;
                var wentDown = false;

                foreach (Site tempSite in action.Transaction.LocksHeld)
                {
                    if (tempSite.DidGoDown(action.Transaction))
                        wentDown = true;
                }

                if (wentDown)
                {
                    TransactionUtilities.AbortTransaction(action.Transaction);
                    State.output.Add("Aborted due to site failure: " + action.Transaction.Id);
                }
                else
                {
                    TransactionUtilities.CommitTransaction(action.Transaction);
                    State.output.Add("Comitted: " + action.Transaction.Id);
                }


            }

        }

        /// <summary>
        /// Rereplicates any data that had to wait for a transaction to end.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        private static void ProcessReReplication(Transaction transaction)
        {
            foreach (ValueSitePair pair in transaction.AwaitingReReplication)
            {
                var available = _siteList.GetRunningSitesWithVariable(pair.Variable.Id.ToString());
                var goodVariable = available.First().GetVariable(pair.Variable.Id.ToString());
                pair.Variable.VariableHistory = goodVariable.VariableHistory;
                pair.Variable.IsReadable = true;
            }
        }

        private static void WriteValue(BaseAction tempAction)
        {
            var action = (Write)tempAction;
            var availableLocations = _siteList.GetRunningSitesWithVariable(action.VariableId);

            //if there are no sites to read from
            if (availableLocations.Count() == 0)
            {
                TransactionUtilities.BlockTransaction(action.Transaction, action);
                State.output.Add("Transaction " + action.Transaction.Id + " blocked due to unavailable sites to write to.");
            }
            else
            {
                //we need to get locks
                var locks = LockAquirer.AquireWriteLocks(action, availableLocations);

                if (locks.Count != 0)
                {
                    foreach (Site temp in locks)
                    {
                        var variable = temp.GetVariable(action.VariableId);
                        variable.Set(action.Value);
                    }

                    State.output.Add("Value Written:" +
                                     locks.First().GetVariable(action.VariableId).GetValue(action.Transaction));
                }
                else
                {
                    //wait die
                    var transactionsToAbort =
                        WaitDie.FindTransToAbort(availableLocations.First().GetVariable(action.VariableId),
                                                 action.Transaction);

                    if (transactionsToAbort.Count == 0)
                    {
                        TransactionUtilities.BlockTransaction(action.Transaction, action);
                        State.output.Add("Transaction " + action.Transaction.Id +
                                         " blocked due to unavailable sites to write to.");
                    }
                    else
                    {
                        foreach (Transaction tempTrans in transactionsToAbort)
                        {
                            TransactionUtilities.AbortTransaction(tempTrans);
                            State.output.Add("Aborted Transaction " + tempTrans.Id + "due to wait-die.");

                            //we need to get locks
                            locks = LockAquirer.AquireWriteLocks(action, availableLocations);

                            foreach (Site temp in locks)
                            {
                                var variable = temp.GetVariable(action.VariableId);
                                variable.Set(action.Value);
                            }
                            State.output.Add("Value written:" +
                                             locks.First().GetVariable(action.VariableId).GetValue(action.Transaction));
                        }
                    }
                }
            }
        }

        private static void ReadValue(BaseAction tempAction)
        {
            var action = (Read)tempAction;
            var availableLocations = _siteList.GetRunningSitesWithVariable(action.VariableId);

            //if there are no sites to read from
            if (availableLocations.Count() == 0)
            {
                TransactionUtilities.BlockTransaction(action.Transaction, action);
                State.output.Add("Transaction " + action.Transaction.Id + " blocked due to unavailable sites to read from.");
            }

            if (action.Transaction.IsReadOnly)
            {
                //read only, no locks needed
                var siteToReadFrom = availableLocations.First();

                State.output.Add("Value Read:" + siteToReadFrom.GetVariable(action.VariableId).GetValue(action.Transaction));
            }
            else
            {
                //we need to get locks
                var locks = LockAquirer.AquireReadLock(action, availableLocations);

                if (locks.Count != 0)
                {
                    State.output.Add("Value Read:" + locks.First().GetVariable(action.VariableId).GetValue(action.Transaction));
                }
                else
                {
                    //wait die
                    var transactionsToAbort =
                        WaitDie.FindTransToAbort(availableLocations.First().GetVariable(action.VariableId),
                                                 action.Transaction);

                    if (transactionsToAbort.Count == 0)
                    {
                        TransactionUtilities.BlockTransaction(action.Transaction, action);
                        State.output.Add("Transaction " + action.Transaction.Id +
                                         " blocked due to unavailable sites to read from.");
                    }
                    else
                    {
                        foreach (Transaction tempTrans in transactionsToAbort)
                        {
                            TransactionUtilities.AbortTransaction(tempTrans);
                            State.output.Add("Aborted Transaction " + tempTrans.Id + "due to wait-die.");

                            //we need to get locks
                            locks = LockAquirer.AquireReadLock(action, availableLocations);
                            State.output.Add("Value Read:" +
                                             locks.First().GetVariable(action.VariableId).GetValue(action.Transaction));
                        }
                    }
                }
            }
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