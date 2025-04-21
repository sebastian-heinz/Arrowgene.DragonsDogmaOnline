using System;
using System.Security.Cryptography;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.Server.Network
{
    public class Challenge
    {
        private static readonly byte[] BlowFishKey = new byte[]
        {
            0x6D, 0x6F, 0x66, 0x75, 0x6D, 0x6F, 0x66, 0x75, 0x20, 0x63, 0x61, 0x70, 0x63, 0x6F, 0x6D, 0x28,
            0x5E, 0x2D, 0x5E, 0x29
        };

        private static readonly SHA1 Sha1 = SHA1.Create();
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

        public Response HandleClientCertChallenge(C2SCertClientChallengeReq request)
        {
            Response response = new Response();
            response.Error = false;
            try
            {
                byte decryptedCamelliaKeyLength = request.CommonKeySrcSize;
                byte[] rsaEncryptedCamelliaKey = request.CommonKeyEnc;
                response.DecryptedBlowFishKeyLength = request.PasswordSrcSize;
                response.EncryptedBlowFishKeyLength = request.PasswordEncSize;
                response.EncryptedBlowFishPassword = request.PasswordEnc;
                response.CamelliaKey = _rsa.Decrypt(rsaEncryptedCamelliaKey, RSAEncryptionPadding.Pkcs1);
                response.BlowFishPassword = BlowFish.Decrypt_ECB(response.EncryptedBlowFishPassword);
            }
            catch (Exception)
            {
                response.Error = true;
            }

            return response;
        }

        public Response HandleClientCertChallenge(C2LClientChallengeReq request)
        {
            Response response = new Response();
            response.Error = false;
            try
            {
                byte decryptedCamelliaKeyLength = request.CommonKeySrcSize;
                byte[] rsaEncryptedCamelliaKey = request.CommonKeyEnc;
                response.DecryptedBlowFishKeyLength = request.PasswordSrcSize;
                response.EncryptedBlowFishKeyLength = request.PasswordEncSize;
                response.EncryptedBlowFishPassword = request.PasswordEnc;
                response.CamelliaKey = _rsa.Decrypt(rsaEncryptedCamelliaKey, RSAEncryptionPadding.Pkcs1);
                response.BlowFishPassword = BlowFish.Decrypt_ECB(response.EncryptedBlowFishPassword);
            }
            catch (Exception)
            {
                response.Error = true;
            }

            return response;
        }
    }
}
