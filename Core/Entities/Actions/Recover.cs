using System;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
	public class Recover :BaseAction
	{
        public Recover(string commandText, TransactionList transactionList, SiteList siteList)
            : base(commandText, transactionList, siteList)
		{
			
		}
		
		        public override string ActionName
        {
            get { throw new NotImplementedException();}
				//return "Reading " + Variable + " for " + Transaction.Id; }
        }
	}
}

