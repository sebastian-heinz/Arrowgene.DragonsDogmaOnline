using System;
using System.IO;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource
{
    /// <summary>
    /// https://github.com/microsoft/DirectXTex/blob/9c72f2c6cdbe3cc9e33ec55f1b15cee356c4ecf6/DirectXTex/DirectXTexImage.cpp#L82
    /// </summary>
    public class Texture : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(Texture));

        public const string DdsHeaderMagic = "DDS ";
        public const string TexHeaderMagic = "TEX\0";
        public const ushort DdonTexHeaderVersion = 0x9D;
        public const ushort DddaTexHeaderVersion = 0x99;

        public TexHeader Header;
        public TexSphericalHarmonics SphericalHarmonics;
        public TexLayer[] Layers;

        protected override void ReadResource(IBuffer buffer)
        {
            if (Magic == TexHeaderMagic)
            {
                ReadTex(buffer);
            }
            else if (Magic == DdsHeaderMagic)
            {
                ReadDds(buffer);
            }
        }

        public void SaveDds(string path)
        {
            GenerateDdsHeader(out DirectXTexUtility.DDSHeader ddsHeader, out DirectXTexUtility.DX10Header dx10Header);
            byte[] ddsFileHeader = DirectXTexUtility.EncodeDDSHeader(ddsHeader, dx10Header);
            StreamBuffer sb = new StreamBuffer();
            sb.WriteBytes(ddsFileHeader);

            for (int layerIndex = 0; layerIndex < Header.LayerCount; layerIndex++)
            {
                sb.WriteBytes(Layers[layerIndex].Data);
            }

            File.WriteAllBytes(path, sb.GetAllBytes());
            if (Header.HasSphericalHarmonicsFactor)
            {
                string shFactorFile = $"{path}.shfactor";;
                File.WriteAllBytes(shFactorFile, SphericalHarmonics.Encode());
            }
        }

        public void SaveTex(string path)
        {

            string shFactorFile = null;
            if (path.LastIndexOf('.') > 0)
            {
                shFactorFile = path.Substring(0, path.LastIndexOf('.'));
                shFactorFile += ".shfactor";
            }
            
            if (!Header.HasSphericalHarmonicsFactor && File.Exists(shFactorFile))
            {
                Header.HasSphericalHarmonicsFactor = true;
                byte[] shFactor = File.ReadAllBytes(shFactorFile);
                SphericalHarmonics.Decode(shFactor);
            }

            StreamBuffer sb = new StreamBuffer();
            sb.WriteBytes(Encoding.UTF8.GetBytes(TexHeaderMagic));
            sb.WriteBytes(Header.Encode());
            
            if (Header.HasSphericalHarmonicsFactor)
            {
                sb.WriteBytes(SphericalHarmonics.Encode());
            }
            
            int offsetByteLength = (int) Header.LayerCount * 4;
            int offsetBytePosition = sb.Position;
            sb.WriteBytes(new byte[offsetByteLength]);
            for (int layerIndex = 0; layerIndex < Header.LayerCount; layerIndex++)
            {
                Layers[layerIndex].Offset = (uint) sb.Position;
                sb.WriteBytes(Layers[layerIndex].Data);
            }

            sb.Position = offsetBytePosition;
            for (int layerIndex = 0; layerIndex < Header.LayerCount; layerIndex++)
            {
                sb.WriteUInt32(Layers[layerIndex].Offset);
            }

            File.WriteAllBytes(path, sb.GetAllBytes());
        }

        private void ReadTex(IBuffer buffer)
        {
            byte[] texHeader = ReadBytes(buffer, TexHeader.Size);
            Header = new TexHeader();
            Header.Decode(texHeader);

            if (Header.HasSphericalHarmonicsFactor)
            {
                byte[] sphericalHarmonics = ReadBytes(buffer, TexSphericalHarmonics.Size);
                SphericalHarmonics = new TexSphericalHarmonics();
                SphericalHarmonics.Decode(sphericalHarmonics);
            }

            Layers = new TexLayer[Header.LayerCount];
            for (int layerIndex = 0; layerIndex < Header.LayerCount; layerIndex++)
            {
                TexLayer layer = new TexLayer();
                layer.Offset = ReadUInt32(buffer);
                Layers[layerIndex] = layer;
            }

            for (int layerIndex = 0; layerIndex < Header.LayerCount - 1; layerIndex++)
            {
                Layers[layerIndex].Size = Layers[layerIndex + 1].Offset - Layers[layerIndex].Offset;
                Layers[layerIndex].Data = ReadBytes(buffer, (int) Layers[layerIndex].Size);
            }

            Layers[Header.LayerCount - 1].Size = (uint) buffer.Size - Layers[Header.LayerCount - 1].Offset;
            Layers[Header.LayerCount - 1].Data = ReadBytes(buffer, (int) Layers[Header.LayerCount - 1].Size);

            // https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-cubic-environment-maps
            // TODO differentiate between cube map, and texture layers, cube map has faces.

            if (buffer.Size != buffer.Position)
            {
                Logger.Error($"buffer.Size()({buffer.Size}) != buffer.Position({buffer.Position})");
            }
        }

        private void ReadDds(IBuffer buffer)
        {
            Header = new TexHeader();
            Header.Version = DdonTexHeaderVersion; // TODO support DDDA ?
            Header.Shift = 0;
            Header.Alpha = 2;
            Header.UnknownA = 0;
            Header.UnknownB = 0;

            byte[] ddsHeaderByes = buffer.ReadBytes(0x7C);
            DirectXTexUtility.DDSHeader ddsHeader = DirectXTexUtility.FromBytes<DirectXTexUtility.DDSHeader>(
                ddsHeaderByes
            );
            Header.Width = ddsHeader.Width;
            Header.Height = ddsHeader.Height;
            Header.Depth = ddsHeader.Depth;
            Header.MipMapCount = ddsHeader.MipMapCount;

            DirectXTexUtility.DX10Header dx10Header = new DirectXTexUtility.DX10Header();
            if (ddsHeader.PixelFormat.FourCC == DirectXTexUtility.PixelFormats.DX10.FourCC)
            {
                byte[] dx10HeaderByes = buffer.ReadBytes(0x14);
                dx10Header = DirectXTexUtility.FromBytes<DirectXTexUtility.DX10Header>(
                    dx10HeaderByes
                );
                Header.PixelFormat = FromDxGiFormat(dx10Header.Format);
                Header.TextureArraySize = (byte) dx10Header.ArraySize;
            }
            else
            {
                Header.PixelFormat = FromDdsPixelFormat(ddsHeader.PixelFormat);
            }

            if (Header.TextureArraySize > 1)
            {
                // if cubemap
                SphericalHarmonics = new TexSphericalHarmonics();

                uint t = ddsHeader.PitchOrLinearSize * ddsHeader.Height;

                DetermineImageArray(dx10Header.Format,
                    DirectXTexUtility.CPFLAGS.NONE,
                    Header.TextureArraySize,
                    ddsHeader.MipMapCount,
                    ddsHeader.Width,
                    ddsHeader.Height,
                    out uint nImages,
                    out uint pixelSize
                );

              Layers =  SetupImageArray(dx10Header.Format,
                    DirectXTexUtility.CPFLAGS.NONE,
                    Header.TextureArraySize,
                    ddsHeader.MipMapCount,
                    ddsHeader.Width,
                    ddsHeader.Height,
                    pixelSize,
                    nImages);

              
              for (int layerIndex = 0; layerIndex < Header.LayerCount - 1; layerIndex++)
              {
                  Layers[layerIndex].Size = Layers[layerIndex + 1].Offset - Layers[layerIndex].Offset;
                  Layers[layerIndex].Data = ReadBytes(buffer, (int) Layers[layerIndex].Size);
              }

              Layers[Header.LayerCount - 1].Size = (uint) buffer.Size - Layers[Header.LayerCount - 1].Offset;
              Layers[Header.LayerCount - 1].Data = ReadBytes(buffer, (int) Layers[Header.LayerCount - 1].Size);
              
            }
            else
            {
                Layers = new TexLayer[1];
                Layers[0] = new TexLayer();
                Layers[0].Data = buffer.ReadBytes(buffer.Size - buffer.Position);
                Layers[0].Size = (uint) Layers[0].Data.Length;
            }
        }

        private void GenerateDdsHeader(out DirectXTexUtility.DDSHeader ddsHeader,
            out DirectXTexUtility.DX10Header dx10Header)
        {
            int gpuFormat = (int) GetGpuFormatType(Header.PixelFormat);

            ddsHeader = new DirectXTexUtility.DDSHeader();
            ddsHeader.Size = 0x7C; //(uint) Marshal.SizeOf<DirectXTexUtility.DDSHeader>();
            ddsHeader.MipMapCount = Header.MipMapCount;
            ddsHeader.Height = Header.Height;
            ddsHeader.Width = Header.Width;
            ddsHeader.Depth = Header.Depth;
            ddsHeader.PixelFormat = ToDdsPixelFormat(Header.PixelFormat);
            if (Header.MipMapCount > 0)
            {
                ddsHeader.Flags |= DirectXTexUtility.DDSHeader.HeaderFlags.MIPMAP;
                ddsHeader.Caps |= (uint) DirectXTexUtility.DDSHeader.SurfaceFlags.MIPMAP;
            }

            DirectXTexUtility.DXGIFormat dxGiFormat = ToDxGiFormat(Header.PixelFormat);
            dx10Header = new DirectXTexUtility.DX10Header();
            if (Header.TextureArraySize > 1)
            {
                ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DX10;
                dx10Header.Format = dxGiFormat;
                dx10Header.ResourceDimension = DirectXTexUtility.TexDimension.TEXTURE2D;
                dx10Header.ArraySize = Header.TextureArraySize;
                if (true) // TODO differentiate between cubemap and texture array
                {
                    dx10Header.MiscFlag |= DirectXTexUtility.TexMiscFlags.TEXTURECUBE;
                }
            }


            // Calculate the Pitch
            DirectXTexUtility.ComputePitch(dxGiFormat, ddsHeader.Width, ddsHeader.Height, out long rowPitch,
                out long slicePitch,
                DirectXTexUtility.CPFLAGS.NONE);
            // Validate results
            if (slicePitch > UInt32.MaxValue || rowPitch > UInt32.MaxValue)
                throw new ArgumentException(
                    "Failed to calculate row and/or slice pitch, values returned were too large");

            if (DirectXTexUtility.IsCompressed(dxGiFormat))
            {
                ddsHeader.Flags |= DirectXTexUtility.DDSHeader.HeaderFlags.LINEARSIZE;
                ddsHeader.PitchOrLinearSize = (uint) slicePitch;
            }
            else
            {
                ddsHeader.Flags |= DirectXTexUtility.DDSHeader.HeaderFlags.PITCH;
                ddsHeader.PitchOrLinearSize = (uint) rowPitch;
            }
        }

        private DirectXTexUtility.DDSHeader.DDSPixelFormat ToDdsPixelFormat(TexPixelFormat texPixelFormat)
        {
            // switch (type)
            // {
            //     case 20:
            //         ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DXT1;
            //         break;
            //     case 24:
            //         ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DXT5;
            //         break;
            //     case 25:
            //         ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DXT1;
            //         break;
            //     case 31:
            //         ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DXT5;
            //         break;
            //     case 47:
            //         ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DXT5;
            //         break;
            //     default:
            //         break;
            // }

            switch (texPixelFormat)
            {
                case TexPixelFormat.FORMAT_BC1_UNORM_SRGB: return DirectXTexUtility.PixelFormats.DXT1;
                case TexPixelFormat.FORMAT_BCX_RGBI_SRGB: return DirectXTexUtility.PixelFormats.BC5UNORM;
                default:
                    Logger.Error($"ToDdsPixelFormat::TexPixelFormat:{texPixelFormat} not handled");
                    return DirectXTexUtility.PixelFormats.DXT1;
            }
        }

        private TexLayer[] SetupImageArray(
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
                   pixels += (uint)slicePitch;
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

        private bool DetermineImageArray(
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

        private uint CalculateMipLevels(uint width, uint height, uint mipLevels)
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

        private uint CountMips(uint width, uint height)
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

        private TexPixelFormat FromDdsPixelFormat(DirectXTexUtility.DDSHeader.DDSPixelFormat ddsPixelFormat)
        {
            if (ddsPixelFormat.FourCC == DirectXTexUtility.PixelFormats.DXT1.FourCC)
            {
                return TexPixelFormat.FORMAT_BC1_UNORM_SRGB;
            }

            if (ddsPixelFormat.FourCC == DirectXTexUtility.PixelFormats.DXT5.FourCC)
            {
                return TexPixelFormat.FORMAT_BCX_RGBI_SRGB;
            }

            Logger.Error($"FromDdsPixelFormat::DDSPixelFormat:{ddsPixelFormat} not handled");
            return TexPixelFormat.FORMAT_UNKNOWN;
        }

        private DirectXTexUtility.DXGIFormat ToDxGiFormat(TexPixelFormat texPixelFormat)
        {
            switch (texPixelFormat)
            {
                case TexPixelFormat.FORMAT_BC1_UNORM_SRGB: return DirectXTexUtility.DXGIFormat.BC1UNORMSRGB;
                case TexPixelFormat.FORMAT_BCX_RGBI_SRGB: return DirectXTexUtility.DXGIFormat.BC3UNORMSRGB;
                default:
                    Logger.Error($"ToDxGiFormat::TexPixelFormat:{texPixelFormat} not handled");
                    return DirectXTexUtility.DXGIFormat.UNKNOWN;
            }
        }

        private TexPixelFormat FromDxGiFormat(DirectXTexUtility.DXGIFormat dxgiFormat)
        {
            switch (dxgiFormat)
            {
                case DirectXTexUtility.DXGIFormat.BC1UNORMSRGB: return TexPixelFormat.FORMAT_BC1_UNORM_SRGB;
                case DirectXTexUtility.DXGIFormat.BC3UNORMSRGB: return TexPixelFormat.FORMAT_BCX_RGBI_SRGB;
                default:
                    Logger.Error($"FromDxGiFormat::DXGIFormat:{dxgiFormat} not handled");
                    return TexPixelFormat.FORMAT_UNKNOWN;
            }
        }

        private uint GetGpuFormatType(TexPixelFormat fmt)
        {
            uint[] gpuFormats = new uint[55];
            gpuFormats[0] = 0;
            gpuFormats[1] = 0xFAC70E;
            gpuFormats[2] = 0xFAC70C;
            gpuFormats[3] = 0xFAC00C;
            gpuFormats[4] = 0xFAC10C;
            gpuFormats[5] = 2279179;
            gpuFormats[6] = 16433161;
            gpuFormats[7] = 0xFAC00A;
            gpuFormats[8] = 16433418;
            gpuFormats[9] = 16433162;
            gpuFormats[10] = 16433171;
            gpuFormats[11] = 2279173;
            gpuFormats[12] = 2277381;
            gpuFormats[13] = 2277637;
            gpuFormats[14] = 2115332;
            //*((unsigned __int8 *)qword_1F7FBC0 + 4 * a1) | ((*((_DWORD *)&qword_1F7FBD0 + a1) & 0xF) << 8) | 0x924000u;
            //gpuFormats[15]= sub_1931990(3u);
            gpuFormats[16] = 2115330;
            gpuFormats[17] = 2113538;
            gpuFormats[18] = 8388609;
            gpuFormats[19] = 0xFAC023;
            gpuFormats[20] = 16433187;
            gpuFormats[21] = 16433188;
            gpuFormats[22] = 16433188;
            gpuFormats[23] = 16433189;
            gpuFormats[24] = 16433189;
            gpuFormats[25] = 2113574;
            gpuFormats[26] = 2113830;
            gpuFormats[27] = 2277671;
            gpuFormats[28] = 3334160;
            gpuFormats[29] = 15917073;
            gpuFormats[30] = 16433187;
            gpuFormats[31] = 2277415;
            gpuFormats[32] = 16433189;
            gpuFormats[33] = 16433189;
            gpuFormats[34] = 3334154;
            gpuFormats[35] = 16433189;
            gpuFormats[36] = 0xFAC025;
            gpuFormats[37] = 16433189;
            gpuFormats[38] = 3852038;
            gpuFormats[38] = 15917066;
            gpuFormats[40] = 15917066;
            gpuFormats[41] = 16433187;
            gpuFormats[42] = 16433189;
            gpuFormats[43] = 16433189;
            gpuFormats[44] = 2113537;
            gpuFormats[45] = 15917066;
            gpuFormats[46] = 15917065;
            gpuFormats[47] = 16433189;
            gpuFormats[48] = 16433193;
            gpuFormats[49] = 16433193;
            gpuFormats[50] = 0x3AC722;
            gpuFormats[51] = 0;
            gpuFormats[52] = 0;
            gpuFormats[53] = 0;
            gpuFormats[54] = 8687617;
            return gpuFormats[(byte) fmt];
        }

        public struct TexLayer
        {
            public uint Offset;
            public uint Size;
            public byte[] Data;
        }

        public struct TexSphericalHarmonics
        {
            public const int Size = 0x6C;

            public bool Loaded;
            public TexSphericalHarmonicsVector Y00;
            public TexSphericalHarmonicsVector Y11;
            public TexSphericalHarmonicsVector Y10;
            public TexSphericalHarmonicsVector Y1_1;
            public TexSphericalHarmonicsVector Y21;
            public TexSphericalHarmonicsVector Y2_1;
            public TexSphericalHarmonicsVector Y2_2;
            public TexSphericalHarmonicsVector Y20;
            public TexSphericalHarmonicsVector Y22;

            public void Decode(byte[] bytes)
            {
                Y00.R = BitConverter.ToSingle(bytes, 4 * 0);
                Y00.G = BitConverter.ToSingle(bytes, 4 * 1);
                Y00.B = BitConverter.ToSingle(bytes, 4 * 2);
                Y11.R = BitConverter.ToSingle(bytes, 4 * 3);
                Y11.G = BitConverter.ToSingle(bytes, 4 * 4);
                Y11.B = BitConverter.ToSingle(bytes, 4 * 5);
                Y10.R = BitConverter.ToSingle(bytes, 4 * 6);
                Y10.G = BitConverter.ToSingle(bytes, 4 * 7);
                Y10.B = BitConverter.ToSingle(bytes, 4 * 8);
                Y1_1.R = BitConverter.ToSingle(bytes, 4 * 9);
                Y1_1.G = BitConverter.ToSingle(bytes, 4 * 10);
                Y1_1.B = BitConverter.ToSingle(bytes, 4 * 11);
                Y21.R = BitConverter.ToSingle(bytes, 4 * 12);
                Y21.G = BitConverter.ToSingle(bytes, 4 * 13);
                Y21.B = BitConverter.ToSingle(bytes, 4 * 14);
                Y2_1.R = BitConverter.ToSingle(bytes, 4 * 15);
                Y2_1.G = BitConverter.ToSingle(bytes, 4 * 16);
                Y2_1.B = BitConverter.ToSingle(bytes, 4 * 17);
                Y2_2.R = BitConverter.ToSingle(bytes, 4 * 18);
                Y2_2.G = BitConverter.ToSingle(bytes, 4 * 19);
                Y2_2.B = BitConverter.ToSingle(bytes, 4 * 20);
                Y20.R = BitConverter.ToSingle(bytes, 4 * 21);
                Y20.G = BitConverter.ToSingle(bytes, 4 * 22);
                Y20.B = BitConverter.ToSingle(bytes, 4 * 23);
                Y22.R = BitConverter.ToSingle(bytes, 4 * 24);
                Y22.G = BitConverter.ToSingle(bytes, 4 * 25);
                Y22.B = BitConverter.ToSingle(bytes, 4 * 26);
                Loaded = true;
            }

            public byte[] Encode()
            {
                using MemoryStream m = new MemoryStream();
                using BinaryWriter w = new BinaryWriter(m);
                w.Write(Y00.R);
                w.Write(Y00.G);
                w.Write(Y00.B);
                w.Write(Y11.R);
                w.Write(Y11.G);
                w.Write(Y11.B);
                w.Write(Y10.R);
                w.Write(Y10.G);
                w.Write(Y10.B);
                w.Write(Y1_1.R);
                w.Write(Y1_1.G);
                w.Write(Y1_1.B);
                w.Write(Y21.R);
                w.Write(Y21.G);
                w.Write(Y21.B);
                w.Write(Y2_1.R);
                w.Write(Y2_1.G);
                w.Write(Y2_1.B);
                w.Write(Y2_2.R);
                w.Write(Y2_2.G);
                w.Write(Y2_2.B);
                w.Write(Y20.R);
                w.Write(Y20.G);
                w.Write(Y20.B);
                w.Write(Y22.R);
                w.Write(Y22.G);
                w.Write(Y22.B);
                return m.ToArray();
            }
        }

        public struct TexSphericalHarmonicsVector
        {
            public float R;
            public float G;
            public float B;
        }

        public struct TexHeader
        {
            public const int Size = 12;

            public uint Version;
            public uint Height;
            public uint Width;
            public uint Shift;
            public uint Alpha;
            public uint Depth;
            public TexPixelFormat PixelFormat;
            public byte TextureArraySize;
            public uint MipMapCount;
            public uint UnknownA;
            public uint UnknownB;
            public bool HasSphericalHarmonicsFactor;

            public uint LayerCount => TextureArraySize * MipMapCount;

            public void Decode(byte[] bytes)
            {
                uint header4 = BitConverter.ToUInt32(bytes, 0);
                uint header8 = BitConverter.ToUInt32(bytes, 4);
                uint header12 = BitConverter.ToUInt32(bytes, 8);

                HasSphericalHarmonicsFactor = (header4 & 0xF0000000) == 0x60000000;

                uint versionBits12__0_11 = header4 & ((1 << 12) - 1);
                uint alphaBits12__12_23 = (header4 >> 12) & ((1 << 12) - 1);
                uint shiftBits4__24_27 = (header4 >> 24) & ((1 << 4) - 1);
                uint unkBits4__28_31 = (header4 >> 28) & ((1 << 4) - 1); // switchNum 1,2|3|6

                uint mipMapCountBits6_0__5 = header8 & ((1 << 6) - 1);
                uint widthBits13_6__18 = (header8 >> 6) & ((1 << 13) - 1);
                uint heightBits13_19__31 = (header8 >> 19) & ((1 << 13) - 1);

                uint textureArraySizeBits8__0_7 = header12 & ((1 << 8) - 1);
                uint pixelFormatBits8__8_15 = (header12 >> 8) & ((1 << 8) - 1);
                uint depthBits13__16_28 = (header12 >> 16) & ((1 << 13) - 1);
                uint unkBits3__29_31 = (header12 >> 29) & ((1 << 3) - 1);

                Version = versionBits12__0_11;
                Alpha = alphaBits12__12_23;
                Shift = shiftBits4__24_27;
                UnknownA = unkBits4__28_31;
                MipMapCount = mipMapCountBits6_0__5;
                Width = widthBits13_6__18 << (byte) shiftBits4__24_27;
                Height = heightBits13_19__31 << (byte) shiftBits4__24_27;
                TextureArraySize = (byte) textureArraySizeBits8__0_7;
                PixelFormat = (TexPixelFormat) pixelFormatBits8__8_15;
                Depth = depthBits13__16_28 << (byte) shiftBits4__24_27;
                UnknownB = unkBits3__29_31;
            }

            public byte[] Encode()
            {
                uint header4 =
                    (uint) Version
                    | ((uint) Alpha << 12)
                    | ((uint) Shift << 24)
                    | ((uint) UnknownA << 28);

                if (HasSphericalHarmonicsFactor)
                {
                    header4 = (header4 | 0x60000000);
                }
                
                uint header8 =
                    (uint) MipMapCount
                    | ((uint) Width << 6)
                    | ((uint) Height << 19);

                uint header12 =
                    (uint) TextureArraySize
                    | ((uint) PixelFormat << 8)
                    | ((uint) Depth << 16) 
                    | ((uint) UnknownB << 29);
                byte[] bytes4 = BitConverter.GetBytes(header4);
                byte[] bytes8 = BitConverter.GetBytes(header8);
                byte[] bytes12 = BitConverter.GetBytes(header12);
                byte[] result = new byte[12];
                result[0] = bytes4[0];
                result[1] = bytes4[1];
                result[2] = bytes4[2];
                result[3] = bytes4[3];
                result[4] = bytes8[0];
                result[5] = bytes8[1];
                result[6] = bytes8[2];
                result[7] = bytes8[3];
                result[8] = bytes12[0];
                result[9] = bytes12[1];
                result[10] = bytes12[2];
                result[11] = bytes12[3];
                return result;
            }
        }

        public enum TexPixelFormat
        {
            FORMAT_UNKNOWN = 0,
            FORMAT_R32G32B32A32_FLOAT = 1,
            FORMAT_R16G16B16A16_FLOAT = 2,
            FORMAT_R16G16B16A16_UNORM = 3,
            FORMAT_R16G16B16A16_SNORM = 4,
            FORMAT_R32G32_FLOAT = 5,
            FORMAT_R10G10B10A2_UNORM = 6,
            FORMAT_R8G8B8A8_UNORM = 7,
            FORMAT_R8G8B8A8_SNORM = 8,
            FORMAT_R8G8B8A8_UNORM_SRGB = 9,
            FORMAT_B4G4R4A4_UNORM = 0x0A,
            FORMAT_R16G16_FLOAT = 0x0B,
            FORMAT_R16G16_UNORM = 0x0C,
            FORMAT_R16G16_SNORM = 0x0D,
            FORMAT_R32_FLOAT = 0x0E,
            FORMAT_D24_UNORM_S8_UINT = 0x0F,
            FORMAT_R16_FLOAT = 0x10,
            FORMAT_R16_UNORM = 0x11,
            FORMAT_A8_UNORM = 0x12,
            FORMAT_BC1_UNORM = 0x13,
            FORMAT_BC1_UNORM_SRGB = 0x14,
            FORMAT_BC2_UNORM = 0x15,
            FORMAT_BC2_UNORM_SRGB = 0x16,
            FORMAT_BC3_UNORM = 0x17,
            FORMAT_BC3_UNORM_SRGB = 0x18,
            FORMAT_BCX_GRAYSCALE = 0x19,
            FORMAT_BCX_ALPHA = 0x1A,
            FORMAT_BC5_SNORM = 0x1B,
            FORMAT_B5G6R5_UNORM = 0x1C,
            FORMAT_B5G5R5A1_UNORM = 0x1D,
            FORMAT_BCX_NM1 = 0x1E,
            FORMAT_BCX_NM2 = 0x1F,
            FORMAT_BCX_RGBI = 0x20,
            FORMAT_BCX_RGBY = 0x21,
            FORMAT_B8G8R8X8_UNORM = 0x22,
            FORMAT_BCX_RGBI_SRGB = 0x23,
            FORMAT_BCX_RGBY_SRGB = 0x24,
            FORMAT_BCX_NH = 0x25,
            FORMAT_R11G11B10_FLOAT = 0x26,
            FORMAT_B8G8R8A8_UNORM = 0x27,
            FORMAT_B8G8R8A8_UNORM_SRGB = 0x28,
            FORMAT_BCX_RGBNL = 0x29,
            FORMAT_BCX_YCCA = 0x2A,
            FORMAT_BCX_YCCA_SRGB = 0x2B,
            FORMAT_R8_UNORM = 0x2C,
            FORMAT_B8G8R8A8_UNORM_LE = 0x2D,
            FORMAT_B10G10R10A2_UNORM_LE = 0x2E,
            FORMAT_BCX_SRGBA = 0x2F,
            FORMAT_BC7_UNORM = 0x30,
            FORMAT_BC7_UNORM_SRGB = 0x31,
            FORMAT_SE5M9M9M9 = 0x32,
            FORMAT_R10G10B10A2_FLOAT = 0x33,
            FORMAT_YVU420P2_CSC1 = 0x34,
            FORMAT_R8A8_UNORM = 0x35,
            FORMAT_A8_UNORM_WHITE = 0x36
        }
    }
}
