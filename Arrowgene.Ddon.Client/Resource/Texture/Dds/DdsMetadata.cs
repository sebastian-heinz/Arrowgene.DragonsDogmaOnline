namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public struct TexMetadata
{
    public enum TexAlphaMode
    {
        UNKNOWN = 0,
        STRAIGHT = 1,
        PREMULTIPLIED = 2,
        OPAQUE = 3,
        CUSTOM = 4,
    }

    public long Width;
    public long Height; // Should be 1 for 1D textures
    public long Depth; // Should be 1 for 1D or 2D textures
    public long ArraySize; // For cubemap, this is a multiple of 6
    public long MipLevels;
    public Dx10Header.TexMiscFlags MiscFlags;
    public Dx10Header.TexMiscFlags2 MiscFlags2;
    public Dx10Header.DXGIFormat Format;
    public Dx10Header.TexDimension Dimension;

    public TexMetadata(long width, long height, long depth, long arraySize, long mipLevels,
        Dx10Header.TexMiscFlags flags,
        Dx10Header.TexMiscFlags2 flags2, Dx10Header.DXGIFormat format, Dx10Header.TexDimension dimension)
    {
        Width = width;
        Height = height;
        Depth = depth;
        ArraySize = arraySize;
        MipLevels = mipLevels;
        MiscFlags = flags;
        MiscFlags2 = flags2;
        Format = format;
        Dimension = dimension;
    }


    public bool IsPMAlpha()
    {
        return (TexAlphaMode) (MiscFlags2 & Dx10Header.TexMiscFlags2.TEXMISC2ALPHAMODEMASK) ==
               TexAlphaMode.PREMULTIPLIED;
    }

    public bool IsCubeMap()
    {
        return (MiscFlags & Dx10Header.TexMiscFlags.TEXTURECUBE) == Dx10Header.TexMiscFlags.TEXTURECUBE;
    }
}
