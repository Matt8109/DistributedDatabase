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
        /// If the transaction should abort, if not, it waits.
        /// </summary>
        /// <param name="variableToAccess">The variable to access.</param>
        /// <param name="transactionSeekingLock">The transaction seeking lock.</param>
        public static bool ShouldAbort(Variable variableToAccess, Transaction transactionSeekingLock)
        {
            bool shouldAbort = false;

            //if we try to access a lock held by an older transaction
            if (variableToAccess.WriteLockHolder != null && variableToAccess.WriteLockHolder.StartTime < transactionSeekingLock.StartTime)
                return true; //we abort
            else
            {
                //if we are waiting on a younger transaction to finish
                if (variableToAccess.WriteLockHolder != null && variableToAccess.WriteLockHolder.StartTime > transactionSeekingLock.StartTime)
                    shouldAbort = false;

                //check the read locks
                foreach (Transaction tempTrans in variableToAccess.ReadLockHolders)
                {
                    if (tempTrans.StartTime < transactionSeekingLock.StartTime)
                        shouldAbort = true;
                }
            }
            return shouldAbort;
        }
    }
}
