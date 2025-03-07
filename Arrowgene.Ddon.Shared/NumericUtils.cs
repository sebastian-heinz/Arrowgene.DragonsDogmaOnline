using System;

namespace Arrowgene.Ddon.GameServer.Utils
{
    public class NumericUtils
    {
        public static T DowncastWithTruncation<T>(ulong value) where T : struct
        {
            unchecked
            {
                object truncated;
                if (typeof(T) == typeof(byte)) truncated = (byte)(value & 0xFFUL);
                else if (typeof(T) == typeof(sbyte)) truncated = (sbyte)(value & 0xFFUL);
                else if (typeof(T) == typeof(short)) truncated = (short)(value & 0xFFFFUL);
                else if (typeof(T) == typeof(ushort)) truncated = (ushort)(value & 0xFFFFUL);
                else if (typeof(T) == typeof(int)) truncated = (int)(value & 0xFFFFFFFFUL);
                else if (typeof(T) == typeof(uint)) truncated = (uint)(value & 0xFFFFFFFFUL);
                else if (typeof(T) == typeof(long)) truncated = (long)value; // No truncation needed
                else if (typeof(T) == typeof(ulong)) truncated = value; // No truncation
                else throw new ArgumentException($"Unsupported type {typeof(T)}");

                return (T)truncated;
            }
        }
    }
}
