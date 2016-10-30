using System.Collections.Generic;

interface IPolicy
{
  void resolveRouting (ICollection<Replica> replicas);
}
