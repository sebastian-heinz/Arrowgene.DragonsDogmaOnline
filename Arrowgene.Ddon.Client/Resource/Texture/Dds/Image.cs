namespace Arrowgene.Ddon.Client.Resource.Texture.Dds;

public struct Image
{
    public ulong Width;
    public ulong Height;
    public DxGiFormat Format;
    public ulong RowPitch;
    public ulong SlicePitch;
    public ulong PixelsOffset;
    public ulong PixelsSize;
    public byte[] Data;
}
