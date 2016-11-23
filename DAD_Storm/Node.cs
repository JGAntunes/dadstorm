using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DADStorm
{
    public class Node: MarshalByRefObject
    {
        public Uri Uri { get; }

        public Node(Uri uri)
        {
            this.Uri = uri;
        }

        public void Run()
        {
            TcpChannel channel = new TcpChannel(this.Uri.Port);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Operator),
                "op",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();
        }
    }
}
