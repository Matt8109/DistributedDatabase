using System;
using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Sites;
using System.Linq;
using DistributedDatabase.Core.Entities.Variables;
using DistributedDatabase.Core.Utilities.VariableUtilities;

namespace DistributedDatabase.Core.Entities.Sites
{
    public class SiteList
    {
        public SiteList(SystemClock systemClock)
        {
            Sites = new List<Site>();
        }

        public List<Site> Sites { get; private set; }

        private SystemClock Clock { get; set; }

        public void AddSite(Site site)
        {
            if (Sites.Where(x => x.Id == site.Id).Count() != 0)
                throw new Exception("Error: Detected Duplicate Site Id: " + site.Id);

            Sites.Add(site);
        }

        /// <summary>
        /// Gets the specified site by id.
        /// </summary>
        /// <returns>
        /// The site.
        /// </returns>
        /// <param name='id'>
        /// Identifier.
        /// </param>
        public Site GetSite(int id)
        {
            return Sites.Where(x => x.Id == id).First();
        }

        public List<Site> FindVariable(int id)
        {

            if (VariableUtilities.IsReplicated(id))
            {
                return Sites;
            }
            else
            {
                return Sites.Where(x => x.Id == id).ToList();
            }
        }

        /// <summary>
        /// Gets the failed sites.
        /// </summary>
        /// <returns>
        /// The failed sites.
        /// </returns>
        public List<Site> GetFailedSites()
        {
            return (List<Site>)Sites.Where(x => x.IsFailed);
        }
    }
}