using System;
using System.Collections.Generic;

namespace Operator
{
  abstract class Operator
  {
    public int Id { get; }
    public ICollection<Operator> InputOps { get; }
    public IPolicy RoutingPolicy { get; }
    public ICollection<Replica> Replicas { get; }
    public String OperatorSpec { get; }
    public ICollection<Object> Parameters { get; }

    abstract public IList<TupleStream> execute(IList<TupleStream> tuples);
  }

  class Unique : Operator
  {
    public int FieldNumber { get; }

    public Unique(int fieldNumber)
    {
      this.FieldNumber = fieldNumber;
    }

    public override IList<TupleStream> execute(IList<TupleStream> tuples) {
      var element = tuples[this.FieldNumber];
      var output = new List<TupleStream>();
      tuples.removeAt(this.FieldNumber);

      foreach (TupleStream t in tuples)
      {
        if(t.Equals(element))
          return output;
      }
      output.Add(element);
      return output;
    }
  }

  class Count : Operator
  {
    public override IList<TupleStream> execute(IList<TupleStream> tuples) {
      var output = new List<TupleStream>();
      output.Add(tuples.Count);
      return output;
    }
  }

  class Dup : Operator
  {
    public override IList<TupleStream> execute(IList<TupleStream> tuples) {
      return tuples;
    }
  }

  class Filter : Operator
  {
    public override IList<TupleStream> execute(IList<TupleStream> tuples) {
      return tuples;
    }
  }

  class Custom : Operator
  {
    public override IList<TupleStream> execute(IList<TupleStream> tuples) {
      return tuples;
    }
  }
}
