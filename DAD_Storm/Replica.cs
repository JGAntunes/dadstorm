using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace DADStormCore
{
    [Serializable]
    public class Replica
    {
        public IList<Uri> Nodes { get; set; }
        public Operator Operator { get; }
        public IList<Replica> OutputReplicas { get; }
        public IPolicy RoutingPolicy;

        public Replica(Operator op, IList<Replica> outputReplicas)
        {
            this.Nodes = new List<Uri>();
            this.RoutingPolicy = new PrimaryRouting();
            this.Operator = op;
            this.OutputReplicas = outputReplicas;
        }

        public Replica(Operator op, IList<Replica> outputReplicas, IPolicy routingPolicy)
        {
            this.Nodes = new List<Uri>();
            this.RoutingPolicy = routingPolicy;
            this.Operator = op;
            this.OutputReplicas = outputReplicas;
        }

        public void AddNode(Uri uri)
        {
            this.Nodes.Add(uri);
        }

        public void AddNode(string uri)
        {
            this.Nodes.Add(new Uri(uri));
        }

        public Uri resolve(TupleStream tuple)
        {
            return this.RoutingPolicy.resolveRouting(this, tuple);
        }

        public void Run()
        {
            foreach(Uri node in this.Nodes)
            {
                
            }
        }
    }

}