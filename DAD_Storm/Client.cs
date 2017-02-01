using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DADStormCore
{
    public class Client
    {
        public delegate void OperatorDelegate(TupleStream tuple);
        public ICollection<Replica> OutputReplicas { get; }
        public Operator InvokingOperator { get; }

        public Client(Operator invokingOperator,  ICollection<Replica> outputReplica)
        {
            this.OutputReplicas = outputReplica;
            this.InvokingOperator = invokingOperator;
        }

        // This is the call that the AsyncCallBack delegate references.
        [OneWayAttribute]
        private void RemoteCallBack(IAsyncResult ar)
        {
            Console.WriteLine("\r\n**SUCCESS**: invoked callback");

            Action result = (ar as AsyncResult).AsyncDelegate as Action;
            try
            {
                result.EndInvoke(ar);
            }
            catch (Exception e)
            {
                // TODO retry!!!
                Console.WriteLine(e.Message);
            }
            return;
        }

        public void SendDownstream(TupleStream tuple)
        {
            // Console.WriteLine("Remote synchronous and asynchronous delegates.");
            // Console.WriteLine(new String('-', 80));
            // Console.WriteLine();
            Console.WriteLine(this.OutputReplicas.Count);
            foreach (Replica rep in this.OutputReplicas)
            {
                Uri nodeUri = rep.resolve(tuple);
                Console.WriteLine(String.Format("Sending {0} to {1}", tuple.Elems[0], nodeUri));
                IDictionary prop = new Hashtable();
                prop["name"] = Guid.NewGuid().ToString();
                prop["typeFilterLevel"] = TypeFilterLevel.Full;
                TcpChannel channel = new TcpChannel(prop, null, null);
                ChannelServices.RegisterChannel(channel, false);

                Node op = (Node)Activator.GetObject(
                    typeof(Node),
                    nodeUri.ToString() + "/op");

                op.Execute(tuple);
                // This delegate is an asynchronous delegate. Two delegates must 
                // be created. The first is the system-defined AsyncCallback 
                // delegate, which references the method that the remote type calls 
                // back when the remote method is done.

                AsyncCallback RemoteCallback = new AsyncCallback(this.RemoteCallBack);

                // Create the delegate to the remote method you want to use 
                // asynchronously.
                OperatorDelegate RemoteDel = new OperatorDelegate(op.Execute);

                // Start the method call. Note that execution on this 
                // thread continues immediately without waiting for the return of 
                // the method call. 
                IAsyncResult RemAr = RemoteDel.BeginInvoke(tuple, RemoteCallback, null);
            }
            // WaitHandle.WaitAll(AsyncHandles);
            // handleNum = 0;
        }
    }
}
