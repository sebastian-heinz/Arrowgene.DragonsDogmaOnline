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
            Util.CryptoRandom.NextBytes(seed);
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

        public void SetSeed(byte[] seed)
        {
            uint r1 = (uint) (seed[0] | seed[1] << 8 | seed[2] << 16 | seed[3] << 24);
            uint r2 = (uint) (seed[4] | seed[5] << 8 | seed[6] << 16 | seed[7] << 24);
            uint r3 = (uint) (seed[8] | seed[9] << 8 | seed[10] << 16 | seed[11] << 24);
            uint r4 = (uint) (seed[12] | seed[13] << 8 | seed[14] << 16 | seed[15] << 24);
            SetSeed(r1, r2, r3, r4);
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
