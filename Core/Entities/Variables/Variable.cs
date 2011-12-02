using System;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Variables
{
    public class Variable
    {
        public Variable(string Id, SystemClock systemClock)
        {
            ReadLockHolders = new List<Transaction>();
            WriteLockHolder = null;
            SystemClock = systemClock;
            VariableHistory= new List<VariableValue>();
            UncomittedValue = string.Empty;
            Id = Id;
        }

        public string Id { get; private set; }

        protected SystemClock SystemClock { get; set; }

        public bool IsReadLocked
        {
            get { return ReadLockHolders.Count != 0; }
        }

        public bool IsWriteLocked
        {
            get { return WriteLockHolder != null; }
        }

        private List<VariableValue> VariableHistory { get; set; }

        public List<Transaction> ReadLockHolders { get; private set; }

        public Transaction WriteLockHolder { get; private set; }

        public void Set(String value)
        {
            if (UncomittedValue!=String.Empty)
                throw new Exception("Attempt to set value of variable that already contains uncommitted data.");

            UncomittedValue = value;
        }

        /// <summary>
        /// Gets or sets the uncommitted value.
        /// </summary>
        /// <value>
        /// The uncommitted value.
        /// </value>
        private string UncomittedValue { get; set; }

        /// <summary>
        /// Commits the uncommitted value.
        /// </summary>
        public void CommitValue()
        {
            if (UncomittedValue.Equals(string.Empty))
                throw new Exception("Trying to commit empty value.");

            VariableHistory.Add(new VariableValue() { TimeStamp = SystemClock.CurrentTick, Value = UncomittedValue });
            UncomittedValue = string.Empty;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="tempTransaction">The temp transaction.</param>
        /// <returns></returns>
        public string GetValue(Transaction tempTransaction = null)
        {
            if (VariableHistory.Count != 0)
            {
                if (tempTransaction != null && tempTransaction.IsReadOnly)
                {
                    return
                        VariableHistory.Where(x => x.TimeStamp <= tempTransaction.StartTime).OrderByDescending(
                            (x => x.TimeStamp)).First().Value;
                }
                return VariableHistory.OrderByDescending((x => x.TimeStamp)).First().Value;
            }

            return null;
        }

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
                return new List<Transaction> { WriteLockHolder };
            else if (IsWriteLocked && WriteLockHolder.Equals(tempTransaction))
            {
                //they do hold the write lock
                return new List<Transaction> { WriteLockHolder };
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
                    return new List<Transaction> { WriteLockHolder };
                }
                else //it doesnt, sad panda
                {
                    return ReadLockHolders;
                }
            }

            if (IsWriteLocked)
            {
                return new List<Transaction> { WriteLockHolder };
            }
            else
            {
                WriteLockHolder = tempTransaction;
                return new List<Transaction> { WriteLockHolder };
            }
        }

        /// <summary>
        /// Removes the read lock if the given transaction holds it.
        /// </summary>
        /// <param name="tempTransaction">The temp transaction.</param>
        public void RemoveReadLock(Transaction tempTransaction)
        {
            if (ReadLockHolders.Contains(tempTransaction))
                ReadLockHolders.Remove(tempTransaction);
        }

        /// <summary>
        /// Removes the write lock if the given transaction holds it
        /// </summary>
        /// <param name="tempTransaction">The temp transaction.</param>
        public void RemoveWriteLock(Transaction tempTransaction)
        {
            if (WriteLockHolder == tempTransaction)
                WriteLockHolder = null;
        }
    }
}