using BankSwitch.Core.Entities;
using BankSwitch.Engine.ProcessorMangement;
using BankSwitch.Engine.Utility;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace BankSwitch.Engine.Connections
{
   public class Client
    {
        //TO INSTANTIATE CLIENT PEER
        TransactionManager trxnManager = new TransactionManager();
        public void StartClient(SinkNode sinkNode)
        {
            string ipAddress = sinkNode.IPAddress;
            int port = Convert.ToInt32(sinkNode.Port);

            ClientPeer client = new ClientPeer(sinkNode.Id.ToString(),
                        new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter(), ipAddress, port),
                        new BasicMessagesIdentifier(11, 41));
           
            client.RequestDone += new PeerRequestDoneEventHandler(Client_RequestDone);
            client.RequestCancelled += new PeerRequestCancelledEventHandler(Client_RequestCancelled);

            client.Connected += new PeerConnectedEventHandler(ClientPeerConnected);
            client.Receive += new PeerReceiveEventHandler(ClientPeerOnReceive);
            client.Disconnected += new PeerDisconnectedEventHandler(ClientPeerDisconnected);

        }

        //When the requested send to a Sink (client) is done
        static void Client_RequestDone(object sender, PeerRequestDoneEventArgs e)
        {
            Iso8583Message response = e.Request.ResponseMessage as Iso8583Message;
            //SourceNode sourceNode = e.Request.Payload as SourceNode;
            SinkNode sinkNode = e.Request.Payload as SinkNode;


            //continue coding
        }

        //When the requested send to a Sink (client) is NOT done
        static void Client_RequestCancelled(object sender, PeerRequestCancelledEventArgs e)
        {
            Iso8583Message message = e.Request.RequestMessage as Iso8583Message;
            SourceNode source = e.Request.Payload as SourceNode;

            //continue coding
        }

        //Client Peer
        private void ClientPeerConnected(object sender, EventArgs e)
        {
            ClientPeer client = sender as ClientPeer;
            if (client == null) return;
            trxnManager.Log("Connected to Client ==> " + client.Name);
        }

        private void ClientPeerDisconnected(object sender, EventArgs e)
        {
            ClientPeer client = sender as ClientPeer;
            if (client == null) return;
            trxnManager.Log("(Disconnected from Client =/=> " + client.Name);
        }

        private void ClientPeerOnReceive(object sender, ReceiveEventArgs e)
        {
            var clientPeer = sender as ClientPeer;
            trxnManager.Log("Client Peer Receiving>>> " + clientPeer.Name);

            Iso8583Message receivedMessage = e.Message as Iso8583Message;
            SinkNode theSinkNode;
            try
            {
                var theSender = sender as ClientPeer;
                theSinkNode = new SinkNodeManager().GetById(Convert.ToInt32(theSender.Name));
            }
            catch (Exception)
            {
                trxnManager.Log(String.Format("Message from Unknown Sink Node: {0}", receivedMessage));
                receivedMessage.SetResponseMessageTypeIdentifier();
                receivedMessage.Fields.Add(39, "91");   //Issuer inoperative
                clientPeer.Send(receivedMessage);
                return;
            }

            bool isValidMti;
            if (receivedMessage == null) return;
            string mtiDescription = MessageDefinition.GetMtiDescription(receivedMessage.MessageTypeIdentifier, "FEP", out isValidMti);

            if (!isValidMti)
            {

                trxnManager.Log(String.Format("Invalid MTI response code {0}, {1}", receivedMessage, theSinkNode)); // work on this
                clientPeer.Send(receivedMessage);

                return;
            }

            string responseCode = receivedMessage.Fields[39].ToString();
            string responseDescription = MessageDefinition.GetResponseDescription(responseCode);
            string stan = receivedMessage.Fields[11].ToString();

            clientPeer.Send(receivedMessage);

        }
    }
}
