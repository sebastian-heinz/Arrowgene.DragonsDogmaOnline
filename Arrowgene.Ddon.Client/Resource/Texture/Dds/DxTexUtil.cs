using System;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds
{
    public static class DxTexUtil
    {
        public static LegacyDds[] LegacyDdsMap = new LegacyDds[]
        {
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC1_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_DXT1
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC2_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_DXT3
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC3_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_DXT5
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC2_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_PMALPHA,
                PixelFormat = DDSPixelFormat.DDSPF_DXT2
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC3_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_PMALPHA,
                PixelFormat = DDSPixelFormat.DDSPF_DXT4
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC4_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_BC4_UNORM
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC4_SNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_BC4_SNORM
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC5_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_BC5_UNORM
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC5_SNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_BC5_SNORM
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC4_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC,
                    DDSPixelFormat.MakeFourCc('A', 'T', 'I', '1'), 0, 0, 0, 0, 0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC5_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC,
                    DDSPixelFormat.MakeFourCc('A', 'T', 'I', '2'), 0, 0, 0, 0, 0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC6H_UF16, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC,
                    DDSPixelFormat.MakeFourCc('B', 'C', '6', 'H'), 0, 0, 0, 0, 0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC7_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC,
                    DDSPixelFormat.MakeFourCc('B', 'C', '7', 'L'), 0, 0, 0, 0, 0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_BC7_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC,
                    DDSPixelFormat.MakeFourCc('B', 'C', '7', '\0'), 0, 0, 0, 0, 0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8_B8G8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_R8G8_B8G8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_G8R8_G8B8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_G8R8_G8B8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A8R8G8B8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_X8R8G8B8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A8B8G8R8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NOALPHA,
                PixelFormat = DDSPixelFormat.DDSPF_X8B8G8R8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16G16_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_G16R16
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R10G10B10A2_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_SWIZZLE,
                PixelFormat = DDSPixelFormat.DDSPF_A2R10G10B10
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R10G10B10A2_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A2B10G10R10
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_EXPAND | ConversionFlags.CONV_FLAGS_NOALPHA |
                            ConversionFlags.CONV_FLAGS_888,
                PixelFormat = DDSPixelFormat.DDSPF_R8G8B8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B5G6R5_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_565,
                PixelFormat = DDSPixelFormat.DDSPF_R5G6B5
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B5G5R5A1_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_5551,
                PixelFormat = DDSPixelFormat.DDSPF_A1R5G5B5
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B5G5R5A1_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_5551 | ConversionFlags.CONV_FLAGS_NOALPHA,
                PixelFormat = DDSPixelFormat.DDSPF_X1R5G5B5
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_EXPAND | ConversionFlags.CONV_FLAGS_8332,
                PixelFormat = DDSPixelFormat.DDSPF_A8R3G3B2
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B5G6R5_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_EXPAND | ConversionFlags.CONV_FLAGS_332,
                PixelFormat = DDSPixelFormat.DDSPF_R3G3B2
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_L8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_L16
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A8L8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A8L8_ALT
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_L8_NVTT1
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_L16_NVTT1
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A8L8_NVTT1
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_A8_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_A8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16G16B16A16_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 36, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16G16B16A16_SNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 110, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 111, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16G16_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 112, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16G16B16A16_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 113, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R32_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 114, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R32G32_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 115, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R32G32B32A32_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_FOURCC, 116, 0, 0, 0,
                    0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R32_FLOAT, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_RGB, 0, 32,
                    0xffffffff,
                    0,
                    0, 0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_EXPAND | ConversionFlags.CONV_FLAGS_PAL8 |
                            ConversionFlags.CONV_FLAGS_A8P8,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDSPAL8A, 0, 16, 0, 0, 0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_EXPAND | ConversionFlags.CONV_FLAGS_PAL8,
                PixelFormat = new DDSPixelFormat(DDSPixelFormat.StructSize, DdsPixelFormatFlag.DDS_PAL8, 0, 8, 0, 0, 0,
                    0)
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B4G4R4A4_UNORM, ConvFlags = ConversionFlags.CONV_FLAGS_4444,
                PixelFormat = DDSPixelFormat.DDSPF_A4R4G4B4
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B4G4R4A4_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_NOALPHA | ConversionFlags.CONV_FLAGS_4444,
                PixelFormat = DDSPixelFormat.DDSPF_X4R4G4B4
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_B4G4R4A4_UNORM,
                ConvFlags = ConversionFlags.CONV_FLAGS_EXPAND | ConversionFlags.CONV_FLAGS_44,
                PixelFormat = DDSPixelFormat.DDSPF_A4L4
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_YUY2, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_YUY2
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_YUY2, ConvFlags = ConversionFlags.CONV_FLAGS_SWIZZLE,
                PixelFormat = DDSPixelFormat.DDSPF_UYVY
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8_SNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_V8U8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R8G8B8A8_SNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_Q8W8V8U8
            },
            new LegacyDds()
            {
                Format = DxGiFormat.DXGI_FORMAT_R16G16_SNORM, ConvFlags = ConversionFlags.CONV_FLAGS_NONE,
                PixelFormat = DDSPixelFormat.DDSPF_V16U16
            },
        };


        public static bool SetupImageArray(
            out Image[] images,
            ulong pixelSize,
            TexMetadata metadata,
            CpFlags cpFlags,
            ulong numberImages)
        {
            images = null;
            ulong index = 0;
            ulong pixels = 0;

            switch (metadata.Dimension)
            {
                case TexDimension.Texture1D:
                case TexDimension.Texture2D:
                {
                    if (metadata.ArraySize == 0 || metadata.MipLevels == 0)
                    {
                        return false;
                    }

                    images = new Image[metadata.ArraySize * metadata.MipLevels];

                    for (ulong item = 0; item < metadata.ArraySize; ++item)
                    {
                        ulong w = metadata.Width;
                        ulong h = metadata.Height;

                        for (ulong level = 0; level < metadata.MipLevels; ++level)
                        {
                            if (index >= numberImages)
                            {
                                return false;
                            }

                            if (!ComputePitch(metadata.Format, w, h, out ulong rowPitch, out ulong slicePitch, cpFlags))
                            {
                                return false;
                            }

                            images[index].Width = w;
                            images[index].Height = h;
                            images[index].Format = metadata.Format;
                            images[index].RowPitch = rowPitch;
                            images[index].SlicePitch = slicePitch;
                            images[index].PixelsOffset = pixels;
                            images[index].PixelsSize = slicePitch;
                            ++index;

                            pixels += slicePitch;
                            if (pixels > pixelSize)
                            {
                                return false;
                            }

                            if (h > 1)
                            {
                                h >>= 1;
                            }

                            if (w > 1)
                            {
                                w >>= 1;
                            }
                        }
                    }

                    return true;
                }
                case TexDimension.Texture3D:
                {
                    if (metadata.MipLevels == 0 || metadata.Depth == 0)
                    {
                        return false;
                    }

                    ulong w = metadata.Width;
                    ulong h = metadata.Height;
                    ulong d = metadata.Depth;

                    for (ulong level = 0; level < metadata.MipLevels; ++level)
                    {
                        if (!ComputePitch(metadata.Format, w, h, out ulong rowPitch, out ulong slicePitch, cpFlags))
                        {
                            return false;
                        }

                        for (ulong slice = 0; slice < d; ++slice)
                        {
                            if (index >= numberImages)
                            {
                                return false;
                            }

                            // We use the same memory organization that Direct3D 11 needs for D3D11_SUBRESOURCE_DATA
                            // with all slices of a given miplevel being continuous in memory
                            images[index].Width = w;
                            images[index].Height = h;
                            images[index].Format = metadata.Format;
                            images[index].RowPitch = rowPitch;
                            images[index].SlicePitch = slicePitch;
                            images[index].PixelsOffset = pixels;
                            images[index].PixelsSize = slicePitch;
                            ++index;

                            pixels += slicePitch;
                            if (pixels > pixelSize)
                            {
                                return false;
                            }
                        }

                        if (h > 1)
                        {
                            h >>= 1;
                        }

                        if (w > 1)
                        {
                            w >>= 1;
                        }

                        if (d > 1)
                        {
                            d >>= 1;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool DetermineImageArray(
            TexMetadata metadata,
            CpFlags cpFlags,
            out ulong numberImages,
            out ulong pixelSize
        )
        {
            ulong totalPixelSize = 0;
            ulong nImages = 0;


            switch (metadata.Dimension)
            {
                case TexDimension.Texture1D:
                case TexDimension.Texture2D:
                {
                    for (uint item = 0; item < metadata.ArraySize; ++item)
                    {
                        ulong w = metadata.Width;
                        ulong h = metadata.Height;

                        for (ulong level = 0; level < metadata.MipLevels; ++level)
                        {
                            if (!ComputePitch(metadata.Format, w, h, out ulong rowPitch, out ulong slicePitch, cpFlags))
                            {
                                numberImages = 0;
                                pixelSize = 0;
                                return false;
                            }


                            totalPixelSize += slicePitch;
                            ++nImages;

                            if (h > 1)
                            {
                                h >>= 1;
                            }

                            if (w > 1)
                            {
                                w >>= 1;
                            }
                        }
                    }

                    break;
                }
                case TexDimension.Texture3D:
                {
                    ulong w = metadata.Width;
                    ulong h = metadata.Height;
                    ulong d = metadata.Depth;

                    for (ulong level = 0; level < metadata.MipLevels; ++level)
                    {
                        if (!ComputePitch(metadata.Format, w, h, out ulong rowPitch, out ulong slicePitch, cpFlags))
                        {
                            numberImages = 0;
                            pixelSize = 0;
                            return false;
                        }

                        for (ulong slice = 0; slice < d; ++slice)
                        {
                            totalPixelSize += slicePitch;
                            ++nImages;
                        }

                        if (h > 1)
                        {
                            h >>= 1;
                        }

                        if (w > 1)
                        {
                            w >>= 1;
                        }

                        if (d > 1)
                        {
                            d >>= 1;
                        }
                    }

                    break;
                }
            }

            numberImages = nImages;
            pixelSize = totalPixelSize;
            return true;
        }

        public static bool CalculateMipLevels(uint width, uint height, ref uint mipLevels)
        {
            if (mipLevels > 1)
            {
                uint maxMips = CountMips(width, height);
                if (mipLevels > maxMips)
                {
                    return false;
                }
            }
            else if (mipLevels == 0)
            {
                mipLevels = CountMips(width, height);
            }
            else
            {
                mipLevels = 1;
            }

            return true;
        }

        public static uint CountMips(uint width, uint height)
        {
            uint mipLevels = 1;
            while (height > 1 || width > 1)
            {
                if (height > 1)
                    height >>= 1;

                if (width > 1)
                    width >>= 1;

                ++mipLevels;
            }

            return mipLevels;
        }

        public static bool ComputePitch(DxGiFormat fmt, ulong width, ulong height,
            out ulong rowPitch, out ulong slicePitch, CpFlags flags)
        {
            ulong pitch = 0;
            ulong slice = 0;

            switch (fmt)
            {
                case DxGiFormat.DXGI_FORMAT_BC1_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC1_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_BC4_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC4_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC4_SNORM:
                    //assert(IsCompressed(fmt));
                {
                    if (flags.HasFlag(CpFlags.BadDxtnTails))
                    {
                        ulong nbw = width >> 2;
                        ulong nbh = height >> 2;
                        pitch = Math.Max(1u, nbw * 8u);
                        slice = Math.Max(1u, pitch * nbh);
                    }
                    else
                    {
                        ulong nbw = Math.Max(1u, (width + 3u) / 4u);
                        ulong nbh = Math.Max(1u, (height + 3u) / 4u);
                        pitch = nbw * 8u;
                        slice = pitch * nbh;
                    }

                    break;
                }
                case DxGiFormat.DXGI_FORMAT_BC2_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC2_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC2_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_BC3_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC3_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_BC5_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC5_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC5_SNORM:
                case DxGiFormat.DXGI_FORMAT_BC6H_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC6H_UF16:
                case DxGiFormat.DXGI_FORMAT_BC6H_SF16:
                case DxGiFormat.DXGI_FORMAT_BC7_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC7_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC7_UNORM_SRGB:
                    //assert(IsCompressed(fmt));
                {
                    if (flags.HasFlag(CpFlags.BadDxtnTails))
                    {
                        ulong nbw = width >> 2;
                        ulong nbh = height >> 2;
                        pitch = Math.Max(1u, nbw * 16u);
                        slice = Math.Max(1u, pitch * nbh);
                    }
                    else
                    {
                        ulong nbw = Math.Max(1u, (width + 3u) / 4u);
                        ulong nbh = Math.Max(1u, (height + 3u) / 4u);
                        pitch = nbw * 16u;
                        slice = pitch * nbh;
                    }

                    break;
                }
                case DxGiFormat.DXGI_FORMAT_R8G8_B8G8_UNORM:
                case DxGiFormat.DXGI_FORMAT_G8R8_G8B8_UNORM:
                case DxGiFormat.DXGI_FORMAT_YUY2:
                {
                    // assert(IsPacked(fmt));
                    pitch = ((width + 1u) >> 1) * 4u;
                    slice = pitch * height;
                    break;
                }
                case DxGiFormat.DXGI_FORMAT_Y210:
                case DxGiFormat.DXGI_FORMAT_Y216:
                {
                    // assert(IsPacked(fmt));
                    pitch = ((width + 1u) >> 1) * 8u;
                    slice = pitch * height;
                    break;
                }

                case DxGiFormat.DXGI_FORMAT_NV12:
                case DxGiFormat.DXGI_FORMAT_420_OPAQUE:
                {
                    //  assert(IsPlanar(fmt));
                    pitch = ((width + 1u) >> 1) * 2u;
                    slice = pitch * (height + ((height + 1u) >> 1));
                    break;
                }
                case DxGiFormat.DXGI_FORMAT_P010:
                case DxGiFormat.DXGI_FORMAT_P016:
                case DxGiFormat.XBOX_DXGI_FORMAT_D16_UNORM_S8_UINT:
                case DxGiFormat.XBOX_DXGI_FORMAT_R16_UNORM_X8_TYPELESS:
                case DxGiFormat.XBOX_DXGI_FORMAT_X16_TYPELESS_G8_UINT:
                {
                    //assert(IsPlanar(fmt));
                    pitch = ((width + 1u) >> 1) * 4u;
                    slice = pitch * (height + ((height + 1u) >> 1));
                    break;
                }
                case DxGiFormat.DXGI_FORMAT_NV11:
                {
                    //   assert(IsPlanar(fmt));
                    pitch = ((width + 3u) >> 2) * 4u;
                    slice = pitch * height * 2u;
                    break;
                }
                case DxGiFormat.WIN10_DXGI_FORMAT_P208:
                {
                    //    assert(IsPlanar(fmt));
                    pitch = ((width + 1u) >> 1) * 2u;
                    slice = pitch * height * 2u;
                    break;
                }
                case DxGiFormat.WIN10_DXGI_FORMAT_V208:
                {
                    //    assert(IsPlanar(fmt));
                    pitch = width;
                    slice = pitch * (height + (((height + 1u) >> 1) * 2u));
                    break;
                }
                case DxGiFormat.WIN10_DXGI_FORMAT_V408:
                {
                    //  assert(IsPlanar(fmt));
                    pitch = width;
                    slice = pitch * (height + (height >> 1) * 4u);
                    break;
                }
                default:
                {
                    //    assert(!IsCompressed(fmt) && !IsPacked(fmt) && !IsPlanar(fmt));
                    ulong bpp;

                    if (flags.HasFlag(CpFlags._24BPP))
                        bpp = 24;
                    else if (flags.HasFlag(CpFlags._16BPP))
                        bpp = 16;
                    else if (flags.HasFlag(CpFlags._8BPP))
                        bpp = 8;
                    else
                        bpp = BitsPerPixel(fmt);

                    if (bpp == 0)
                    {
                        rowPitch = 0;
                        slicePitch = 0;
                        return false;
                    }

                    if (flags.HasFlag(CpFlags.LegacyDword
                                      | CpFlags.Paragraph
                                      | CpFlags.Ymm
                                      | CpFlags.Zmm
                                      | CpFlags.Page4K)
                       )
                    {
                        if (flags.HasFlag(CpFlags.Page4K))
                        {
                            pitch = ((width * bpp + 32767u) / 32768u) * 4096u;
                            slice = pitch * height;
                        }
                        else if (flags.HasFlag(CpFlags.Zmm))
                        {
                            pitch = ((width * bpp + 511u) / 512u) * 64u;
                            slice = pitch * height;
                        }
                        else if (flags.HasFlag(CpFlags.Ymm))
                        {
                            pitch = ((width * bpp + 255u) / 256u) * 32u;
                            slice = pitch * height;
                        }
                        else if (flags.HasFlag(CpFlags.Paragraph))
                        {
                            pitch = ((width * bpp + 127u) / 128u) * 16u;
                            slice = pitch * height;
                        }
                        else // DWORD alignment
                        {
                            // Special computation for some incorrectly created DDS files based on
                            // legacy DirectDraw assumptions about pitch alignment
                            pitch = ((width * bpp + 31u) / 32u) * sizeof(uint);
                            slice = pitch * height;
                        }
                    }
                    else
                    {
                        // Default byte alignment
                        pitch = (width * bpp + 7u) / 8u;
                        slice = pitch * height;
                    }

                    break;
                }
            }

            rowPitch = pitch;
            slicePitch = slice;

            return true;
        }

        public static uint BitsPerPixel(DxGiFormat fmt)
        {
            switch (fmt)
            {
                case DxGiFormat.DXGI_FORMAT_R32G32B32A32_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R32G32B32A32_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R32G32B32A32_UINT:
                case DxGiFormat.DXGI_FORMAT_R32G32B32A32_SINT:
                    return 128;

                case DxGiFormat.DXGI_FORMAT_R32G32B32_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R32G32B32_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R32G32B32_UINT:
                case DxGiFormat.DXGI_FORMAT_R32G32B32_SINT:
                    return 96;

                case DxGiFormat.DXGI_FORMAT_R16G16B16A16_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R16G16B16A16_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R16G16B16A16_UNORM:
                case DxGiFormat.DXGI_FORMAT_R16G16B16A16_UINT:
                case DxGiFormat.DXGI_FORMAT_R16G16B16A16_SNORM:
                case DxGiFormat.DXGI_FORMAT_R16G16B16A16_SINT:
                case DxGiFormat.DXGI_FORMAT_R32G32_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R32G32_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R32G32_UINT:
                case DxGiFormat.DXGI_FORMAT_R32G32_SINT:
                case DxGiFormat.DXGI_FORMAT_R32G8X24_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_D32_FLOAT_S8X24_UINT:
                case DxGiFormat.DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_X32_TYPELESS_G8X24_UINT:
                case DxGiFormat.DXGI_FORMAT_Y416:
                case DxGiFormat.DXGI_FORMAT_Y210:
                case DxGiFormat.DXGI_FORMAT_Y216:
                    return 64;

                case DxGiFormat.DXGI_FORMAT_R10G10B10A2_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R10G10B10A2_UNORM:
                case DxGiFormat.DXGI_FORMAT_R10G10B10A2_UINT:
                case DxGiFormat.DXGI_FORMAT_R11G11B10_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM:
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_UINT:
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_SNORM:
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_SINT:
                case DxGiFormat.DXGI_FORMAT_R16G16_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R16G16_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R16G16_UNORM:
                case DxGiFormat.DXGI_FORMAT_R16G16_UINT:
                case DxGiFormat.DXGI_FORMAT_R16G16_SNORM:
                case DxGiFormat.DXGI_FORMAT_R16G16_SINT:
                case DxGiFormat.DXGI_FORMAT_R32_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_D32_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R32_FLOAT:
                case DxGiFormat.DXGI_FORMAT_R32_UINT:
                case DxGiFormat.DXGI_FORMAT_R32_SINT:
                case DxGiFormat.DXGI_FORMAT_R24G8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_D24_UNORM_S8_UINT:
                case DxGiFormat.DXGI_FORMAT_R24_UNORM_X8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_X24_TYPELESS_G8_UINT:
                case DxGiFormat.DXGI_FORMAT_R9G9B9E5_SHAREDEXP:
                case DxGiFormat.DXGI_FORMAT_R8G8_B8G8_UNORM:
                case DxGiFormat.DXGI_FORMAT_G8R8_G8B8_UNORM:
                case DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM:
                case DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM:
                case DxGiFormat.DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM:
                case DxGiFormat.DXGI_FORMAT_B8G8R8A8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_B8G8R8X8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_AYUV:
                case DxGiFormat.DXGI_FORMAT_Y410:
                case DxGiFormat.DXGI_FORMAT_YUY2:
                case DxGiFormat.XBOX_DXGI_FORMAT_R10G10B10_7E3_A2_FLOAT:
                case DxGiFormat.XBOX_DXGI_FORMAT_R10G10B10_6E4_A2_FLOAT:
                case DxGiFormat.XBOX_DXGI_FORMAT_R10G10B10_SNORM_A2_UNORM:
                    return 32;

                case DxGiFormat.DXGI_FORMAT_P010:
                case DxGiFormat.DXGI_FORMAT_P016:
                case DxGiFormat.XBOX_DXGI_FORMAT_D16_UNORM_S8_UINT:
                case DxGiFormat.XBOX_DXGI_FORMAT_R16_UNORM_X8_TYPELESS:
                case DxGiFormat.XBOX_DXGI_FORMAT_X16_TYPELESS_G8_UINT:
                case DxGiFormat.WIN10_DXGI_FORMAT_V408:
                    return 24;

                case DxGiFormat.DXGI_FORMAT_R8G8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R8G8_UNORM:
                case DxGiFormat.DXGI_FORMAT_R8G8_UINT:
                case DxGiFormat.DXGI_FORMAT_R8G8_SNORM:
                case DxGiFormat.DXGI_FORMAT_R8G8_SINT:
                case DxGiFormat.DXGI_FORMAT_R16_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R16_FLOAT:
                case DxGiFormat.DXGI_FORMAT_D16_UNORM:
                case DxGiFormat.DXGI_FORMAT_R16_UNORM:
                case DxGiFormat.DXGI_FORMAT_R16_UINT:
                case DxGiFormat.DXGI_FORMAT_R16_SNORM:
                case DxGiFormat.DXGI_FORMAT_R16_SINT:
                case DxGiFormat.DXGI_FORMAT_B5G6R5_UNORM:
                case DxGiFormat.DXGI_FORMAT_B5G5R5A1_UNORM:
                case DxGiFormat.DXGI_FORMAT_A8P8:
                case DxGiFormat.DXGI_FORMAT_B4G4R4A4_UNORM:
                case DxGiFormat.WIN10_DXGI_FORMAT_P208:
                case DxGiFormat.WIN10_DXGI_FORMAT_V208:
                    return 16;

                case DxGiFormat.DXGI_FORMAT_NV12:
                case DxGiFormat.DXGI_FORMAT_420_OPAQUE:
                case DxGiFormat.DXGI_FORMAT_NV11:
                    return 12;

                case DxGiFormat.DXGI_FORMAT_R8_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_R8_UNORM:
                case DxGiFormat.DXGI_FORMAT_R8_UINT:
                case DxGiFormat.DXGI_FORMAT_R8_SNORM:
                case DxGiFormat.DXGI_FORMAT_R8_SINT:
                case DxGiFormat.DXGI_FORMAT_A8_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC2_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC2_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC2_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_BC3_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC3_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_BC5_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC5_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC5_SNORM:
                case DxGiFormat.DXGI_FORMAT_BC6H_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC6H_UF16:
                case DxGiFormat.DXGI_FORMAT_BC6H_SF16:
                case DxGiFormat.DXGI_FORMAT_BC7_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC7_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC7_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_AI44:
                case DxGiFormat.DXGI_FORMAT_IA44:
                case DxGiFormat.DXGI_FORMAT_P8:
                case DxGiFormat.XBOX_DXGI_FORMAT_R4G4_UNORM:
                    return 8;

                case DxGiFormat.DXGI_FORMAT_R1_UNORM:
                    return 1;

                case DxGiFormat.DXGI_FORMAT_BC1_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC1_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB:
                case DxGiFormat.DXGI_FORMAT_BC4_TYPELESS:
                case DxGiFormat.DXGI_FORMAT_BC4_UNORM:
                case DxGiFormat.DXGI_FORMAT_BC4_SNORM:
                    return 4;

                default:
                    return 0;
            }
        }

        public static DxGiFormat GetDxGiFormat(DdsHeader hdr, DDSPixelFormat ddpf, DdsFlags flags,
            out ConversionFlags convFlags)
        {
            DdsPixelFormatFlag ddpfFlags = ddpf.Flags;
            if (hdr.Reserved1[9] == DDSPixelFormat.MakeFourCc('N', 'V', 'T', 'T'))
            {
                // Clear out non-standard nVidia DDS flags
                ddpfFlags &= ~(DdsPixelFormatFlag) 0xC0000000 /* DDPF_SRGB | DDPF_NORMAL */;
            }

            uint index = 0;
            for (index = 0; index < LegacyDdsMap.Length; ++index)
            {
                LegacyDds entry = LegacyDdsMap[index];


                if (ddpfFlags.HasFlag(DdsPixelFormatFlag.DDS_FOURCC) &&
                    entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_FOURCC))
                {
                    // In case of FourCC codes, ignore any other bits in ddpf.flags
                    if (ddpf.FourCc == entry.PixelFormat.FourCc)
                        break;
                }
                else if (ddpfFlags == entry.PixelFormat.Flags)
                {
                    if (entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_PAL8))
                    {
                        if (ddpf.RgbBitCount == entry.PixelFormat.RgbBitCount)
                            break;
                    }
                    else if (entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_ALPHA))
                    {
                        if (ddpf.RgbBitCount == entry.PixelFormat.RgbBitCount
                            && ddpf.ABitMask == entry.PixelFormat.ABitMask)
                            break;
                    }
                    else if (entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_LUMINANCE))
                    {
                        if (entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_ALPHAPIXELS))
                        {
                            // LUMINANCEA
                            if (ddpf.RgbBitCount == entry.PixelFormat.RgbBitCount
                                && ddpf.RBitMask == entry.PixelFormat.RBitMask
                                && ddpf.ABitMask == entry.PixelFormat.ABitMask)
                                break;
                        }
                        else
                        {
                            // LUMINANCE
                            if (ddpf.RgbBitCount == entry.PixelFormat.RgbBitCount
                                && ddpf.RBitMask == entry.PixelFormat.RBitMask)
                                break;
                        }
                    }
                    else if (entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_BUMPDUDV))
                    {
                        if (ddpf.RgbBitCount == entry.PixelFormat.RgbBitCount
                            && ddpf.RBitMask == entry.PixelFormat.RBitMask
                            && ddpf.GBitMask == entry.PixelFormat.GBitMask
                            && ddpf.BBitMask == entry.PixelFormat.BBitMask
                            && ddpf.ABitMask == entry.PixelFormat.ABitMask)
                            break;
                    }
                    else if (ddpf.RgbBitCount == entry.PixelFormat.RgbBitCount)
                    {
                        if (entry.PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.DDS_ALPHAPIXELS))
                        {
                            // RGBA
                            if (ddpf.RBitMask == entry.PixelFormat.RBitMask
                                && ddpf.GBitMask == entry.PixelFormat.GBitMask
                                && ddpf.BBitMask == entry.PixelFormat.BBitMask
                                && ddpf.ABitMask == entry.PixelFormat.ABitMask)
                                break;
                        }
                        else
                        {
                            // RGB
                            if (ddpf.RBitMask == entry.PixelFormat.RBitMask
                                && ddpf.GBitMask == entry.PixelFormat.GBitMask
                                && ddpf.BBitMask == entry.PixelFormat.BBitMask)
                                break;
                        }
                    }
                }
            }

            if (index >= LegacyDdsMap.Length)
            {
                convFlags = 0;
                return DxGiFormat.DXGI_FORMAT_UNKNOWN;
            }


            ConversionFlags cflags = LegacyDdsMap[index].ConvFlags;
            DxGiFormat format = LegacyDdsMap[index].Format;

            if (cflags.HasFlag(ConversionFlags.CONV_FLAGS_EXPAND) && flags.HasFlag(DdsFlags.NoLegacyExpansion))
            {
                convFlags = 0;
                return DxGiFormat.DXGI_FORMAT_UNKNOWN;
            }


            if (format == DxGiFormat.DXGI_FORMAT_R10G10B10A2_UNORM && flags.HasFlag(DdsFlags.NoR10B10G10A2Fixup))
            {
                cflags ^= ConversionFlags.CONV_FLAGS_SWIZZLE;
            }

            if (hdr.Reserved1[9] == DDSPixelFormat.MakeFourCc('N', 'V', 'T', 'T')
                && ddpf.Flags.HasFlag((DdsPixelFormatFlag) 0x40000000)) /* DDPF_SRGB */
            {
                format = MakeSRGB(format);
            }

            convFlags = cflags;

            return format;
        }
        
        public static DxGiFormat MakeSRGB(DxGiFormat fmt)
        {
            switch (fmt)
            {
                case DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM:
                    return DxGiFormat.DXGI_FORMAT_R8G8B8A8_UNORM_SRGB;

                case DxGiFormat.DXGI_FORMAT_BC1_UNORM:
                    return DxGiFormat.DXGI_FORMAT_BC1_UNORM_SRGB;

                case DxGiFormat.DXGI_FORMAT_BC2_UNORM:
                    return DxGiFormat.DXGI_FORMAT_BC2_UNORM_SRGB;

                case DxGiFormat.DXGI_FORMAT_BC3_UNORM:
                    return DxGiFormat.DXGI_FORMAT_BC3_UNORM_SRGB;

                case DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM:
                    return DxGiFormat.DXGI_FORMAT_B8G8R8A8_UNORM_SRGB;

                case DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM:
                    return DxGiFormat.DXGI_FORMAT_B8G8R8X8_UNORM_SRGB;

                case DxGiFormat.DXGI_FORMAT_BC7_UNORM:
                    return DxGiFormat.DXGI_FORMAT_BC7_UNORM_SRGB;

                default:
                    return fmt;
            }
        }
    }
}
