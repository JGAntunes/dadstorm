using System;
using System.Collections.Generic;

namespace DADStorm
{
    public class Replica
    {
        public ICollection<Node> Nodes { get; set; }
        public IPolicy RoutingPolicy;

        public Replica()
        {
            this.Nodes = new List<Node>();
        }

        public Replica(List<Node> nodes)
        {
            this.Nodes = nodes;
        }

        public void AddNode(Node node)
        {
            this.Nodes.Add(node);
        }

        public void AddNode(Uri uri)
        {
            this.Nodes.Add(new Node(uri));
        }

        public void AddNode(string uri)
        {
            this.Nodes.Add(new Node(new Uri(uri)));
        }

        public Uri resolve()
        {
            return this.RoutingPolicy.resolveRouting(this);
        }
    }

}