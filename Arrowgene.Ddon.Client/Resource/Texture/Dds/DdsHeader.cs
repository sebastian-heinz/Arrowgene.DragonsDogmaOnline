using System.Runtime.InteropServices;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public struct DdsHeader
{
    public const string Magic = "DDS ";
    public const int StructSize = 124;
    
    public uint Size;
    public DdsHeaderFlags Flags;
    public uint Height;
    public uint Width;
    public uint PitchOrLinearSize;
    public uint Depth;
    public uint MipMapCount;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
    public uint[] Reserved1;

    public DDSPixelFormat PixelFormat;
    public DdsCaps Caps;
    public DdsCaps2 Caps2;
    public uint Caps3;
    public uint Caps4;
    public uint Reserved2;
}
