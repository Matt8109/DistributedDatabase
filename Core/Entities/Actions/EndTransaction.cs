using System;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Action to end a transaction
    /// </summary>
    public class EndTransaction : BaseAction
    {
        public EndTransaction(string commandText, TransactionList transactionList)
            : base(commandText, transactionList)
        {
            string[] info = commandText.Split(new[] {'(', ')'});

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);

            Transaction = transactionList.GetTransaction(info[1]);

            if (Transaction == null)
                throw new Exception("Transaction not found: " + commandText);
        }

        public Transaction Transaction { get; set; }

        public override string ActionName
        {
            get { return "End Transaction: " + Transaction.Id; }
        }
    }
}