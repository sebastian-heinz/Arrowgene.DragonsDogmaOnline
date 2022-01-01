using System;
using System.Numerics;
using System.Security.Cryptography;
using Arrowgene.Buffers;
using Buffer = Arrowgene.Buffers.Buffer;

namespace Arrowgene.Ddo.Shared.Crypto
{
    public class Handshake
    {
        private static byte[] InitialKey = new byte[]
        {
            0x66, 0x32, 0x33, 0x65, 0x39, 0x38, 0x48, 0x61, 0x66, 0x4A, 0x64, 0x53, 0x6F, 0x61, 0x6A, 0x38,
            0x30, 0x51, 0x42, 0x6A, 0x68, 0x68, 0x32, 0x33, 0x6F, 0x61, 0x6A, 0x67, 0x6B, 0x6C, 0x53, 0x61,
        };

        private static byte[] InitialPrev = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };


        private static byte[] BlowFishKey = new byte[]
        {
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66
        };


        private static SHA1Managed _sha1 = new SHA1Managed();
        private static Camellia _camellia = new Camellia();
        private static RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider(2048);
        private static BlowFish _blowFish = new BlowFish(new byte[16]);

        private byte[] _rsaModulus;
        private byte[] _rsaExponent;
        private byte[] _serverUnk;
        private RSAParameters _rsaKeyInfo;

        public Handshake()
        {
            _rsaKeyInfo = _rsa.ExportParameters(true);
            _rsaModulus = _rsaKeyInfo.Modulus;
            _rsaExponent = new byte[16];
            _serverUnk = new byte[12];
            byte[] exponent = _rsaKeyInfo.Exponent;
            System.Buffer.BlockCopy(exponent, 0, _rsaExponent, _rsaExponent.Length - exponent.Length, exponent.Length);
          //  System.Buffer.BlockCopy(exponent, 0, _rsaExponent, 0, exponent.Length);
        }

        private byte[] Copy(byte[] src)
        {
            int srcLen = src.Length;
            byte[] dst = new byte[srcLen];
            System.Buffer.BlockCopy(src, 0, dst, 0, srcLen);
            return dst;
        }

        private void Decrypt(byte[] keyData, byte[] iv, byte[] unknown, byte[] allBytes)
        {
            //   BigInteger cKey = new BigInteger(keyData);
            //   BigInteger cIv = new BigInteger(iv);
            //   BigInteger cUn = new BigInteger(unknown);
            // Console.WriteLine($"res:{Environment.NewLine}{Util.HexDump(res)}");


            BigInteger decrypted = BigInteger.ModPow(
                new BigInteger(allBytes.AsSpan(1,256), true, true),
                new BigInteger(_rsaKeyInfo.D, true, false),
                new BigInteger(_rsaKeyInfo.Modulus, true, false)
            );
            Console.WriteLine($"decrypted:{Environment.NewLine}{Util.HexDump(decrypted.ToByteArray())}");

            //   StreamBuffer b = new StreamBuffer();
            //    b.WriteBytes(keyData); 
            //  b.WriteByte(iv[0]);
            //   b.WriteByte(iv[1]);

              byte[] d = _rsa.Decrypt(allBytes.AsSpan(1,256).ToArray(), RSAEncryptionPadding.Pkcs1);
               Console.WriteLine($"d:{Environment.NewLine}{Util.HexDump(d)}");
        }

        public byte[] CreateClientCertChallenge()
        {
            StreamBuffer buffer = new StreamBuffer();
            buffer.WriteBytes(_rsaModulus);
            buffer.WriteBytes(_rsaExponent);
            byte[] hashMaterial = buffer.GetAllBytes();
            byte[] calculatedHash = _sha1.ComputeHash(hashMaterial);
            buffer.WriteBytes(calculatedHash);
            buffer.WriteBytes(_serverUnk);
            byte[] certChallenge = buffer.GetAllBytes();
            byte[] certChallengeEncrypted = new byte[certChallenge.Length];
            Console.WriteLine($"Created Cert Challenge:{Environment.NewLine}{Util.HexDump(certChallenge)}");
            // InitialPrev will be modified - need to be preserved for decryption
            _camellia.Encrypt(certChallenge, certChallengeEncrypted, InitialKey, Copy(InitialPrev));
            return certChallengeEncrypted;
        }

        public byte[] ValidateClientCertChallenge(byte[] challengeResponse)
        {
            byte[] decrypted = new byte[challengeResponse.Length];
            Camellia camellia = new Camellia();
            camellia.Decrypt(challengeResponse, decrypted, InitialKey, Copy(InitialPrev));
            IBuffer decBuffer = new StreamBuffer(decrypted);
            decBuffer.SetPositionStart();
            byte[] keyData = decBuffer.ReadBytes(256);
            byte[] iv = decBuffer.ReadBytes(16);
            byte[] unk = decBuffer.ReadBytes(64);
            Console.WriteLine($"Received CertChallenge Response:{Environment.NewLine}" +
                              $"Key:{Environment.NewLine}{Util.HexDump(keyData)}" +
                              $"Iv:{Environment.NewLine}{Util.HexDump(iv)}" +
                              $"Unk:{Environment.NewLine}{Util.HexDump(unk)}"
            );

            Decrypt(keyData, iv, unk, decBuffer.GetAllBytes());

            string hexResponse1 =
                "3b440b4e0e65f4d73322e9f37c0d73ad" +
                "b4b72750bc9e7a45d14bf59e1031576f" +
                "db9dce65b0ce1743c69ce4a1dafd8eb5" +
                "175f0ec9372ed50d7b59f68ce1b87a13" +
                "d4472c8478240b3c37dd2229d254337b" +
                "ede3f8f1f60a0d263634a994b6abbd97";

            byte[] response1 = Util.FromHexString(hexResponse1);
            return response1;
        }
    }
}
