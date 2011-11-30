using System;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
	public class Recover :BaseAction
	{
        public Recover(string commandText, TransactionList transactionList, SiteList siteList, SystemClock systemClock)
            : base(commandText, transactionList, siteList, systemClock)
		{
            string[] info = commandText.Split(new[] { '(', ')' });

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);

            Site site = siteList.GetSite(info[1]);

            if (site == null)
                throw new Exception("Site not found:" + info[1]);
        }

        public Site Site { get; set; }

        public override string ActionName
        {
            get { return "Recovery at Site: " + Site.Id; }
        }
	}
}

