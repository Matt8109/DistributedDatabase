using System;
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
       public BaseAction(string commandText,  TransactionList transactionList)
       {
           CommandText = commandText;
       }

       public String CommandText { get; set; }
       public abstract String ActionName { get; }
    }
}
