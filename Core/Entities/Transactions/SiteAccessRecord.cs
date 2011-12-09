using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Sites;

namespace DistributedDatabase.Core.Entities.Transactions
{
    public class SiteAccessRecord
    {
        public Site Site { get; set; }
        public int TimeStamp { get; set; }
    }
}
