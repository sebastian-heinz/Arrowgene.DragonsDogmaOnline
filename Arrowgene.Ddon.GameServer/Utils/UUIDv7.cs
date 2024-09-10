using System;
using System.Security.Cryptography;

public class UUIDv7
{
    private static readonly RandomNumberGenerator random = RandomNumberGenerator.Create();

    public static byte[] Generate()
    {
        // random bytes
        byte[] value = new byte[16];
        random.GetBytes(value);

        // current timestamp in ms
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // timestamp
        value[0] = (byte)((timestamp >> 40) & 0xFF);
        value[1] = (byte)((timestamp >> 32) & 0xFF);
        value[2] = (byte)((timestamp >> 24) & 0xFF);
        value[3] = (byte)((timestamp >> 16) & 0xFF);
        value[4] = (byte)((timestamp >> 8) & 0xFF);
        value[5] = (byte)(timestamp & 0xFF);

        // version and variant
        value[6] = (byte)((value[6] & 0x0F) | 0x70);
        value[8] = (byte)((value[8] & 0x3F) | 0x80);

        return value;
    }
}
