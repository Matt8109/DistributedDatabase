using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Entities.Sites
{
    /// <summary>
    /// Records when a site goes down, and comes back up.
    /// </summary>
    public class FailureRecoverPair
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
