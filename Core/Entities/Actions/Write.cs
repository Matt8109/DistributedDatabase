using System;

namespace DistributedDatabase.Core
{
	public class Write : BaseAction
	{
		public Write (string commandText)
            : base(commandText)
		{
			
		}
		
		  public override string ActionName
        {
            get { return "Write"; }
        }
	}
}

