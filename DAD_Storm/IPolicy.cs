using System.Collections.Generic;

namespace DADStorm
{
    public interface IPolicy
    {
        Uri resolveRouting(Replica replica);
    }
}