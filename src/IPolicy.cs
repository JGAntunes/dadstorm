using System.Collections.Generic;

interface IPolicy
{
  void resolveRouting (ICollection<IReplica> replicas);
}
