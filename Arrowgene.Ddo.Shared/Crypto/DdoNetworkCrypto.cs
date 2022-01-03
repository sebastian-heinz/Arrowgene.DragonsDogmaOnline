using System;
using System.Security.Cryptography;
using Arrowgene.Buffers;
using Buffer = System.Buffer;

namespace Arrowgene.Ddo.Shared.Crypto
{
    public class DdoNetworkCrypto
    {
        private static readonly byte[] CamelliaKey = new byte[]
        {
            0x66, 0x32, 0x33, 0x65, 0x39, 0x38, 0x48, 0x61, 0x66, 0x4A, 0x64, 0x53, 0x6F, 0x61, 0x6A, 0x38,
            0x30, 0x51, 0x42, 0x6A, 0x68, 0x68, 0x32, 0x33, 0x6F, 0x61, 0x6A, 0x67, 0x6B, 0x6C, 0x53, 0x61,
        };

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        private static readonly byte[] BlowFishKey = new byte[]
        {
            0x6D, 0x6F, 0x66, 0x75, 0x6D, 0x6F, 0x66, 0x75, 0x20, 0x63, 0x61, 0x70, 0x63, 0x6F, 0x6D, 0x28,
            0x5E, 0x2D, 0x5E, 0x29
        };

        private static readonly SHA1Managed _sha1 = new SHA1Managed();
        private static readonly Camellia _camellia = new Camellia(); 
        private static readonly BlowFish _blowFish = new BlowFish(BlowFishKey, true);
        
        private readonly RSACryptoServiceProvider _rsa;
        private readonly RSAParameters _rsaKeyInfo;
        private byte[] _camelliaKey;
        private byte[] _blowFishPassword;
        private byte[] _encryptedBlowFishPassword;

        public DdoNetworkCrypto()
        {
            _rsa = new RSACryptoServiceProvider(2048);
            _rsaKeyInfo = _rsa.ExportParameters(true);
            _camelliaKey = CamelliaKey;
        }

        private byte[] Copy(byte[] src)
        {
            int srcLen = src.Length;
            byte[] dst = new byte[srcLen];
            Buffer.BlockCopy(src, 0, dst, 0, srcLen);
            return dst;
        }

        public byte[] Encrypt(byte[] data)
        {
            _camellia.Encrypt(data, out Span<byte> encrypted, _camelliaKey, Copy(CamelliaIv));
            return encrypted.ToArray();
        }

        public byte[] Decrypt(byte[] encrypted)
        {
            _camellia.Decrypt(encrypted, out Span<byte> decrypted, _camelliaKey, Copy(CamelliaIv));
            return decrypted.ToArray();
        }
        
        public byte[] EncryptBlowFish(byte[] data)
        {
            return _blowFish.Encrypt_ECB(data);
        }

        public byte[] GetEncryptedBlowFishPassword()
        {
            return _encryptedBlowFishPassword;
        }
        
        public byte[] DecryptBlowFish(byte[] encrypted)
        {
            return _blowFish.Decrypt_ECB(encrypted);
        }
        
        public byte[] CreateClientCertChallenge()
        {
            StreamBuffer buffer = new StreamBuffer();
            buffer.WriteBytes(_rsaKeyInfo.Modulus);
            buffer.WriteBytes(new byte[16 - _rsaKeyInfo.Exponent.Length]);
            buffer.WriteBytes(_rsaKeyInfo.Exponent);
            byte[] hashMaterial = buffer.GetAllBytes();
            byte[] calculatedHash = _sha1.ComputeHash(hashMaterial);
            buffer.WriteBytes(calculatedHash);
            buffer.WriteBytes(new byte[12]); // unknown
            byte[] certChallenge = buffer.GetAllBytes();
            Console.WriteLine($"Created Cert Challenge:{Environment.NewLine}{Util.HexDump(certChallenge)}");
            // InitialPrev will be modified - need to be preserved for decryption
            _camellia.Encrypt(certChallenge, out Span<byte> certChallengeEncrypted, _camelliaKey, Copy(CamelliaIv));
            return certChallengeEncrypted.ToArray();
        }

        public bool HandleClientCertChallenge(byte[] challengeResponse)
        {
            _camellia.Decrypt(challengeResponse, out Span<byte> decrypted, _camelliaKey, Copy(CamelliaIv));
            IBuffer decBuffer = new StreamBuffer(decrypted.ToArray());
            decBuffer.SetPositionStart();
            byte decryptedCamelliaKeyLength = decBuffer.ReadByte();
            byte[] rsaEncryptedCamelliaKey = decBuffer.ReadBytes(256);
            decBuffer.ReadBytes(3);
            byte decryptedBlowFishKeyLength = decBuffer.ReadByte();
            byte encryptedBlowFishKeyLength = decBuffer.ReadByte();
            _encryptedBlowFishPassword = decBuffer.ReadBytesTerminated(0); //62 length

            Console.WriteLine($"Received CertChallenge Response:{Environment.NewLine}" +
                              $"decryptedCamelliaKeyLength:{decryptedCamelliaKeyLength}{Environment.NewLine}" +
                              $"rsaEncryptedCamelliaKey:{Environment.NewLine}{Util.HexDump(rsaEncryptedCamelliaKey)}" +
                              $"decryptedBlowFishKeyLength:{decryptedBlowFishKeyLength}{Environment.NewLine}" +
                              $"encryptedBlowFishKeyLength:{encryptedBlowFishKeyLength}{Environment.NewLine}" +
                              $"_encryptedBlowFishPassword:{Environment.NewLine}{Util.HexDump(_encryptedBlowFishPassword)}"
            );

            _camelliaKey = _rsa.Decrypt(rsaEncryptedCamelliaKey, RSAEncryptionPadding.Pkcs1);
            Console.WriteLine($"_camelliaKey:{Environment.NewLine}{Util.HexDump(_camelliaKey)}");

            _blowFishPassword = _blowFish.Decrypt_ECB(_encryptedBlowFishPassword);
            Console.WriteLine($"_blowFishPassword:{Environment.NewLine}{Util.HexDump(_blowFishPassword)}");
            
            return true;
        }
    }
}
