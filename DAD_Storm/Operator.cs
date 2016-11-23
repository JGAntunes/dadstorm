using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace DADStorm
{
    public abstract class Operator : MarshalByRefObject
    {
        public delegate void OperatorDelegate(TupleStream tuple);

        public string Id { get; }
        // public ICollection<Operator> InputOps { get; }
        public IList<TupleStream> ResultTuples { get; }
        public ICollection<Replica> OutputReplicas { get; }
        public Replica Replica { get; }
        public string OperatorSpec { get; }
        public ICollection<object> OperatorParameters { get; }

        WaitHandle[] AsyncHandles = new WaitHandle[10];

        public Operator(string id, string operatorSpec, ICollection<object> operatorParameters, Replica replica)
        {
            this.Id = id;
            this.OperatorSpec = operatorSpec;
            this.Replica = replica;
            this.ResultTuples = new List<TupleStream>();
        }

        abstract public void Execute(TupleStream tuple);

        // This is the call that the AsyncCallBack delegate references.
        [OneWayAttribute]
        private void AsyncCallBack(IAsyncResult ar)
        {
            // OperatorDelegate del = (OperatorDelegate)((AsyncResult)ar).AsyncDelegate;
            Console.WriteLine("\r\n**SUCCESS**: invoked callback");
            // int handleNum = (int) ar.AsyncState;
            // Signal the thread.
            // this.AsyncHandles[handleNum].Set();
            return;
        }

        public void Done()
        {
            // Console.WriteLine("Remote synchronous and asynchronous delegates.");
            // Console.WriteLine(new String('-', 80));
            // Console.WriteLine();
            int handleNum = 0;
            foreach (Replica rep in this.OutputReplicas)
            {
                foreach (TupleStream tuple in this.ResultTuples)
                {
                    string nodeUri = rep.resolve().ToString();
                    TcpChannel channel = new TcpChannel();
                    ChannelServices.RegisterChannel(channel, true);
                    Operator op = (Operator)Activator.GetObject(
                        typeof(Operator),
                        nodeUri);
                    // This delegate is an asynchronous delegate. Two delegates must 
                    // be created. The first is the system-defined AsyncCallback 
                    // delegate, which references the method that the remote type calls 
                    // back when the remote method is done.

                    AsyncCallback RemoteCallback = new AsyncCallback(this.AsyncCallBack);

                    // Create the delegate to the remote method you want to use 
                    // asynchronously.
                    OperatorDelegate RemoteDel = new OperatorDelegate(op.Execute);

                    // Start the method call. Note that execution on this 
                    // thread continues immediately without waiting for the return of 
                    // the method call. 
                    IAsyncResult RemAr = RemoteDel.BeginInvoke(tuple, RemoteCallback, handleNum);
                    // AsyncHandles[handleNum] = RemAr.AsyncWaitHandle.WaitAll();
                    // handleNum++;
                }
            }
            WaitHandle.WaitAll(AsyncHandles);
        }
    }

    public class Unique : Operator
    {
        public int FieldNumber { get; }
        public IDictionary<string, int> InputTuples { get; }

        public Unique(int fieldNumber, string id, string operatorSpec, ICollection<object> operatorParameters, Replica replica) : base(id, operatorSpec, operatorParameters, replica)
        {
            this.FieldNumber = fieldNumber;
            this.InputTuples = new Dictionary<string, int>();
        }

        public override void Execute(TupleStream tuple)
        {
            var elem = tuple.Elems[this.FieldNumber];
            if (InputTuples.ContainsKey(elem))
            {
                InputTuples[elem]++;
            }
            InputTuples.Add(elem, 0);
        }
    }

    public class Count : Operator
    {
        public int TupleCount { get; set; }

        public Count(string id, string operatorSpec, ICollection<object> operatorParameters, Replica replica) : base(id, operatorSpec, operatorParameters, replica)
        {
            this.TupleCount = 0;
        }
        public override void Execute(TupleStream tuple)
        {
            this.TupleCount++;
        }
    }

    public class Dup : Operator
    {
        public IList<TupleStream> InputTuples { get; }

        public Dup(string id, string operatorSpec, ICollection<object> operatorParameters, Replica replica) : base(id, operatorSpec, operatorParameters, replica)
        {
            this.InputTuples = new List<TupleStream>();
        }

        public override void Execute(TupleStream tuple)
        {
            this.InputTuples.Add(tuple);
        }
    }

    public class Filter : Operator
    {
        public IList<TupleStream> InputTuples { get; }

        public Filter(string id, string operatorSpec, ICollection<object> operatorParameters, Replica replica) : base(id, operatorSpec, operatorParameters, replica)
        {
            this.InputTuples = new List<TupleStream>();
        }

        public override void Execute(TupleStream tuple)
        {
            this.InputTuples.Add(tuple);
        }
    }

    public class Custom : Operator
    {
        public IList<TupleStream> InputTuples { get; }

        public Custom(string id, string operatorSpec, ICollection<object> operatorParameters, Replica replica) : base(id, operatorSpec, operatorParameters, replica)
        {
            this.InputTuples = new List<TupleStream>();
        }

        public override void Execute(TupleStream tuple)
        {
            this.InputTuples.Add(tuple);
        }
    }
}