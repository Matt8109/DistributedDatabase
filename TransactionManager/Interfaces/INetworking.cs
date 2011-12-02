using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.TransactionManager.Entities;

namespace DistributedDatabase.TransactionManager.Interfaces
{
	public interface INetworking
	{
		/// <summary>
		/// Opens a connection to the specified site.
		/// </summary>
		/// <param name='site'>
		/// Site.
		/// </param>
		void OpenConnection(Site site);
		
		/// <summary>
		/// Closes a connection to a site. This will not throw an error
		/// if the connection never existed or has already been closed.
		/// It will simply return silently. 
		/// </summary>
		/// <param name='site'>
		/// Site.
		/// </param>
		void CloseConnection(Site site);
		
		/// <summary>
		/// Sends a message to a site. It will return a string
		/// containing the site's response. If the response is a failure
		/// the site's status will be automatically updated to reflect this
		/// in the site graph by this function.
		/// </summary>
		/// <returns>
		/// The message.
		/// </returns>
		/// <param name='site'>
		/// Site.
		/// </param>
		string SendMessage(Site site);
	}
}

