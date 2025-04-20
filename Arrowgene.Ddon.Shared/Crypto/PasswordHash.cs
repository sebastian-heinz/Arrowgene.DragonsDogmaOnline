using System;
using System.Security.Cryptography;

namespace Arrowgene.Ddon.Shared.Crypto
{
    public static class PasswordHash
    {
        private const int SaltSize = 24;
        private const int HashSize = 24;
        private const int Iterations = 100000;

        private static readonly RandomNumberGenerator Provider = RandomNumberGenerator.Create();

        public static string CreateHash(string password)
        {
            Span<byte> container = new byte[SaltSize + HashSize];
            Span<byte> salt = container.Slice(0, SaltSize);
            Provider.GetBytes(salt);
            byte[] hash = HashWithSalt(password, salt);
            hash.CopyTo(container.Slice(SaltSize, HashSize));
            return Convert.ToBase64String(container);
        }

        public static bool Verify(string password, string expectedHash)
        {
            Span<byte> container = Convert.FromBase64String(expectedHash);
            Span<byte> salt = container.Slice(0, SaltSize);
            Span<byte> hashBytes = container.Slice(SaltSize, HashSize);
            byte[] expectedHashBytes = HashWithSalt(password, salt);
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i] != expectedHashBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static byte[] HashWithSalt(string input, Span<byte> salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt.ToArray(), Iterations, HashAlgorithmName.SHA1);
            return pbkdf2.GetBytes(HashSize);
        }
    }
}
