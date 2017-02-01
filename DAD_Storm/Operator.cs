using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace DADStormCore
{
    public abstract class Operator : MarshalByRefObject
    {
        public string Id { get; }
        public string OperatorSpec { get; }
        public ICollection<object> OperatorParameters { get; }

        public Operator(string id, string operatorSpec, ICollection<object> operatorParameters)
        {
            this.Id = id;
            this.OperatorSpec = operatorSpec;
        }

        abstract public TupleStream Execute(TupleStream tuple);
    }

    public class UNIQ : Operator
    {
        public int FieldNumber { get; }
        public IDictionary<string, int> InputTuples { get; }

        public UNIQ(int fieldNumber, string id, string operatorSpec, ICollection<object> operatorParameters) : base(id, operatorSpec, operatorParameters)
        {
            // Field number inputs start at 1
            this.FieldNumber = fieldNumber - 1;
            this.InputTuples = new Dictionary<string, int>();
        }

        public override TupleStream Execute(TupleStream tuple)
        {
            var elem = tuple.Elems[this.FieldNumber];
            if (!InputTuples.ContainsKey(elem))
            {
                InputTuples.Add(elem, 1);
                return (tuple);
            }
            InputTuples[elem]++;
            return null;
        }
    }

    public class COUNT : Operator
    {
        public int TupleCount { get; set; }

        public COUNT(string id, string operatorSpec, ICollection<object> operatorParameters) : base(id, operatorSpec, operatorParameters)
        {
            this.TupleCount = 0;
        }

        public override TupleStream Execute(TupleStream tuple)
        {
            this.TupleCount++;
            var output = new List<string>();
            output.Add(this.TupleCount.ToString());
            return new TupleStream(output);
        }
    }

    public class DUP : Operator
    {

        public DUP(string id, string operatorSpec, ICollection<object> operatorParameters) : base(id, operatorSpec, operatorParameters)
        {
        }

        public override TupleStream Execute(TupleStream tuple)
        {
            Console.WriteLine("Executing dup " + tuple.Elems[0]);
            return tuple;
        }
    }

    public class FILTER : Operator
    {
        public IList<TupleStream> InputTuples { get; }

        public FILTER(string id, string operatorSpec, ICollection<object> operatorParameters) : base(id, operatorSpec, operatorParameters)
        {
            this.InputTuples = new List<TupleStream>();
        }

        public override TupleStream Execute(TupleStream tuple)
        {
            return tuple;
        }
    }

    public class CUSTOM : Operator
    {
        public IList<TupleStream> InputTuples { get; }

        public CUSTOM(string id, string operatorSpec, ICollection<object> operatorParameters) : base(id, operatorSpec, operatorParameters)
        {
            this.InputTuples = new List<TupleStream>();
        }

        public override TupleStream Execute(TupleStream tuple)
        {
            return tuple;
        }
    }
}