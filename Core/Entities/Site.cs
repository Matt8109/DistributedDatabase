using System;
using System.Collections.Generic;

namespace DistributedDatabase.TransactionManager.Entities
{
	public class Site
	{
		/// <summary>
		/// Gets or sets the site identifier.
		/// </summary>
		/// <value>
		/// The site identifier.
		/// </value>
		public int SiteId {get;set;}
		
		/// <summary>
		/// Gets or sets a value indicating whether the site is currently
		/// considered to be in a failed state.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is failed; otherwise, <c>false</c>.
		/// </value>
		public bool IsFailed {get;set;}
		
		/// <summary>
		/// Gets or sets the network location of the site.
		/// </summary>
		/// <value>
		/// The location.
		/// </value>
		public string Location {get;set;}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Site"/> class.
		/// </summary>
		/// <param name='siteId'>
		/// Site identifier.
		/// </param>
		public Site (int siteId)
		{
			SiteId=siteId;
		}
	}
}

