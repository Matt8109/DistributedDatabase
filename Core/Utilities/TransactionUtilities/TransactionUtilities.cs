using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Utilities.TransactionUtilities
{
    public static class TransactionUtilities
    {
        public static void BlockTransaction(Transaction transaction, BaseAction blockedAction)
        {
            transaction.Status = TransactionStatus.Blocked;
            transaction.QueuedCommands.Enqueue(blockedAction);
        }
    }
}
