using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds
{
    public static class DirectXTexUtility
    {
        
        public static TexLayer[] SetupImageArray(
            DirectXTexUtility.DXGIFormat format,
            DirectXTexUtility.CPFLAGS flags,
            uint arraySize,
            uint mipLevels,
            uint width,
            uint height,
            uint pixelSize,
            uint nImages)
        {
            TexLayer[] layers = new TexLayer[arraySize * mipLevels];
            uint index = 0;
            uint pixels = 0;
            for (uint item = 0; item < arraySize; ++item)
            {
                uint w = width;
                uint h = height;

                for (uint level = 0; level < mipLevels; ++level)
                {
                    if (index >= nImages)
                    {
                        return layers;
                    }

                    DirectXTexUtility.ComputePitch(format, w, h, out long rowPitch, out long slicePitch, flags);

                    //   size_t rowPitch, slicePitch;
                    //   if (FAILED(ComputePitch(metadata.format, w, h, rowPitch, slicePitch, cpFlags)))
                    //       return false;

                    layers[index].Offset = pixels;
                    //    images[index].width = w;
                    //    images[index].height = h;
                    //    images[index].format = metadata.format;
                    //    images[index].rowPitch = rowPitch;
                    //    images[index].slicePitch = slicePitch;
                    //    images[index].pixels = pixels;
                    ++index;
//
                    pixels += (uint) slicePitch;
                    //  if (pixels > pEndBits)
                    //  {
                    //      return false;
                    //  }

                    if (h > 1)
                        h >>= 1;

                    if (w > 1)
                        w >>= 1;
                }
            }

            return layers;
        }

        public static bool DetermineImageArray(
            DirectXTexUtility.DXGIFormat format,
            DirectXTexUtility.CPFLAGS flags,
            uint arraySize,
            uint mipLevels,
            uint width,
            uint height,
            out uint nImages,
            out uint pixelSize
        )
        {
            ulong totalPixelSize = 0;
            uint nimages = 0;

            for (uint item = 0; item < arraySize; ++item)
            {
                uint w = width;
                uint h = height;

                for (uint level = 0; level < mipLevels; ++level)
                {
                    DirectXTexUtility.ComputePitch(format, w, h, out long rowPitch, out long slicePitch, flags);

                    // if (FAILED(ComputePitch(metadata.format, w, h, rowPitch, slicePitch, cpFlags)))
                    // {
                    //     nImages = pixelSize = 0;
                    //     return false;
                    // }

                    totalPixelSize += (uint) slicePitch;
                    ++nimages;

                    if (h > 1)
                        h >>= 1;

                    if (w > 1)
                        w >>= 1;
                }
            }

            nImages = nimages;
            pixelSize = (uint) totalPixelSize;
            return true;
        }

        public static uint CalculateMipLevels(uint width, uint height, uint mipLevels)
        {
            if (mipLevels > 1)
            {
                uint maxMips = CountMips(width, height);
                if (mipLevels > maxMips)
                {
                    throw new Exception("mipLevels > maxMips");
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

            return mipLevels;
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

        /// <summary>
        /// Gets the Bits Per Pixel for the given format
        /// </summary>
        private static long BitsPerPixel(DXGIFormat format)
        {
            switch (format)
            {
                case DXGIFormat.R32G32B32A32TYPELESS:
                case DXGIFormat.R32G32B32A32FLOAT:
                case DXGIFormat.R32G32B32A32UINT:
                case DXGIFormat.R32G32B32A32SINT:
                    return 128;
                case DXGIFormat.R32G32B32TYPELESS:
                case DXGIFormat.R32G32B32FLOAT:
                case DXGIFormat.R32G32B32UINT:
                case DXGIFormat.R32G32B32SINT:
                    return 96;
                case DXGIFormat.R16G16B16A16TYPELESS:
                case DXGIFormat.R16G16B16A16FLOAT:
                case DXGIFormat.R16G16B16A16UNORM:
                case DXGIFormat.R16G16B16A16UINT:
                case DXGIFormat.R16G16B16A16SNORM:
                case DXGIFormat.R16G16B16A16SINT:
                case DXGIFormat.R32G32TYPELESS:
                case DXGIFormat.R32G32FLOAT:
                case DXGIFormat.R32G32UINT:
                case DXGIFormat.R32G32SINT:
                case DXGIFormat.R32G8X24TYPELESS:
                case DXGIFormat.D32FLOATS8X24UINT:
                case DXGIFormat.R32FLOATX8X24TYPELESS:
                case DXGIFormat.X32TYPELESSG8X24UINT:
                case DXGIFormat.Y416:
                case DXGIFormat.Y210:
                case DXGIFormat.Y216:
                    return 64;
                case DXGIFormat.R10G10B10A2TYPELESS:
                case DXGIFormat.R10G10B10A2UNORM:
                case DXGIFormat.R10G10B10A2UINT:
                case DXGIFormat.R11G11B10FLOAT:
                case DXGIFormat.R8G8B8A8TYPELESS:
                case DXGIFormat.R8G8B8A8UNORM:
                case DXGIFormat.R8G8B8A8UNORMSRGB:
                case DXGIFormat.R8G8B8A8UINT:
                case DXGIFormat.R8G8B8A8SNORM:
                case DXGIFormat.R8G8B8A8SINT:
                case DXGIFormat.R16G16TYPELESS:
                case DXGIFormat.R16G16FLOAT:
                case DXGIFormat.R16G16UNORM:
                case DXGIFormat.R16G16UINT:
                case DXGIFormat.R16G16SNORM:
                case DXGIFormat.R16G16SINT:
                case DXGIFormat.R32TYPELESS:
                case DXGIFormat.D32FLOAT:
                case DXGIFormat.R32FLOAT:
                case DXGIFormat.R32UINT:
                case DXGIFormat.R32SINT:
                case DXGIFormat.R24G8TYPELESS:
                case DXGIFormat.D24UNORMS8UINT:
                case DXGIFormat.R24UNORMX8TYPELESS:
                case DXGIFormat.X24TYPELESSG8UINT:
                case DXGIFormat.R9G9B9E5SHAREDEXP:
                case DXGIFormat.R8G8B8G8UNORM:
                case DXGIFormat.G8R8G8B8UNORM:
                case DXGIFormat.B8G8R8A8UNORM:
                case DXGIFormat.B8G8R8X8UNORM:
                case DXGIFormat.R10G10B10XRBIASA2UNORM:
                case DXGIFormat.B8G8R8A8TYPELESS:
                case DXGIFormat.B8G8R8A8UNORMSRGB:
                case DXGIFormat.B8G8R8X8TYPELESS:
                case DXGIFormat.B8G8R8X8UNORMSRGB:
                case DXGIFormat.AYUV:
                case DXGIFormat.Y410:
                case DXGIFormat.YUY2:
                    return 32;
                case DXGIFormat.P010:
                case DXGIFormat.P016:
                    return 24;
                case DXGIFormat.R8G8TYPELESS:
                case DXGIFormat.R8G8UNORM:
                case DXGIFormat.R8G8UINT:
                case DXGIFormat.R8G8SNORM:
                case DXGIFormat.R8G8SINT:
                case DXGIFormat.R16TYPELESS:
                case DXGIFormat.R16FLOAT:
                case DXGIFormat.D16UNORM:
                case DXGIFormat.R16UNORM:
                case DXGIFormat.R16UINT:
                case DXGIFormat.R16SNORM:
                case DXGIFormat.R16SINT:
                case DXGIFormat.B5G6R5UNORM:
                case DXGIFormat.B5G5R5A1UNORM:
                case DXGIFormat.A8P8:
                case DXGIFormat.B4G4R4A4UNORM:
                    return 16;
                case DXGIFormat.NV12:
                case DXGIFormat.OPAQUE420:
                case DXGIFormat.NV11:
                    return 12;
                case DXGIFormat.R8TYPELESS:
                case DXGIFormat.R8UNORM:
                case DXGIFormat.R8UINT:
                case DXGIFormat.R8SNORM:
                case DXGIFormat.R8SINT:
                case DXGIFormat.A8UNORM:
                case DXGIFormat.AI44:
                case DXGIFormat.IA44:
                case DXGIFormat.P8:
                    return 8;
                case DXGIFormat.R1UNORM:
                    return 1;
                case DXGIFormat.BC1TYPELESS:
                case DXGIFormat.BC1UNORM:
                case DXGIFormat.BC1UNORMSRGB:
                case DXGIFormat.BC4TYPELESS:
                case DXGIFormat.BC4UNORM:
                case DXGIFormat.BC4SNORM:
                    return 4;
                case DXGIFormat.BC2TYPELESS:
                case DXGIFormat.BC2UNORM:
                case DXGIFormat.BC2UNORMSRGB:
                case DXGIFormat.BC3TYPELESS:
                case DXGIFormat.BC3UNORM:
                case DXGIFormat.BC3UNORMSRGB:
                case DXGIFormat.BC5TYPELESS:
                case DXGIFormat.BC5UNORM:
                case DXGIFormat.BC5SNORM:
                case DXGIFormat.BC6HTYPELESS:
                case DXGIFormat.BC6HUF16:
                case DXGIFormat.BC6HSF16:
                case DXGIFormat.BC7TYPELESS:
                case DXGIFormat.BC7UNORM:
                case DXGIFormat.BC7UNORMSRGB:
                    return 8;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Computes Row and Slice Pitch
        /// </summary>
        public static void ComputePitch(DXGIFormat format, long width, long height, out long rowPitch,
            out long slicePitch, CPFLAGS flags)
        {
            switch (format)
            {
                case DXGIFormat.BC1TYPELESS:
                case DXGIFormat.BC1UNORM:
                case DXGIFormat.BC1UNORMSRGB:
                case DXGIFormat.BC4TYPELESS:
                case DXGIFormat.BC4UNORM:
                case DXGIFormat.BC4SNORM:
                {
                    if (flags.HasFlag(CPFLAGS.BADDXTNTAILS))
                    {
                        long nbw = width >> 2;
                        long nbh = height >> 2;
                        rowPitch = Clamp(1, nbw * 8, Int64.MaxValue);
                        slicePitch = Clamp(1, rowPitch * nbh, Int64.MaxValue);
                    }
                    else
                    {
                        long nbw = Math.Max(1, (width + 3) / 4);
                        long nbh = Math.Max(1, (height + 3) / 4);
                        // long nbw = Clamp(1, (width + 3) / 4, Int64.MaxValue);
                        //  long nbh = Clamp(1, (height + 3) / 4, Int64.MaxValue);
                        rowPitch = nbw * 8;
                        slicePitch = rowPitch * nbh;
                    }
                }
                    break;
                case DXGIFormat.BC2TYPELESS:
                case DXGIFormat.BC2UNORM:
                case DXGIFormat.BC2UNORMSRGB:
                case DXGIFormat.BC3TYPELESS:
                case DXGIFormat.BC3UNORM:
                case DXGIFormat.BC3UNORMSRGB:
                case DXGIFormat.BC5TYPELESS:
                case DXGIFormat.BC5UNORM:
                case DXGIFormat.BC5SNORM:
                case DXGIFormat.BC6HTYPELESS:
                case DXGIFormat.BC6HUF16:
                case DXGIFormat.BC6HSF16:
                case DXGIFormat.BC7TYPELESS:
                case DXGIFormat.BC7UNORM:
                case DXGIFormat.BC7UNORMSRGB:
                {
                    if (flags.HasFlag(CPFLAGS.BADDXTNTAILS))
                    {
                        long nbw = width >> 2;
                        long nbh = height >> 2;
                        rowPitch = Clamp(1, nbw * 16, Int64.MaxValue);
                        slicePitch = Clamp(1, rowPitch * nbh, Int64.MaxValue);
                    }
                    else
                    {
                        long nbw = Math.Max(1, (width + 3) / 4);
                        long nbh = Math.Max(1, (height + 3) / 4);
                        //  long nbw = Clamp(1, (width + 3) / 4, Int64.MaxValue);
                        //  long nbh = Clamp(1, (height + 3) / 4, Int64.MaxValue);
                        rowPitch = nbw * 16;
                        slicePitch = rowPitch * nbh;
                    }
                }
                    break;
                case DXGIFormat.R8G8B8G8UNORM:
                case DXGIFormat.G8R8G8B8UNORM:
                case DXGIFormat.YUY2:
                    rowPitch = ((width + 1) >> 1) * 4;
                    slicePitch = rowPitch * height;
                    break;
                case DXGIFormat.Y210:
                case DXGIFormat.Y216:
                    rowPitch = ((width + 1) >> 1) * 8;
                    slicePitch = rowPitch * height;
                    break;

                case DXGIFormat.NV12:
                case DXGIFormat.OPAQUE420:
                    rowPitch = ((width + 1) >> 1) * 2;
                    slicePitch = rowPitch * (height + ((height + 1) >> 1));
                    break;

                case DXGIFormat.P010:
                case DXGIFormat.P016:
                    rowPitch = ((width + 1) >> 1) * 4;
                    slicePitch = rowPitch * (height + ((height + 1) >> 1));
                    break;
                case DXGIFormat.NV11:
                    rowPitch = ((width + 3) >> 2) * 4;
                    slicePitch = rowPitch * height * 2;
                    break;
                default:
                {
                    long bpp;

                    if (flags.HasFlag(CPFLAGS.BPP24))
                        bpp = 24;
                    else if (flags.HasFlag(CPFLAGS.BPP16))
                        bpp = 16;
                    else if (flags.HasFlag(CPFLAGS.BPP8))
                        bpp = 8;
                    else
                        bpp = BitsPerPixel(format);

                    if (flags.HasFlag(CPFLAGS.LEGACYDWORD | CPFLAGS.PARAGRAPH | CPFLAGS.YMM | CPFLAGS.ZMM |
                                      CPFLAGS.PAGE4K))
                    {
                        if (flags.HasFlag(CPFLAGS.PAGE4K))
                        {
                            rowPitch = ((width * bpp + 32767) / 32768) * 4096;
                            slicePitch = rowPitch * height;
                        }
                        else if (flags.HasFlag(CPFLAGS.ZMM))
                        {
                            rowPitch = ((width * bpp + 511) / 512) * 64;
                            slicePitch = rowPitch * height;
                        }
                        else if (flags.HasFlag(CPFLAGS.YMM))
                        {
                            rowPitch = ((width * bpp + 255) / 256) * 32;
                            slicePitch = rowPitch * height;
                        }
                        else if (flags.HasFlag(CPFLAGS.PARAGRAPH))
                        {
                            rowPitch = ((width * bpp + 127) / 128) * 16;
                            slicePitch = rowPitch * height;
                        }
                        else // DWORD alignment
                        {
                            // Special computation for some incorrectly created DDS files based on
                            // legacy DirectDraw assumptions about pitch alignment
                            rowPitch = ((width * bpp + 31) / 32) * 4;
                            slicePitch = rowPitch * height;
                        }
                    }
                    else
                    {
                        // Default byte alignment
                        rowPitch = (width * bpp + 7) / 8;
                        slicePitch = rowPitch * height;
                    }
                }
                    break;
            }
        }

        /// <summary>
        /// Checks is the given format compressed
        /// </summary>
        public static bool IsCompressed(DXGIFormat format)
        {
            switch (format)
            {
                case DXGIFormat.BC1TYPELESS:
                case DXGIFormat.BC1UNORM:
                case DXGIFormat.BC1UNORMSRGB:
                case DXGIFormat.BC2TYPELESS:
                case DXGIFormat.BC2UNORM:
                case DXGIFormat.BC2UNORMSRGB:
                case DXGIFormat.BC3TYPELESS:
                case DXGIFormat.BC3UNORM:
                case DXGIFormat.BC3UNORMSRGB:
                case DXGIFormat.BC4TYPELESS:
                case DXGIFormat.BC4UNORM:
                case DXGIFormat.BC4SNORM:
                case DXGIFormat.BC5TYPELESS:
                case DXGIFormat.BC5UNORM:
                case DXGIFormat.BC5SNORM:
                case DXGIFormat.BC6HTYPELESS:
                case DXGIFormat.BC6HUF16:
                case DXGIFormat.BC6HSF16:
                case DXGIFormat.BC7TYPELESS:
                case DXGIFormat.BC7UNORM:
                case DXGIFormat.BC7UNORMSRGB:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Encodes the DDS Header and if DX10, the DX10 Header
        /// </summary>
        /// <param name="header">DDS Header</param>
        /// <param name="dx10Header">DX10 Header</param>
        /// <returns>Resulting DDS File Header in bytes</returns>
        public static byte[] EncodeDDSHeader(DDSHeader header, DX10Header dx10Header)
        {
            // Create stream
            using (var output = new BinaryWriter(new MemoryStream()))
            {
                // Write DDS Magic
                output.Write(DDSHeader.DDSMagic);
                // Write Header
                output.Write(StructToBytes(header));
                // Check for DX10 Header
                if (header.PixelFormat.FourCC == PixelFormats.DX10.FourCC)
                    // Write Header
                    output.Write(StructToBytes(dx10Header));
                // Done
                return ((MemoryStream) (output.BaseStream)).ToArray();
            }
        }

        /// <summary>
        /// Generates DirectXTex Meta Data
        /// </summary>
        /// <param name="width">Image Width</param>
        /// <param name="height">Image Height</param>
        /// <param name="mipMapLevels">Number of Mip Maps</param>
        /// <param name="format">Compression Format</param>
        /// <param name="isCubeMap">Whether or not this is a cube map</param>
        /// <returns>Resulting TexMetaData Object</returns>
        public static TexMetadata GenerateMataData(int width, int height, int mipMapLevels, DXGIFormat format,
            bool isCubeMap)
        {
            // Create Texture MetaData
            return new TexMetadata(
                width,
                height,
                1,
                isCubeMap ? 6 : 1,
                mipMapLevels,
                isCubeMap ? TexMiscFlags.TEXTURECUBE : 0,
                0,
                format,
                TexDimension.TEXTURE2D
            );
        }

        /// <summary>
        /// Generates a DDS Header, and if requires, a DX10 Header
        /// </summary>
        /// <param name="metaData">Meta Data</param>
        /// <param name="flags">Flags</param>
        /// <param name="header">DDS Header Output</param>
        /// <param name="dx10Header">DX10 Header Output</param>
        public static void GenerateDDSHeader(TexMetadata metaData, DDSFlags flags, out DDSHeader header,
            out DX10Header dx10Header)
        {
            // Check array size
            if (metaData.ArraySize > 1)
                // Check if we have an array and whether we're cube maps/non-2D
                if (metaData.ArraySize != 6 || metaData.Dimension != TexDimension.TEXTURE2D || !metaData.IsCubeMap())
                    // Texture1D arrays, Texture2D arrays, and Cubemap arrays must be stored using 'DX10' extended header
                    flags |= DDSFlags.FORCEDX10EXT;

            // Check for DX10 Ext
            if (flags.HasFlag(DDSFlags.FORCEDX10EXTMISC2))
                flags |= DDSFlags.FORCEDX10EXT;
            // Create DDS Header
            header = new DDSHeader
            {
                // Set Data
                Size = (uint) Marshal.SizeOf<DDSHeader>(),
                Flags = DDSHeader.HeaderFlags.TEXTURE,
                Caps = (uint) DDSHeader.SurfaceFlags.TEXTURE,
                PixelFormat = new DDSHeader.DDSPixelFormat(0, 0, 0, 0, 0, 0, 0, 0)
            };
            // Create DX10 Header
            dx10Header = new DX10Header();
            // Switch format
            switch (metaData.Format)
            {
                case DXGIFormat.R8G8B8A8UNORM:
                    header.PixelFormat = PixelFormats.A8B8G8R8;
                    break;
                case DXGIFormat.R16G16UNORM:
                    header.PixelFormat = PixelFormats.G16R16;
                    break;
                case DXGIFormat.R8G8UNORM:
                    header.PixelFormat = PixelFormats.A8L8;
                    break;
                case DXGIFormat.R16UNORM:
                    header.PixelFormat = PixelFormats.L16;
                    break;
                case DXGIFormat.R8UNORM:
                    header.PixelFormat = PixelFormats.L8;
                    break;
                case DXGIFormat.A8UNORM:
                    header.PixelFormat = PixelFormats.A8;
                    break;
                case DXGIFormat.R8G8B8G8UNORM:
                    header.PixelFormat = PixelFormats.R8G8B8G8;
                    break;
                case DXGIFormat.G8R8G8B8UNORM:
                    header.PixelFormat = PixelFormats.G8R8G8B8;
                    break;
                case DXGIFormat.BC1UNORM:
                    header.PixelFormat = PixelFormats.DXT1;
                    break;
                case DXGIFormat.BC2UNORM:
                    header.PixelFormat = metaData.IsPMAlpha() ? (PixelFormats.DXT2) : (PixelFormats.DXT3);
                    break;
                case DXGIFormat.BC3UNORM:
                    header.PixelFormat = metaData.IsPMAlpha() ? (PixelFormats.DXT4) : (PixelFormats.DXT5);
                    break;
                case DXGIFormat.BC4UNORM:
                    header.PixelFormat = PixelFormats.BC4UNORM;
                    break;
                case DXGIFormat.BC4SNORM:
                    header.PixelFormat = PixelFormats.BC4SNORM;
                    break;
                case DXGIFormat.BC5UNORM:
                    header.PixelFormat = PixelFormats.BC5UNORM;
                    break;
                case DXGIFormat.BC5SNORM:
                    header.PixelFormat = PixelFormats.BC5SNORM;
                    break;
                case DXGIFormat.B5G6R5UNORM:
                    header.PixelFormat = PixelFormats.R5G6B5;
                    break;
                case DXGIFormat.B5G5R5A1UNORM:
                    header.PixelFormat = PixelFormats.A1R5G5B5;
                    break;
                case DXGIFormat.R8G8SNORM:
                    header.PixelFormat = PixelFormats.V8U8;
                    break;
                case DXGIFormat.R8G8B8A8SNORM:
                    header.PixelFormat = PixelFormats.Q8W8V8U8;
                    break;
                case DXGIFormat.R16G16SNORM:
                    header.PixelFormat = PixelFormats.V16U16;
                    break;
                case DXGIFormat.B8G8R8A8UNORM:
                    header.PixelFormat = PixelFormats.A8R8G8B8;
                    break;
                case DXGIFormat.B8G8R8X8UNORM:
                    header.PixelFormat = PixelFormats.X8R8G8B8;
                    break;
                case DXGIFormat.B4G4R4A4UNORM:
                    header.PixelFormat = PixelFormats.A4R4G4B4;
                    break;
                case DXGIFormat.YUY2:
                    header.PixelFormat = PixelFormats.YUY2;
                    break;
                // Legacy D3DX formats using D3DFMT enum value as FourCC
                case DXGIFormat.R32G32B32A32FLOAT:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 116; // D3DFMTA32B32G32R32F
                    break;
                case DXGIFormat.R16G16B16A16FLOAT:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 113; // D3DFMTA16B16G16R16F
                    break;
                case DXGIFormat.R16G16B16A16UNORM:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 36; // D3DFMTA16B16G16R16
                    break;
                case DXGIFormat.R16G16B16A16SNORM:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 110; // D3DFMTQ16W16V16U16
                    break;
                case DXGIFormat.R32G32FLOAT:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 115; // D3DFMTG32R32F
                    break;
                case DXGIFormat.R16G16FLOAT:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 112; // D3DFMTG16R16F
                    break;
                case DXGIFormat.R32FLOAT:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 114; // D3DFMTR32F
                    break;
                case DXGIFormat.R16FLOAT:
                    header.PixelFormat.Flags = PixelFormats.DDSFOURCC;
                    header.PixelFormat.FourCC = 111; // D3DFMTR16F
                    break;
                default:
                    break;
            }

            // Check for mips
            if (metaData.MipLevels > 0)
            {
                // Set flag
                header.Flags |= DDSHeader.HeaderFlags.MIPMAP;
                // Check size
                if (metaData.MipLevels > UInt16.MaxValue)
                    throw new ArgumentException(String.Format("Too many mipmaps: {0}. Max: {1}", metaData.MipLevels,
                        UInt16.MaxValue));
                // Set
                header.MipMapCount = (uint) metaData.MipLevels;
                // Check count
                if (header.MipMapCount > 1)
                    header.Caps |= (uint) DDSHeader.SurfaceFlags.MIPMAP;
            }

            // Switch Dimension
            switch (metaData.Dimension)
            {
                case TexDimension.TEXTURE1D:
                {
                    // Check size
                    if (metaData.Width > Int32.MaxValue)
                        throw new ArgumentException(String.Format("Image Width too large: {0}. Max: {1}",
                            metaData.Width, Int32.MaxValue));
                    // Set
                    header.Width = (uint) metaData.Width;
                    header.Height = header.Depth = 1;
                    // Check size
                    break;
                }
                case TexDimension.TEXTURE2D:
                {
                    // Check size
                    if (metaData.Width > Int32.MaxValue || metaData.Height > Int32.MaxValue)
                        throw new ArgumentException(String.Format(
                            "Image Width and/or Height too large: {0}x{1}. Max: {2}",
                            metaData.Width,
                            metaData.Height,
                            Int32.MaxValue));
                    // Set
                    header.Width = (uint) metaData.Width;
                    header.Height = (uint) metaData.Height;
                    header.Depth = 1;
                    // Check size
                    break;
                }
                case TexDimension.TEXTURE3D:
                {
                    // Check size
                    if (metaData.Width > Int32.MaxValue || metaData.Height > Int32.MaxValue)
                        throw new ArgumentException(String.Format(
                            "Image Width and/or Height too large: {0}x{1}. Max: {2}",
                            metaData.Width,
                            metaData.Height,
                            Int32.MaxValue));
                    // Check size
                    if (metaData.Depth > UInt16.MaxValue)
                        throw new ArgumentException(String.Format("Image Depth too large: {0}. Max: {1}",
                            metaData.Depth, UInt16.MaxValue));
                    // Set
                    header.Flags |= DDSHeader.HeaderFlags.VOLUME;
                    header.Caps2 |= 0x00200000;
                    header.Width = (uint) metaData.Width;
                    header.Height = (uint) metaData.Height;
                    header.Depth = (uint) metaData.Depth;
                    // Check size
                    break;
                }
                default:
                    throw new ArgumentException("Invalid Texture Dimension.");
            }

            // Calculate the Pitch
            ComputePitch(metaData.Format, metaData.Width, metaData.Height, out long rowPitch, out long slicePitch,
                CPFLAGS.NONE);
            // Validate results
            if (slicePitch > UInt32.MaxValue || rowPitch > UInt32.MaxValue)
                throw new ArgumentException(
                    "Failed to calculate row and/or slice pitch, values returned were too large");
            // Check is it compressed
            if (IsCompressed(metaData.Format))
            {
                header.Flags |= DDSHeader.HeaderFlags.LINEARSIZE;
                header.PitchOrLinearSize = (uint) slicePitch;
            }
            else
            {
                header.Flags |= DDSHeader.HeaderFlags.PITCH;
                header.PitchOrLinearSize = (uint) rowPitch;
            }

            // Check for do we need to create the DX10 Header
            if (header.PixelFormat.Size == 0)
            {
                // Check size
                if (metaData.ArraySize > UInt16.MaxValue)
                    throw new ArgumentException(String.Format("Array Size too large: {0}. Max: {1}", metaData.ArraySize,
                        UInt16.MaxValue));
                // Set Pixel format
                header.PixelFormat = PixelFormats.DX10;
                // Set Data
                dx10Header.Format = metaData.Format;
                dx10Header.ResourceDimension = metaData.Dimension;
                dx10Header.MiscFlag = metaData.MiscFlags & ~TexMiscFlags.TEXTURECUBE;
                dx10Header.ArraySize = (uint) metaData.ArraySize;
                // Check for Cube Maps
                if (metaData.MiscFlags.HasFlag(TexMiscFlags.TEXTURECUBE))
                {
                    // Check array size, must be a multiple of 6 for cube maps
                    if ((metaData.ArraySize % 6) != 0)
                        throw new ArgumentException("Array size must be a multiple of 6");
                    // Set Flag
                    dx10Header.MiscFlag |= TexMiscFlags.TEXTURECUBE;
                    dx10Header.ArraySize /= 6;
                }

                // Check for mist flags
                if (flags.HasFlag(DDSFlags.FORCEDX10EXTMISC2))
                    // This was formerly 'reserved'. D3DX10 and D3DX11 will fail if this value is anything other than 0
                    dx10Header.MiscFlags2 = (uint) metaData.MiscFlags2;
            }
        }
    }
}
