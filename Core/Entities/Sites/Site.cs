using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DistributedDatabase.Core.Entities.StateHolder;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;
using DistributedDatabase.Core.Extensions;
using DistributedDatabase.Core.Utilities.VariableUtilities;

namespace DistributedDatabase.Core.Entities.Sites
{
    public class Site
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        /// <param name='siteId'>
        /// Site identifier.
        /// </param>
        public Site(int siteId, SiteList siteList, SystemClock systemClock)
        {
            Id = siteId;
            VariableList = new List<Variable>();
            SiteList = siteList;
            FailTimes = new List<FailureRecoverPair>();
            SystemClock = systemClock;
            IsFailed = false;
        }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is currently
        /// considered to be in a failed state.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is failed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFailed { get; set; }

        public SiteList SiteList { get; set; }

        public SystemClock SystemClock { get; set; }

        public List<Variable> VariableList { get; set; }

        public List<FailureRecoverPair> FailTimes { get; set; }

        /// <summary>
        /// Determines whether the specified transaction has locks on a variable at this site.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>
        ///   <c>true</c> if the specified transaction has locks; otherwise, <c>false</c>.
        /// </returns>
        public bool HasLocks(Transaction transaction)
        {
            return true;
        }

        ///// <summary>
        ///// If the site when down between when the transaction started and the current time.
        ///// Used to decide if the transaction should commit or abort.
        ///// </summary>
        ///// <param name="transaction">The transaction.</param>
        ///// <returns></returns>
        //[Obsolete]
        //public bool DidGoDown(Transaction transaction)
        //{
        //    int startTime = transaction.StartTime;
        //    int currentTime = SystemClock.CurrentTick;

        //    IEnumerable<FailureRecoverPair> wentDown =
        //        FailTimes.Where(x => x.StartTime >= startTime || x.EndTime >= startTime);


        //    return wentDown.Count() != 0;
        //}

        /// <summary>
        /// If the site when down between when the transaction started and the current time.
        /// Used to decide if the transaction should commit or abort.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public bool DidGoDown(SiteAccessRecord record)
        {
            int startTime = record.TimeStamp;

            var wentDown = FailTimes.Where(x => x.StartTime >= startTime);

            return wentDown.Count() != 0;
        }

        /// <summary>
        /// Returns a specific variable from the list.
        /// </summary>
        /// <param name="variableId">The variable id.</param>
        /// <returns></returns>
        public Variable GetVariable(int variableId)
        {
            return VariableList.Where(x => x.Id == variableId).First();
        }

        public Variable GetVariable(string variableId)
        {
            return GetVariable(VariableUtilities.VariableIdToInt(variableId));
        }

        public void Recover()
        {
            RecoverNonReplicatedVariables();
            RecoverReplicatedVariables();
            IsFailed = false;

            //set the recover time
            var lastFail = FailTimes.OrderByDescending(x => x.StartTime).First();
            lastFail.EndTime = SystemClock.CurrentTick;

            Debug.WriteLine("Site " + Id + " recovered.");
        }

        /// <summary>
        /// Recovers the non replicated variables by simply setting their IsReadable
        /// status to true. Their locks and uncommitted values have already been erased.
        /// </summary>
        protected void RecoverNonReplicatedVariables()
        {
            IEnumerable<Variable> nonReplicatedVariables = VariableList.Where(x => !x.IsReplicated);

            foreach (Variable currentVariable in nonReplicatedVariables)
            {
                currentVariable.IsReadable = true;
            }
        }

        protected void RecoverReplicatedVariables()
        {
            var replicatedVariables = VariableList.Where(x => x.IsReplicated).ToList();

            //attempt to recover the variables
            foreach (Variable currentVariable in replicatedVariables)
            {
                var locations =
                    SiteList.FindVariable(currentVariable.Id.ToString()).Where(x => x.IsFailed == false).ToList();

                if (locations.Count() == 0)
                    throw new Exception("All sites are down, unable to continue.");
                else
                {
                    Site availableLocation = locations.First();
                    Variable availableVariable = availableLocation.GetVariable(currentVariable.Id);

                    //ok we want to re-replicate, but a transaction is currently writing to this
                    //variable, so we will have to wait until after it commits or aborts, otherwise
                    //we might not get the full variable history which we need for mvrc
                    if (availableVariable.IsWriteLocked)
                    {
                        availableVariable.WriteLockHolder.AwaitingReReplication.SilentAdd(new ValueSitePair
                                                                                              {
                                                                                                  Site = this,
                                                                                                  Variable =
                                                                                                      currentVariable
                                                                                              });

                        currentVariable.IsReadable = false;
                        Debug.WriteLine("Replicated variable " + currentVariable.Id +
                                        " put on re-replication list because of pending write by " +
                                        availableVariable.WriteLockHolder.Id);
                        State.Add("Replicated variable " + currentVariable.Id +
                                         " put on re-replication list because of pending write by " +
                                         availableVariable.WriteLockHolder.Id);
                    }
                    else
                    {
                        currentVariable.VariableHistory = availableVariable.VariableHistory;
                        currentVariable.IsReadable = true;
                    }
                }
            }
        }

        protected List<Transaction> FailSite()
        {
            IsFailed = true;
            var transactionsEffected = new List<Transaction>();

            //set the fail time
            FailTimes.Add(new FailureRecoverPair() { StartTime = SystemClock.CurrentTick, EndTime = int.MaxValue });

            foreach (Variable tempVar in VariableList)
                tempVar.ResetToComitted().ForEach(transactionsEffected.SilentAdd);

            return transactionsEffected;
        }

        /// <summary>
        /// Causes a failure in this site.
        /// </summary>
        public List<Transaction> Fail()
        {
            List<Transaction> affectedTransactions = FailSite();
            Debug.WriteLine("Site " + Id + " failed.");

            return affectedTransactions;
        }
    }
}