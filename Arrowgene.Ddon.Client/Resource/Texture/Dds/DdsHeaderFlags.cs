using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

[Flags]
public enum DdsHeaderFlags : uint
{
    Texture = 0x00001007,
    MipMap = 0x00020000,
    Volume = 0x00800000,
    Pitch = 0x00000008,
    LinearSize = 0x00080000,
}
