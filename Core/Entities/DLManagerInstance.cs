using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;

namespace DistributedDatabase.TransactionManager.Entities
{
    public class DLManagerInstance
    {
        public NamedPipeClientStream ClientPipe { get; set; }
        public NamedPipeServerStream ServerPipe { get; set; }
        public String InstanceName { get; set; }

        public DLManagerInstance(String instanceName)
        {
            ClientPipe = new NamedPipeClientStream(instanceName + "Receive");
            ServerPipe = new NamedPipeServerStream(instanceName + "Send");
        }
    }
}
