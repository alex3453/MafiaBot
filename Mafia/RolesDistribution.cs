using System;
using System.Collections.Generic;

namespace Mafia
{
    public class RolesDistribution
    {
        public static List<Role> DistributeRoles(int allPlayersCount)
        {
            var result = new List<Role>();
            var mafiaCount = allPlayersCount / 5 + 1;
            for (var i = 0; i < mafiaCount; i++)
                result.Add(new MafiaRole());
            for (var i = 0; i < allPlayersCount - mafiaCount; i++)
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