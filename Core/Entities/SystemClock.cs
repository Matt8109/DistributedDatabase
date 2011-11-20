using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Entities
{
   public class SystemClock
    {
        public int CurrentTick { get; set; }
       
       public SystemClock()
       {
           CurrentTick = 0;
       }

       public void Tick()
       {
           CurrentTick++;
       }
    }
}
