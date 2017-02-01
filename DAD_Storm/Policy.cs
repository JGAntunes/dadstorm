using System;
using System.Collections.Generic;

namespace DADStormCore
{
    public interface IPolicy
    {
        Uri resolveRouting(Replica replica, TupleStream tuple);
    }

    [Serializable]
    public class PrimaryRouting : IPolicy
    {
        public Uri resolveRouting(Replica replica, TupleStream tuple)
        {
            return replica.Nodes[0];
        }
    }

    [Serializable]
    public class RandomRouting : IPolicy
    {
        Random RandomGen = new Random();

        public Uri resolveRouting(Replica replica, TupleStream tuple)
        {
            
            int r = this.RandomGen.Next(0, replica.Nodes.Count);
            return replica.Nodes[r];
        }
    }

    [Serializable]
    public class HashRouting : IPolicy
    {
        int fieldNum;

        public HashRouting(int fieldNum)
        {
            // Field number inputs start at 1
            this.fieldNum = fieldNum - 1;
        }

        public Uri resolveRouting(Replica replica, TupleStream tuple)
        {
            var index = Math.Abs(tuple.Elems[this.fieldNum].GetHashCode() % replica.Nodes.Count);
            return replica.Nodes[index];
        }
    }
}