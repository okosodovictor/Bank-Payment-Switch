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
                Console.WriteLine("Switch waiting for connection... \nPress s to shutdown, \t r to restart, \t q to quit/Exit");

                if (Console.ReadLine() == "s")
                {
                    Shutdown();
                }
                else if (Console.ReadLine() == "r")
                {
                    Startup();
                }
                else if (Console.ReadLine() == "q")
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
            Logger.Log("Searching for pre-configured source servers");
            List<SourceNode> allSourceNode = new SourceNodeManager().RetrieveAll().ToList();
            Logger.Log(allSourceNode.Count + " Source server(s) found:.\t ");

            new Listener().StartListener(allSourceNode);

            //Start up all clients
            Logger.Log("Searching for pre-configured sink Clients:-->> ");
            List<SinkNode> allSinkNode = new SinkNodeManager().GetAllSinkNode().ToList();
            Logger.Log(allSinkNode.Count + " Sink Client(s) found");
            new Client().StartClient(allSinkNode);

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
                Logger.Log(thisNode.Name + " shutting down at " + thisNode.IPAddress + " on " + thisNode.Port);
            }

            IList<SinkNode> SinkNodes = new SinkNodeManager().GetAllSinkNode();

            foreach (var sinkNode in SinkNodes)
            {
                sinkNode.IsActive = false;
                new SinkNodeManager().Update(sinkNode);
                Logger.Log(sinkNode.Name + " shutting down at " + sinkNode.IPAddress + " on " + sinkNode.Port);
                Console.WriteLine("SinkNode ShortDown" + sinkNode.IPAddress + "\t" + sinkNode.Port);
            }
        }
    }
}
