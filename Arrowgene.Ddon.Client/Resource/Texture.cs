using System;
using System.IO;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource
{
    public class Texture : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(Texture));

        public const string DdsHeaderMagic = "DDS ";
        public const string TexHeaderMagic = "TEX\0";
        public const ushort DdonTexHeaderVersion = 0x9D;
        public const ushort DddaTexHeaderVersion = 0x99;

        public TexHeader Header;
        public byte[] HeaderA { get; set; }
        public byte[] HeaderB { get; set; }
        public byte[] Data { get; set; }

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
            sb.WriteBytes(Data);
            File.WriteAllBytes(path, sb.GetAllBytes());
        }

        public void SaveTex(string path)
        {
            byte[] texHeader = Header.Encode();
            StreamBuffer sb = new StreamBuffer();
            sb.WriteBytes(Encoding.UTF8.GetBytes(TexHeaderMagic));
            sb.WriteBytes(texHeader);
            sb.WriteBytes(HeaderA);
            sb.WriteBytes(HeaderB);
            sb.WriteBytes(Data);
            File.WriteAllBytes(path, sb.GetAllBytes());
        }

        private void ReadTex(IBuffer buffer)
        {
            byte[] texHeader = ReadBytes(buffer, 12);

            Header = new TexHeader();
            Header.Decode(texHeader);

            if (Header.HasShFactor)
            {
                HeaderA = ReadBytes(buffer, 0x6C); // &this->mSHFactor,
            }
            
            uint layerCount = Header.TextureArraySize * Header.MipMapCount; // layer count
            // uint ab111 = ab11 << 0x2;

            uint readCount = 4 * layerCount;
            HeaderB = ReadBytes(buffer, (int) readCount);

            //  uint v19 = aa1;
            //  if (a2 >= aa1)
            //  {
            //      v19 = a2;
            //  }
            //  uint mipLevelCount = CalcMipLevelCount(v19);
            //  uint switchNum = MagicId >> 0x1C;
            //  switch (switchNum)
            //  {
            //      case 1:
            //      case 2:
            //          break;
            //      case 3:
            //          break;
            //      case 6:
            //          break;
            //  }
            Data = buffer.ReadBytes(buffer.Size - buffer.Position);
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

            if (ddsHeader.PixelFormat.FourCC == DirectXTexUtility.PixelFormats.DX10.FourCC)
            {
                byte[] dx10HeaderByes = buffer.ReadBytes(0x14);
                DirectXTexUtility.DX10Header dx10Header = DirectXTexUtility.FromBytes<DirectXTexUtility.DX10Header>(
                    dx10HeaderByes
                );
                Header.PixelFormat = FromDxGiFormat(dx10Header.Format);
                Header.TextureArraySize = (byte) dx10Header.ArraySize;
            }
            else
            {
                Header.PixelFormat = FromDdsPixelFormat(ddsHeader.PixelFormat);
            }

            HeaderA = new byte[0];
            HeaderB = new byte[0];
            Data = buffer.ReadBytes(buffer.Size - buffer.Position);
        }

        private void GenerateDdsHeader(out DirectXTexUtility.DDSHeader ddsHeader,
            out DirectXTexUtility.DX10Header dx10Header)
        {
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

            dx10Header = new DirectXTexUtility.DX10Header();
            if (Header.TextureArraySize > 1)
            {
                ddsHeader.PixelFormat = DirectXTexUtility.PixelFormats.DX10;
                dx10Header.Format = ToDxGiFormat(Header.PixelFormat);
                dx10Header.ResourceDimension = DirectXTexUtility.TexDimension.TEXTURE2D;
                dx10Header.ArraySize = Header.TextureArraySize;
                if (true) // TODO differentiate between cubemap and texture array
                {
                    dx10Header.MiscFlag |= DirectXTexUtility.TexMiscFlags.TEXTURECUBE;
                }
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
                case TexPixelFormat.FORMAT_BCX_RGBI_SRGB: return DirectXTexUtility.PixelFormats.DXT5;
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

        private uint CalcMipLevelCount(uint size)
        {
            uint v1 = 0xFFFFFFFF;
            do
                ++v1;
            while (1 << (int) v1 <= size);
            return v1;
        }

        public struct TexHeader
        {
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
            public bool HasShFactor;

            public void Decode(byte[] bytes)
            {
                uint header4 = BitConverter.ToUInt32(bytes, 0);
                uint header8 = BitConverter.ToUInt32(bytes, 4);
                uint header12 = BitConverter.ToUInt32(bytes, 8);

                HasShFactor = (header4 & 0xF0000000) == 0x60000000;

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
