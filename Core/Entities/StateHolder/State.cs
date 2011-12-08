using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Entities.StateHolder
{
    public static class State
    {
        public static List<String> output;
        public static SystemClock Clock;

        public static void Add(string textToAdd)
        {
            output.Add(Clock.CurrentTick + " " + textToAdd);
        }
    }
}
