using System;
using System.Linq;
using Arrowgene.Ddon.Client.Resource.Texture.Dds;
using Arrowgene.Ddon.Client.Resource.Texture.Tex;

namespace Arrowgene.Ddon.Client.Resource.Texture;

public static class TexConvert
{
    public static DdsTexture ToDdsTexture(TexTexture texTexture)
    {
        DdsTexture ddsTexture = new DdsTexture();
        ddsTexture.Header.Size = DdsHeader.StructSize;
        ddsTexture.Header.Flags = GetDDSDFlags(texTexture.Header);
        ddsTexture.Header.Height = texTexture.Header.Height;
        ddsTexture.Header.Width = texTexture.Header.Width;
        ddsTexture.Header.PitchOrLinearSize = GetDDSPitchOrLinearSize(texTexture);
        ddsTexture.Header.Depth = texTexture.Header.Depth;
        ddsTexture.Header.MipMapCount = texTexture.Header.MipMapCount;
        ddsTexture.Header.Reserved1 = new uint[11];
        ddsTexture.Header.PixelFormat = GetDDSPixelFormat(texTexture.Header);
        ddsTexture.Header.Caps = GetDDSCapsFlags(texTexture.Header);
        ddsTexture.Header.Caps2 = GetDDSCaps2Flags(texTexture.Header);
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
        DdsHeaderFlags requiredHeaderFlags = DdsHeaderFlags.Caps | DdsHeaderFlags.Height | DdsHeaderFlags.Width | DdsHeaderFlags.PixelFormat;
        if ((ddsTexture.Header.Flags & requiredHeaderFlags) != requiredHeaderFlags)
        {
            throw new Exception("Unsupported DDS header flags");
        }

        TexTexture texTexture = new TexTexture();
        texTexture.Header.Version = headerVersion;
        texTexture.Header.Height = (uint) ddsTexture.Metadata.Height;
        texTexture.Header.Width = (uint) ddsTexture.Metadata.Width;
        texTexture.Header.Shift = 0; // TODO
        texTexture.Header.Alpha = 0; // TODO
        texTexture.Header.Depth = (uint) ddsTexture.Metadata.Depth;
        texTexture.Header.PixelFormat = GetTexPixelFormat(ddsTexture); // TODO
        texTexture.Header.TextureArraySize = (byte) ddsTexture.Metadata.ArraySize;
        texTexture.Header.MipMapCount = (uint) ddsTexture.Metadata.MipLevels;
        texTexture.Header.Type = GetTexType(ddsTexture.Header);
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

    private static DdsHeaderFlags GetDDSDFlags(TexHeader header) {
        DdsHeaderFlags ddsdFlags = DdsHeaderFlags.Caps
                | DdsHeaderFlags.Height
                | DdsHeaderFlags.Width
                | DdsHeaderFlags.PixelFormat;
        if (header.MipMapCount > 1) {
            ddsdFlags |= DdsHeaderFlags.MipMapCount;
        }
        // Assume any BC-derivative is compressed and some form of FOURCC DXT1-5 style format
        if (header.PixelFormat.ToString().StartsWith("FORMAT_BC")) {
            ddsdFlags |= DdsHeaderFlags.LinearSize;
        } else {
            ddsdFlags |= DdsHeaderFlags.Pitch;
        }
        return ddsdFlags;
    }

    private static uint GetDDSPitchOrLinearSize(TexTexture texTexture) {
        // For compressed textures the linear size is equal to the first non-mip-mapped face
        if (texTexture.Header.PixelFormat.ToString().StartsWith("FORMAT_BC")) {
            // For DDON this is fine, because there are only 2D & CubeMap textures, 
            // which means either there is an array of size 1 or 6 and for cube maps
            // all faces will share the same size.
            if (texTexture.Header.MipMapCount == 1) {
                return texTexture.Images.Last().Offset - texTexture.Images.First().Offset;
            } else if (texTexture.Header.MipMapCount > 1) {
                return texTexture.Images[1].Offset - texTexture.Images[0].Offset;
            } else {
                throw new Exception("Invalid MipMapCount value encountered");
            }
        } else {
            // For non-compressed textures the linear size is equal to channels * total bits
            switch (texTexture.Header.PixelFormat) {
                case TexPixelFormat.FORMAT_B8G8R8A8_UNORM:
                case TexPixelFormat.FORMAT_B8G8R8A8_UNORM_SRGB:
                    return 128;
                case TexPixelFormat.FORMAT_R32_FLOAT:
                    return 32;
                case TexPixelFormat.FORMAT_B4G4R4A4_UNORM:
                    return 64;
                default:
                    throw new Exception("Unsupported format: " + texTexture.Header.PixelFormat.ToString());
            };
        }
    }

    private static DDSPixelFormat GetDDSPixelFormat(TexHeader header) {
        DDSPixelFormat ddspf;
        if (
                header.PixelFormat == TexPixelFormat.FORMAT_BC1_UNORM
                        || header.PixelFormat == TexPixelFormat.FORMAT_BC1_UNORM_SRGB
                        || header.PixelFormat == TexPixelFormat.FORMAT_BCX_GRAYSCALE
        ) {
            ddspf = DDSPixelFormat.DDSPF_DXT1;
            ddspf.Flags = DdsPixelFormatFlag.DDS_FOURCC;
        } else if (
                header.PixelFormat == TexPixelFormat.FORMAT_BC2_UNORM_SRGB
        ) {
            ddspf = DDSPixelFormat.DDSPF_DXT3;
            ddspf.Flags = DdsPixelFormatFlag.DDS_FOURCC;
        } else if (
                header.PixelFormat == TexPixelFormat.FORMAT_BC3_UNORM_SRGB
                        || header.PixelFormat == TexPixelFormat.FORMAT_BCX_NH
                        || header.PixelFormat == TexPixelFormat.FORMAT_BCX_NM2
                        || header.PixelFormat == TexPixelFormat.FORMAT_BCX_YCCA_SRGB
        ) {
            ddspf = DDSPixelFormat.DDSPF_DXT5;
            ddspf.Flags = DdsPixelFormatFlag.DDS_FOURCC;
        } else if (
                header.PixelFormat == TexPixelFormat.FORMAT_B8G8R8A8_UNORM
                        || header.PixelFormat == TexPixelFormat.FORMAT_B8G8R8A8_UNORM_SRGB
                        || header.PixelFormat == TexPixelFormat.FORMAT_R32_FLOAT
        ) {
            ddspf = new DDSPixelFormat();
            ddspf.Flags = DdsPixelFormatFlag.DDS_ALPHAPIXELS | DdsPixelFormatFlag.DDS_RGB;
            ddspf.RgbBitCount = 32;
            ddspf.BBitMask = 0xFF_00_00_00;
            ddspf.GBitMask = 0x00_FF_00_00;
            ddspf.RBitMask = 0x00_00_FF_00;
            ddspf.ABitMask = 0x00_00_00_FF;
        } else if (
                header.PixelFormat == TexPixelFormat.FORMAT_B4G4R4A4_UNORM
        ) {
            ddspf = new DDSPixelFormat();
            ddspf.Flags = DdsPixelFormatFlag.DDS_ALPHAPIXELS | DdsPixelFormatFlag.DDS_RGB;
            ddspf.RgbBitCount = 16;
            ddspf.BBitMask = 0b1111_0000_0000_0000;
            ddspf.GBitMask = 0b0000_1111_0000_0000;
            ddspf.RBitMask = 0b0000_0000_1111_0000;
            ddspf.ABitMask = 0b0000_0000_0000_1111;
        }
        else
        {
            throw new Exception("Unsupported format: " + header.PixelFormat.ToString());
        }
        ddspf.Size = 32;
        return ddspf;
    }

    private static DdsCaps GetDDSCapsFlags(TexHeader header) {
        DdsCaps capsFlags = DdsCaps.Texture;
        // Mip Map
        if (header.MipMapCount > 1) {
            capsFlags |= DdsCaps.MipMap;
        }
        // Cube Map
        if (header.Type == TexType.Cube) {
            capsFlags |= DdsCaps.Complex;
        }
        return capsFlags;
    }

    private static DdsCaps2 GetDDSCaps2Flags(TexHeader header) {
        DdsCaps2 caps2Flags = 0;
        if (header.Type == TexType.Cube) {
            caps2Flags |= DdsCaps2.CubeMapAllFaces;
        }
        return caps2Flags;
    }

    private static TexType GetTexType(DdsHeader ddsHeader)
    {
        // DDON only makes use of 2D & Cube
        return (ddsHeader.Caps2 & DdsCaps2.CubeMapAllFaces) != 0 
            ? TexType.Cube 
            : TexType.Texture2D;
    }

    private static TexPixelFormat GetTexPixelFormat(DdsTexture ddsTexture)
    {
        if(ddsTexture.Header.PixelFormat.Size != 32)
        {
            throw new Exception("Unsupported DDS pixel format size: " + ddsTexture.Header.PixelFormat.Size);
        }

        switch (ddsTexture.Header.PixelFormat.Flags)
        {
            case DdsPixelFormatFlag.DDS_FOURCC:
                switch (ddsTexture.Header.PixelFormat.FourCc)
                {
                    case 0x31545844: // 'DXT1'
                        return TexPixelFormat.FORMAT_BC1_UNORM;
                    case 0x33545844: // 'DXT3'
                        return TexPixelFormat.FORMAT_BC2_UNORM;
                    case 0x35545844: // 'DXT5'
                        return TexPixelFormat.FORMAT_BC3_UNORM_SRGB;
                    default:
                        throw new Exception("Unsupported FOURCC format: " + ddsTexture.Header.PixelFormat.FourCc);
                }
            case DdsPixelFormatFlag.DDS_ALPHAPIXELS | DdsPixelFormatFlag.DDS_RGB:
                switch (ddsTexture.Header.PixelFormat.RgbBitCount)
                {
                    case 32:
                        if (ddsTexture.Header.PixelFormat.RBitMask != 0x00_00_FF_00 ||
                            ddsTexture.Header.PixelFormat.GBitMask != 0x00_FF_00_00 ||
                            ddsTexture.Header.PixelFormat.BBitMask != 0xFF_00_00_00 ||
                            ddsTexture.Header.PixelFormat.ABitMask != 0x00_00_00_FF)
                        {
                            throw new Exception("Unsupported RGBA bit mask");
                        }
                        return TexPixelFormat.FORMAT_B8G8R8A8_UNORM;
                    case 16:
                        if (ddsTexture.Header.PixelFormat.RBitMask != 0b0000_0000_1111_0000 ||
                            ddsTexture.Header.PixelFormat.GBitMask != 0b0000_1111_0000_0000 ||
                            ddsTexture.Header.PixelFormat.BBitMask != 0b1111_0000_0000_0000 ||
                            ddsTexture.Header.PixelFormat.ABitMask != 0b0000_0000_0000_1111)
                        {
                            throw new Exception("Unsupported RGBA bit mask");
                        }
                        return TexPixelFormat.FORMAT_B4G4R4A4_UNORM;
                    default:
                        throw new Exception("Unsupported RGBA bit count: " + ddsTexture.Header.PixelFormat.RgbBitCount);
                }
            default:
                throw new Exception("Unsupported DDS pixel format flag: " + ddsTexture.Header.PixelFormat.Flags.ToString("X8"));
        }
    }
}
