using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;

namespace DistributedDatabase.Core.Entities.ConflictResolution
{
    public static class WaitDie
    {
        /// <summary>
        /// Finds the trans to due to wait die. If the transaction has to wait on some transaction, it wont kill anything.
        /// </summary>
        /// <param name="variableToAccess">The variable to access.</param>
        /// <param name="transactionSeekingLock">The transaction seeking lock.</param>
        /// <returns></returns>
        public static List<Transaction> FindTransToAbort(Variable variableToAccess, Transaction transactionSeekingLock)
        {
            var transactionsToAbort = new List<Transaction>();
            var readTransactionsToAbort = new List<Transaction>();
            bool mustWait = false;

            if (variableToAccess.WriteLockHolder != null && variableToAccess.WriteLockHolder.StartTime > transactionSeekingLock.StartTime)
                transactionsToAbort.Add(variableToAccess.WriteLockHolder);
            else if (variableToAccess.WriteLockHolder.StartTime != null)
                mustWait = true;

            foreach (Transaction tempTrans in variableToAccess.ReadLockHolders)
            {
                if (tempTrans.StartTime > transactionSeekingLock.StartTime)
                    readTransactionsToAbort.Add(tempTrans);
                else
                    mustWait = true;
            }

            if (mustWait)
                transactionsToAbort.Clear();

            return transactionsToAbort;
        }
    }
}
