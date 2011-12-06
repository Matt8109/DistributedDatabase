using System;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Utilities.InputUtilities;
using DistributedDatabase.Core.Utilities.VariableUtilities;

namespace DistributedDatabase.Core
{
    /// <summary>
    /// An action representing a write to the system.
    /// </summary>
    public class Write : BaseAction
    {
        public Write(string commandText, TransactionList transactionList, SiteList siteList, SystemClock systemClock)
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
        public String VariableId { get; set; }

        /// <summary>
        /// Gets the variable in int form.
        /// </summary>
        public int VariableIdInt { get { return VariableUtilities.VariableIdToInt(VariableId); } }

        public override string ActionName
        {
            get { return "Write " + Value + " to " + VariableId + " from transaction " + Transaction.Id; }
        }
    }
}