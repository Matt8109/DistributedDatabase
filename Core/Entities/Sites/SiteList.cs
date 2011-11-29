using System;
using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Sites;
using System.Linq;

namespace DistributedDatabase.Core.Entities.Sites
{
	public class SiteList
	{
		public SiteList (SystemClock systemClock)
		{
			Sites = new List<Site> ();
		}

		public List<Site> Sites { get; private set; }

		private SystemClock Clock { get; set; }
		
		public void AddSite (Site site)
		{
			if (Sites.Where (x => x.Id.ToLower ().Equals (site.Id.ToLower ())).Count () != 0)
				throw new Exception ("Error: Detected Duplicate Site Id: " + site.Id);
			
			Sites.Add (site);
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
		public Site GetSite (string id)
		{
			return Sites.Where (x => x.Id.ToLower ().Equals (id.ToLower ())).First ();
		}
		
		/// <summary>
		/// Gets the failed sites.
		/// </summary>
		/// <returns>
		/// The failed sites.
		/// </returns>
		public List<Stite> GetFailedSites ()
		{
			return Sites.Where (x => x.IsFailed);
		}
	}
}