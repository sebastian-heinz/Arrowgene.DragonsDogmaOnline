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
    }
}
