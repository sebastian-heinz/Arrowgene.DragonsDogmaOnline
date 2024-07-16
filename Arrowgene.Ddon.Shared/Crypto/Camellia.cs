using System;

namespace Arrowgene.Ddon.Shared.Crypto
{
    public class Camellia
    {
        public uint GetKeyLength(Span<byte> key)
        {
            return (uint) key.Length * 8;
        }

        public void Encrypt(Span<byte> input, out Span<byte> output, Span<byte> key, Span<byte> prv, bool pad = false)
        {
            KeySchedule(key, out Memory<byte> subKey);
            Encrypt(input, out output, GetKeyLength(key), subKey.Span, prv, new byte[8], pad);
        }

        public void Decrypt(Span<byte> input, out Span<byte> output, Span<byte> key, Span<byte> prv, bool pad = false)
        {
            KeySchedule(key, out Memory<byte> subKey);
            Decrypt(input, out output, GetKeyLength(key), subKey.Span, prv, new byte[8], pad);
        }

        public void Encrypt(Span<byte> input, out Span<byte> output, uint keyLength, Span<byte> subKey, Span<byte> prv, Span<byte> t8, bool pad = false)
        {
            int mod = input.Length % 16;
            if (mod > 0)
            {
                if (!pad)
                {
                    throw new Exception("Invalid PlainText Size");
                }

                int padding = 16 - mod;
                byte[] padInput = new byte[input.Length + padding];
                Buffer.BlockCopy(input.ToArray(), 0, padInput, 0, input.Length);
                input = padInput;
            }

            int length = input.Length;
            output = new byte[length];
            int current = 0;
            while (current < length)
            {
                int xorLen = current + 16 < length ? 16 : length - current;
                for (int i = 0; i < xorLen; i++)
                {
                    input[current + i] = (byte) (input[current + i] ^ prv[i]);
                }

                CryptBlock(
                    false,
                    keyLength,
                    input.Slice(current, 16),
                    subKey,
                    output.Slice(current, 16),
                    t8
                );
                for (int i = 0; i < xorLen; i++)
                {
                    prv[i] = output[current + i];
                }

                current += xorLen;
            }
        }

        public void Decrypt(Span<byte> input, out Span<byte> output, uint keyLength, Span<byte> subKey, Span<byte> prv, Span<byte> t8, bool pad = false)
        {
            int mod = input.Length % 16;
            if (mod > 0)
            {
                if (!pad)
                {
                    throw new Exception("Invalid CipherText Size");
                }

                int padding = 16 - mod;
                byte[] padInput = new byte[input.Length + padding];
                Buffer.BlockCopy(input.ToArray(), 0, padInput, 0, input.Length);
                input = padInput;
            }

            int length = input.Length;
            output = new byte[length];
            int current = 0;
            while (current < length)
            {
                CryptBlock(
                    true,
                    keyLength,
                    input.Slice(current, 16),
                    subKey,
                    output.Slice(current, 16),
                    t8
                );
                int xorLen = current + 16 < length ? 16 : length - current;
                for (int i = 0; i < xorLen; i++)
                {
                    output[current + i] = (byte) (output[current + i] ^ prv[i]);
                }

                for (int i = 0; i < xorLen; i++)
                {
                    prv[i] = input[current + i];
                }

                current += xorLen;
            }
        }

        public void KeySchedule(Span<byte> key, out Memory<byte> subKey)
        {
            KeySchedule(key, out subKey, new byte[8]);
        }

