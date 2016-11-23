using System;
using System.Collections.Generic;

namespace DADStorm
{
    public interface IPolicy
    {
        Uri resolveRouting(Replica replica);
    }

    public class PrimaryRouting : IPolicy
    {
        public Uri resolveRouting(Replica replica)
        {
            return replica.Nodes[0].Uri;
        }
    }

    public class RandomRouting : IPolicy
    {
        Random RandomGen = new Random();

        public Uri resolveRouting(Replica replica)
        {
            
            int r = this.RandomGen.Next(0, replica.Nodes.Count);
            return replica.Nodes[r].Uri;
        }
    }
}