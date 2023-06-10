using System;
using System.Collections;
using System.Text;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;
using Xunit;

namespace Arrowgene.Ddon.Test.Shared.Crypto
{
    public class CamelliaTest
    {
        [Fact]
        public void TestCamellia()
        {
            Camellia c = new Camellia();
            byte[] input = new byte[1024];
            byte[] key = Encoding.UTF8.GetBytes("0123456789012345");
            byte[] iv = Encoding.UTF8.GetBytes("5432109876543210");
            c.Encrypt(input.AsSpan().ToArray(), out Span<byte> encrypted, key, iv);
            iv = Encoding.UTF8.GetBytes("5432109876543210");
            c.Decrypt(encrypted, out Span<byte> decrypted, key, iv);

            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(input, decrypted.ToArray()));
        }

        [Fact]
        public void TestCamelliaSubKey()
        {
            Camellia c = new Camellia();
            byte[] input = new byte[1024];
            byte[] key = Encoding.UTF8.GetBytes("0123456789012345");
            byte[] iv = Encoding.UTF8.GetBytes("5432109876543210");
            uint keyLength = c.GetKeyLength(key);
            Span<byte> t8 = new byte[8];
            c.KeySchedule(key, out Memory<byte> subKey, t8);
            c.Encrypt(Util.Copy(input), out Span<byte> encrypted, keyLength, subKey.Span, iv, t8);
            iv = Encoding.UTF8.GetBytes("5432109876543210");
            c.Decrypt(encrypted, out Span<byte> decrypted, keyLength, subKey.Span, iv, t8);
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(input, decrypted.ToArray()));
        }
    }
}
