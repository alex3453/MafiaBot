using System.Collections.Generic;

namespace Mafia
{
    public interface IRoleDistribution
    {
        List<Role> DistributeRoles(int playersCount);
    }
}