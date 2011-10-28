using System;
using System.Collections.Generic;

namespace DistributedDatabase.TransactionManager.Interfaces
{
    public interface ISystemInformation
    {
        /// <summary>
        /// The current time tick that the system is on.
        /// </summary>
        /// <value>
        /// The current tick.
        /// </value>
        int CurrentTick { get; set; }

        /// <summary>
        /// Gets or sets the number of running transactions.
        /// </summary>
        /// <value>
        /// The running transactions.
        /// </value>
        int RunningTransactions { get; set; }

        /// <summary>
        /// Moves the system to the next tick.
        /// </summary>
        void Tick();
    }
}