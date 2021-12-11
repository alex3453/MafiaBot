using System;
using System.Collections.Generic;

namespace Mafia
{
    public class SimpleRoleDist : IRoleDistribution
    {
        public List<Role> DistributeRoles(int playersCount)
        {
            var result = new List<Role>();
            var mafiaCount = playersCount / 5 + 1;
            for (var i = 0; i < mafiaCount; i++)
                result.Add(new MafiaRole());
            for (var i = 0; i < playersCount - mafiaCount; i++)
                result.Add(new PeacefulRole());
            
            var random = new Random();
            for (var i = result.Count - 1; i >= 1; i--)
            {
                var j = random.Next(i + 1);
                (result[j], result[i]) = (result[i], result[j]);
            }
            return result;
        }
    }
}