using System;

namespace DistributedDatabase.Core.Entities.Sites
{
    public class Site
    {
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

        /// <summary>
        /// List of variables held by the site.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        public VariableList Variables { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        /// <param name='siteId'>
        /// Site identifier.
        /// </param>
        public Site(String siteId)
        {
            Id = siteId;
            Variables = new VariableList();
        }

        /// <summary>
        /// Causes a failure in this site.
        /// </summary>
        public void Fail()
        {

        }
    }
}