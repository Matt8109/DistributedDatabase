using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Utilities.InputUtilities;
using DistributedDatabase.Core.Utilities.VariableUtilities;

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
        public Read(string commandText, TransactionList transactionList, SiteList siteList, SystemClock systemClock)
            : base(commandText, transactionList, siteList, systemClock)
        {
            string[] info = commandText.Split(new[] { '(', ')' });

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);


            string[] parameters = info[1].Split(',');

            parameters = InputParser.TrimStringList(parameters);

            Transaction = transactionList.GetTransaction(parameters[0]);

            if (Transaction == null)
                throw new Exception("Transaction not found: " + commandText);

            VariableId = parameters[1];
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
        /// Gets or sets the to write to.
        /// </summary>
        /// <value>
        /// The variable.
        /// </value>
        public String VariableId { get; set; }

        /// <summary>
        /// Gets the variable in int form.
        /// </summary>
        public int VariableIdInt { get { return VariableUtilities.VariableIdToInt(VariableId); } }

        public string Value { get; set; }

        public override string ActionName
        {
            get { return "Reading " + VariableId + " for " + Transaction.Id; }
        }
    }
}
