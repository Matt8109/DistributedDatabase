using System;
using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Variables;
using DistributedDatabase.Core.Extensions;

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
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="systemClock">The system clock.</param>
        public Transaction(string transactionId, SystemClock systemClock)
        {
            Id = transactionId;
            Status = TransactionStatus.Created;
            LocksHeld = new List<Site>();
            QueuedCommands = new Queue<BaseAction>();
            AwaitingReReplication = new List<ValueSitePair>();
            SystemClock = systemClock;
        }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the status of the transaction.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start clock time of the transaction.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public int StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end clock time of the transaction.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public int EndTime { get; set; }

        /// <summary>
        /// A list that contains the locks currently held by the transaction
        /// </summary>
        /// <value>
        /// The locks held.
        /// </value>
        public List<Site> LocksHeld { get; set; }

        /// <summary>
        /// Add a new location the transaction holds a lock at.
        /// </summary>
        /// <param name="location">The location.</param>
        public void AddLockHeldLocation(Site location)
        {
            LocksHeld.SilentAdd(location);
        }

        /// <summary>
        ///  Actions that have yet to be executed because the transaction is waiting on something.
        /// </summary>
        /// <value>
        /// The queued commands.
        /// </value>
        public Queue<BaseAction> QueuedCommands { get; set; }

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
        public List<BaseAction> TransactionHistory { get; set; }

        /// <summary>
        /// Simply holds a reference to the system clock
        /// </summary>
        /// <value>
        /// The system clock.
        /// </value>
        private SystemClock SystemClock { get; set; }

        /// <summary>
        /// Should a site recover holding a value that this transaction has a lock on
        /// and the variable is replicated, we want to re-replicate that variable after
        /// the transaction has completed or 
        /// </summary>
        /// <value>
        /// The awaiting re replication.
        /// </value>
        public List<ValueSitePair> AwaitingReReplication { get; set; }
    }
}