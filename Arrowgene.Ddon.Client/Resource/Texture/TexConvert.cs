using Arrowgene.Ddon.Client.Resource.Texture.Dds;
using Arrowgene.Ddon.Client.Resource.Texture.Tex;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource.Texture;

public static class TexConvert
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(TexConvert));


    public static DdsTexture ToDdsTexture(TexTexture texTexture)
    {
        DDSPixelFormat? ddsPixelFormat = ToDdsPixelFormat(texTexture.Header.PixelFormat);
        DxGiFormat dxGiFormat = ToDxGiFormat(texTexture.Header.PixelFormat);
        if (ddsPixelFormat == null)
        {
            Logger.Error(
                $"No suitable DDSPixelFormat format found for TexPixelFormat: {texTexture.Header.PixelFormat}, trying DxGiFormat");
            if (dxGiFormat == DxGiFormat.DXGI_FORMAT_UNKNOWN)
            {
                Logger.Error(
                    $"No suitable DxGiFormat format found for TexPixelFormat: {texTexture.Header.PixelFormat}, giving up");
                return null;
            }

            ddsPixelFormat = DDSPixelFormat.DX10;
        }

        DdsTexture ddsTexture = new DdsTexture();
        ddsTexture.Header.Size = DdsHeader.StructSize;
        ddsTexture.Header.Flags = DdsHeaderFlags.Texture;
        ddsTexture.Header.Height = texTexture.Header.Height;
        ddsTexture.Header.Width = texTexture.Header.Width;
        ddsTexture.Header.PitchOrLinearSize = 0;
        ddsTexture.Header.Depth = texTexture.Header.Depth;
        ddsTexture.Header.MipMapCount = texTexture.Header.MipMapCount;
        ddsTexture.Header.Reserved1 = new uint[11];
        ddsTexture.Header.PixelFormat = ddsPixelFormat.Value;
        ddsTexture.Header.Caps = DdsCaps.Texture;
        ddsTexture.Header.Caps2 = 0;
        ddsTexture.Header.Caps3 = 0;
        ddsTexture.Header.Caps4 = 0;
        ddsTexture.Header.Reserved2 = 0;

        if (ddsTexture.Header.Depth == 1)
        {
            // from testing 1D and 2D .tex files have always a depth of 1
            ddsTexture.Header.Depth = 0;
        }

        if (texTexture.Header.MipMapCount > 0)
        {
            // has mip maps
            ddsTexture.Header.Caps |= DdsCaps.MipMap | DdsCaps.Complex;
            ddsTexture.Header.Flags |= DdsHeaderFlags.MipMap;
        }

        if (texTexture.Header.PixelFormat == TexPixelFormat.FORMAT_BC1_UNORM_SRGB)
        {
            // TODO exporting this with mipmaps causes GIMP not to show export Cubemap option
            ddsTexture.Header.Caps &= ~(DdsCaps.MipMap | DdsCaps.Complex);
            ddsTexture.Header.Flags &= ~DdsHeaderFlags.MipMap;
        }

        if (texTexture.Header.UnknownA == 6)
        {
            // Assuming Cube Map
            ddsTexture.Header.Caps |= DdsCaps.Complex;
            ddsTexture.Header.Caps2 |= DdsCaps2.CubeMap | DdsCaps2.CubeMapAllFaces;
        }

        if (ddsTexture.Header.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
        {
            if (dxGiFormat == DxGiFormat.DXGI_FORMAT_UNKNOWN)
            {
                Logger.Error(
                    $"Requested Dx10Header but no suitable DxGiFormat format found for TexPixelFormat: {texTexture.Header.PixelFormat}");
                return null;
            }

            ddsTexture.Dx10Header.Format = dxGiFormat;
        }

        if (texTexture.Header.TextureArraySize > 1)
        {
            // require DX10 header
            ddsTexture.Header.Caps |= DdsCaps.Complex;

            if (dxGiFormat == DxGiFormat.DXGI_FORMAT_UNKNOWN)
            {
                Logger.Error(
                    $"Required Dx10Header for TextureArray, but no suitable DxGiFormat format found for TexPixelFormat: {texTexture.Header.PixelFormat}");
                return null;
            }

            ddsTexture.Header.PixelFormat = DDSPixelFormat.DX10;
            ddsTexture.Dx10Header.Format = dxGiFormat;
            ddsTexture.Dx10Header.ResourceDimension = TexDimension.Texture2D;
            ddsTexture.Dx10Header.MiscFlag = 0;
            ddsTexture.Dx10Header.ArraySize = texTexture.Header.TextureArraySize;
            ddsTexture.Dx10Header.MiscFlags2 = 0;

            if (texTexture.Header.UnknownA == 6)
            {
                // Assuming Cube Map
                ddsTexture.Dx10Header.MiscFlag |= TexMiscFlag.TextureCube;
            }
        }

        int imageCount = texTexture.Images.Length;
        ddsTexture.Images = new Image[imageCount];
        for (int i = 0; i < imageCount; i++)
        {
            ddsTexture.Images[i].Data = texTexture.Images[i].Data;
        }

        return ddsTexture;
    }

    public static TexTexture ToTexTexture(DdsTexture ddsTexture, TexHeaderVersion headerVersion,
        TexSphericalHarmonics? sphericalHarmonics)
    {
        TexPixelFormat? texPixelFormat = ToTexPixelFormat(ddsTexture.Metadata.Format);
        if (texPixelFormat == TexPixelFormat.FORMAT_UNKNOWN)
        {
            return null;
        }

        TexTexture texTexture = new TexTexture();
        texTexture.Header.Version = headerVersion;
        texTexture.Header.Height = (uint) ddsTexture.Metadata.Height;
        texTexture.Header.Width = (uint) ddsTexture.Metadata.Width;
        texTexture.Header.Shift = 0;
        texTexture.Header.Alpha = 0;
        texTexture.Header.Depth = (uint) ddsTexture.Metadata.Depth;
        texTexture.Header.PixelFormat = texPixelFormat.Value;
        texTexture.Header.TextureArraySize = (byte) ddsTexture.Metadata.ArraySize;
        texTexture.Header.MipMapCount = (uint) ddsTexture.Metadata.MipLevels;
        texTexture.Header.UnknownA = 0;
        texTexture.Header.UnknownB = 0;
        texTexture.Header.HasSphericalHarmonicsFactor = false;

        if (texTexture.Header.Depth == 0)
        {
            // from testing 1D and 2D .tex files have always a depth of 1
            texTexture.Header.Depth = 1;
        }

        if (sphericalHarmonics.HasValue)
        {
            texTexture.Header.HasSphericalHarmonicsFactor = true;
            texTexture.SphericalHarmonics = sphericalHarmonics.Value;
        }
        
        if (ddsTexture.Header.Caps2.HasFlag(DdsCaps2.CubeMap))
        {
            // Assuming Cube Map
            texTexture.Header.UnknownA = 6;
        }
        
        if (ddsTexture.Header.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
        {
            if (ddsTexture.Dx10Header.MiscFlag.HasFlag(TexMiscFlag.TextureCube))
            {
                texTexture.Header.UnknownA = 6;
            }
        }
        
        int imageCount = ddsTexture.Images.Length;
        texTexture.Images = new TexImage[imageCount];
        for (int i = 0; i < imageCount; i++)
        {
            texTexture.Images[i].Data = ddsTexture.Images[i].Data;
        }

        return texTexture;
    }

    public static DxGiFormat ToDxGiFormat(TexPixelFormat texPixelFormat)
    {
        switch (texPixelFormat)
        {
            case TexPixelFormat.FORMAT_BC1_UNORM_SRGB: return DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB;
            case TexPixelFormat.FORMAT_BCX_RGBI_SRGB: return DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB;
        }

        return DxGiFormat.DXGI_FORMAT_UNKNOWN;
    }

    /**
     * manual mapping
     */
    public static DDSPixelFormat? ToDdsPixelFormat(TexPixelFormat texPixelFormat)
    {
        switch (texPixelFormat)
        {
            case TexPixelFormat.FORMAT_BC1_UNORM_SRGB: return DDSPixelFormat.DDSPF_DXT1;
            case TexPixelFormat.FORMAT_BCX_RGBI_SRGB: return DDSPixelFormat.DDSPF_BC5_UNORM;
            case TexPixelFormat.FORMAT_B8G8R8A8_UNORM_SRGB: return DDSPixelFormat.DDSPF_DXT1;
        }

        return null;
    }

    /**
     * manual mapping
     */
    public static TexPixelFormat ToTexPixelFormat(DxGiFormat dxgi)
    {
        switch (dxgi)
        {
            case DxGiFormat.DXGI_FORMAT_BC1_UNORM: // GIMP Bc1 / DXT
                return TexPixelFormat.FORMAT_BC1_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB: return TexPixelFormat.FORMAT_BC1_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB: return TexPixelFormat.FORMAT_BCX_RGBI_SRGB;
        }

        return TexPixelFormat.FORMAT_UNKNOWN;
    }

    /**
     * Conversion based on format name, seems not to work
     */
    public static TexPixelFormat ToTexPixelFormatEx(DxGiFormat dxgi)
    {
        switch (dxgi)
        {
            case DxGiFormat.DXGI_FORMAT_R32G32B32A32_FLOAT:
                return TexPixelFormat.FORMAT_R32G32B32A32_FLOAT;
            case DxGiFormat.DXGI_FORMAT_R16G16B16A16_FLOAT:
                return TexPixelFormat.FORMAT_R16G16B16A16_FLOAT;
            case DxGiFormat.DXGI_FORMAT_R16G16B16A16_UNORM:
                return TexPixelFormat.FORMAT_R16G16B16A16_UNORM;
            case DxGiFormat.DXGI_FORMAT_R16G16B16A16_SNORM:
                return TexPixelFormat.FORMAT_R16G16B16A16_SNORM;
            case DxGiFormat.DXGI_FORMAT_R32G32_FLOAT:
                return TexPixelFormat.FORMAT_R32G32_FLOAT;
            case DxGiFormat.DXGI_FORMAT_R10G10B10A2_UNORM:
                return TexPixelFormat.FORMAT_R10G10B10A2_UNORM;
            case DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM:
                return TexPixelFormat.FORMAT_R8G8B8A8_UNORM;
            case DxGiFormat.DXGI_FORMAT_R8G8B8A8_SNORM:
                return TexPixelFormat.FORMAT_R8G8B8A8_SNORM;
            case DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM_SRGB:
                return TexPixelFormat.FORMAT_R8G8B8A8_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_B4G4R4A4_UNORM:
                return TexPixelFormat.FORMAT_B4G4R4A4_UNORM;
            case DxGiFormat.DXGI_FORMAT_R16G16_FLOAT:
                return TexPixelFormat.FORMAT_R16G16_FLOAT;
            case DxGiFormat.DXGI_FORMAT_R16G16_UNORM:
                return TexPixelFormat.FORMAT_R16G16_UNORM;
            case DxGiFormat.DXGI_FORMAT_R16G16_SNORM:
                return TexPixelFormat.FORMAT_R16G16_SNORM;
            case DxGiFormat.DXGI_FORMAT_R32_FLOAT:
                return TexPixelFormat.FORMAT_R32_FLOAT;
            case DxGiFormat.DXGI_FORMAT_D24_UNORM_S8_UINT:
                return TexPixelFormat.FORMAT_D24_UNORM_S8_UINT;
            case DxGiFormat.DXGI_FORMAT_R16_FLOAT:
                return TexPixelFormat.FORMAT_R16_FLOAT;
            case DxGiFormat.DXGI_FORMAT_R16_UNORM:
                return TexPixelFormat.FORMAT_R16_UNORM;
            case DxGiFormat.DXGI_FORMAT_A8_UNORM:
                return TexPixelFormat.FORMAT_A8_UNORM;
            case DxGiFormat.DXGI_FORMAT_BC1_UNORM:
                return TexPixelFormat.FORMAT_BC1_UNORM;
            case DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB:
                return TexPixelFormat.FORMAT_BC1_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_BC2_UNORM:
                return TexPixelFormat.FORMAT_BC2_UNORM;
            case DxGiFormat.DXGI_FORMAT_BC2_UNORM_SRGB:
                return TexPixelFormat.FORMAT_BC2_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_BC3_UNORM:
                return TexPixelFormat.FORMAT_BC3_UNORM;
            case DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB:
                return TexPixelFormat.FORMAT_BC3_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_BC5_SNORM:
                return TexPixelFormat.FORMAT_BC5_SNORM;
            case DxGiFormat.DXGI_FORMAT_B5G6R5_UNORM:
                return TexPixelFormat.FORMAT_B5G6R5_UNORM;
            case DxGiFormat.DXGI_FORMAT_B5G5R5A1_UNORM:
                return TexPixelFormat.FORMAT_B5G5R5A1_UNORM;
            case DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM:
                return TexPixelFormat.FORMAT_B8G8R8X8_UNORM;
            case DxGiFormat.DXGI_FORMAT_R11G11B10_FLOAT:
                return TexPixelFormat.FORMAT_R11G11B10_FLOAT;
            case DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM:
                return TexPixelFormat.FORMAT_B8G8R8A8_UNORM;
            case DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM_SRGB:
                return TexPixelFormat.FORMAT_B8G8R8A8_UNORM_SRGB;
            case DxGiFormat.DXGI_FORMAT_R8_UNORM:
                return TexPixelFormat.FORMAT_R8_UNORM;
            case DxGiFormat.DXGI_FORMAT_BC7_UNORM:
                return TexPixelFormat.FORMAT_BC7_UNORM;
            case DxGiFormat.DXGI_FORMAT_BC7_UNORM_SRGB:
                return TexPixelFormat.FORMAT_BC7_UNORM_SRGB;
        }

        return TexPixelFormat.FORMAT_UNKNOWN;
    }

    /**
     * Conversion based on format name, seems not to work
     */
    public static DxGiFormat ToDxGiFormatEx(TexPixelFormat texPixelFormat)
    {
        switch (texPixelFormat)
        {
            case TexPixelFormat.FORMAT_R32G32B32A32_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R32G32B32A32_FLOAT;
            case TexPixelFormat.FORMAT_R16G16B16A16_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R16G16B16A16_FLOAT;
            case TexPixelFormat.FORMAT_R16G16B16A16_UNORM:
                return DxGiFormat.DXGI_FORMAT_R16G16B16A16_UNORM;
            case TexPixelFormat.FORMAT_R16G16B16A16_SNORM:
                return DxGiFormat.DXGI_FORMAT_R16G16B16A16_SNORM;
            case TexPixelFormat.FORMAT_R32G32_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R32G32_FLOAT;
            case TexPixelFormat.FORMAT_R10G10B10A2_UNORM:
                return DxGiFormat.DXGI_FORMAT_R10G10B10A2_UNORM;
            case TexPixelFormat.FORMAT_R8G8B8A8_UNORM:
                return DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM;
            case TexPixelFormat.FORMAT_R8G8B8A8_SNORM:
                return DxGiFormat.DXGI_FORMAT_R8G8B8A8_SNORM;
            case TexPixelFormat.FORMAT_R8G8B8A8_UNORM_SRGB:
                return DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM_SRGB;
            case TexPixelFormat.FORMAT_B4G4R4A4_UNORM:
                return DxGiFormat.DXGI_FORMAT_B4G4R4A4_UNORM;
            case TexPixelFormat.FORMAT_R16G16_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R16G16_FLOAT;
            case TexPixelFormat.FORMAT_R16G16_UNORM:
                return DxGiFormat.DXGI_FORMAT_R16G16_UNORM;
            case TexPixelFormat.FORMAT_R16G16_SNORM:
                return DxGiFormat.DXGI_FORMAT_R16G16_SNORM;
            case TexPixelFormat.FORMAT_R32_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R32_FLOAT;
            case TexPixelFormat.FORMAT_D24_UNORM_S8_UINT:
                return DxGiFormat.DXGI_FORMAT_D24_UNORM_S8_UINT;
            case TexPixelFormat.FORMAT_R16_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R16_FLOAT;
            case TexPixelFormat.FORMAT_R16_UNORM:
                return DxGiFormat.DXGI_FORMAT_R16_UNORM;
            case TexPixelFormat.FORMAT_A8_UNORM:
                return DxGiFormat.DXGI_FORMAT_A8_UNORM;
            case TexPixelFormat.FORMAT_BC1_UNORM:
                return DxGiFormat.DXGI_FORMAT_BC1_UNORM;
            case TexPixelFormat.FORMAT_BC1_UNORM_SRGB:
                return DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB;
            case TexPixelFormat.FORMAT_BC2_UNORM:
                return DxGiFormat.DXGI_FORMAT_BC2_UNORM;
            case TexPixelFormat.FORMAT_BC2_UNORM_SRGB:
                return DxGiFormat.DXGI_FORMAT_BC2_UNORM_SRGB;
            case TexPixelFormat.FORMAT_BC3_UNORM:
                return DxGiFormat.DXGI_FORMAT_BC3_UNORM;
            case TexPixelFormat.FORMAT_BC3_UNORM_SRGB:
                return DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB;
            case TexPixelFormat.FORMAT_BC5_SNORM:
                return DxGiFormat.DXGI_FORMAT_BC5_SNORM;
            case TexPixelFormat.FORMAT_B5G6R5_UNORM:
                return DxGiFormat.DXGI_FORMAT_B5G6R5_UNORM;
            case TexPixelFormat.FORMAT_B5G5R5A1_UNORM:
                return DxGiFormat.DXGI_FORMAT_B5G5R5A1_UNORM;
            case TexPixelFormat.FORMAT_B8G8R8X8_UNORM:
                return DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM;
            case TexPixelFormat.FORMAT_R11G11B10_FLOAT:
                return DxGiFormat.DXGI_FORMAT_R11G11B10_FLOAT;
            case TexPixelFormat.FORMAT_B8G8R8A8_UNORM:
                return DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM;
            case TexPixelFormat.FORMAT_B8G8R8A8_UNORM_SRGB:
                return DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM_SRGB;
            case TexPixelFormat.FORMAT_R8_UNORM:
                return DxGiFormat.DXGI_FORMAT_R8_UNORM;
            case TexPixelFormat.FORMAT_BC7_UNORM:
                return DxGiFormat.DXGI_FORMAT_BC7_UNORM;
            case TexPixelFormat.FORMAT_BC7_UNORM_SRGB:
                return DxGiFormat.DXGI_FORMAT_BC7_UNORM_SRGB;
        }

        return DxGiFormat.DXGI_FORMAT_UNKNOWN;
    }
}
