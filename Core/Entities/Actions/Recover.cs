using System;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
	public class Recover :BaseAction
	{
		public Recover(string commandText, TransactionList transactionList)
            : base(commandText, transactionList)
		{
			
		}
		
		        public override string ActionName
        {
            get { throw new NotImplementedException();}
				//return "Reading " + Variable + " for " + Transaction.Id; }
        }
	}
}

