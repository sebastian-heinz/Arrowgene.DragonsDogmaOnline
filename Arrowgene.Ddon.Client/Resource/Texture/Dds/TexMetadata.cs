namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public struct TexMetadata
{
    public ulong Width;
    public ulong Height;
    public ulong Depth;
    public ulong ArraySize;
    public ulong MipLevels;
    public TexMiscFlag MiscFlags;
    public TexMiscFlag2 MiscFlags2;
    public DxGiFormat Format;
    public TexDimension Dimension;

    public bool IsPmAlpha()
    {
        return (TexAlphaMode) (MiscFlags2 & TexMiscFlag2.AlphaModeMask) == TexAlphaMode.Premultiplied;
    }

    public bool IsCubeMap()
    {
        return (MiscFlags & TexMiscFlag.TextureCube) == TexMiscFlag.TextureCube;
    }
}
