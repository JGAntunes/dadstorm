using System;
using System.Collections.Generic;

namespace DADStorm
{
    public class Replica
    {
        public IList<Node> Nodes { get; set; }
        public Type OperatorType;
        public IPolicy RoutingPolicy;

        public Replica(Type operatorType)
        {
            this.OperatorType = operatorType;
            this.Nodes = new List<Node>();
            this.RoutingPolicy = new PrimaryRouting();
        }

        public Replica(Type operatorType, List<Node> nodes)
        {
            this.OperatorType = operatorType;
            this.Nodes = nodes;
            this.RoutingPolicy = new PrimaryRouting();
        }

        public Replica(Type operatorType, IPolicy routingPolicy)
        {
            this.OperatorType = operatorType;
            this.Nodes = new List<Node>();
            this.RoutingPolicy = routingPolicy;
        }

        public Replica(Type operatorType, List<Node> nodes, IPolicy routingPolicy)
        {
            this.OperatorType = operatorType;
            this.Nodes = nodes;
            this.RoutingPolicy = routingPolicy;
        }

        public void AddNode(Node node)
        {
            this.Nodes.Add(node);
        }

        public void AddNode(Uri uri, string name)
        {
            this.Nodes.Add(new Node(uri, name));
        }

        public void AddNode(string uri, string name)
        {
            this.Nodes.Add(new Node(uri, name));
        }

        public Uri resolve()
        {
            return this.RoutingPolicy.resolveRouting(this);
        }

        public void Run()
        {
            foreach(Node node in this.Nodes)
            {
                node.Run(this.OperatorType);
            }
        }
    }

}