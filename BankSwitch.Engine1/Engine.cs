using BankSwitch.Core.Entities;
using BankSwitch.Engine.Connections;
using BankSwitch.Engine.ProcessorMangement;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Engine1
{
    class Engine
    {
        static void Main(string[] args)
        {
            Startup();
            //Do autoReversal 
            new TransactionManager().DoAutoReversal();

            while (true)
            {
                Console.WriteLine("Switch waiting for connection... \nPress s to shutdown, r to restart, e to exit");

                if (Console.ReadLine() == "s")
                {
                    Shutdown();
                }
                else if (Console.ReadLine() == "r")
                {
                    Startup();
                }
                else if (Console.ReadLine() == "e")
                {
                    Shutdown();
                    Environment.Exit(0);
                }
                else
                {
                    Startup();
                }
            }
        }

      public static void Startup()
        {
             //TO INSTANTIATE CLIENT PEER
        TransactionManager trxnManager = new TransactionManager();
            //Start up all listeners
            trxnManager.Log("Searching for pre-configured source servers............\t");
            IList<SourceNode> allSourceNode = new SourceNodeManager().RetrieveAll();
            trxnManager.Log(allSourceNode.Count + " Source server(s) found:.........\t ");

            foreach (var sourceNode in allSourceNode)
            {
                new Listener().StartListener(sourceNode);
                sourceNode.IsActive = true;
                new SourceNodeManager().Update(sourceNode);
            }
        }
        public static void Shutdown()  //Set status to inactive
        {
               //TO INSTANTIATE CLIENT PEER
        TransactionManager trxnManager = new TransactionManager();

            IList<SourceNode> allSourceNode = new SourceNodeManager().RetrieveAll();

            foreach (var thisNode in allSourceNode)
            {
                thisNode.IsActive = false;
                new SourceNodeManager().Update(thisNode);
                trxnManager.Log(thisNode.Name + " shutting down at " + thisNode.IPAddress + " on " + thisNode.Port);
            }

            IList<SinkNode> SinkNodes = new SinkNodeManager().GetAllSinkNode();

            foreach (var sinkNode in SinkNodes)
            {
                sinkNode.IsActive = false;
                new SinkNodeManager().Update(sinkNode);
                trxnManager.Log(sinkNode.Name + " shutting down at " + sinkNode.IPAddress + " on " + sinkNode.Port);
            }
        }
    }
}
