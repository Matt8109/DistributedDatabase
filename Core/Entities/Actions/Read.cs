using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Utilities.InputUtilities;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// An action representing a read from the system.
    /// </summary>
    public class Read : BaseAction
    {
        /// <summary>
        /// Initializes a new instance of class to read from the system.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="transactionList">The transaction list.</param>
        public Read(string commandText, TransactionList transactionList)
            : base(commandText, transactionList)
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
        }

        /// <summary>
        /// Gets or sets the value of the transaction doing the writing.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the to write to.
        /// </summary>
        /// <value>
        /// The variable.
        /// </value>
        public String Variable { get; set; }

        public override string ActionName
        {
            get { return "Reading " + Variable + " for " + Transaction.Id; }
        }
    }
}
