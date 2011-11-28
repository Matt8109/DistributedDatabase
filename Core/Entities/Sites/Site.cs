using System;

namespace DistributedDatabase.Core.Entities.Sites
{
	public class Site
	{
		public Site (string id)
		{
			Id=id;
		}
		
		public string Id {get; protected set;}
	}
}

