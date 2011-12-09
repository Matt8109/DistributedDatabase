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

        /// <summary>
        /// Converts a variable id to an int.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static int VariableIdToInt(string id)
        {
            if (id.Length > 1 && id.Substring(0, 1).ToLower().Equals("x"))
                return int.Parse(id.Substring(1));
            else
                return int.Parse(id);
        }
    }
}
