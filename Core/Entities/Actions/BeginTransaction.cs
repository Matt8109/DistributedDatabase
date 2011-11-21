﻿using System;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Action that begins a transaction
    /// </summary>
    public class BeginTransaction : BaseAction
    {
        public BeginTransaction(string commandText)
            : base(commandText)
        {
            string[] info = commandText.Split(new[] { '(', ')' });

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);

            var transaction = new Transaction(int.Parse(info[1]));

        }

        public Transaction Transaction { get; set; }

        public override string ActionName
        {
            get { return "Begin Transaction"; }
        }
    }
}