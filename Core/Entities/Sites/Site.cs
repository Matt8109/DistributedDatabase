using System;

namespace DistributedDatabase.Core.Entities.Sites
{
    public class Site
    {
        public Site(string id)
        {
            Id = id;
            IsFailed = false;
        }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is currently
        /// considered to be in a failed state.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is failed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFailed { get; set; }
    }
}