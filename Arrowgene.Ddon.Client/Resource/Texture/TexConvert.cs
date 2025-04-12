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
        ddsTexture.Magic = DdsHeader.Magic;
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

    public static TexTexture ToTexTexture(DdsTexture ddsTexture, TexHeader originalTexHeader)
    {
        TexTexture texTexture = new TexTexture();
        texTexture.Magic = TexTexture.TexHeaderMagic;
        texTexture.Header.Version = originalTexHeader.Version;
        texTexture.Header.Height = (uint) ddsTexture.Metadata.Height;
        texTexture.Header.Width = (uint) ddsTexture.Metadata.Width;
        texTexture.Header.Shift = originalTexHeader.Shift;
        texTexture.Header.Alpha = originalTexHeader.Alpha;
        texTexture.Header.Type = originalTexHeader.Type;
        texTexture.Header.Depth = (uint) ddsTexture.Metadata.Depth;
        texTexture.Header.PixelFormat = originalTexHeader.PixelFormat;
        texTexture.Header.TextureArraySize = (byte) ddsTexture.Metadata.ArraySize;
        texTexture.Header.MipMapCount = (uint) ddsTexture.Metadata.MipLevels;
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

    public static string DumpTexHeader(TexHeader header)
    {
        // Dump using the ARCtool format for compatibility
        return $"TEX\n" +
               $"Texversion=RE6\n" +
               $"Textype={(int) header.PixelFormat}\n" +
               $"Width={header.Width}\n" +
               $"Height={header.Height}\n" +
               $"Mips={header.MipMapCount}\n" +
               $"Uint1={header._header4}\n" +
               $"Byte1={header.TextureArraySize}\n" +
               $"Byte2={header.Depth}\n" +
               $"Byte3={header.UnknownB}\n" +
               $"DDSFormat={DDSPixelFormat.FormatFourCc(GetDDSPixelFormat(header).FourCc)}";
    }

    public static TexHeader ReadTexHeaderDump(string dump)
    {
        // Read a TexHeader dump in the ARCtool format
        string[] lines = dump.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (!lines.Any(line => line.Trim() == "TEX"))
        {
            throw new Exception("Invalid TEX header dump format.");
        }

        TexHeader header = new TexHeader();

        foreach (string line in lines)
        {
            string[] parts = line.Split('=');
            if (parts.Length != 2) continue;

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            switch (key)
            {
                case "Textype":
                    header.PixelFormat = (TexPixelFormat)int.Parse(value);
                    break;
                case "Width":
                    header.Width = uint.Parse(value);
                    break;
                case "Height":
                    header.Height = uint.Parse(value);
                    break;
                case "Mips":
                    header.MipMapCount = uint.Parse(value);
                    break;
                case "Uint1":
                    header._header4 = uint.Parse(value);
                    TexHeader._parseHeader4(header._header4, out header.Version, out header.Alpha, out header.Shift, out header.Type);
                    break;
                case "Byte1":
                    header.TextureArraySize = byte.Parse(value);
                    break;
                case "Byte2":
                    header.Depth = uint.Parse(value);
                    break;
                case "Byte3":
                    header.UnknownB = uint.Parse(value);
                    break;
            }
        }

        return header;
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
}