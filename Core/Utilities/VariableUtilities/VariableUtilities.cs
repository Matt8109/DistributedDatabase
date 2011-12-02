using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Utilities.VariableUtilities
{
    public static class VariableUtilities
    {
        /// <summary>
        /// Determines whether the specified id is replicated or not.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if the specified id is replicated; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsReplicated(int id)
        {
            return id % 2 != 1;
        }

        /// <summary>
        /// Calculates the site location of a non replicated variable.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static int CalculateSite(int id)
        {
            return (id + 1) % 10;
        }
    }
}
