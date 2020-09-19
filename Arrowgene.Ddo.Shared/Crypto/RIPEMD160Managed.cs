// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Contributed to .NET Foundation by Darren R. Starr - Conscia Norway AS
//
// Awaiting permission from Antoon Bosselaers - Katholieke Universiteit Leuven 
//   for permission/clarification regarding the use the original code from which
//   this C# port is created.
//  Until he clarifies the license status of his code (referenced at
//   https://homes.esat.kuleuven.be/~bosselae/ripemd160.html) the legal license
//   status of this code is not clear. 

using System;
using System.Linq;
using System.Text;

namespace System.Security.Cryptography
{
    public class RIPEMD160Managed : RIPEMD160
    {
        static public UInt32 ReadUInt32(byte[] buffer, long offset)
        {
            return
                (Convert.ToUInt32(buffer[3 + offset]) << 24) |
                (Convert.ToUInt32(buffer[2 + offset]) << 16) |
                (Convert.ToUInt32(buffer[1 + offset]) << 8) |
                (Convert.ToUInt32(buffer[0 + offset]));
        }

        static UInt32 RotateLeft(UInt32 value, int bits)
        {
            return (value << bits) | (value >> (32 - bits));
        }

        /* the five basic functions F(), G() and H() */
        static UInt32 F(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ y ^ z;
        }

        static UInt32 G(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) | (~x & z);
        }

