using Arrowgene.Ddon.Client.Resource.Texture.Dds;
using Arrowgene.Ddon.Client.Resource.Texture.Tex;

namespace Arrowgene.Ddon.Client.Resource.Texture;

public static class TexConvert
{
    public static DdsTexture ToDdsTexture(TexTexture texTexture)
    {
        DdsTexture ddsTexture = new DdsTexture();
        ddsTexture.Header.Size = DdsHeader.StructSize;
        ddsTexture.Header.Flags = 0; // TODO
        ddsTexture.Header.Height = texTexture.Header.Height;
        ddsTexture.Header.Width = texTexture.Header.Width;
        ddsTexture.Header.PitchOrLinearSize = 0; // TODO
        ddsTexture.Header.Depth = texTexture.Header.Depth;
        ddsTexture.Header.MipMapCount = texTexture.Header.MipMapCount;
        ddsTexture.Header.Reserved1 = new uint[11];
        ddsTexture.Header.PixelFormat = DDSPixelFormat.DDSPF_DXT1; // TODO
        ddsTexture.Header.Caps = 0; // TODO
        ddsTexture.Header.Caps2 = 0; // TODO
        ddsTexture.Header.Caps3 = 0; // TODO
        ddsTexture.Header.Caps4 = 0; // TODO
        ddsTexture.Header.Reserved2 = 0;

        if (texTexture.Header.TextureArraySize > 1)
        {
            ddsTexture.Header.PixelFormat = DDSPixelFormat.DX10;

            ddsTexture.Dx10Header.Format = DxGiFormat.DXGI_FORMAT_UNKNOWN; // TODO
            ddsTexture.Dx10Header.ResourceDimension = TexDimension.Texture2D;
            ddsTexture.Dx10Header.MiscFlag = 0; // TODO
            ddsTexture.Dx10Header.ArraySize = texTexture.Header.TextureArraySize;
            ddsTexture.Dx10Header.MiscFlags2 = 0; // TODO
        }

        int imageCount = texTexture.Images.Length;
        ddsTexture.Images = new Image[imageCount];
        for (int i = 0; i < imageCount; i++)
        {
            ddsTexture.Images[i].Data = texTexture.Images[i].Data;
        }

        return ddsTexture;
    }

    public static TexTexture ToTexTexture(DdsTexture ddsTexture, TexHeaderVersion headerVersion)
    {
        TexTexture texTexture = new TexTexture();
        texTexture.Header.Version = headerVersion;
        texTexture.Header.Height = (uint) ddsTexture.Metadata.Height;
        texTexture.Header.Width = (uint) ddsTexture.Metadata.Width;
        texTexture.Header.Shift = 0; // TODO
        texTexture.Header.Alpha = 0; // TODO
        texTexture.Header.Depth = (uint) ddsTexture.Metadata.Depth;
        texTexture.Header.PixelFormat = 0; // TODO
        texTexture.Header.TextureArraySize = (byte) ddsTexture.Metadata.ArraySize;
        texTexture.Header.MipMapCount = (uint) ddsTexture.Metadata.MipLevels;
        texTexture.Header.UnknownA = 0; // TODO
        texTexture.Header.UnknownB = 0; // TODO
        texTexture.Header.HasSphericalHarmonicsFactor = false;

        int imageCount = ddsTexture.Images.Length;
        texTexture.Images = new TexImage[imageCount];
        for (int i = 0; i < imageCount; i++)
        {
            texTexture.Images[i].Data = ddsTexture.Images[i].Data;
        }

        return texTexture;
    }
}
