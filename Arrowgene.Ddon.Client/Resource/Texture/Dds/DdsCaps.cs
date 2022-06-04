using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

[Flags]
public enum DdsCaps : uint
{
    Complex = 0x8,
    MipMap = 0x400000,
    Texture = 0x1000,
}
