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

        public List<Site> FindVariable(string id)
        {
            List<Site> siteList;

            if (VariableUtilities.IsReplicated(VariableUtilities.VariableIdToInt(id)))
            {
                siteList = Sites;
            }
            else
            {
                var variableLoc = VariableUtilities.CalculateSite(VariableUtilities.VariableIdToInt(id));

                siteList = Sites.Where(x => x.Id == variableLoc).ToList();
            }

            //make sure the variables exist first.
            if (Sites.First().VariableList.Where(x => x.Id.Equals(id)).Count() == 0)
            {
                CreateVariable(siteList, id);
            }

            return siteList;
        }

        /// <summary>
        /// Creates the variable at all sites given.
        /// </summary>
        /// <param name="siteToCreateVariable">The site to create variable.</param>
        /// <param name="id">The id.</param>
        private void CreateVariable(List<Site> siteToCreateVariable, string id)
        {
            foreach (Site temp in siteToCreateVariable)
            {
                temp.VariableList.Add(new Variable(VariableUtilities.VariableIdToInt(id), temp, Clock));
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

        /// <summary>
        /// Gets the running sites that hold a given variable id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public List<Site> GetRunningSitesWithVariable(string id)
        {
            var variableLocations = FindVariable(id);

            var locations = variableLocations.Where(x => !x.IsFailed).ToList();

            var isReadable = new List<Site>();

            foreach (Site temp in locations)
            {
                if (temp.GetVariable(id).IsReadable)
                    isReadable.Add(temp);
            }

            return isReadable;
        }
    }
}