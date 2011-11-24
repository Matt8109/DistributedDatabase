using System;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Represents a site failure action
    /// </summary>
    internal class Fail : BaseAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fail"/> class.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="transactionList">The transaction list.</param>
        public Fail(string commandText, TransactionList transactionList) : base(commandText, transactionList)
        {
        }

        public override string ActionName
        {
            get { throw new NotImplementedException(); }
        }
    }
}