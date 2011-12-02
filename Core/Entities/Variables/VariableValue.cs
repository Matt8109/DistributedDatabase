using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Entities.Variables
{
    /// <summary>
    /// Holds a committed variable value. 
    /// </summary>
    public class VariableValue
    {
        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the time of the variable for multi version read consistency.  
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public int TimeStamp { get; set; }
    }
}
