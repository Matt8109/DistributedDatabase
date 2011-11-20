using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Entities.Actions
{
    public class EndTransaction : BaseAction
    {
        public override string ActionName
        {
            get { return "End Transaction"; }
        }
    }
}
