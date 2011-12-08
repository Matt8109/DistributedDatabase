using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedDatabase.Core.Entities.Sites;

namespace DistributedDatabase.Core.Entities.Variables
{
    /// <summary>
    /// Holds a site/value pair.
    /// </summary>
    public class ValueSitePair : IEquatable<ValueSitePair>
    {
        public Variable Variable { get; set; }
        public Site Site { get; set; }

        public bool Equals(ValueSitePair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Variable, Variable) && Equals(other.Site, Site);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ValueSitePair)) return false;
            return Equals((ValueSitePair)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Variable.GetHashCode() * 397) ^ Site.GetHashCode();
            }
        }

        public static bool operator ==(ValueSitePair left, ValueSitePair right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueSitePair left, ValueSitePair right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Site.Id.ToString() + ":" +
            Variable.Id.ToString();
        }
    }
}
