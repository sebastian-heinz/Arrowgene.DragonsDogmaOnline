using System;
using System.Runtime.InteropServices;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public struct DDSPixelFormat
{
    public const uint DDSFOURCC = 0x00000004; // DDPFFOURCC
    public const uint DDSRGB = 0x00000040; // DDPFRGB
    public const uint DDSRGBA = 0x00000041; // DDPFRGB | DDPFALPHAPIXELS
    public const uint DDSLUMINANCE = 0x00020000; // DDPFLUMINANCE
    public const uint DDSLUMINANCEA = 0x00020001; // DDPFLUMINANCE | DDPFALPHAPIXELS
    public const uint DDSALPHAPIXELS = 0x00000001; // DDPFALPHAPIXELS
    public const uint DDSALPHA = 0x00000002; // DDPFALPHA
    public const uint DDSPAL8 = 0x00000020; // DDPFPALETTEINDEXED8
    public const uint DDSPAL8A = 0x00000021; // DDPFPALETTEINDEXED8 | DDPFALPHAPIXELS
    public const uint DDSBUMPDUDV = 0x00080000; // DDPFBUMPDUDV
    
    public static uint MakePixelFormatFourCC(char char1, char char2, char char3, char char4)
    {
        return Convert.ToByte(char1) | (uint) Convert.ToByte(char2) << 8 | (uint) Convert.ToByte(char3) << 16 |
               (uint) Convert.ToByte(char4) << 24;
    }

    public static readonly uint StructSize = (uint) Marshal.SizeOf<DDSPixelFormat>();
    
    
    public uint Size;
    public uint Flags;
    public uint FourCC;
    public uint RGBBitCount;
    public uint RBitMask;
    public uint GBitMask;
    public uint BBitMask;
    public uint ABitMask;
    
    public DDSPixelFormat(uint size, uint flags, uint fourCC, uint rgbBitCount, uint rBitMask,
        uint gBitMask, uint bBitMask, uint aBitMask)
    {
        Size = size;
        Flags = flags;
        FourCC = fourCC;
        RGBBitCount = rgbBitCount;
        RBitMask = rBitMask;
        GBitMask = gBitMask;
        BBitMask = bBitMask;
        ABitMask = aBitMask;
    }

    public static DDSPixelFormat DXT1 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('D', 'X', 'T', '1'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DXT2 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('D', 'X', 'T', '2'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DXT3 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('D', 'X', 'T', '3'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DXT4 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('D', 'X', 'T', '4'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat DXT5 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('D', 'X', 'T', '5'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat BC4UNORM =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('B', 'C', '4', 'U'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat BC4SNORM =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('B', 'C', '4', 'S'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat BC5UNORM =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('B', 'C', '5', 'U'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat BC5SNORM =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('B', 'C', '5', 'S'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat R8G8B8G8 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('R', 'G', 'B', 'G'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat G8R8G8B8 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('G', 'R', 'G', 'B'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat YUY2 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('Y', 'U', 'Y', '2'), 0, 0, 0, 0, 0);

    public static DDSPixelFormat A8R8G8B8 =
        new DDSPixelFormat(StructSize, DDSRGBA, 0, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000);

    public static DDSPixelFormat X8R8G8B8 =
        new DDSPixelFormat(StructSize, DDSRGB, 0, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0x00000000);

    public static DDSPixelFormat A8B8G8R8 =
        new DDSPixelFormat(StructSize, DDSRGBA, 0, 32, 0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000);

    public static DDSPixelFormat X8B8G8R8 =
        new DDSPixelFormat(StructSize, DDSRGB, 0, 32, 0x000000ff, 0x0000ff00, 0x00ff0000, 0x00000000);

    public static DDSPixelFormat G16R16 =
        new DDSPixelFormat(StructSize, DDSRGB, 0, 32, 0x0000ffff, 0xffff0000, 0x00000000, 0x00000000);

    public static DDSPixelFormat R5G6B5 =
        new DDSPixelFormat(StructSize, DDSRGB, 0, 16, 0x0000f800, 0x000007e0, 0x0000001f, 0x00000000);

    public static DDSPixelFormat A1R5G5B5 =
        new DDSPixelFormat(StructSize, DDSRGBA, 0, 16, 0x00007c00, 0x000003e0, 0x0000001f, 0x00008000);

    public static DDSPixelFormat A4R4G4B4 =
        new DDSPixelFormat(StructSize, DDSRGBA, 0, 16, 0x00000f00, 0x000000f0, 0x0000000f, 0x0000f000);

    public static DDSPixelFormat R8G8B8 =
        new DDSPixelFormat(StructSize, DDSRGB, 0, 24, 0x00ff0000, 0x0000ff00, 0x000000ff, 0x00000000);

    public static DDSPixelFormat L8 =
        new DDSPixelFormat(StructSize, DDSLUMINANCE, 0, 8, 0xff, 0x00, 0x00, 0x00);

    public static DDSPixelFormat L16 =
        new DDSPixelFormat(StructSize, DDSLUMINANCE, 0, 16, 0xffff, 0x0000, 0x0000, 0x0000);

    public static DDSPixelFormat A8L8 =
        new DDSPixelFormat(StructSize, DDSLUMINANCEA, 0, 16, 0x00ff, 0x0000, 0x0000, 0xff00);

    public static DDSPixelFormat A8L8ALT =
        new DDSPixelFormat(StructSize, DDSLUMINANCEA, 0, 8, 0x00ff, 0x0000, 0x0000, 0xff00);

    public static DDSPixelFormat A8 =
        new DDSPixelFormat(StructSize, DDSALPHA, 0, 8, 0x00, 0x00, 0x00, 0xff);

    public static DDSPixelFormat V8U8 =
        new DDSPixelFormat(StructSize, DDSBUMPDUDV, 0, 16, 0x00ff, 0xff00, 0x0000, 0x0000);

    public static DDSPixelFormat Q8W8V8U8 =
        new DDSPixelFormat(StructSize, DDSBUMPDUDV, 0, 32, 0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000);

    public static DDSPixelFormat V16U16 =
        new DDSPixelFormat(StructSize, DDSBUMPDUDV, 0, 32, 0x0000ffff, 0xffff0000, 0x00000000, 0x00000000);

    public static DDSPixelFormat DX10 =
        new DDSPixelFormat(StructSize, DDSFOURCC, MakePixelFormatFourCC('D', 'X', '1', '0'), 0, 0, 0, 0, 0);
}
