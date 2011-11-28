using System;
using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Sites;

namespace DistributedDatabase.Core.Entities.Sites
{
	public class SiteList
	{
		public SiteList (SystemClock systemClock)
        {
            Sites = new List<Site>();
        }

        public List<Site> Sites { get; private set; }
        private SystemClock Clock { get; set; }
		
		public void AddSite(Site site)
		{
			 if (Sites.Where(x => x.Id.ToLower().Equals(Sites.Id.ToLower())).Count() != 0)
                throw new Exception("Error: Detected Duplicate Site Id: " + site.Id);
			
			Sites.Add(site);
		}

		public Site GetSite(string id)
		{
			return Sites.Where(x=>x.Id.ToLower().Equals(id.ToLower())).First();
		}
	}
}

