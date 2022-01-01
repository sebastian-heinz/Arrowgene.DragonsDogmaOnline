using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Arrowgene.Ddo.Shared.Crypto
{
    public class DdoRsa
    {
        private static RandomNumberGenerator rng = new RNGCryptoServiceProvider();

        public struct PrivateKey
        {
            public PublicKey PublicKey;
            public BigInteger Q;
            public BigInteger P;
            public BigInteger D;
            public BigInteger Phi;
        }

        public struct PublicKey
        {
            public BigInteger N;
            public BigInteger E;
        }
        
        
        public static int e = 0x10001;
        
        public PrivateKey GenerateKey(int bits)
        {
            BigInteger p = GetRandomPrime(rng, bits / 2);
            BigInteger q = GetRandomPrime(rng, bits / 2);
            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);
            BigInteger d = ModInverse(e, phi);
            PrivateKey privateKey = new PrivateKey()
            {
                PublicKey = new PublicKey
                {
                    E = e,
                    N = n
                },
                Q = q,
                P = p,
                Phi = phi,
                D = d,
            };
            return privateKey;
        }

        public byte[] Encrypt(PublicKey publicKey, byte[] plainText)
        {
            if (1>plainText.Length || plainText.Length>=publicKey.N.ToByteArray().Length)
            {
                return null;
                
            }

            //Padding the array to unsign.
            byte[] bytes_padded = new byte[plainText.Length+2];
            Array.Copy(plainText, bytes_padded, plainText.Length);
            bytes_padded[bytes_padded.Length-1] = 0x00;
            
            //Setting high byte right before the data, to prevent data loss.
            bytes_padded[bytes_padded.Length-2] = 0xFF;

            //Computing as a BigInteger the encryption operation.
            var cipher_bigint = new BigInteger();
            var padded_bigint = new BigInteger(bytes_padded);
            cipher_bigint = BigInteger.ModPow(padded_bigint, publicKey.E, publicKey.N);

            //Returning the byte array of encrypted bytes.
            return cipher_bigint.ToByteArray();
        }

        public byte[] 
           Decrypt(PrivateKey privateKey, byte[] cipherText)
        {

            //Decrypting.
            var plain_bigint = new BigInteger();
            var padded_bigint = new BigInteger(cipherText);
            plain_bigint = BigInteger.ModPow(padded_bigint, privateKey.D, privateKey.PublicKey.N);

            //Removing all padding bytes, including the marker 0xFF.
            byte[] plain_bytes = plain_bigint.ToByteArray();
            int lengthToCopy=-1;
            for (int i=plain_bytes.Length-1; i>=0; i--) 
            {
                if (plain_bytes[i]==0xFF)
                {
                    lengthToCopy = i;
                    break;
                }
            }

            //Checking for a failure to find marker byte.
            if (lengthToCopy==-1)
            {
                throw new Exception("Marker byte for padding (0xFF) not found in plain bytes.\nPossible Reasons:\n1: PAYLOAD TOO LARGE\n2: KEYS INVALID\n3: ENCRYPT/DECRYPT FUNCTIONS INVALID");
            }

            //Copying into return array, returning.
            byte[] return_array = new byte[lengthToCopy];
            Array.Copy(plain_bytes, return_array, lengthToCopy);
            return return_array;
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
