using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    public class Dump : BaseAction
    {
        public Dump(string commandText, TransactionList transactionList, SiteList siteList, SystemClock systemClock)
            : base(commandText, transactionList, siteList, systemClock)
        {
            string[] info = commandText.Split(new[] { '(', ')' });

            DumpFull = info.Count() == 2;

            if (info.Count() == 3)
                DumpObject = info[1];
        }

        public string DumpObject { get; set; }
        public bool DumpFull { get; set; }

        public override string ActionName
        {
            get { throw new NotImplementedException(); }
        }
    }
}
