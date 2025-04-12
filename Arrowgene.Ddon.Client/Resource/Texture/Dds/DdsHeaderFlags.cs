using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

[Flags]
public enum DdsHeaderFlags : uint
{
    Caps = 0x1,            
    Height = 0x2,          
    Width = 0x4,           
    Pitch = 0x8,           
    PixelFormat = 0x1000,  //13
    MipMapCount = 0x20000, //18
    LinearSize = 0x80000,  //20
    Depth = 0x800000,      //24
}
