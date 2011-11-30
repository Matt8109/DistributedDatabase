using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Variables
{
    public class Variable
    {
        public Variable()
        {
            ReadLockHolders = new List<Transaction>();
            WriteLockHolder = null;
        }

        public bool IsReadLocked { get { return ReadLockHolders.Count == 0; } }
        public bool IsWriteLocked { get { return WriteLockHolder == null; } }

        private List<Transaction> ReadLockHolders { get; set; }
        private Transaction WriteLockHolder { get; set; }

        /// <summary>
        /// Attempts to get a read lock. Returns a list of transactions holding the lock,
        /// if it doesnt include the given transaction, the lock failed.  This also 
        /// check to see if there is a write lock and will return that transaction too.
        /// </summary>
        /// <param name="tempTransaction">The temp transaction.</param>
        /// <returns>List of transactions holding the lock.</returns>
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

        /// <summary>
        /// Attempts go get a write lock. Returns a list of transaction holding the lock, if it doesnt
        /// include the given transaction, the lock failed. This also checks to see if there is a read
        /// lock and will return those transaction too.
        /// </summary>
        /// <param name="tempTransaction">The temp transaction.</param>
        /// <returns>List of transaction holding the lock.</returns>
        public List<Transaction> GetWriteLock(Transaction tempTransaction)
        {
            //we cant get a write lock if there are write lock holders
            //so lets check that first
            if (IsReadLocked)
            {
                //maybe the same transaction holds the only read lock
                //in which case we can give it the write lock
                if (ReadLockHolders.Count == 1 && !ReadLockHolders.Contains(tempTransaction))
                {
                    WriteLockHolder = tempTransaction;
                    return new List<Transaction>() { WriteLockHolder };
                }
                else //it doesnt, sad panda
                {
                    return ReadLockHolders;
                }
            }

            if (IsWriteLocked)
            {
                return new List<Transaction>() {WriteLockHolder};
            }
            else
            {
                WriteLockHolder = tempTransaction;
                return new List<Transaction>() {WriteLockHolder};
            }
        }
    }
}

