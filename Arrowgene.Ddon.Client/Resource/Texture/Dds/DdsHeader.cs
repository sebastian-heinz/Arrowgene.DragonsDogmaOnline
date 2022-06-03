using System.Runtime.InteropServices;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public struct DDSHeader
{
    public enum HeaderFlags : uint
    {
        TEXTURE = 0x00001007, // DDSDCAPS | DDSDHEIGHT | DDSDWIDTH | DDSDPIXELFORMAT 
        MIPMAP = 0x00020000, // DDSDMIPMAPCOUNT
        VOLUME = 0x00800000, // DDSDDEPTH
        PITCH = 0x00000008, // DDSDPITCH
        LINEARSIZE = 0x00080000, // DDSDLINEARSIZE
    }
    
    public enum SurfaceFlags : uint
    {
        TEXTURE = 0x00001000, // DDSCAPSTEXTURE
        MIPMAP = 0x00400008, // DDSCAPSCOMPLEX | DDSCAPSMIPMAP
        CUBEMAP = 0x00000008, // DDSCAPSCOMPLEX
    }
    
    public const uint DDSMagic = 0x20534444;


    public uint Size;
    public HeaderFlags Flags;
    public uint Height;
    public uint Width;
    public uint PitchOrLinearSize;
    public uint Depth; // only if DDSHEADERFLAGSVOLUME is set in flags
    public uint MipMapCount;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
    public uint[] Reserved1;

    public DDSPixelFormat PixelFormat;
    public uint Caps;
    public uint Caps2;
    public uint Caps3;
    public uint Caps4;
    public uint Reserved2;
}
