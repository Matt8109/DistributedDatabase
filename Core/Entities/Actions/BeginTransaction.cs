using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Actions
{
    public class BeginTransaction : BaseAction
    {
        public Transaction Transaction { get; set; }

        public override string ActionName
        {
            get { return "Begin Transaction"; }
        }
    }
}
