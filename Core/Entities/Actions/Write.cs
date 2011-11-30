using System;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Utilities.InputUtilities;

namespace DistributedDatabase.Core
{
    /// <summary>
    /// An action representing a write to the system.
    /// </summary>
    public class Write : BaseAction
    {
        public Write(string commandText, TransactionList transactionList, SiteList siteList)
            : base(commandText, transactionList, siteList)
        {
            string[] info = commandText.Split(new[] { '(', ')' });

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);


            string[] parameters = info[1].Split(',');

            parameters = InputParser.TrimStringList(parameters);

            Transaction = transactionList.GetTransaction(parameters[0]);

            if (Transaction == null)
                throw new Exception("Transaction not found: " + commandText);

            Variable = parameters[1];
            Value = parameters[2];
        }


        /// <summary>
        /// Gets or sets the value of the transaction doing the writing.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the value to be written.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public String Value { get; set; }

        /// <summary>
        /// Gets or sets the to write to.
        /// </summary>
        /// <value>
        /// The variable.
        /// </value>
        public String Variable { get; set; }

        public override string ActionName
        {
            get { return "Write " + Value + " to " + Variable + " from transaction " + Transaction.Id; }
        }
    }
}