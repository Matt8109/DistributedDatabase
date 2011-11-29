using System;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.TransactionManager.Entities.Variables
{
	public class Variable
	{
		public Variable ()
		{
			ReadLockHolders = new List<Transaction>();
			WriteLockHolder=null;
		}
		
		public bool IsReadLocked {get {return ReadLockHolder==null || WriteLockHolder==null;}}
		public bool IsWriteLocked {get{ return WriteLockHolder==null || ReadLockHolder==null;}}
		
		private List<Trasaction> ReadLockHolders {get ;set;}
		private Transaction WriteLockHolder {get;set;}
		
		public Transaction GetReadLock(Transaction tempTransaction)
		{
			//check to see if they currently hold
			if (IsWriteLocked && !WriteLockHolder.Equals(tempTransaction))
				return false;
			else
			{
				WriteLockHolder=tempTransaction;
				return true;
			}
			
		}
	}
}

