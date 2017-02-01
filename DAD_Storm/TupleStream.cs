using System;
using System.Collections.Generic;

namespace DADStormCore
{
    [Serializable]
    public class TupleStream
    {
        public IList<string> Elems { get; }

        public TupleStream(IList<string> elems)
        {
            this.Elems = elems;
        }

        public TupleStream()
        {
            this.Elems = new List<string>();
        }
    }
}