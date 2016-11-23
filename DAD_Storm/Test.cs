using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADStorm
{
    public class Test
    {
        public static void Main()
        {
            var replicaDownstream = new Replica(typeof(Dup));
            replicaDownstream.AddNode("tcp://localhost:8888", "replica-down-1");
            replicaDownstream.AddNode("tcp://localhost:8889", "replica-down-2");
            replicaDownstream.RoutingPolicy = new RandomRouting();

            var replicaUpstream = new Replica(typeof(Dup));
            replicaUpstream.AddNode("tcp://localhost:9000", "replica-up-1");
            replicaUpstream.RoutingPolicy = new RandomRouting();

            var outputLocal = new List<Replica>();
            outputLocal.Add(replicaUpstream);

            var outputUp = new List<Replica>();
            outputUp.Add(replicaDownstream);

            var operatorDownstream = startOperator("a", "foobar", replicaDownstream, new List<Replica>());
            var operatorUpstream = startOperator("b", "foobar", replicaUpstream, outputUp);
            var operatorLocal = startOperator("c", "foobar", new Replica(typeof(Dup)), outputLocal);

            var resultLocal1 = new TupleStream();
            resultLocal1.Elems.Add("test");
            var resultLocal2 = new TupleStream();
            resultLocal2.Elems.Add("foo");
            operatorLocal.ResultTuples.Add(resultLocal1);
            operatorLocal.ResultTuples.Add(resultLocal2);

            operatorLocal.Done();
            System.Console.WriteLine("<enter> to exit...");
            System.Console.ReadLine();
        }

        public static Operator startOperator(string id, string operatorSpec, Replica replica, ICollection<Replica> outputReplicas)
        {
            Operator op = new Dup(id, operatorSpec, new List<Object>(), replica, outputReplicas);
            op.Replica.Run();
            return op;
        }
    }
}
