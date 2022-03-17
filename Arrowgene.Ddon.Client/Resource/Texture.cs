using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource
{
    public class Texture : ResourceFile
    {
        // [StructLayout(LayoutKind.Explicit)]
        // struct WordUnion
        // {
        //     [FieldOffset(0)]
        //     public uint Number;

        //     [FieldOffset(0)]
        //     public ushort Low;

        //     [FieldOffset(2)]
        //     public ushort High;
        // }
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(Texture));

        protected override MagicIdWidth IdWidth => MagicIdWidth.UInt;

        protected override void ReadResource(IBuffer buffer)
        {
            org = buffer.GetAllBytes();
            
            uint a = ReadUInt32(buffer);
            uint b = ReadUInt32(buffer);
            uint c = ReadUInt32(buffer);

            uint t = MagicId >> 0x18;
            uint t1 = t & 0xF;

            uint a1 = a >> 0x6;
            uint a2 = a1 & 0x1FFF;
            uint mOrgWidth = a2 << (byte) t1;

            uint aa1 = a >> 0x13;
            uint mOrgHeight = aa1 << (byte) t1;

            uint aa2 = a >> 0x10;
            uint aa3 = aa2 & 0x1FFF;
            uint mOrgDepth = aa3 << (byte) t1;


            byte drawFormatType = (byte) b;
            FormatType formatType = (FormatType) drawFormatType;


            if ((MagicId & 0xF0000000) == 0x60000000)
            {
                Logger.Error("TODO");
                // MtDataReader::read(&v250, &this->mSHFactor, 108u);
                byte[] data = ReadBytes(buffer, 0x6C);
            }

            byte b1 = (byte) (b & 0xFF);
            uint a11 = a & 0x3F;
            uint ab11 = b1 * a11;
            uint ab111 = ab11 << 0x2;

            uint mipLevelCount = CalcMipLevelCount(mOrgWidth);


            uint tasd = MagicId >> 0x28;
            // From https://raw.githubusercontent.com/FrozenFish24/TurnaboutTools/master/TEXporter/TEXporter/Program.cs

            //00000070 mSHFactor       rTexture::SHFACTOR ?
            //    000000DC                 db ? ; undefined
            //    000000DD                 db ? ; undefined
            //    000000DE                 db ? ; undefined
            //    000000DF                 db ? ; undefined
            //    000000E0 mpTexture       dq ?                    ; offset
            //    000000E8 mOrgInvWidth    dd ?
            //        000000EC mOrgInvHeight   dd ?
            //        000000F0 mOrgWidth       dd ?
            //        000000F4 mOrgHeight      dd ?
            //        000000F8 mOrgDepth       dd ?
            //        000000FC mDetailBias     dd ?


            // Int32 magic = BitConverter.ToInt32(array_input, 0);
            // uint[] header = new uint[3];
            // for (int i = 0; i < 3; i++)
            //     header[i] = BitConverter.ToUInt32(array_input, i * 4 + 4);

            int version = (int) (MagicId & 0xfff); // First dword
            int alpha_flag = (int) ((MagicId >> 12) & 0xfff);
            int shift = (int) ((MagicId >> 24) & 0xf);
            int unk2 = (int) ((MagicId >> 28) & 0xf);
            int mipmapCount = (int) (a & 0x3f); // Second dword
            int width = (int) ((a >> 6) & 0x1fff);
            int height = (int) ((a >> 19) & 0x1fff);
            int unk3 = (int) (b & 0xff); // Third dword  // pixel ordering? 0x01 == swizzled, 0x06 == linear?
            int type = (int) ((b >> 8) & 0xff);
            int unk4 = (int) ((b >> 16) & 0x1fff);

            string string_type = "";
            if (type == 20)
                string_type = "DXT1";
            else if (type == 24)
                string_type = "DXT5";
            else if (type == 25)
                string_type = "DXT1";
            else if (type == 31)
                string_type = "DXT5";
            else if (type == 47)
                string_type = "DXT5";
            else
                string_type = type.ToString();

            int headerLength = 0x10 + (4 * mipmapCount);

            FormatType formatType1 = (FormatType) type;

            DirectXTexUtility.DXGIFormat dxGiFormat = ToDxGiFormat(formatType);
            DirectXTexUtility.DDSFlags flags = DirectXTexUtility.DDSFlags.NONE;
            DirectXTexUtility.TexMetadata texMetadata = DirectXTexUtility.GenerateMataData(
                (int) mOrgWidth,
                (int) mOrgHeight,
                mipmapCount,
                dxGiFormat,
                true
            );
            
            DirectXTexUtility.GenerateDDSHeader(
                texMetadata,
                flags,
                out DirectXTexUtility.DDSHeader ddsHeader,
                out DirectXTexUtility.DX10Header dx10Header
            );
            ddsFileHeader = DirectXTexUtility.EncodeDDSHeader(ddsHeader, dx10Header);
            
       

            int i = 1;
        }

        private byte[] ddsFileHeader;
        private byte[] org;
        
        private uint CalcMipLevelCount(uint size)
        {
            uint v1 = 0xFFFFFFFF;
            do
                ++v1;
            while (1 << (int) v1 <= size);
            return v1;
        }

        public void SaveDds(string path)
        {
            File.WriteAllBytes(path, ddsFileHeader);
        }
        
        public void Save(string path)
        {
            File.WriteAllBytes(path, org);
        }

        private DirectXTexUtility.DXGIFormat ToDxGiFormat(FormatType fmt)
        {
            switch (fmt)
            {
                case FormatType.FORMAT_R8G8B8A8_UNORM: return DirectXTexUtility.DXGIFormat.R8G8B8A8UNORM;
                default: return DirectXTexUtility.DXGIFormat.UNKNOWN;
            }
        }

        private enum FormatType
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
