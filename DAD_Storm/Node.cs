using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;

namespace DADStormCore
{
    public interface INode
    {
        void Execute(TupleStream tuple);
    }

    public class Node: MarshalByRefObject, INode
    {
        public Uri Uri { get; }
        public Operator Operator { get; }
        public Client Client { get; set; }

        public delegate void SendDownstreamDelegate(TupleStream tuple);

        public override object InitializeLifetimeService() { return null; }

        public Node(Uri uri, Operator op, ICollection<Replica> outputReplicas)
        {
            this.Uri = uri;
            this.Operator = op;
            this.Client = new Client(this.Operator, outputReplicas);
        }

        public Node(string uri, Operator op, ICollection<Replica> outputReplicas)
        {
            this.Uri = new Uri(uri);
            this.Operator = op;
            this.Client = new Client(this.Operator, outputReplicas);
        }

        public void Run()
        {
            // IDictionary prop = new Hashtable();
            // prop["name"] = Guid.NewGuid().ToString();
            // prop["typeFilterLevel"] = TypeFilterLevel.Full;
            // prop["port"] = this.Uri.Port;
            // TcpServerChannel channel = new TcpServerChannel(prop, null);
            // ChannelServices.RegisterChannel(channel, true);
            // System.Console.WriteLine(ChannelServices.RegisteredChannels.ToString());
            // RemotingServices.Marshal(this, "op", typeof(Node));
            // RemotingConfiguration.RegisterWellKnownServiceType(
            //    typeof(PCS.OperatorHandle),
            //    "op",
            //    WellKnownObjectMode.Singleton);

            IDictionary prop = new Hashtable();
            prop["name"] = Guid.NewGuid().ToString();
            prop["typeFilterLevel"] = TypeFilterLevel.Full;
            prop["port"] = this.Uri.Port;
            TcpServerChannel channel = new TcpServerChannel(prop, null);
            ChannelServices.RegisterChannel(channel, false);
            System.Console.WriteLine(ChannelServices.RegisteredChannels.ToString());
            ObjRef serviceRef = RemotingServices.Marshal(this, "op", typeof(INode));
            System.Console.WriteLine("Running on: " + serviceRef.URI.ToString());

            System.Console.WriteLine("Running on: " + this.Uri.ToString());
            // System.Console.WriteLine("<enter> to exit...");
            // System.Console.ReadLine();
        }

        // This is the call that the AsyncCallBack delegate references.
        [OneWayAttribute]
        private void SendDownstreamCallback(IAsyncResult ar)
        {
            // OperatorDelegate del = (OperatorDelegate)((AsyncResult)ar).AsyncDelegate;
            Console.WriteLine("\r\n**SUCCESS**: invoked callback at Node");
            return;
        }

        public void Execute(TupleStream tuple)
        {
            var output = this.Operator.Execute(tuple);
            if (output != null)
            {
                SendDownstreamDelegate caller = new SendDownstreamDelegate(this.Client.SendDownstream);
                IAsyncResult result = caller.BeginInvoke(output, new AsyncCallback(this.SendDownstreamCallback), null);
            }
            return;
        }
    }
}
