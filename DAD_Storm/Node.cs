using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DADStorm
{
    public class Node: MarshalByRefObject
    {
        public Uri Uri { get; }
        public string Name { get; }

        public Node(Uri uri, string name)
        {
            this.Uri = uri;
            this.Name = name;
        }

        public Node(string uri, string name)
        {
            this.Uri = new Uri(uri);
            this.Name = name;
        }

        public void Run(Type operatorType)
        {
            IDictionary prop = new Hashtable();
            prop["name"] = this.Name;
            prop["port"] = this.Uri.Port;
            TcpChannel channel = new TcpChannel(prop, null, null);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                operatorType,
                "op",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("Running on: " + this.Uri.ToString());
            // System.Console.WriteLine("<enter> to exit...");
            // System.Console.ReadLine();
        }
    }
}
