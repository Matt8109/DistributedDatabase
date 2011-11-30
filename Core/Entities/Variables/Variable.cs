using System;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.TransactionManager.Entities.Variables
{
    public class Variable
    {
        public Variable()
        {
            ReadLockHolders = new List<Transaction>();
            WriteLockHolder = null;
        }

        public bool IsReadLocked { get { return ReadLockHolders.Count == 0 || WriteLockHolder == null; } }
        public bool IsWriteLocked { get { return WriteLockHolder == null || ReadLockHolders.Count == 0; } }

        private List<Transaction> ReadLockHolders { get; set; }
        private Transaction WriteLockHolder { get; set; }

        /// <summary>
        /// Attempts to get a read lock. Returns a list of transactions holding the lock,
        /// if it doesnt include the given transaction, the lock failed.  This also 
        /// check to see if there is a write lock and will return that transaction too.
        /// </summary>
        /// <param name="tempTransaction">The temp transaction.</param>
        /// <returns></returns>
        public List<Transaction> GetReadLock(Transaction tempTransaction)
        {
            //check to see if the there is currently a write lock
            if (IsWriteLocked && !WriteLockHolder.Equals(tempTransaction))
                //they don't hold the write lock
                return new List<Transaction>() { WriteLockHolder };
            else if (IsWriteLocked && WriteLockHolder.Equals(tempTransaction))
            {
                //they do hold the write lock
                return new List<Transaction>() { WriteLockHolder };
            }

            //there is no write lock, so add the transaction to the list of read lock holders
            if (!ReadLockHolders.Contains(tempTransaction))
                ReadLockHolders.Add(tempTransaction);
            
            return ReadLockHolders;
        }
    }
}

