using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace DADStorm
{
    class PCS : MarshalByRefObject
    {
        public Uri Uri { get; }


        public PCS(Uri uri)
        {
            this.Uri = uri;
        }

        public void Run()
        {
            TcpChannel channel = new TcpChannel(this.Uri.Port);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(OperatorServices),
                "pcs",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();
        }

        public class OperatorServices : MarshalByRefObject
        {
            public ICollection<Operator> Operators { get; }

            public OperatorServices()
            {
                this.Operators = new List<Operator>();
            }

            public void AddOperator(Operator op)
            {
                this.Operators.Add(op);
            }
        }
    }
}
