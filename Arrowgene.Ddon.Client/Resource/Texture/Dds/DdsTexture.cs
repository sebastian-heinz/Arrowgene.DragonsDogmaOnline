using System;
using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds
{
    /// <summary>
    /// TEX parsing & saving is mostly understood.
    /// DDS parsing is a bit more tricky, and very hacky at the moment, one should implement some helper functions from:
    /// https://github.com/microsoft/DirectXTex/blob/9c72f2c6cdbe3cc9e33ec55f1b15cee356c4ecf6/DirectXTex/DirectXTexImage.cpp#L82
    /// https://github.com/microsoft/DirectXTex/blob/9c72f2c6cdbe3cc9e33ec55f1b15cee356c4ecf6/DirectXTex/DirectXTexUtil.cpp
    /// to make better use of DDS parsing.
    /// </summary>
    public class DdsTexture : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdsTexture));

        public const string DdsHeaderMagic = "DDS ";
        
        
        public enum DDSFlags
        {
            NONE = 0x0,
            LEGACYDWORD = 0x1,
            NOLEGACYEXPANSION = 0x2,
            NOR10B10G10A2FIXUP = 0x4,
            FORCERGB = 0x8,
            NO16BPP = 0x10,
            EXPANDLUMINANCE = 0x20,
            BADDXTNTAILS = 0x40,
            FORCEDX10EXT = 0x10000,
            FORCEDX10EXTMISC2 = 0x20000,
        }




        public enum CPFLAGS
        {
            NONE = 0x0, // Normal operation
            LEGACYDWORD = 0x1, // Assume pitch is DWORD aligned instead of BYTE aligned
            PARAGRAPH = 0x2, // Assume pitch is 16-byte aligned instead of BYTE aligned
            YMM = 0x4, // Assume pitch is 32-byte aligned instead of BYTE aligned
            ZMM = 0x8, // Assume pitch is 64-byte aligned instead of BYTE aligned
            PAGE4K = 0x200, // Assume pitch is 4096-byte aligned instead of BYTE aligned
            BADDXTNTAILS = 0x1000, // BC formats with malformed mipchain blocks smaller than 4x4
            BPP24 = 0x10000, // Override with a legacy 24 bits-per-pixel format size
            BPP16 = 0x20000, // Override with a legacy 16 bits-per-pixel format size
            BPP8 = 0x40000, // Override with a legacy 8 bits-per-pixel format size
        };
        
        protected override void ReadResource(IBuffer buffer)
        {
            if (Magic != DdsHeaderMagic)
            {
                Logger.Error("DdsHeader bytes mismatch");
                return;
            }
            
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

                Layers = SetupImageArray(dx10Header.Format,
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

        public void Save(string path)
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
                string shFactorFile = $"{path}.shfactor";
                ;
                File.WriteAllBytes(shFactorFile, SphericalHarmonics.Encode());
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

            // causes gimp not to show texture array export option..
            //      if (DirectXTexUtility.IsCompressed(dxGiFormat))
            //      {
            //          ddsHeader.Flags |= DirectXTexUtility.DDSHeader.HeaderFlags.LINEARSIZE;
            //          ddsHeader.PitchOrLinearSize = (uint) slicePitch;
            //      }
            //      else
            //      {
            //          ddsHeader.Flags |= DirectXTexUtility.DDSHeader.HeaderFlags.PITCH;
            //          ddsHeader.PitchOrLinearSize = (uint) rowPitch;
            //      }
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
    }
}
