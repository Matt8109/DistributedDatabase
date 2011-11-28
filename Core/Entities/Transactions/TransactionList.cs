using System;
using System.Collections.Generic;
using System.Linq;

namespace DistributedDatabase.Core.Entities.Transactions
{
    /// <summary>
    /// Holds all the transactions currently registered with the system.
    /// </summary>
    public class TransactionList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionList"/> class.
        /// </summary>
        /// <param name="systemClock">The system clock.</param>
        public TransactionList(SystemClock systemClock)
        {
            Transactions = new List<Transaction>();
        }

        public List<Transaction> Transactions { get; private set; }
        private SystemClock Clock { get; set; }

        public void AddTransaction(Transaction tempTransaction)
        {
            if (Transactions.Where(x => x.Id == tempTransaction.Id).Count() != 0)
                throw new Exception("Error: Detected Duplicate Transaction Id:" + tempTransaction.Id);

            Transactions.Add(tempTransaction);
        }

        /// <summary>
        /// Returns a transaction by given id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Transaction GetTransaction(String id)
        {
			//TODO: FIX THIS
            id = id.ToLower();
            return Transactions.Where(x => x.Id.ToLower().Equals(id)).First();
        }

        /// <summary>
        /// Returns the set of all transactions in the system with a given status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsByStats(TransactionStatus status)
        {
            return Transactions.Where(x => x.Status.Equals(status)).ToList();
        }
    }
}