using System.Collections.Generic;
using DistributedDatabase.TransactionManager.Entities;

namespace DistributedDatabase.TransactionManager.Interfaces
{
	public interface IObjectGraph
	{
		/// <summary>
		/// Returns the listing of sites that a variable can be read at. This 
		/// may be only one in some cases, or multiples if the object is replicated.
		/// We want the full list, so that if a single site fails, we can fall over
		/// to another site.
		/// </summary>
		/// <returns>
		/// The site list.
		/// </returns>
		/// <param name='variable'>
		/// Variable.
		/// </param>
		List<Site> GetSiteReadListList(string variable);
		
		/// <summary>
		///Returns the listing of sites that a variable can be read at. This
		/// may be only one in some cases, or multiples if the object is replicated.
		/// </summary>
		/// <returns>
		/// The site write list.
		/// </returns>
		/// <param name='varaible'>
		/// Varaible.
		/// </param>
		List<Site> GetSiteWriteList(string varaible);
	}
}

