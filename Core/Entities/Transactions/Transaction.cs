using System;
using System.Collections.Generic;

namespace DistributedDatabase.Core.Entities.Transactions
{
    /// <summary>
    /// The status of a transaction.
    /// </summary>
    public enum TransactionStatus
    {
        Created,
        Running,
        Blocked,
        Finished,
        Aborted
    }

    /// <summary>
    /// Holds the information relating to a single transaction.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        public Transaction(int transactionId)
        {
            TransactionId = transactionId;
            Status = TransactionStatus.Created;
            LocksHeld = new List<String>();
        }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the status of the transaction.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// A list that contains the locks currently held by the transaction
        /// </summary>
        /// <value>
        /// The locks held.
        /// </value>
        public List<String> LocksHeld { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only. 
        /// Thus we can use multiversion read consistancy.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the transaction history list. This list contains all the previously
        /// executed commands by the transaction. Note, if the <see ref="TransactionManager.TransactionStatus"/>
        /// is Blocked, then the last value in this list holds the statement that the transaction tried to execute
        /// before it got blocked, and thus must be executed again.
        /// </summary>
        /// <value>
        /// The transaction history.
        /// </value>
        public List<String> TransactionHistory { get; set; }
    }
}