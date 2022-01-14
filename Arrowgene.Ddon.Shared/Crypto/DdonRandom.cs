using System;

namespace Arrowgene.Ddon.Shared.Crypto
{
    public class DdonRandom
    {
        private static string KeyPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

        private uint _r1;
        private uint _r2;
        private uint _r3;
        private uint _r4;

        public DdonRandom()
        {
            byte[] seed = new byte[16];
            CryptoRandom.Instance.NextBytes(seed);
            SetSeed(seed);
        }

        public string GenerateKey()
        {
            string key = "";
            for (int i = 0; i < 0x30; i++)
            {
                uint next = Next();
                uint n = next & 0x3F;
                key += KeyPool[(int) n];
            }

            return key;
        }

        public byte NextKeyByte()
        {
            uint next = Next();
            uint n = next & 0x3F;
            return (byte) KeyPool[(int) n];
        }

        public void SetSeed(byte[] seed)
        {
            uint r1 = (uint) (seed[0] | seed[1] << 8 | seed[2] << 16 | seed[3] << 24);
            uint r2 = (uint) (seed[4] | seed[5] << 8 | seed[6] << 16 | seed[7] << 24);
            uint r3 = (uint) (seed[8] | seed[9] << 8 | seed[10] << 16 | seed[11] << 24);
            uint r4 = (uint) (seed[12] | seed[13] << 8 | seed[14] << 16 | seed[15] << 24);
            SetSeed(r1, r2, r3, r4);
        }

        public void SetSeed(uint seed)
        {
            uint v2;
            uint v3;
            uint v4;
            uint v5;
            uint v7 = 0;

            v2 = 0x159A55E5;
            _r1 = 123456789;
            _r2 = 362436069;
            _r3 = 521288629;
            _r4 = 88675123;

            v3 = 521288629;
            v4 = 88675123;
            v5 = seed ^ 0xAC9365;

            for (int i = 0; i < 100; i++)
            {
                v7 = v2;
                v2 = v3;
                v3 = v4;
                v5 ^= (0x65AC9365u >> (int) (v5 & 3)) ^ ((v5 ^ (0x65AC9365u >> (int) (v5 & 3))) >> 3) ^ ((v5 ^ (0x65AC9365u >> (int) (v5 & 3))) >> 4) ^ (8 * (v5 ^ (0x65AC9365u >> (int) (v5 & 3)))) ^ (16 * (v5 ^ (0x65AC9365u >> (int) (v5 & 3))));
                v4 ^= v5 ^ (v5 << 15) ^ ((v5 ^ (v5 << 15)) >> 4) ^ (v4 >> 21);
            }

            SetSeed(v7, v2, v3, v4);
        }

        public void SetSeed(uint r1, uint r2, uint r3, uint r4)
        {
            _r1 = r1;
            _r2 = r2;
            _r3 = r3;
            _r4 = r4;
        }

        public uint Current()
        {
            return _r4;
        }

        public uint Next()
        {
            Advance(1);
            return Current();
        }

        public uint Previous()
        {
            Advance(-1);
            return Current();
        }

        public void Advance(int steps)
        {
            if (steps > 0)
            {
                for (int i = 0; i < steps; i++)
                {
                    uint a2 = _r1 << 0xF;
                    uint a3 = _r1 ^ a2;
                    uint d2 = _r4 >> 0x11;
                    uint d3 = d2 ^ a3;
                    uint d4 = d3 >> 0x4;
                    uint d5 = d4 ^ _r4;
                    uint d6 = d5 ^ a3;
                    _r1 = _r2;
                    _r2 = _r3;
                    _r3 = _r4;
                    _r4 = d6;
                }
            }
            else if (steps < 0)
            {
                throw new NotSupportedException();
            }
        }
    }
}
