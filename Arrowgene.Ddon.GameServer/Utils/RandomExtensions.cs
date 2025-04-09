using System;

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
    }
}