        /*
         * 	Camellia key schedule
         * 	subkey[26] should be allocated for keyLen == 128.
         * 	otherwise subkey[34] should be allocated.
         */
        public void KeySchedule(Span<byte> key, out Memory<byte> subKey, Span<byte> t8)
        {
            uint keyLen = GetKeyLength(key);

            /* 0...KL, 1...KR, 2...KA, 3...KB */
            byte[][] ikey = new byte[4][]
            {
                new byte[16],
                new byte[16],
                new byte[16],
                new byte[16],
            };

            Span<byte> pl;
            Span<byte> pr;
            Span<byte> p;

            int aki; /* all intermediate key index */
            int dki; /* drop key index */
            int ski; /* subkey index */
            int maxikey;
            int i;
            int j;
            int[] drop128 =
            {
                8, 9, 15, 16, 22, 23, 0
            };
            int[] drop256 =
            {
                2, 3, 4, 5, 8, 9, 14, 15, 16, 17, 20, 21, 26, 27, 30, 31,
                36, 37, 42, 43, 46, 47, 48, 49, 54, 55, 58, 59, 60, 61, 0
            };
            int[] drop; /* pointer to drop128[] or drop256[] */

            /* padding */
            int bytes = (int) keyLen / 8;

            int rounds = bytes / 16;
            for (int round = 0; round < rounds; round++)
            {
                int currentBytes = round * 16;
                int remainingBytes = bytes - currentBytes;
                int count = remainingBytes > 16 ? 16 : remainingBytes;
                // TODO
                Buffer.BlockCopy(key.ToArray(), round * 16, ikey[round], 0, count);
            }

            if (keyLen == 192)
            {
                for (i = 0; i < 8; i++)
                {
                    ikey[1][i + 8] = (byte) ~ikey[1][i];
                }
            }

            /* generate intermediate keys KA, KB */
            Span<byte> spanKey2 = new Span<byte>(ikey[2]);
            pl = spanKey2.Slice(0, 8);
            pr = spanKey2.Slice(8, 8);

            for (i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    xorOctets(16, ikey[i / 2 + 1], ikey[0], ikey[2]);
                }

                CamelliaRound(Sigma[i], pl, pr, t8);
                p = pl;
                pl = pr;
                pr = p;
            }

            if (keyLen != 128)
            {
                xorOctets(16, ikey[2], ikey[1], ikey[3]); /* KB <- KA ^ KR */
                Span<byte> spanKey3 = new Span<byte>(ikey[3]);
                Span<byte> spanKey3_0 = spanKey3.Slice(0, 8);
                Span<byte> spanKey3_8 = spanKey3.Slice(8, 8);
                CamelliaRound(Sigma[4], spanKey3_0, spanKey3_8, t8);
                CamelliaRound(Sigma[5], spanKey3_8, spanKey3_0, t8);
            }

            /* subkey generation */
            aki = dki = ski = 0;
            int subKeyCount;
            if (keyLen == 128)
            {
                subKeyCount = 26;
                maxikey = 2;
                drop = drop128;
                // memcpy(ikey[1], ikey[2], 16); /* ikey[1] is KA for 128-bit key */
                Buffer.BlockCopy(ikey[2], 0, ikey[1], 0, 16);
            }
            else
            {
                /* keyLen == 192 or 256 */
                subKeyCount = 34;
                maxikey = 4;
                drop = drop256;
            }

            subKey = new byte[subKeyCount * 8];

            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 2 * maxikey; j++)
                {
                    if (aki != drop[dki])
                    {
                        byte[] iKeySrc = ikey[j / 2];
                        Span<byte> iKeySrcSpan = new Span<byte>(iKeySrc);
                        Span<byte> src = iKeySrcSpan.Slice((j % 2) * 8, 8);
                        src.CopyTo(subKey.Span.Slice(ski * 8));
                        ski++;
                    }
                    else
                    {
                        dki++;
                    }

                    aki++;
                }

