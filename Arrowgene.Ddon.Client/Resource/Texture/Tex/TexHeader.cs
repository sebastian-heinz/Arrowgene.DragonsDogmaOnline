using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Tex;

public struct TexHeader
{
    public const int Size = 12;

    public uint _header4;
    public uint _header8;
    public uint _header12;

    public TexHeaderVersion Version;
    public uint Height;
    public uint Width;
    public uint Shift;
    public uint Alpha;
    public uint Depth;
    public TexPixelFormat PixelFormat;
    public byte TextureArraySize;
    public uint MipMapCount;
    public TexType Type;
    public uint UnknownB;
    public bool HasSphericalHarmonicsFactor;

    public uint LayerCount => TextureArraySize * MipMapCount;

    public void Decode(byte[] bytes)
    {
        _header4 = BitConverter.ToUInt32(bytes, 0);
        _header8 = BitConverter.ToUInt32(bytes, 4);
        _header12 = BitConverter.ToUInt32(bytes, 8);

        HasSphericalHarmonicsFactor = (_header4 & 0xF0000000) == 0x60000000;

        _parseHeader4(_header4, out var versionBits16__0_15, out var alphaBits8__16_23, out var shiftBits4__24_27, out var typeBits4__28_31);
        _parseHeader8(_header8, out var mipMapCountBits6_0__5, out var widthBits13_6__18, out var heightBits13_19__31);
        _parseHeader12(_header12, out var textureArraySizeBits8__0_7, out var pixelFormatBits8__8_15, out var depthBits13__16_28, out var unkBits3__29_31);

        Version = (TexHeaderVersion)versionBits16__0_15;
        Alpha = alphaBits8__16_23;
        Shift = shiftBits4__24_27;
        Type = (TexType)typeBits4__28_31;
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
            | ((uint) Type << 28);

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

    public static void _parseHeader4(uint header4, out TexHeaderVersion version, out uint alpha, out uint shift, out TexType type)
    {
        version = (TexHeaderVersion) (header4 & ((1 << 16) - 1));
        alpha = (header4 >> 16) & ((1 << 8) - 1);
        shift = (header4 >> 24) & ((1 << 4) - 1);
        type = (TexType) ((header4 >> 28) & ((1 << 4) - 1)); // switchNum 1,2|3|6
    }

    public static void _parseHeader8(uint header8, out uint mipMapCount, out uint width, out uint height)
    {
        mipMapCount = header8 & ((1 << 6) - 1);
        width = (header8 >> 6) & ((1 << 13) - 1);
        height = (header8 >> 19) & ((1 << 13) - 1);
    }

    public static void _parseHeader12(uint header12, out uint textureArraySize, out TexPixelFormat pixelFormat, out uint depth, out uint unknownB)
    {
        textureArraySize = header12 & ((1 << 8) - 1);
        pixelFormat = (TexPixelFormat) ((header12 >> 8) & ((1 << 8) - 1));
        depth = (header12 >> 16) & ((1 << 13) - 1);
        unknownB = (header12 >> 29) & ((1 << 3) - 1);
    }
}
