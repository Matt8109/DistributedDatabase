using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            State.Clock = _systemClock;

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
            List<Transaction> blockedTransactions =
                _transactionList.Transactions.Where(x => x.Status == TransactionStatus.Blocked).ToList();

            foreach (Transaction blocked in blockedTransactions)
            {
                blocked.Status = TransactionStatus.Running;
                BaseAction action = blocked.QueuedCommands.Dequeue();
                ProcessEntity(action);
            }
        }

        private static void ProcessEntity(BaseAction tempAction)
        {
            //begin a transaction of any type
            if (tempAction is BeginTransaction)
            {
                ((BeginTransaction) tempAction).Transaction.Status = TransactionStatus.Running;
                ((BeginTransaction) tempAction).Transaction.StartTime = _systemClock.CurrentTick;
                State.Add(tempAction.ActionName);
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
                var action = (Dump) tempAction;
                if (action.DumpFull)
                {
                    foreach (Site tempSite in _siteList.Sites)
                    {
                        string outputString = "Site: " + tempSite.Id + " - ";
                        foreach (Variable tempvar in tempSite.VariableList)
                        {
                            outputString = outputString + tempvar.Id + ":" + tempvar.GetValue() + " ";
                        }
                        State.Add("Dump - " + outputString);
                    }
                }
                else if (action.DumpObject.Substring(0, 1).ToLower().Equals("x"))
                {
                    //print out variable at each site
                    List<Site> locations =
                        _siteList.FindVariable(VariableUtilities.VariableIdToInt(action.DumpObject).ToString());
                    string outputString = "Variable " + VariableUtilities.VariableIdToInt(action.DumpObject).ToString() +
                                          " - ";

                    foreach (Site tempsite in locations)
                    {
                        Variable variable = tempsite.GetVariable(VariableUtilities.VariableIdToInt(action.DumpObject));
                        outputString = outputString + " " + tempsite.Id + ":" + variable.GetValue();
                    }
                    State.Add("Dump - " + outputString);
                }
                else
                {
                    //print out all values at a given site
                    Site site = _siteList.GetSite(int.Parse(action.DumpObject));
                    string outputString = "Site - ";

                    foreach (Variable currentVar in site.VariableList)
                    {
                        outputString = outputString + " " + currentVar.Id + ":" + currentVar.GetValue();
                    }
                    State.Add("Dump - " + outputString);
                }
            }

            if (tempAction is Fail)
            {
                var action = (Fail) tempAction;
                action.Site.Fail();
                State.Add(tempAction.ActionName);
            }

            if (tempAction is Recover)
            {
                var action = (Recover) tempAction;
                action.Site.Recover();
                State.Add(tempAction.ActionName);
            }

            if (tempAction is EndTransaction)
            {
                var action = (EndTransaction) tempAction;
                bool wentDown = false;

                foreach (Site tempSite in action.Transaction.LocksHeld)
                {
                    if (tempSite.DidGoDown(action.Transaction))
                        wentDown = true;
                }

                if (wentDown)
                {
                    TransactionUtilities.AbortTransaction(action.Transaction);
                    State.Add(action.Transaction.Id + " aborted due to site failure: " + action.Transaction.Id);
                }
                else
                {
                    TransactionUtilities.CommitTransaction(action.Transaction);
                    State.Add(action.Transaction.Id + " comitted: " + action.Transaction.Id);
                }
            }
        }

        /// <summary>
        /// Rereplicates any data that had to wait for a transaction to end.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        private static void ProcessReReplication(Transaction transaction)
        {
            string output = "Rereplicating: ";
            foreach (ValueSitePair pair in transaction.AwaitingReReplication)
            {
                List<Site> available = _siteList.GetRunningSitesWithVariable(pair.Variable.Id.ToString());
                Variable goodVariable = available.First().GetVariable(pair.Variable.Id.ToString());
                pair.Variable.VariableHistory = goodVariable.VariableHistory;
                pair.Variable.IsReadable = true;
                output = output + pair;
            }
            State.Add(output);
        }

        private static void WriteValue(BaseAction tempAction)
        {
            var action = (Write) tempAction;
            List<Site> availableLocations = _siteList.GetRunningSitesWithVariable(action.VariableId);

            string output = action + " ";

            //if there are no sites to read from
            if (availableLocations.Count() == 0)
            {
                TransactionUtilities.BlockTransaction(action.Transaction, action);
                output = output + "Transaction " + action.Transaction.Id +
                         " blocked due to unavailable sites to write to.";
            }
            else
            {
                //we need to get locks
                List<Site> locks = LockAquirer.AquireWriteLocks(action, availableLocations);

                if (locks.Count != 0)
                {
                    foreach (Site temp in locks)
                    {
                        Variable variable = temp.GetVariable(action.VariableId);
                        variable.Set(action.Value);

                        //add a record saying we used the transaction
                        action.Transaction.SiteUsedList.Add(new SiteAccessRecord
                                                                {Site = temp, TimeStamp = _systemClock.CurrentTick});
                    }

                    output = output + "Value Written:" +
                             locks.First().GetVariable(action.VariableId).GetValue(action.Transaction);
                }
                else
                {
                    //wait die
                    List<Transaction> transactionsToAbort =
                        WaitDie.FindTransToAbort(availableLocations.First().GetVariable(action.VariableId),
                                                 action.Transaction);

                    if (transactionsToAbort.Count == 0)
                    {
                        TransactionUtilities.BlockTransaction(action.Transaction, action);
                        output = output + "Transaction " + action.Transaction.Id +
                                 " blocked due to unavailable sites to write to.";
                    }
                    else
                    {
                        foreach (Transaction tempTrans in transactionsToAbort)
                        {
                            TransactionUtilities.AbortTransaction(tempTrans);
                            output = output + "Aborted Transaction " + tempTrans.Id + "due to wait-die.";
                            ;
                            //we need to get locks
                            locks = LockAquirer.AquireWriteLocks(action, availableLocations);

                            foreach (Site temp in locks)
                            {
                                Variable variable = temp.GetVariable(action.VariableId);
                                variable.Set(action.Value);

                                //add a record saying we used the transaction
                                action.Transaction.SiteUsedList.Add(new SiteAccessRecord
                                                                        {
                                                                            Site = temp,
                                                                            TimeStamp = _systemClock.CurrentTick
                                                                        });
                            }
                            output = output + "Value written:" +
                                     locks.First().GetVariable(action.VariableId).GetValue(action.Transaction);
                        }
                    }
                }
            }
        }

        private static void ReadValue(BaseAction tempAction)
        {
            var action = (Read) tempAction;
            List<Site> availableLocations = _siteList.GetRunningSitesWithVariable(action.VariableId);

            string output = action.ActionName + " ";

            //if there are no sites to read from
            if (availableLocations.Count() == 0)
            {
                TransactionUtilities.BlockTransaction(action.Transaction, action);
                output = output + "Transaction " + action.Transaction.Id +
                         " blocked due to unavailable sites to read from.";
            }

            if (action.Transaction.IsReadOnly)
            {
                //read only, no locks needed
                Site siteToReadFrom = availableLocations.First();

                //add a record saying we used the transaction
                action.Transaction.SiteUsedList.Add(new SiteAccessRecord
                                                        {Site = siteToReadFrom, TimeStamp = _systemClock.CurrentTick});
                output = output + "Value Read:" +
                         siteToReadFrom.GetVariable(action.VariableId).GetValue(action.Transaction);
            }
            else
            {
                //we need to get locks
                List<Site> locks = LockAquirer.AquireReadLock(action, availableLocations);

                if (locks.Count != 0)
                {
                    output = output + "Value Read:" +
                             locks.First().GetVariable(action.VariableId).GetValue(action.Transaction);
                    action.Transaction.SiteUsedList.Add(new SiteAccessRecord
                                                            {Site = locks.First(), TimeStamp = _systemClock.CurrentTick});
                }
                else
                {
                    //wait die
                    List<Transaction> transactionsToAbort =
                        WaitDie.FindTransToAbort(availableLocations.First().GetVariable(action.VariableId),
                                                 action.Transaction);

                    if (transactionsToAbort.Count == 0)
                    {
                        TransactionUtilities.BlockTransaction(action.Transaction, action);
                        output = output + "Transaction " + action.Transaction.Id +
                                 " blocked due to unavailable sites to read from.";
                    }
                    else
                    {
                        foreach (Transaction tempTrans in transactionsToAbort)
                        {
                            TransactionUtilities.AbortTransaction(tempTrans);
                            output = output + "Aborted Transaction " + tempTrans.Id + "due to wait-die. ";
                        }

                        //try to get locks after wait die
                        locks = LockAquirer.AquireReadLock(action, availableLocations);

                        //ok we got a lock
                        if (locks.Count != 0)
                        {
                            output = output + "Value Read:" +
                                     locks.First().GetVariable(action.VariableId).GetValue(action.Transaction);
                            action.Transaction.SiteUsedList.Add(new SiteAccessRecord
                                                                    {
                                                                        Site = locks.First(),
                                                                        TimeStamp = _systemClock.CurrentTick
                                                                    });
                        }
                        else
                        {
                            TransactionUtilities.BlockTransaction(action.Transaction, action);
                            output = output + "Transaction " + action.Transaction.Id +
                                     " blocked due to unavailable sites to read from.";
                        }
                    }
                }
            }
            State.Add(output);
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