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

    abstract public IList<Tuple> execute(IList<Tuple> tuples);
  }

  class Unique : Operator
  {
    public int FieldNumber { get; }

    public Unique(int fieldNumber)
    {
      this.FieldNumber = fieldNumber;
    }

    public override IList<Tuple> execute(IList<Tuple> tuples) {
      var element = tuples[this.FieldNumber];
      var output = new List<Tuple>();
      tuples.removeAt(this.FieldNumber);

      foreach (Tuple t in tuples)
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
    public override IList<Tuple> execute(IList<Tuple> tuples) {
      var output = new List<Tuple>();
      output.Add(tuples.Count);
      return output;
    }
  }

  class Dup : Operator
  {
    public override IList<Tuple> execute(IList<Tuple> tuples) {
      return tuples;
    }
  }

  class Filter : Operator
  {
    public override IList<Tuple> execute(IList<Tuple> tuples) {
      return tuples;
    }
  }

  class Custom : Operator
  {
    public override IList<Tuple> execute(IList<Tuple> tuples) {
      return tuples;
    }
  }
}
