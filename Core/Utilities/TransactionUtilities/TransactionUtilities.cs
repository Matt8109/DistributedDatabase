using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;

namespace DistributedDatabase.Core.Utilities.TransactionUtilities
{
    public static class TransactionUtilities
    {
        public static void BlockTransaction(Transaction transaction, BaseAction blockedAction)
        {
            transaction.Status = TransactionStatus.Blocked;
            transaction.QueuedCommands.Enqueue(blockedAction);
        }

        /// <summary>
        /// Aborts a given transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public static void AbortTransaction(Transaction transaction)
        {
            transaction.Status = TransactionStatus.Aborted;

            foreach (Site tempSite in transaction.LocksHeld)
            {
                var variableList = tempSite.VariableList;

                foreach (Variable tempVariable in variableList)
                {
                    tempVariable.RemoveReadLock(transaction);

                    if (tempVariable.WriteLockHolder == transaction)
                    {
                        tempVariable.RemoveWriteLock(transaction);
                        tempVariable.ClearUncomitted();
                    }
                }
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public static void CommitTransaction(Transaction transaction)
        {
            foreach (Site tempSite in transaction.LocksHeld)
            {
                foreach (Variable tempVar in tempSite.VariableList)
                {
                    tempVar.RemoveReadLock(transaction); //remove any read locks we might have

                    if(tempVar.WriteLockHolder==transaction)
                    {
                        tempVar.CommitValue();
                        tempVar.RemoveWriteLock(transaction);
                    }
                }
            }
        }
    }
}
