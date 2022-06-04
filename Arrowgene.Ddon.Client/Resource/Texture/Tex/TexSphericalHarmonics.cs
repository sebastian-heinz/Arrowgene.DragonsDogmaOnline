using System;
using System.IO;

namespace Arrowgene.Ddon.Client.Resource.Texture.Tex;

public struct TexSphericalHarmonics
{
    public const int Size = 0x6C;

    public bool Loaded;
    public TexSphericalHarmonicsVector Y00;
    public TexSphericalHarmonicsVector Y11;
    public TexSphericalHarmonicsVector Y10;
    public TexSphericalHarmonicsVector Y1_1;
    public TexSphericalHarmonicsVector Y21;
    public TexSphericalHarmonicsVector Y2_1;
    public TexSphericalHarmonicsVector Y2_2;
    public TexSphericalHarmonicsVector Y20;
    public TexSphericalHarmonicsVector Y22;

    public void Decode(byte[] bytes)
    {
        Y00.R = BitConverter.ToSingle(bytes, 4 * 0);
        Y00.G = BitConverter.ToSingle(bytes, 4 * 1);
        Y00.B = BitConverter.ToSingle(bytes, 4 * 2);
        Y11.R = BitConverter.ToSingle(bytes, 4 * 3);
        Y11.G = BitConverter.ToSingle(bytes, 4 * 4);
        Y11.B = BitConverter.ToSingle(bytes, 4 * 5);
        Y10.R = BitConverter.ToSingle(bytes, 4 * 6);
        Y10.G = BitConverter.ToSingle(bytes, 4 * 7);
        Y10.B = BitConverter.ToSingle(bytes, 4 * 8);
        Y1_1.R = BitConverter.ToSingle(bytes, 4 * 9);
        Y1_1.G = BitConverter.ToSingle(bytes, 4 * 10);
        Y1_1.B = BitConverter.ToSingle(bytes, 4 * 11);
        Y21.R = BitConverter.ToSingle(bytes, 4 * 12);
        Y21.G = BitConverter.ToSingle(bytes, 4 * 13);
        Y21.B = BitConverter.ToSingle(bytes, 4 * 14);
        Y2_1.R = BitConverter.ToSingle(bytes, 4 * 15);
        Y2_1.G = BitConverter.ToSingle(bytes, 4 * 16);
        Y2_1.B = BitConverter.ToSingle(bytes, 4 * 17);
        Y2_2.R = BitConverter.ToSingle(bytes, 4 * 18);
        Y2_2.G = BitConverter.ToSingle(bytes, 4 * 19);
        Y2_2.B = BitConverter.ToSingle(bytes, 4 * 20);
        Y20.R = BitConverter.ToSingle(bytes, 4 * 21);
        Y20.G = BitConverter.ToSingle(bytes, 4 * 22);
        Y20.B = BitConverter.ToSingle(bytes, 4 * 23);
        Y22.R = BitConverter.ToSingle(bytes, 4 * 24);
        Y22.G = BitConverter.ToSingle(bytes, 4 * 25);
        Y22.B = BitConverter.ToSingle(bytes, 4 * 26);
        Loaded = true;
    }

    public byte[] Encode()
    {
        using MemoryStream m = new MemoryStream();
        using BinaryWriter w = new BinaryWriter(m);
        w.Write(Y00.R);
        w.Write(Y00.G);
        w.Write(Y00.B);
        w.Write(Y11.R);
        w.Write(Y11.G);
        w.Write(Y11.B);
        w.Write(Y10.R);
        w.Write(Y10.G);
        w.Write(Y10.B);
        w.Write(Y1_1.R);
        w.Write(Y1_1.G);
        w.Write(Y1_1.B);
        w.Write(Y21.R);
        w.Write(Y21.G);
        w.Write(Y21.B);
        w.Write(Y2_1.R);
        w.Write(Y2_1.G);
        w.Write(Y2_1.B);
        w.Write(Y2_2.R);
        w.Write(Y2_2.G);
        w.Write(Y2_2.B);
        w.Write(Y20.R);
        w.Write(Y20.G);
        w.Write(Y20.B);
        w.Write(Y22.R);
        w.Write(Y22.G);
        w.Write(Y22.B);
        return m.ToArray();
    }
}
