using System;
using System.Security.Cryptography;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Crypto;

namespace Arrowgene.Ddon.Server.Network
{
    public class Challenge
    {
        private static readonly byte[] BlowFishKey = new byte[]
        {
            0x6D, 0x6F, 0x66, 0x75, 0x6D, 0x6F, 0x66, 0x75, 0x20, 0x63, 0x61, 0x70, 0x63, 0x6F, 0x6D, 0x28,
            0x5E, 0x2D, 0x5E, 0x29
        };

        private static readonly SHA1Managed Sha1 = new SHA1Managed();
        private static readonly BlowFish BlowFish = new BlowFish(BlowFishKey, true);

        private readonly RSACryptoServiceProvider _rsa;
        private readonly RSAParameters _rsaKeyInfo;

        public class Response
        {
            public bool Error;
            public byte[] BlowFishPassword;
            public byte[] EncryptedBlowFishPassword;
            public byte[] CamelliaKey;
            public byte DecryptedBlowFishKeyLength;
            public byte EncryptedBlowFishKeyLength;
        }

        public Challenge()
        {
            _rsa = new RSACryptoServiceProvider(2048);
            _rsaKeyInfo = _rsa.ExportParameters(true);
        }

        public byte[] CreateClientCertChallenge()
        {
            StreamBuffer buffer = new StreamBuffer();
            buffer.WriteBytes(_rsaKeyInfo.Modulus);
            buffer.WriteBytes(new byte[16 - _rsaKeyInfo.Exponent.Length]);
            buffer.WriteBytes(_rsaKeyInfo.Exponent);
            byte[] hashMaterial = buffer.GetAllBytes();
            byte[] calculatedHash = Sha1.ComputeHash(hashMaterial);
            buffer.WriteBytes(calculatedHash);
            buffer.WriteBytes(new byte[12]); // unknown
            byte[] certChallenge = buffer.GetAllBytes();
            return certChallenge;
        }

        public Response HandleClientCertChallenge(byte[] challengeResponse)
        {
            Response response = new Response();
            response.Error = false;
            try
            {
                IBuffer buffer = new StreamBuffer(challengeResponse);
                buffer.SetPositionStart();
                byte decryptedCamelliaKeyLength = buffer.ReadByte();
                byte[] rsaEncryptedCamelliaKey = buffer.ReadBytes(256);
                buffer.ReadBytes(3);
                response.DecryptedBlowFishKeyLength = buffer.ReadByte();
                response.EncryptedBlowFishKeyLength = buffer.ReadByte();
                response.EncryptedBlowFishPassword = buffer.ReadBytesTerminated(0); //62 length
                response.CamelliaKey = _rsa.Decrypt(rsaEncryptedCamelliaKey, RSAEncryptionPadding.Pkcs1);
                response.BlowFishPassword = BlowFish.Decrypt_ECB(response.EncryptedBlowFishPassword);
            }
            catch (Exception ex)
            {
                response.Error = true;
            }

            return response;
        }
    }
}
