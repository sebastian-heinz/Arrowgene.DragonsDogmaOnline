using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

[Flags]
public enum ConversionFlags
{
    CONV_FLAGS_NONE = 0x0,
    CONV_FLAGS_EXPAND = 0x1, // Conversion requires expanded pixel size
    CONV_FLAGS_NOALPHA = 0x2, // Conversion requires setting alpha to known value
    CONV_FLAGS_SWIZZLE = 0x4, // BGR/RGB order swizzling required
    CONV_FLAGS_PAL8 = 0x8, // Has an 8-bit palette
    CONV_FLAGS_888 = 0x10, // Source is an 8:8:8 (24bpp) format
    CONV_FLAGS_565 = 0x20, // Source is a 5:6:5 (16bpp) format
    CONV_FLAGS_5551 = 0x40, // Source is a 5:5:5:1 (16bpp) format
    CONV_FLAGS_4444 = 0x80, // Source is a 4:4:4:4 (16bpp) format
    CONV_FLAGS_44 = 0x100, // Source is a 4:4 (8bpp) format
    CONV_FLAGS_332 = 0x200, // Source is a 3:3:2 (8bpp) format
    CONV_FLAGS_8332 = 0x400, // Source is a 8:3:3:2 (16bpp) format
    CONV_FLAGS_A8P8 = 0x800, // Has an 8-bit palette with an alpha channel
    CONV_FLAGS_DX10 = 0x10000, // Has the 'DX10' extension header
    CONV_FLAGS_PMALPHA = 0x20000, // Contains premultiplied alpha data
    CONV_FLAGS_L8 = 0x40000, // Source is a 8 luminance format
    CONV_FLAGS_L16 = 0x80000, // Source is a 16 luminance format
    CONV_FLAGS_A8L8 = 0x100000, // Source is a 8:8 luminance format
}
