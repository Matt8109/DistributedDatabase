using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Action to end a transaction
    /// </summary>
    public class EndTransaction : BaseAction
    {
        public EndTransaction(string commandText) : base(commandText)
        {
        }

        public override string ActionName
        {
            get { return "End Transaction"; }
        }
    }
}
