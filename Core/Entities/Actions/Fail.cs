using System;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    /// <summary>
    /// Represents a site failure action
    /// </summary>
    internal class Fail : BaseAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fail"/> class.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="transactionList">The transaction list.</param>
        /// <param name="siteList"></param>
        /// <param name="systemClock"></param>
        public Fail(string commandText, TransactionList transactionList, SiteList siteList, SystemClock systemClock)
            : base(commandText, transactionList, siteList, systemClock)
        {
            string[] info = commandText.Split(new[] {'(', ')'});

            if (info.Length != 3)
                throw new Exception("Invalid command format: " + commandText);

            Site site = siteList.GetSite(int.Parse(info[1]));

            if (site == null)
                throw new Exception("Site not found:" + info[1]);
        }

        public Site Site { get; set; }

        public override string ActionName
        {
            get { return "Failure at Site: " + Site.Id; }
        }
    }
}