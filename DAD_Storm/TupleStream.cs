using System.Collections.Generic;

namespace DADStorm
{
    public class TupleStream
    {
        public IList<string> Elems { get; }

        public TupleStream(IList<string> elems)
        {
            this.Elems = elems;
        }
    }
}