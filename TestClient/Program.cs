using DADStormCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IDictionary prop = new Hashtable();
            prop["name"] = prop.GetHashCode().ToString();
            prop["typeFilterLevel"] = TypeFilterLevel.Full;
            TcpChannel channel = new TcpChannel(prop, null, null);
            ChannelServices.RegisterChannel(channel, false);

            INode op = (INode)Activator.GetObject(
                typeof(INode),
                "tcp://localhost:8888/op");

            op.Execute(null);
        }
    }
}
