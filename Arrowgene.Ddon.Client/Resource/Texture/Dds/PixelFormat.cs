using System;
using System.Runtime.InteropServices;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

/// <summary>
/// https://github.com/microsoft/DirectXTex/blob/main/DirectXTex/DDS.h
/// </summary>
public struct DDSPixelFormat
{
    public static readonly uint StructSize = (uint) Marshal.SizeOf<DDSPixelFormat>();

    public static uint MakeFourCc(char char1, char char2, char char3, char char4)
    {
        return Convert.ToByte(char1) | (uint) Convert.ToByte(char2) << 8 | (uint) Convert.ToByte(char3) << 16 |
               (uint) Convert.ToByte(char4) << 24;
    }

    public static string FormatFourCc(uint fourCc)
    {
        return $"{(char) (fourCc & 0xFF)}{(char) ((fourCc >> 8) & 0xFF)}{(char) ((fourCc >> 16) & 0xFF)}{(char) ((fourCc >> 24) & 0xFF)}";
    }

    public uint Size;
    public DdsPixelFormatFlag Flags;
    public uint FourCc;
    public uint RgbBitCount;
    public uint RBitMask;
    public uint GBitMask;
    public uint BBitMask;
    public uint ABitMask;

    public DDSPixelFormat(uint size, DdsPixelFormatFlag flags, uint fourCc, uint rgbBitCount, uint rBitMask,
        uint gBitMask, uint bBitMask, uint aBitMask)
    {
        Size = size;
        Flags = flags;
        FourCc = fourCc;
        RgbBitCount = rgbBitCount;
        RBitMask = rBitMask;
        GBitMask = gBitMask;
        BBitMask = bBitMask;
        ABitMask = aBitMask;
    }

    public static DDSPixelFormat DDSPF_DXT1 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('D', 'X', 'T', '1'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_DXT2 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('D', 'X', 'T', '2'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_DXT3 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('D', 'X', 'T', '3'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_DXT4 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('D', 'X', 'T', '4'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_DXT5 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('D', 'X', 'T', '5'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_BC4_UNORM =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('B', 'C', '4', 'U'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_BC4_SNORM =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('B', 'C', '4', 'S'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_BC5_UNORM =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('B', 'C', '5', 'U'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_BC5_SNORM =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('B', 'C', '5', 'S'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_R8G8_B8G8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('R', 'G', 'B', 'G'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_G8R8_G8B8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('G', 'R', 'G', 'B'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_YUY2 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('Y', 'U', 'Y', '2'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_UYVY =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('U', 'Y', 'V', 'Y'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DDSPF_A8R8G8B8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 32, 0x00ff0000, 0x0000ff00, 0x000000ff,
            0xff000000);

    public static DDSPixelFormat DDSPF_X8R8G8B8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0);

    public static DDSPixelFormat DDSPF_A8B8G8R8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 32, 0x000000ff, 0x0000ff00, 0x00ff0000,
            0xff000000);

    public static DDSPixelFormat DDSPF_X8B8G8R8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 32, 0x000000ff, 0x0000ff00, 0x00ff0000, 0);

    public static DDSPixelFormat DDSPF_G16R16 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 32, 0x0000ffff, 0xffff0000, 0, 0);

    public static DDSPixelFormat DDSPF_R5G6B5 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 16, 0xf800, 0x07e0, 0x001f, 0);

    public static DDSPixelFormat DDSPF_A1R5G5B5 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 16, 0x7c00, 0x03e0, 0x001f, 0x8000);

    public static DDSPixelFormat DDSPF_X1R5G5B5 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 16, 0x7c00, 0x03e0, 0x001f, 0);

    public static DDSPixelFormat DDSPF_A4R4G4B4 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 16, 0x0f00, 0x00f0, 0x000f, 0xf000);

    public static DDSPixelFormat DDSPF_X4R4G4B4 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 16, 0x0f00, 0x00f0, 0x000f, 0);

    public static DDSPixelFormat DDSPF_R8G8B8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 24, 0xff0000, 0x00ff00, 0x0000ff, 0);

    public static DDSPixelFormat DDSPF_A8R3G3B2 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 16, 0x00e0, 0x001c, 0x0003, 0xff00);

    public static DDSPixelFormat DDSPF_R3G3B2 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 8, 0xe0, 0x1c, 0x03, 0);

    public static DDSPixelFormat DDSPF_A4L4 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_LUMINANCEA, 0, 8, 0x0f, 0, 0, 0xf0);

    public static DDSPixelFormat DDSPF_L8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_LUMINANCE, 0, 8, 0xff, 0, 0, 0);

    public static DDSPixelFormat DDSPF_L16 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_LUMINANCE, 0, 16, 0xffff, 0, 0, 0);

    public static DDSPixelFormat DDSPF_A8L8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_LUMINANCEA, 0, 16, 0x00ff, 0, 0, 0xff00);

    public static DDSPixelFormat DDSPF_A8L8_ALT =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_LUMINANCEA, 0, 8, 0x00ff, 0, 0, 0xff00);

    public static DDSPixelFormat DDSPF_L8_NVTT1 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 8, 0xff, 0, 0, 0);

    public static DDSPixelFormat DDSPF_L16_NVTT1 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 16, 0xffff, 0, 0, 0);

    public static DDSPixelFormat DDSPF_A8L8_NVTT1 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 16, 0x00ff, 0, 0, 0xff00);

    public static DDSPixelFormat DDSPF_A8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_ALPHA, 0, 8, 0, 0, 0, 0xff);

    public static DDSPixelFormat DDSPF_V8U8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_BUMPDUDV, 0, 16, 0x00ff, 0xff00, 0, 0);

    public static DDSPixelFormat DDSPF_Q8W8V8U8 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_BUMPDUDV, 0, 32, 0x000000ff, 0x0000ff00, 0x00ff0000,
            0xff000000);

    public static DDSPixelFormat DDSPF_V16U16 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_BUMPDUDV, 0, 32, 0x0000ffff, 0xffff0000, 0, 0);

    public static DDSPixelFormat DDSPF_A2R10G10B10 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 32, 0x000003ff, 0x000ffc00, 0x3ff00000,
            0xc0000000);

    public static DDSPixelFormat DDSPF_A2B10G10R10 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_RGBA, 0, 32, 0x3ff00000, 0x000ffc00, 0x000003ff,
            0xc0000000);

    public static DDSPixelFormat DX10 =
        new DDSPixelFormat(StructSize, DdsPixelFormatFlag.DDS_FOURCC, MakeFourCc('D', 'X', '1', '0'), 0, 0, 0, 0, 0);
}
