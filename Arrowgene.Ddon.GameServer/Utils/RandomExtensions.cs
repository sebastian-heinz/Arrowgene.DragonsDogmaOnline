using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Utils
{
    public static class RandomExtensions
    {
        public static UInt64 NextU64(this Random rnd)
        {
            var buffer = new byte[sizeof(Int64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public static UInt32 NextU32(this Random rnd)
        {
            var buffer = new byte[sizeof(Int64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }


        /// <summary>
        /// Favors lower over higher results. The range is [min,max).
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int WeightedNext(this Random rnd, int min, int max, double bias = 2.0)
        {
            double r = rnd.NextDouble();
            double weighted = Math.Pow(r, bias);

            int range = max - min;
            int result = min + (int)(weighted * range);
            return Math.Min(max - 1, Math.Max(min, result));
        }

        /// <summary>
        /// Favors lower over higher results. The max is not included in the results.
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int WeightedNext(this Random rnd, int max, double bias = 2.0)
        {
            return WeightedNext(rnd, 0, max, bias);
        }

        /// <summary>
        /// Random normal variates by Box-Muller transform
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="mean"></param>
        /// <param name="std"></param>
        /// <returns></returns>
        public static double NextNormal(this Random rnd, double mean, double std)
        {
            if (std < 0)
            {
                throw new ArgumentException("Negative standard deviation is not allowed.");
            }

            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return mean + std * randStdNormal; //random normal(mean,stdDev^2)
        }

        /// <summary>
        /// Choose from a list of objects according to their weights.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rnd"></param>
        /// <param name="objects"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T ChooseWeighted<T>(this Random rnd, List<T> objects, List<double> weights = null)
        {
            if (objects is null)
            {
                throw new ArgumentException("Cannot choose from null list.");
            }

            if (objects.Count == 0)
            {
                throw new ArgumentException("Cannot choose from an empty list of objects.");
            }

            weights = weights ?? Enumerable.Repeat(1.0, objects.Count).ToList();

            if (weights.Count != objects.Count)
            {
                throw new ArgumentException("Number of weights does not match number of objects.");
            }

            if (weights.Any(x => x < 0))
            {
                throw new ArgumentException("Negative weights are not allowed.");
            }

            if (objects.Count == 1)
            {
                return objects.First();
            }

            List<double> cumWeights = [weights[0]];
            for (int i = 1; i < weights.Count; i++)
            {
                cumWeights.Add(weights[i] + cumWeights[i - 1]);
            }

            double rv = rnd.NextDouble() * cumWeights.Last();
            int index = cumWeights.Count(x => x < rv);
            return objects[index];
        }
    }
}
