using System;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Base type of all actions that can be performed by the system
    /// </summary>
    public abstract class BaseAction
    {
        /// <summary>
        /// The full and original text of the command.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="transactionList">The transaction list.</param>
        /// <param name="siteList"></param>
        /// <param name="systemClock"></param>
        public BaseAction(string commandText, TransactionList transactionList, SiteList siteList, SystemClock systemClock)
        {
            CommandText = commandText;

        }

        /// <summary>
        /// The full text of the command to be executed
        /// </summary>
        /// <value>
        /// The command text.
        /// </value>
        public String CommandText { get; private set; }

        /// <summary>
        /// Gets or sets the transaction list.
        /// </summary>
        /// <value>
        /// The transaction list.
        /// </value>
        public TransactionList TransactionList { get; protected set; }

        /// <summary>
        /// Gets or sets the site list.
        /// </summary>
        /// <value>
        /// The site list.
        /// </value>
        public SiteList SiteList { get; protected set; }

        /// <summary>
        /// Gets or sets the system clock.
        /// </summary>
        /// <value>
        /// The system clock.
        /// </value>
        public SystemClock SystemClock { get; protected set; }

        /// <summary>
        /// The string version of what is happening in the event
        /// e.g. Failure at site: 5
        /// </summary>
        public abstract String ActionName { get; }

       
    }
}