using System;
using System.IO;
using System.Reflection;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddo.Shared.Crypto;

namespace Arrowgene.Ddo.Shared
{
    public static class Util
    {
        public static IBufferProvider Buffer = new StreamBuffer();
        public static readonly CryptoRandom CryptoRandom = new CryptoRandom();

        private static readonly Random Random = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (Random)
            {
                return Random.Next(min, max);
            }
        }

        public static long GetUnixTime(DateTime dateTime)
        {
            return ((DateTimeOffset) dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        ///     The directory of the executing assembly.
        ///     This might not be the location where the .dll files are located.
        /// </summary>
        public static string ExecutingDirectory()
        {
            var path = Assembly.GetEntryAssembly().CodeBase;
            var uri = new Uri(path);
            var directory = Path.GetDirectoryName(uri.LocalPath);
            return directory;
        }

        public static byte[] FromHexString(string hexString)
        {
            if ((hexString.Length & 1) != 0)
            {
                throw new ArgumentException("Input must have even number of characters");
            }

            byte[] ret = new byte[hexString.Length / 2];
            for (int i = 0; i < ret.Length; i++)
            {
                int high = hexString[i * 2];
                int low = hexString[i * 2 + 1];
                high = (high & 0xf) + ((high & 0x40) >> 6) * 9;
                low = (low & 0xf) + ((low & 0x40) >> 6) * 9;

                ret[i] = (byte) ((high << 4) | low);
            }

            return ret;
        }

        public static string ToHexString(byte[] data, char? seperator = null)
        {
            StringBuilder sb = new StringBuilder();
            int len = data.Length;
            for (int i = 0; i < len; i++)
            {
                sb.Append(data[i].ToString("X2"));
                if (seperator != null && i < len - 1)
                {
                    sb.Append(seperator);
                }
            }

            return sb.ToString();
        }

        public static string ToAsciiString(byte[] data, bool spaced)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                char c = '.';
                if (data[i] >= 'A' && data[i] <= 'Z') c = (char) data[i];
                if (data[i] >= 'a' && data[i] <= 'z') c = (char) data[i];
                if (data[i] >= '0' && data[i] <= '9') c = (char) data[i];
                if (spaced && i != 0)
                {
                    sb.Append("  ");
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        public static void DumpBuffer(IBuffer buffer)
        {
            int pos = buffer.Position;
            buffer.SetPositionStart();
            Console.WriteLine(ToAsciiString(buffer.GetAllBytes(), true));
            while (buffer.Size > buffer.Position)
            {
                byte[] row = buffer.ReadBytes(16);
                Console.WriteLine(ToHexString(row, ' '));
            }

            buffer.Position = pos;
        }
    }
}