                for (j = 0; j < maxikey; j++)
                {
                    if (i < 4)
                    {
                        rot15(ikey[j]);
                    }
                    else
                    {
                        rot17(ikey[j]);
                    }
                }
            }
        }

        public void CryptBlock(bool decrypt, uint keyLen, Span<byte> pt, Span<byte> subKey, Span<byte> ct)
        {
            CryptBlock(decrypt, keyLen, pt, subKey, ct, new byte[8]);
        }

        /// <summary>
        /// Crypts 16byte block
        /// </summary>
        /// <param name="decrypt"></param>
        /// <param name="keyLen"></param>
        /// <param name="pt"></param>
        /// <param name="subKey"></param>
        /// <param name="ct"></param>
        /// <param name="t8">temporary 8byte array to use</param>
        public void CryptBlock(bool decrypt, uint keyLen, Span<byte> pt, Span<byte> subKey, Span<byte> ct, Span<byte> t8)
        {
            int r; /* round */
            int ski; /* subkey index */
            int direction;

            if (decrypt)
            {
                /* decryption */
                direction = -1;
                ski = keyLen == 128 ? 26 - 2 : 34 - 2;
            }
            else
            {
                /* encryption */
                direction = 1;
                ski = 0;
            }

            /* prewhitening */
            xorOctets(16, pt.Slice(0, 16), subKey.Slice(ski * 8, 16), ct.Slice(0, 16));
            if (decrypt)
            {
                /* decryption */
                ski--;
            }
            else
            {
                /* encryption */
                ski += 2;
            }

            /* main iteration */
            for (r = 0; r < 24; r += 2)
            {
                if (keyLen == 128 && r >= 18)
                {
                    break;
                }

                if (r == 6 || r == 12 || r == 18)
                {
                    CamelliaFL(subKey.Slice(ski * 8, 8), ct.Slice(0, 8), t8);
                    ski += direction;
                    CamelliaFLinv(subKey.Slice(ski * 8, 8), ct.Slice(8, 8), t8);
                    ski += direction;
                }

                CamelliaRound(subKey.Slice(ski * 8, 8), ct.Slice(0, 8), ct.Slice(8, 8), t8);
                ski += direction;
                CamelliaRound(subKey.Slice(ski * 8, 8), ct.Slice(8, 8), ct.Slice(0, 8), t8);
                ski += direction;
            }

            swapHalfBlock(ct.Slice(0, 8), ct.Slice(8, 8));

            /* postwhitening */
            if (decrypt)
            {
                ski--;
            }

            xorOctets(16, ct.Slice(0, 16), subKey.Slice(ski * 8, 16), ct.Slice(0, 16));
        }

        private byte s1(int x)
        {
            return S[x];
        }

        private byte s2(int x)
        {
            unchecked
            {
                return (byte)((S[x] << 1) + (S[x] >> 7));
            }
        }

        private byte s3(int x)
        {
            unchecked
            {
                return (byte)((S[x] << 7) + (S[x] >> 1));
            }
        }

        private byte s4(int x)
        {
            unchecked
            {
                return S[(byte)(x << 1) + (x >> 7)];
            }
        }

        /* dst[] <- src1[] ^ src2[] */
        private void xorOctets(uint nOctets, Span<byte> src1, Span<byte> src2, Span<byte> dst)
        {
            int i;

            for (i = 0; i < nOctets; i++)
            {
                dst[i] = (byte) (src1[i] ^ src2[i]);
            }
        }

        /* a[] <-> b[] */
        private void swapHalfBlock(Span<byte> a, Span<byte> b)
        {
            byte t;
            for (int i = 0; i < 8; i++)
            {
                t = a[i];
                a[i] = b[i];
                b[i] = t;
            }
        }

        /* dst[] <- src1[] & src2[] */
        private void and4octets(Span<byte> src1, Span<byte> src2, Span<byte> dst)
        {
            int i;

            for (i = 0; i < 4; i++)
            {
                dst[i] = (byte) (src1[i] & src2[i]);
            }
        }

        /* dst[] <- src1[] | src2[] */
        private void or4octets(Span<byte> src1, Span<byte> src2, Span<byte> dst)
        {
            int i;

            for (i = 0; i < 4; i++)
            {
                dst[i] = (byte) (src1[i] | src2[i]);
            }
        }

        /* x[] <<<= 1 */
        private void rot1(int nOctets, Span<byte> x)
        {
            unchecked
            {
                byte x0 = x[0];
                nOctets--;
                for (int i = 0; i < nOctets; i++)
                {
                    x[i] = (byte)((x[i] << 1) ^ (x[i + 1] >> 7));
                }

                x[nOctets] = (byte)((x[nOctets] << 1) ^ (x0 >> 7));
            }
        }

        /* rotate 128-bit data to the left by 16 bits */
        private void rot16(Span<byte> x)
        {
            byte x0 = x[0];
            byte x1 = x[1];
            int i;

            for (i = 0; i < 14; i++)
            {
                x[i] = x[i + 2];
            }

            x[i++] = x0;
            x[i] = x1;
        }

        /* rotate 128-bit data to the left by 15 bits */
        private void rot15(Span<byte> x)
        {
            unchecked
            {
                byte x15;
                int i;

                rot16(x);
                x15 = x[15];
                for (i = 15; i >= 1; i--)
                {
                    x[i] = (byte)((x[i] >> 1) ^ (x[i - 1] << 7));
                }

                x[0] = (byte)((x[0] >> 1) ^ (x15 << 7));
            }
        }

        /* rotate 128-bit data to the left by 17 bits */
        private void rot17(Span<byte> x)
        {
            rot16(x);
            rot1(16, x);
        }

        /* Camellia round function without swap */
        private void CamelliaRound(Span<byte> subKey, Span<byte> l, Span<byte> r, Span<byte> t8)
        {
            /* key XOR */
            xorOctets(8, subKey, l, t8);

            /* S-Function */
            t8[0] = s1(t8[0]);
            t8[1] = s2(t8[1]);
            t8[2] = s3(t8[2]);
            t8[3] = s4(t8[3]);
            t8[4] = s2(t8[4]);
            t8[5] = s3(t8[5]);
            t8[6] = s4(t8[6]);
            t8[7] = s1(t8[7]);

            /* P-Function with Feistel XOR */
            byte a = (byte) (t8[0] ^ t8[3] ^ t8[4] ^ t8[5] ^ t8[6]);
            r[7] ^= a;
            a ^= (byte) (t8[0] ^ t8[1] ^ t8[2]);
            r[3] ^= a;
            a ^= (byte) (t8[1] ^ t8[6] ^ t8[7]);
            r[6] ^= a;
            a ^= (byte) (t8[0] ^ t8[1] ^ t8[3]);
            r[2] ^= a;
            a ^= (byte) (t8[0] ^ t8[5] ^ t8[6]);
            r[5] ^= a;
            a ^= (byte) (t8[0] ^ t8[2] ^ t8[3]);
            r[1] ^= a;
            a ^= (byte) (t8[3] ^ t8[4] ^ t8[5]);
            r[4] ^= a;
            a ^= (byte) (t8[1] ^ t8[2] ^ t8[3]);
            r[0] ^= a;
        }

        /* Camellia FL function */
        private void CamelliaFL(Span<byte> subKey, Span<byte> x, Span<byte> t4)
        {
            and4octets(x.Slice(0, 4), subKey.Slice(0, 4), t4);
            rot1(4, t4);
            xorOctets(4, t4, x.Slice(4, 4), x.Slice(4, 4));
            or4octets(x.Slice(4, 4), subKey.Slice(4, 4), t4);
            xorOctets(4, x.Slice(0, 4), t4, x.Slice(0, 4));
        }

        /* Camellia FL^{-1} function */
        private void CamelliaFLinv(Span<byte> subKey, Span<byte> y, Span<byte> t4)
        {
            or4octets(y.Slice(4, 4), subKey.Slice(4, 4), t4);
            xorOctets(4, y.Slice(0, 4), t4, y.Slice(0, 4));
            and4octets(y.Slice(0, 4), subKey.Slice(0, 4), t4);
            rot1(4, t4);
            xorOctets(4, t4, y.Slice(4, 4), y.Slice(4, 4));
        }

        /* sbox */
        private static readonly byte[] S = new byte[256]
        {
            0x70, 0x82, 0x2c, 0xec, 0xb3, 0x27, 0xc0, 0xe5,
            0xe4, 0x85, 0x57, 0x35, 0xea, 0x0c, 0xae, 0x41,
            0x23, 0xef, 0x6b, 0x93, 0x45, 0x19, 0xa5, 0x21,
            0xed, 0x0e, 0x4f, 0x4e, 0x1d, 0x65, 0x92, 0xbd,
            0x86, 0xb8, 0xaf, 0x8f, 0x7c, 0xeb, 0x1f, 0xce,
            0x3e, 0x30, 0xdc, 0x5f, 0x5e, 0xc5, 0x0b, 0x1a,
            0xa6, 0xe1, 0x39, 0xca, 0xd5, 0x47, 0x5d, 0x3d,
            0xd9, 0x01, 0x5a, 0xd6, 0x51, 0x56, 0x6c, 0x4d,
            0x8b, 0x0d, 0x9a, 0x66, 0xfb, 0xcc, 0xb0, 0x2d,
            0x74, 0x12, 0x2b, 0x20, 0xf0, 0xb1, 0x84, 0x99,
            0xdf, 0x4c, 0xcb, 0xc2, 0x34, 0x7e, 0x76, 0x05,
            0x6d, 0xb7, 0xa9, 0x31, 0xd1, 0x17, 0x04, 0xd7,
            0x14, 0x58, 0x3a, 0x61, 0xde, 0x1b, 0x11, 0x1c,
            0x32, 0x0f, 0x9c, 0x16, 0x53, 0x18, 0xf2, 0x22,
            0xfe, 0x44, 0xcf, 0xb2, 0xc3, 0xb5, 0x7a, 0x91,
            0x24, 0x08, 0xe8, 0xa8, 0x60, 0xfc, 0x69, 0x50,
            0xaa, 0xd0, 0xa0, 0x7d, 0xa1, 0x89, 0x62, 0x97,
            0x54, 0x5b, 0x1e, 0x95, 0xe0, 0xff, 0x64, 0xd2,
            0x10, 0xc4, 0x00, 0x48, 0xa3, 0xf7, 0x75, 0xdb,
            0x8a, 0x03, 0xe6, 0xda, 0x09, 0x3f, 0xdd, 0x94,
            0x87, 0x5c, 0x83, 0x02, 0xcd, 0x4a, 0x90, 0x33,
            0x73, 0x67, 0xf6, 0xf3, 0x9d, 0x7f, 0xbf, 0xe2,
            0x52, 0x9b, 0xd8, 0x26, 0xc8, 0x37, 0xc6, 0x3b,
            0x81, 0x96, 0x6f, 0x4b, 0x13, 0xbe, 0x63, 0x2e,
            0xe9, 0x79, 0xa7, 0x8c, 0x9f, 0x6e, 0xbc, 0x8e,
            0x29, 0xf5, 0xf9, 0xb6, 0x2f, 0xfd, 0xb4, 0x59,
            0x78, 0x98, 0x06, 0x6a, 0xe7, 0x46, 0x71, 0xba,
            0xd4, 0x25, 0xab, 0x42, 0x88, 0xa2, 0x8d, 0xfa,
            0x72, 0x07, 0xb9, 0x55, 0xf8, 0xee, 0xac, 0x0a,
            0x36, 0x49, 0x2a, 0x68, 0x3c, 0x38, 0xf1, 0xa4,
            0x40, 0x28, 0xd3, 0x7b, 0xbb, 0xc9, 0x43, 0xc1,
            0x15, 0xe3, 0xad, 0xf4, 0x77, 0xc7, 0x80, 0x9e
        };

        /* key schedule constants */
        private static readonly byte[][] Sigma = new byte[6][]
        {
            new byte[8] {0xa0, 0x9e, 0x66, 0x7f, 0x3b, 0xcc, 0x90, 0x8b},
            new byte[8] {0xb6, 0x7a, 0xe8, 0x58, 0x4c, 0xaa, 0x73, 0xb2},
            new byte[8] {0xc6, 0xef, 0x37, 0x2f, 0xe9, 0x4f, 0x82, 0xbe},
            new byte[8] {0x54, 0xff, 0x53, 0xa5, 0xf1, 0xd3, 0x6f, 0x1c},
            new byte[8] {0x10, 0xe5, 0x27, 0xfa, 0xde, 0x68, 0x2d, 0x1d},
            new byte[8] {0xb0, 0x56, 0x88, 0xc2, 0xb3, 0xe6, 0xc1, 0xfd}
        };
    }
}
