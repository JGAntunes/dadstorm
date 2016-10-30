using System;
using System.Collections.Generic;

abstract class Operator
{
  public int Id { get; }
  public ICollection<Operator> InputOps { get; }
  public int ReplicaFactor { get; }
  public IPolicy RoutingPolicy { get; }
  public ICollection<Uri> Addresses { get; }
  public String OperatorSpec { get; }
  public ICollection<Object> Parameters { get; }

  abstract public void execute ();
}
