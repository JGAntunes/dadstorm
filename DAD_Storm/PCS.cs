using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace DADStormCore
{
    public class PCS : MarshalByRefObject
    {
        public Uri Uri { get; }


        public PCS(Uri uri)
        {
            this.Uri = uri;
        }

        public void Run()
        {
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary prop = new Hashtable();
            prop["name"] = "pcs";
            prop["port"] = this.Uri.Port;
            TcpChannel channel = new TcpChannel(prop, null, provider);
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(OperatorServices),
                "pcs",
                WellKnownObjectMode.Singleton);
        }

        public class OperatorServices : MarshalByRefObject
        {
            public ICollection<Replica> Operators { get; set; }

            public OperatorServices()
            {
                this.Operators = new List<Replica>();
            }

            public void AddOperator(Replica replica)
            {
                replica.Run();
                this.Operators.Add(replica);
            }
        }
    }
}
