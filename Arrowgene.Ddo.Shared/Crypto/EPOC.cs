using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Arrowgene.Ddo.Shared.Crypto
{
    public class EPOC
    {
        private static RandomNumberGenerator rng = new RNGCryptoServiceProvider();

        public struct PrivateKey
        {
            public PublicKey PublicKey;
            public BigInteger Q;
            public BigInteger P;
            public BigInteger PSquared;
        }

        public struct PublicKey
        {
            public BigInteger N;
            public BigInteger G;
            public BigInteger H;
        }

        public PrivateKey GenerateKey(int bits)
        {
            // prime number p
            BigInteger p = GetRandomPrime(rng, bits / 2);
            // prime number q
            BigInteger q = GetRandomPrime(rng, bits / 2);
            // psquare = p * p
            BigInteger psquare = BigInteger.Multiply(p, p);
            // n = psquare * q
            BigInteger n = BigInteger.Multiply(psquare, q);
            // randomly choosing ineger g from {2...n-1},
            // such that g^(p-1) mod p^2 != 1
            BigInteger g;
            BigInteger gCriteria;
            BigInteger pMinusOne;
            BigInteger two = new BigInteger(2);
            while (true)
            {
                g = RandomInRange(rng, two, BigInteger.Subtract(n, BigInteger.One));
                pMinusOne = BigInteger.Subtract(p, BigInteger.One);
                gCriteria = BigInteger.ModPow(g, pMinusOne, psquare);
                if (gCriteria.CompareTo(BigInteger.One) != 0)
                {
                    break;
                }
            }

            // h = g^n mod n
            BigInteger h = BigInteger.Remainder(BigInteger.ModPow(g, n, n), n);
            PrivateKey privateKey = new PrivateKey()
            {
                PublicKey = new PublicKey
                {
                    G = g,
                    H = h,
                    N = n
                },
                Q = q,
                P = p,
                PSquared = psquare
            };
            return privateKey;
        }

        public byte[] Encrypt(PublicKey publicKey, byte[] plainText)
        {
            // choose a random integer r from {1...n-1}
            BigInteger r = RandomInRange(rng, BigInteger.One, BigInteger.Subtract(publicKey.N, BigInteger.One));
            BigInteger m = new BigInteger(plainText);
            //  m < N
            if (m.CompareTo(publicKey.N) == 1)
            {
                return null;
            }

            // c = g^m * h^r mod N
            BigInteger c = BigInteger.Remainder(
                BigInteger.Multiply(
                    BigInteger.ModPow(publicKey.G, m, publicKey.N),
                    BigInteger.ModPow(publicKey.H, r, publicKey.N)
                ),
                publicKey.N
            );
            return c.ToByteArray();
        }

        public byte[] Decrypt(PrivateKey privateKey, byte[] cipherText)
        {
            BigInteger c = new BigInteger(cipherText);
            if (c.CompareTo(privateKey.PublicKey.N) == 1)
            {
                return null;
            }

            BigInteger pMinusOne = BigInteger.Subtract(privateKey.P, BigInteger.One);
            // c^(p-1) mod p^2
            BigInteger a = BigInteger.Divide(
                BigInteger.Subtract(
                    BigInteger.ModPow(c, pMinusOne, privateKey.PSquared),
                    BigInteger.One
                ),
                privateKey.P
            );

            BigInteger b = BigInteger.Divide(
                BigInteger.Subtract(
                    BigInteger.ModPow(privateKey.PublicKey.G, pMinusOne, privateKey.PSquared),
                    BigInteger.One
                ),
                privateKey.P
            );

            BigInteger bInverse = ModInverse(b, privateKey.P);
            
            BigInteger m = BigInteger.Remainder(BigInteger.Multiply(a, bInverse), privateKey.P);
            return m.ToByteArray();
        }

        private BigInteger RandomInteger(RandomNumberGenerator rng, Int64 bits = 10000000000)
        {
            byte[] bytes = new byte[bits / 8];

            rng.GetBytes(bytes);

            //  var msb = bytes[bits / 8 - 1];
            //  var mask = 0;
            //  while (mask < msb)
            //      mask = (mask << 1) + 1;

            //  bytes[bits - 1] &= Convert.ToByte(mask);
            BigInteger i = new BigInteger(bytes);
            return i;
        }
        
        private BigInteger ModInverse(BigInteger value, BigInteger modulo) {
            BigInteger x, y;

            if (1 != Egcd(value, modulo, out x, out y))
                throw new ArgumentException("Invalid modulo", "modulo");

            if (x < 0)
                x += modulo;

            return x % modulo;
        }
        
        private BigInteger Egcd(BigInteger left, 
            BigInteger right, 
            out BigInteger leftFactor, 
            out BigInteger rightFactor) {
            leftFactor = 0;
            rightFactor = 1;
            BigInteger u = 1;
            BigInteger v = 0;
            BigInteger gcd = 0;

            while (left != 0) {
                BigInteger q = right / left;
                BigInteger r = right % left;

                BigInteger m = leftFactor - u * q;
                BigInteger n = rightFactor - v * q;

                right = left;
                left = r;
                leftFactor = u;
                rightFactor = v;
                u = m;
                v = n;

                gcd = right;
            }

            return gcd;
        }
        
        private BigInteger RandomInRange(RandomNumberGenerator rng, BigInteger min, BigInteger max)
        {
            if (min > max)
            {
                var buff = min;
                min = max;
                max = buff;
            }

            // offset to set min = 0
            BigInteger offset = -min;
            min = 0;
            max += offset;

            var value = randomInRangeFromZeroToPositive(rng, max) - offset;
            return value;
        }

        private BigInteger randomInRangeFromZeroToPositive(RandomNumberGenerator rng, BigInteger max)
        {
            BigInteger value;
            var bytes = max.ToByteArray();

            // count how many bits of the most significant byte are 0
            // NOTE: sign bit is always 0 because `max` must always be positive
            byte zeroBitsMask = 0b00000000;

            var mostSignificantByte = bytes[bytes.Length - 1];

            // we try to set to 0 as many bits as there are in the most significant byte, starting from the left (most significant bits first)
            // NOTE: `i` starts from 7 because the sign bit is always 0
            for (var i = 7; i >= 0; i--)
            {
                // we keep iterating until we find the most significant non-0 bit
                if ((mostSignificantByte & (0b1 << i)) != 0)
                {
                    var zeroBits = 7 - i;
                    zeroBitsMask = (byte) (0b11111111 >> zeroBits);
                    break;
                }
            }

            do
            {
                rng.GetBytes(bytes);

                // set most significant bits to 0 (because `value > max` if any of these bits is 1)
                bytes[bytes.Length - 1] &= zeroBitsMask;

                value = new BigInteger(bytes);

                // `value > max` 50% of the times, in which case the fastest way to keep the distribution uniform is to try again
            } while (value > max);

            return value;
        }

        private bool IsProbablePrime(RandomNumberGenerator rng, BigInteger source, int certainty)
        {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            // There is no built-in method for generating random BigInteger values.
            // Instead, random BigIntegers are constructed from randomly generated
            // byte arrays of the same length as the source.
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    // This may raise an exception in Mono 2.10.8 and earlier.
                    // http://bugzilla.xamarin.com/show_bug.cgi?id=2761
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= source - 2);

                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }

            return true;
        }

        private BigInteger GetRandomPrime(RandomNumberGenerator rng, Int64 bits)
        {
            BigInteger p = RandomInteger(rng, bits);
            while (!IsProbablePrime(rng, p, 100))
            {
                p = RandomInteger(rng, bits);
            }

            return p;
        }
    }
}
