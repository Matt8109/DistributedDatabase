using System;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Action that begins a transaction
    /// </summary>
    public class BeginTransaction : BaseAction
    {
        public BeginTransaction(string commandText, TransactionList transactionList, SiteList siteList,
                                SystemClock systemClock)
            : base(commandText, transactionList, siteList, systemClock)
        {
            string[] info = commandText.Split(new[] {'(', ')'});

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);

            Transaction = new Transaction(info[1], systemClock) {IsReadOnly = info[0].ToLower().Equals("beginro")};
            transactionList.AddTransaction(Transaction);

        }

        public Transaction Transaction { get; set; }

        public override string ActionName
        {
            get { return "Begin Transaction" + Transaction.Id; }
        }
    }
}