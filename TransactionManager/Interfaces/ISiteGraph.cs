using System.Collections.Generic;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.TransactionManager.Entities;

namespace DistributedDatabase.TransactionManager.Interfaces
{
	public interface ISiteGraph
	{
		/// <summary>
		/// Sets up the site list with the full listing of sites.
		/// </summary>
		void InitializeSiteList();
		
		/// <summary>
		/// Returns the full listing of sites.
		/// </summary>
		/// <returns>
		/// The full site list.
		/// </returns>
		List<Site> GetFullSiteList();
		
		/// <summary>
		/// Gets the listing of sites that are currently marked as failed.
		/// </summary>
		/// <returns>
		/// The failed site list.
		/// </returns>
		List<Site> GetFailedSiteList();
		
		/// <summary>
		/// Allows the system to update the current status of a given site
		/// to be marked as failed or not.
		/// </summary>
		/// <param name='tempSite'>
		/// Temp site.
		/// </param>
		/// <param name='isFailed'>
		/// Is failed.
		/// </param>
		void SetSiteStatus(Site tempSite, bool isFailed);
	}
}