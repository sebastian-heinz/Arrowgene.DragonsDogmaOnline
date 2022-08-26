using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

[Flags]
public enum CpFlags
{
    None = 0x0,
    LegacyDword = 0x1,
    Paragraph = 0x2,
    Ymm = 0x4,
    Zmm = 0x8,
    Page4K = 0x200,
    BadDxtnTails = 0x1000,
    _24BPP = 0x10000,
    _16BPP = 0x20000,
    _8BPP = 0x40000,
}
