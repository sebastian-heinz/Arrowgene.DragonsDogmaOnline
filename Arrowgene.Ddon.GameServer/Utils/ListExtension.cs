using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Utils
{
    public static class ListExtension
    {
        public static T GetWeightedRandomElement<T>(this List<T> list, double bias)
        {
            double totalWeight = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalWeight += Math.Pow(list.Count - i, bias);
            }

            double x = Random.Shared.NextDouble() * totalWeight;

            double cumulativeWeight = 0;
            for (int i = 0; i < list.Count; i++)
            {
                cumulativeWeight += Math.Pow(list.Count - i, bias);
                if (x < cumulativeWeight)
                {
                    return list[i];
                }
            }

            return list[^1];
        }
    }
}
