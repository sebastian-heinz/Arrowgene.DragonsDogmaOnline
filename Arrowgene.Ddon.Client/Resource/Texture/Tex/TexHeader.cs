using System;
using System.Text;

namespace Arrowgene.Ddon.Client.Resource.Texture.Tex;

public struct TexHeader
{
    public const int Size = 12;

    public TexHeaderVersion Version;
    public uint Height;
    public uint Width;
    public uint Shift;
    public uint Alpha;
    public uint Depth;
    public TexPixelFormat PixelFormat;
    public byte TextureArraySize;
    public uint MipMapCount;
    public uint UnknownA; // (1,2),(3),(6) DDON
    public uint UnknownB;
    public bool HasSphericalHarmonicsFactor;

    public uint LayerCount => TextureArraySize * MipMapCount;
    
    public string GetMetadata()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Version:{Version}");
        sb.AppendLine($"Height:{Height}");
        sb.AppendLine($"Width:{Width}");
        sb.AppendLine($"Shift:{Shift}");
        sb.AppendLine($"Alpha:{Alpha}");
        sb.AppendLine($"Depth:{Depth}");
        sb.AppendLine($"PixelFormat:{PixelFormat}");
        sb.AppendLine($"TextureArraySize:{TextureArraySize}");
        sb.AppendLine($"MipMapCount:{MipMapCount}");
        sb.AppendLine($"UnknownA:{UnknownA}");
        sb.AppendLine($"UnknownB:{UnknownB}");
        sb.AppendLine($"HasSphericalHarmonicsFactor:{HasSphericalHarmonicsFactor}");
        return sb.ToString();
    }
    
    public void Decode(byte[] bytes)
    {
        uint header4 = BitConverter.ToUInt32(bytes, 0);
        uint header8 = BitConverter.ToUInt32(bytes, 4);
        uint header12 = BitConverter.ToUInt32(bytes, 8);

        HasSphericalHarmonicsFactor = (header4 & 0xF0000000) == 0x60000000;

        uint versionBits12__0_11 = header4 & ((1 << 12) - 1);
        uint alphaBits12__12_23 = (header4 >> 12) & ((1 << 12) - 1);
        uint shiftBits4__24_27 = (header4 >> 24) & ((1 << 4) - 1);
        uint unkBits4__28_31 = (header4 >> 28) & ((1 << 4) - 1); // switchNum 1,2|3|6

        uint mipMapCountBits6_0__5 = header8 & ((1 << 6) - 1);
        uint widthBits13_6__18 = (header8 >> 6) & ((1 << 13) - 1);
        uint heightBits13_19__31 = (header8 >> 19) & ((1 << 13) - 1);

        uint textureArraySizeBits8__0_7 = header12 & ((1 << 8) - 1);
        uint pixelFormatBits8__8_15 = (header12 >> 8) & ((1 << 8) - 1);
        uint depthBits13__16_28 = (header12 >> 16) & ((1 << 13) - 1);
        uint unkBits3__29_31 = (header12 >> 29) & ((1 << 3) - 1);

        Version = (TexHeaderVersion)versionBits12__0_11;
        Alpha = alphaBits12__12_23;
        Shift = shiftBits4__24_27;
        UnknownA = unkBits4__28_31;
        MipMapCount = mipMapCountBits6_0__5;
        Width = widthBits13_6__18 << (byte) shiftBits4__24_27;
        Height = heightBits13_19__31 << (byte) shiftBits4__24_27;
        TextureArraySize = (byte) textureArraySizeBits8__0_7;
        PixelFormat = (TexPixelFormat) pixelFormatBits8__8_15;
        Depth = depthBits13__16_28 << (byte) shiftBits4__24_27;
        UnknownB = unkBits3__29_31;
    }

    public byte[] Encode()
    {
        uint header4 =
            (uint) Version
            | ((uint) Alpha << 12)
            | ((uint) Shift << 24)
            | ((uint) UnknownA << 28);

        if (HasSphericalHarmonicsFactor)
        {
            header4 = (header4 | 0x60000000);
        }

        uint header8 =
            (uint) MipMapCount
            | ((uint) Width << 6)
            | ((uint) Height << 19);

        uint header12 =
            (uint) TextureArraySize
            | ((uint) PixelFormat << 8)
            | ((uint) Depth << 16)
            | ((uint) UnknownB << 29);
        byte[] bytes4 = BitConverter.GetBytes(header4);
        byte[] bytes8 = BitConverter.GetBytes(header8);
        byte[] bytes12 = BitConverter.GetBytes(header12);
        byte[] result = new byte[12];
        result[0] = bytes4[0];
        result[1] = bytes4[1];
        result[2] = bytes4[2];
        result[3] = bytes4[3];
        result[4] = bytes8[0];
        result[5] = bytes8[1];
        result[6] = bytes8[2];
        result[7] = bytes8[3];
        result[8] = bytes12[0];
        result[9] = bytes12[1];
        result[10] = bytes12[2];
        result[11] = bytes12[3];
        return result;
    }
}
