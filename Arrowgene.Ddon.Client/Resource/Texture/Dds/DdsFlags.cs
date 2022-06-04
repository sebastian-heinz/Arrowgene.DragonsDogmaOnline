using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

[Flags]
public enum DdsFlags
{
    None = 0x0,
    LegacyDword = 0x1,
    NoLegacyExpansion = 0x2,
    NoR10B10G10A2Fixup = 0x4,
    ForceRgb = 0x8,
    No16BPP = 0x10,
    ExpandLuminance = 0x20,
    ForceDx10Ext = 0x10000,
    ForceDx10ExtMisc2 = 0x20000,
}
