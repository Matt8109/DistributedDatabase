using System;
using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Sites;

namespace DistributedDatabase.Core
{
    public static class LockAquirer
    {
        /// <summary>
        /// Attempts to acquire read lock.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="availableSiteList">The available site list.</param>
        /// <returns></returns>
        public static List<Site> AquireReadLock(Read action, List<Site> availableSiteList)
        {
            var lockedSites = new List<Site>();

            foreach (Site tempSite in availableSiteList)
            {
                var variable = tempSite.GetVariable(action.VariableIdInt);

                var result = variable.GetReadLock(action.Transaction);

                //we got the lock
                if (result.Contains(action.Transaction))
                {
                    lockedSites.Add(tempSite);
                }
            }

            return lockedSites;
        }

        /// <summary>
        /// Aquires the write locks. Either get them all, or none.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="availableSiteList">The available site list.</param>
        /// <returns></returns>
        public static List<Site> AquireWriteLocks(Read action, List<Site> availableSiteList)
        {
            var lockedSites = new List<Site>();

            foreach (Site tempSite in availableSiteList)
            {
                var variable = tempSite.GetVariable(action.VariableIdInt);

                var result = variable.GetWriteLock(action.Transaction);

                //we got the lock
                if (result.Contains(action.Transaction))
                {
                    lockedSites.Add(tempSite);
                }
                else //we failed to get a lock somewhere, we have to stop
                {
                    break;
                }
            }

            //we weren't able to get the locks at all locations we needed to
            if (lockedSites.Count!=availableSiteList.Count)
            {
                //undo any locks we hold
                foreach (Site tempSite in lockedSites)
                {
                    var variable = tempSite.GetVariable(action.VariableIdInt);

                   variable.RemoveWriteLock(action.Transaction);
                }
                //empty out the list
                lockedSites.Clear();
            }

            return lockedSites;
        }
    }
}

