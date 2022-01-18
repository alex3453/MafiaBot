using System;
using System.Collections.Generic;
using System.Linq;

namespace Mafia
{
    public class SimpleRoleDist : IRoleDistribution
    {
        public List<Role> DistributeRoles(int playersCount)
        {
            var result = new List<Role>();
            var mafiaCount = playersCount / 10 + 1;
            for (var i = 0; i < mafiaCount; i++)
                result.Add(new MafiaRole());
            for (var i = 0; i < playersCount - mafiaCount; i++)
                result.Add(new PeacefulRole());

            var random = new Random();
            return result.OrderBy(r => random.Next()).ToList();
        }
    }
}