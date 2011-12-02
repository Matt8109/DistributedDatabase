using System;
using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Transactions;
using DistributedDatabase.Core.Entities.Variables;
using DistributedDatabase.Core.Extensions;

namespace DistributedDatabase.Core.Entities.Sites
{
    public class Site
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        /// <param name='siteId'>
        /// Site identifier.
        /// </param>
        public Site(int siteId, SiteList siteList)
        {
            Id = siteId;
            VariableList = new List<Variable>();
        }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is currently
        /// considered to be in a failed state.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is failed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFailed { get; set; }

        public List<Variable> VariableList { get; set; }

        public void RecoverSite()
        {
            IsFailed = false;
             

        }

        public List<Transaction> FailSite()
        {
            IsFailed = true;
            var transactionsEffected = new List<Transaction>();

            foreach (Variable tempVar in VariableList)
                tempVar.ResetToComitted().ForEach(transactionsEffected.SilentAdd);

            return transactionsEffected;
        }



        /// <summary>
        /// Causes a failure in this site.
        /// </summary>
        public List<Transaction> Fail()
        {
            return FailSite();
        }
    }
}