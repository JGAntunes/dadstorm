using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADStormCore
{
    public class Test
    {

        public static string ExecPath()
        {
            return Environment.CurrentDirectory + "\\DADStormCore.exe";
        }

        public static void Main(string[] args)
        {
            var op = Int32.Parse(args[0]);
            switch(op)
            {
                case 1:
                    var operator1 = startOperator("a", "foobar");
                    var node1 = new Node("tcp://localhost:8889", operator1, new List<Replica>());
                    node1.Run();
                    break;
                case 2:
                    var operator2 = startOperator("b", "foobar");
                    var node2 = new Node("tcp://localhost:8888", operator2, new List<Replica>());
                    node2.Run();
                    break;
            }
            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();
            /*
            var nodeUri = "tcp://localhost:9003/";
            var PCS = new PCS(new Uri(nodeUri));
            PCS.Run();
            IDictionary prop = new Hashtable();
            prop["name"] = prop.GetHashCode().ToString();
            TcpChannel channel = new TcpChannel(prop, null, null);
            ChannelServices.RegisterChannel(channel, false);

            PCS.OperatorServices pcs = (PCS.OperatorServices)Activator.GetObject(
                typeof(PCS.OperatorServices),
                nodeUri + "pcs");

            var operatorDownstream = startOperator("a", "foobar");
            var operatorUpstream = startOperator("b", "foobar");
            var operatorLocal = startOperator("c", "foobar");

            var replicaDownstream = new Replica(operatorDownstream, new List<Replica>());
            replicaDownstream.AddNode("tcp://localhost:8888");
            replicaDownstream.AddNode("tcp://localhost:8889");

            var outputUp = new List<Replica>();
            outputUp.Add(replicaDownstream);
            var replicaUpstream = new Replica(operatorUpstream, outputUp);
            replicaUpstream.AddNode("tcp://localhost:9000");

            var outputLocal = new List<Replica>();
            outputLocal.Add(replicaUpstream);

            Thread task0 = new Thread(() =>
            {
                IDictionary prop2 = new Hashtable();
                prop2["name"] = "tcp9000";
                prop2["port"] = 9000;
                TcpServerChannel channel2 = new TcpServerChannel(prop2, null, null);
                ChannelServices.RegisterChannel(channel2, false);
                ObjRef serviceRef = RemotingServices.Marshal(replicaDownstream.Nodes[0], "op", typeof(INode));
                System.Console.WriteLine("Running on: " + serviceRef.URI.ToString());
            });
            Thread task1 = new Thread(() =>
            {
                IDictionary prop3 = new Hashtable();
                prop3["name"] = "tcp8080";
                prop3["port"] = 8080;
                TcpServerChannel channel3 = new TcpServerChannel(prop3, null, null);
                ChannelServices.RegisterChannel(channel3, false);
                ObjRef serviceRef2 = RemotingServices.Marshal(replicaUpstream.Nodes[0], "op");
                System.Console.WriteLine("Running on: " + serviceRef2.URI.ToString());
            });

            task0.Start();
            task1.Start();

            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();

            pcs.AddOperator(replicaDownstream);
            pcs.AddOperator(replicaUpstream);

            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();

            var localHandler = new Node("tcp://localhost:9001", operatorLocal, outputLocal);

            var resultLocal1 = new TupleStream();
            resultLocal1.Elems.Add("test");
            var resultLocal2 = new TupleStream();
            resultLocal2.Elems.Add("foo");
            localHandler.Execute(resultLocal1);
            localHandler.Execute(resultLocal2);

            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();
            */
        }

        public static Operator startOperator(string id, string operatorSpec)
        {
            Operator op = new DUP(id, operatorSpec, new List<Object>());
            return op;
        }
    }
}
