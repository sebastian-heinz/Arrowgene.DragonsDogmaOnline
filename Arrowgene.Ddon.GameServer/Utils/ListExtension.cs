using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Utils
{
    public static class ListExtension
    {
        public static T GetWeightedRandomElement<T>(this List<T> list, double lowBias, double middleBias = 0.3)
        {
            int maxIndex = list.Count - 1;
            double x = Random.Shared.NextDouble();
            double rawValue = Math.Pow(x, lowBias) * (1 - middleBias) + Math.Min(x, 1 - x) * middleBias;
            int weightedIndex = (int)(maxIndex * rawValue);
            return list[weightedIndex];
        }
    }
}