        static UInt32 H(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x | ~y) ^ z;
        }

        static UInt32 I(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & z) | (y & ~z);
        }

        static UInt32 J(UInt32 x, UInt32 y, UInt32 z)
        {
            return x ^ (y | ~z);
        }

        /* the ten basic operations FF() through III() */

        static void FF(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += F(b, c, d) + x;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }


        static void GG(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += G(b, c, d) + x + (UInt32)0x5a827999;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }


        static void HH(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += H(b, c, d) + x + (UInt32)0x6ed9eba1;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void II(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += I(b, c, d) + x + (UInt32)0x8f1bbcdc;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void JJ(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += J(b, c, d) + x + (UInt32)0xa953fd4e;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void FFF(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += F(b, c, d) + x;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void GGG(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += G(b, c, d) + x + (UInt32)0x7a6d76e9;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void HHH(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += H(b, c, d) + x + (UInt32)0x6d703ef3;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void III(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += I(b, c, d) + x + (UInt32)0x5c4dd124;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        static void JJJ(ref UInt32 a, UInt32 b, ref UInt32 c, UInt32 d, UInt32 e, UInt32 x, int s)
        {
            a += J(b, c, d) + x + (UInt32)0x50a28be6;
            a = RotateLeft(a, s) + e;
            c = RotateLeft(c, 10);
        }

        /// initializes MDbuffer to "magic constants"
        static public void MDinit(ref UInt32[] MDbuf)
        {
            MDbuf[0] = (UInt32)0x67452301;
            MDbuf[1] = (UInt32)0xefcdab89;
            MDbuf[2] = (UInt32)0x98badcfe;
            MDbuf[3] = (UInt32)0x10325476;
            MDbuf[4] = (UInt32)0xc3d2e1f0;
        }

        ///  the compression function.
        ///  transforms MDbuf using message bytes X[0] through X[15]
        static public void compress(ref UInt32[] MDbuf, UInt32[] X)
        {
            UInt32 aa = MDbuf[0];
            UInt32 bb = MDbuf[1];
            UInt32 cc = MDbuf[2];
            UInt32 dd = MDbuf[3];
            UInt32 ee = MDbuf[4];
            UInt32 aaa = MDbuf[0];
            UInt32 bbb = MDbuf[1];
            UInt32 ccc = MDbuf[2];
            UInt32 ddd = MDbuf[3];
            UInt32 eee = MDbuf[4];

            /* round 1 */
            FF(ref aa, bb, ref cc, dd, ee, X[0], 11);
            FF(ref ee, aa, ref bb, cc, dd, X[1], 14);
            FF(ref dd, ee, ref aa, bb, cc, X[2], 15);
            FF(ref cc, dd, ref ee, aa, bb, X[3], 12);
            FF(ref bb, cc, ref dd, ee, aa, X[4], 5);
            FF(ref aa, bb, ref cc, dd, ee, X[5], 8);
            FF(ref ee, aa, ref bb, cc, dd, X[6], 7);
            FF(ref dd, ee, ref aa, bb, cc, X[7], 9);
            FF(ref cc, dd, ref ee, aa, bb, X[8], 11);
            FF(ref bb, cc, ref dd, ee, aa, X[9], 13);
            FF(ref aa, bb, ref cc, dd, ee, X[10], 14);
            FF(ref ee, aa, ref bb, cc, dd, X[11], 15);
            FF(ref dd, ee, ref aa, bb, cc, X[12], 6);
            FF(ref cc, dd, ref ee, aa, bb, X[13], 7);
            FF(ref bb, cc, ref dd, ee, aa, X[14], 9);
            FF(ref aa, bb, ref cc, dd, ee, X[15], 8);

            /* round 2 */
            GG(ref ee, aa, ref bb, cc, dd, X[7], 7);
            GG(ref dd, ee, ref aa, bb, cc, X[4], 6);
            GG(ref cc, dd, ref ee, aa, bb, X[13], 8);
            GG(ref bb, cc, ref dd, ee, aa, X[1], 13);
            GG(ref aa, bb, ref cc, dd, ee, X[10], 11);
            GG(ref ee, aa, ref bb, cc, dd, X[6], 9);
            GG(ref dd, ee, ref aa, bb, cc, X[15], 7);
            GG(ref cc, dd, ref ee, aa, bb, X[3], 15);
            GG(ref bb, cc, ref dd, ee, aa, X[12], 7);
            GG(ref aa, bb, ref cc, dd, ee, X[0], 12);
            GG(ref ee, aa, ref bb, cc, dd, X[9], 15);
            GG(ref dd, ee, ref aa, bb, cc, X[5], 9);
            GG(ref cc, dd, ref ee, aa, bb, X[2], 11);
            GG(ref bb, cc, ref dd, ee, aa, X[14], 7);
            GG(ref aa, bb, ref cc, dd, ee, X[11], 13);
            GG(ref ee, aa, ref bb, cc, dd, X[8], 12);

            /* round 3 */
            HH(ref dd, ee, ref aa, bb, cc, X[3], 11);
            HH(ref cc, dd, ref ee, aa, bb, X[10], 13);
            HH(ref bb, cc, ref dd, ee, aa, X[14], 6);
            HH(ref aa, bb, ref cc, dd, ee, X[4], 7);
            HH(ref ee, aa, ref bb, cc, dd, X[9], 14);
            HH(ref dd, ee, ref aa, bb, cc, X[15], 9);
            HH(ref cc, dd, ref ee, aa, bb, X[8], 13);
            HH(ref bb, cc, ref dd, ee, aa, X[1], 15);
            HH(ref aa, bb, ref cc, dd, ee, X[2], 14);
            HH(ref ee, aa, ref bb, cc, dd, X[7], 8);
            HH(ref dd, ee, ref aa, bb, cc, X[0], 13);
            HH(ref cc, dd, ref ee, aa, bb, X[6], 6);
            HH(ref bb, cc, ref dd, ee, aa, X[13], 5);
            HH(ref aa, bb, ref cc, dd, ee, X[11], 12);
            HH(ref ee, aa, ref bb, cc, dd, X[5], 7);
            HH(ref dd, ee, ref aa, bb, cc, X[12], 5);

            /* round 4 */
            II(ref cc, dd, ref ee, aa, bb, X[1], 11);
            II(ref bb, cc, ref dd, ee, aa, X[9], 12);
            II(ref aa, bb, ref cc, dd, ee, X[11], 14);
            II(ref ee, aa, ref bb, cc, dd, X[10], 15);
            II(ref dd, ee, ref aa, bb, cc, X[0], 14);
            II(ref cc, dd, ref ee, aa, bb, X[8], 15);
            II(ref bb, cc, ref dd, ee, aa, X[12], 9);
            II(ref aa, bb, ref cc, dd, ee, X[4], 8);
            II(ref ee, aa, ref bb, cc, dd, X[13], 9);
            II(ref dd, ee, ref aa, bb, cc, X[3], 14);
            II(ref cc, dd, ref ee, aa, bb, X[7], 5);
            II(ref bb, cc, ref dd, ee, aa, X[15], 6);
            II(ref aa, bb, ref cc, dd, ee, X[14], 8);
            II(ref ee, aa, ref bb, cc, dd, X[5], 6);
            II(ref dd, ee, ref aa, bb, cc, X[6], 5);
            II(ref cc, dd, ref ee, aa, bb, X[2], 12);

            /* round 5 */
            JJ(ref bb, cc, ref dd, ee, aa, X[4], 9);
            JJ(ref aa, bb, ref cc, dd, ee, X[0], 15);
            JJ(ref ee, aa, ref bb, cc, dd, X[5], 5);
            JJ(ref dd, ee, ref aa, bb, cc, X[9], 11);
            JJ(ref cc, dd, ref ee, aa, bb, X[7], 6);
            JJ(ref bb, cc, ref dd, ee, aa, X[12], 8);
            JJ(ref aa, bb, ref cc, dd, ee, X[2], 13);
            JJ(ref ee, aa, ref bb, cc, dd, X[10], 12);
            JJ(ref dd, ee, ref aa, bb, cc, X[14], 5);
            JJ(ref cc, dd, ref ee, aa, bb, X[1], 12);
            JJ(ref bb, cc, ref dd, ee, aa, X[3], 13);
            JJ(ref aa, bb, ref cc, dd, ee, X[8], 14);
            JJ(ref ee, aa, ref bb, cc, dd, X[11], 11);
            JJ(ref dd, ee, ref aa, bb, cc, X[6], 8);
            JJ(ref cc, dd, ref ee, aa, bb, X[15], 5);
            JJ(ref bb, cc, ref dd, ee, aa, X[13], 6);

            /* parallel round 1 */
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[5], 8);
            JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[14], 9);
            JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[7], 9);
            JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[0], 11);
            JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[9], 13);
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[2], 15);
            JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[11], 15);
            JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[4], 5);
            JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[13], 7);
            JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[6], 7);
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[15], 8);
            JJJ(ref eee, aaa, ref bbb, ccc, ddd, X[8], 11);
            JJJ(ref ddd, eee, ref aaa, bbb, ccc, X[1], 14);
            JJJ(ref ccc, ddd, ref eee, aaa, bbb, X[10], 14);
            JJJ(ref bbb, ccc, ref ddd, eee, aaa, X[3], 12);
            JJJ(ref aaa, bbb, ref ccc, ddd, eee, X[12], 6);

            /* parallel round 2 */
            III(ref eee, aaa, ref bbb, ccc, ddd, X[6], 9);
            III(ref ddd, eee, ref aaa, bbb, ccc, X[11], 13);
            III(ref ccc, ddd, ref eee, aaa, bbb, X[3], 15);
            III(ref bbb, ccc, ref ddd, eee, aaa, X[7], 7);
            III(ref aaa, bbb, ref ccc, ddd, eee, X[0], 12);
            III(ref eee, aaa, ref bbb, ccc, ddd, X[13], 8);
            III(ref ddd, eee, ref aaa, bbb, ccc, X[5], 9);
            III(ref ccc, ddd, ref eee, aaa, bbb, X[10], 11);
            III(ref bbb, ccc, ref ddd, eee, aaa, X[14], 7);
            III(ref aaa, bbb, ref ccc, ddd, eee, X[15], 7);
            III(ref eee, aaa, ref bbb, ccc, ddd, X[8], 12);
            III(ref ddd, eee, ref aaa, bbb, ccc, X[12], 7);
            III(ref ccc, ddd, ref eee, aaa, bbb, X[4], 6);
            III(ref bbb, ccc, ref ddd, eee, aaa, X[9], 15);
            III(ref aaa, bbb, ref ccc, ddd, eee, X[1], 13);
            III(ref eee, aaa, ref bbb, ccc, ddd, X[2], 11);

            /* parallel round 3 */
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[15], 9);
            HHH(ref ccc, ddd, ref eee, aaa, bbb, X[5], 7);
            HHH(ref bbb, ccc, ref ddd, eee, aaa, X[1], 15);
            HHH(ref aaa, bbb, ref ccc, ddd, eee, X[3], 11);
            HHH(ref eee, aaa, ref bbb, ccc, ddd, X[7], 8);
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[14], 6);
            HHH(ref ccc, ddd, ref eee, aaa, bbb, X[6], 6);
            HHH(ref bbb, ccc, ref ddd, eee, aaa, X[9], 14);
            HHH(ref aaa, bbb, ref ccc, ddd, eee, X[11], 12);
            HHH(ref eee, aaa, ref bbb, ccc, ddd, X[8], 13);
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[12], 5);
            HHH(ref ccc, ddd, ref eee, aaa, bbb, X[2], 14);
            HHH(ref bbb, ccc, ref ddd, eee, aaa, X[10], 13);
            HHH(ref aaa, bbb, ref ccc, ddd, eee, X[0], 13);
            HHH(ref eee, aaa, ref bbb, ccc, ddd, X[4], 7);
            HHH(ref ddd, eee, ref aaa, bbb, ccc, X[13], 5);

            /* parallel round 4 */
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[8], 15);
            GGG(ref bbb, ccc, ref ddd, eee, aaa, X[6], 5);
            GGG(ref aaa, bbb, ref ccc, ddd, eee, X[4], 8);
            GGG(ref eee, aaa, ref bbb, ccc, ddd, X[1], 11);
            GGG(ref ddd, eee, ref aaa, bbb, ccc, X[3], 14);
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[11], 14);
            GGG(ref bbb, ccc, ref ddd, eee, aaa, X[15], 6);
            GGG(ref aaa, bbb, ref ccc, ddd, eee, X[0], 14);
            GGG(ref eee, aaa, ref bbb, ccc, ddd, X[5], 6);
            GGG(ref ddd, eee, ref aaa, bbb, ccc, X[12], 9);
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[2], 12);
            GGG(ref bbb, ccc, ref ddd, eee, aaa, X[13], 9);
            GGG(ref aaa, bbb, ref ccc, ddd, eee, X[9], 12);
            GGG(ref eee, aaa, ref bbb, ccc, ddd, X[7], 5);
            GGG(ref ddd, eee, ref aaa, bbb, ccc, X[10], 15);
            GGG(ref ccc, ddd, ref eee, aaa, bbb, X[14], 8);

            /* parallel round 5 */
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[12], 8);
            FFF(ref aaa, bbb, ref ccc, ddd, eee, X[15], 5);
            FFF(ref eee, aaa, ref bbb, ccc, ddd, X[10], 12);
            FFF(ref ddd, eee, ref aaa, bbb, ccc, X[4], 9);
            FFF(ref ccc, ddd, ref eee, aaa, bbb, X[1], 12);
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[5], 5);
            FFF(ref aaa, bbb, ref ccc, ddd, eee, X[8], 14);
            FFF(ref eee, aaa, ref bbb, ccc, ddd, X[7], 6);
            FFF(ref ddd, eee, ref aaa, bbb, ccc, X[6], 8);
            FFF(ref ccc, ddd, ref eee, aaa, bbb, X[2], 13);
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[13], 6);
            FFF(ref aaa, bbb, ref ccc, ddd, eee, X[14], 5);
            FFF(ref eee, aaa, ref bbb, ccc, ddd, X[0], 15);
            FFF(ref ddd, eee, ref aaa, bbb, ccc, X[3], 13);
            FFF(ref ccc, ddd, ref eee, aaa, bbb, X[9], 11);
            FFF(ref bbb, ccc, ref ddd, eee, aaa, X[11], 11);

            // combine results */
            ddd += cc + MDbuf[1];               /* final result for MDbuf[0] */
            MDbuf[1] = MDbuf[2] + dd + eee;
            MDbuf[2] = MDbuf[3] + ee + aaa;
            MDbuf[3] = MDbuf[4] + aa + bbb;
            MDbuf[4] = MDbuf[0] + bb + ccc;
            MDbuf[0] = ddd;
        }

        ///  puts bytes from strptr into X and pad out; appends length 
        ///  and finally, compresses the last block(s)
        ///  note: length in bits == 8 * (lswlen + 2^32 mswlen).
        ///  note: there are (lswlen mod 64) bytes left in strptr.
        static public void MDfinish(ref UInt32[] MDbuf, byte[] strptr, long index, UInt32 lswlen, UInt32 mswlen)
        {
            //UInt32 i;                                 /* counter       */
            var X = Enumerable.Repeat((UInt32)0, 16).ToArray();                             /* message words */


            /* put bytes from strptr into X */
            for (var i = 0; i < (lswlen & 63); i++)
            {
                /* byte i goes into word X[i div 4] at pos.  8*(i mod 4)  */
                X[i >> 2] ^= Convert.ToUInt32(strptr[i + index]) << (8 * (i & 3));
            }

            /* append the bit m_n == 1 */
            X[(lswlen >> 2) & 15] ^= (UInt32)1 << Convert.ToInt32(8 * (lswlen & 3) + 7);

            if ((lswlen & 63) > 55)
            {
                /* length goes to next block */
                compress(ref MDbuf, X);
                X = Enumerable.Repeat((UInt32)0, 16).ToArray();
            }

            /* append length in bits*/
            X[14] = lswlen << 3;
            X[15] = (lswlen >> 29) | (mswlen << 3);
            compress(ref MDbuf, X);
        }
        static int RMDsize = 160;
        UInt32 [] MDbuf = new UInt32 [RMDsize / 32];
        UInt32 [] X = new UInt32[16];               /* current 16-word chunk        */
        byte [] UnhashedBuffer = new byte[64];
        int UnhashedBufferLength = 0;
        long HashedLength = 0;

        public RIPEMD160Managed()
        {
            Initialize();
        }

        protected override void HashCore (byte[] array, int ibStart, int cbSize)
        {
            var index = 0;
            while(index < cbSize)
            {
                var bytesRemaining = cbSize - index;
                if(UnhashedBufferLength > 0)
                {
                    if((bytesRemaining + UnhashedBufferLength) >= (UnhashedBuffer.Length))
                    {
                        Array.Copy(array, ibStart + index, UnhashedBuffer, UnhashedBufferLength, (UnhashedBuffer.Length) - UnhashedBufferLength);
                        index += (UnhashedBuffer.Length) - UnhashedBufferLength;
                        UnhashedBufferLength = UnhashedBuffer.Length;

                        for (var i = 0; i < 16; i++)
                            X[i] = ReadUInt32(UnhashedBuffer, i * 4);

                        compress(ref MDbuf, X);
                        UnhashedBufferLength = 0;
                    }
                    else
                    {
                        Array.Copy(array, ibStart + index, UnhashedBuffer, UnhashedBufferLength, bytesRemaining);
                        UnhashedBufferLength += bytesRemaining;
                        index += bytesRemaining;
                    }
                }
                else
                {
                    if(bytesRemaining >= (UnhashedBuffer.Length))
                    {
                        for (var i = 0; i < 16; i++)
                            X[i] = ReadUInt32(array, index + (i * 4));
                        index += UnhashedBuffer.Length;

                        compress(ref MDbuf, X);
                    }
                    else
                    {
                        Array.Copy(array, ibStart + index, UnhashedBuffer, 0, bytesRemaining);
                        UnhashedBufferLength = bytesRemaining;
                        index += bytesRemaining;
                    }
                }
            }

            HashedLength += cbSize;
        }

        protected override byte[] HashFinal ()
        {
            MDfinish(ref MDbuf, UnhashedBuffer, 0, Convert.ToUInt32(HashedLength), 0);

            var result = new byte [RMDsize / 8];

            for (var i = 0; i < RMDsize / 8; i += 4)
            {
                result[i] = Convert.ToByte(MDbuf[i >> 2] & 0xFF);         /* implicit cast to byte  */
                result[i + 1] = Convert.ToByte((MDbuf[i >> 2] >> 8) & 0xFF);  /*  extracts the 8 least  */
                result[i + 2] = Convert.ToByte((MDbuf[i >> 2] >> 16) & 0xFF);  /*  significant bits.     */
                result[i + 3] = Convert.ToByte((MDbuf[i >> 2] >> 24) & 0xFF);
            }

            return result;
        }

        public override void Initialize ()
        {
            MDinit(ref MDbuf);
            X = Enumerable.Repeat((UInt32)0, 16).ToArray();
            HashedLength = 0;
            UnhashedBufferLength = 0;
        }
    }
}
