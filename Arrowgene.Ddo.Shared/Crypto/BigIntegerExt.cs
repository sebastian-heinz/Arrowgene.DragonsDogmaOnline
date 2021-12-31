using System.Numerics;

namespace Arrowgene.Ddo.Shared.Crypto
{
    public static class BigIntegerExt
    {
        private static BigInteger _0xFF = new BigInteger(0xFF);
        private static BigInteger _0xFFFFFFFF = new BigInteger(0xFFFFFFFF);

        public static BigInteger SetByte(this BigInteger self, int index, byte value)
        {
            return (self & ~(_0xFF << (index * 8))) | (new BigInteger(value) << (index * 8));
        }

        public static BigInteger SetInteger(this BigInteger self, int byteOffset, uint value)
        {
            return (self & ~(_0xFFFFFFFF << (byteOffset * 8))) | (new BigInteger(value) << (byteOffset * 8));
        }

        public static BigInteger DdoSetInteger(this BigInteger self, uint value)
        {
            return self.SetInteger(0, value).SetInteger(4, 0);
        }
    }
}
