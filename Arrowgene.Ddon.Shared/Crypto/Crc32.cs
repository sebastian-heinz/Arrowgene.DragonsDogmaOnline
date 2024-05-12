using System;
using System.Security.Cryptography;
using System.Text;

namespace Arrowgene.Ddon.Shared.Crypto
{
    public class Crc32 : HashAlgorithm
    {
        public static uint GetHash(string str)
        {
            return GetHash(str, Encoding.UTF8);
        }

        public static uint GetHash(string str, Encoding encoding)
        {
            return GetHash(encoding.GetBytes(str));
        }

        public static uint GetHash(byte[] data)
        {
            Crc32 crc32 = new Crc32();
            byte[] hash = crc32.ComputeHash(data);
            uint hasVal = BitConverter.ToUInt32(hash);
            return hasVal;
        }

        private const uint Poly = 0x04C11DB7;
        private const uint Seed = 0xffffffff;
        private const int Width = 32;
        private readonly uint[] _table;

        private uint _hash;

        private static uint ReverseBits(uint value, int valueLength)
        {
            uint output = 0;

            for (var i = valueLength - 1; i >= 0; i--)
            {
                output |= (value & 1) << i;
                value >>= 1;
            }

            return output;
        }

        private static uint[] InitializeTable()
        {
            var table = new uint[256];
            for (uint i = 0; i < table.Length; ++i)
            {
                uint r = i;
                r = ReverseBits(r, Width);
                ulong lastBit = 1ul << (Width - 1);
                for (var j = 0; j < 8; j++)
                {
                    if ((r & lastBit) != 0)
                    {
                        r = (r << 1) ^ Poly;
                    }
                    else
                    {
                        r <<= 1;
                    }
                }

                r = ReverseBits(r, Width);
                table[i] = r;
            }

            return table;
        }

        public Crc32()
        {
            _table = InitializeTable();
        }


        public override void Initialize()
        {
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            _hash = ComputeChecksum(array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            HashValue = BitConverter.GetBytes(_hash);
            return HashValue;
        }

        public override int HashSize => Width;

        private uint ComputeChecksum(byte[] bytes, int index, int count)
        {
            uint crc = Seed;
            for (var i = index; i < (index + count); ++i)
            {
                var idx = (byte)((crc & 0xFF) ^ bytes[i]);
                crc = _table[idx] ^ (crc >> 8);
            }

            return crc;
        }
    }
}
