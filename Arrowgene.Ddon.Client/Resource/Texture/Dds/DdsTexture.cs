using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds
{
    /// <summary>
    /// https://github.com/microsoft/DirectXTex/blob/main/DirectXTex/DirectXTexImage.cpp#L82
    /// https://github.com/microsoft/DirectXTex/blob/main/DirectXTex/DirectXTexUtil.cpp
    /// https://github.com/microsoft/DirectXTex/blob/main/DirectXTex/DirectXTexDDS.cpp
    /// </summary>
    public class DdsTexture : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdsTexture));

        public DdsHeader Header;
        public Dx10Header Dx10Header;
        public Image[] Images;
        public TexMetadata Metadata;

        protected override void ReadResource(IBuffer buffer)
        {
            DdsFlags ddsFlags = DdsFlags.None;

            if (Magic != DdsHeader.Magic)
            {
                Logger.Error("DdsHeader bytes mismatch");
                return;
            }

            Header.Size = buffer.ReadUInt32();
            if (Header.Size != DdsHeader.StructSize)
            {
                return;
            }

            Header.Flags = (DdsHeaderFlags) buffer.ReadUInt32();
            Header.Height = buffer.ReadUInt32();
            Header.Width = buffer.ReadUInt32();
            Header.PitchOrLinearSize = buffer.ReadUInt32();
            Header.Depth = buffer.ReadUInt32();
            Header.MipMapCount = buffer.ReadUInt32();
            Header.Reserved1 = new uint[11];
            for (int i = 0; i < 11; i++)
            {
                Header.Reserved1[i] = buffer.ReadUInt32();
            }

            Header.PixelFormat.Size = buffer.ReadUInt32();
            Header.PixelFormat.Flags = (DdsPixelFormatFlag) buffer.ReadUInt32();
            Header.PixelFormat.FourCc = buffer.ReadUInt32();
            Header.PixelFormat.RgbBitCount = buffer.ReadUInt32();
            Header.PixelFormat.RBitMask = buffer.ReadUInt32();
            Header.PixelFormat.GBitMask = buffer.ReadUInt32();
            Header.PixelFormat.BBitMask = buffer.ReadUInt32();
            Header.PixelFormat.ABitMask = buffer.ReadUInt32();
            Header.Caps = (DdsCaps) buffer.ReadUInt32();
            Header.Caps2 = (DdsCaps2) buffer.ReadUInt32();
            Header.Caps3 = buffer.ReadUInt32();
            Header.Caps4 = buffer.ReadUInt32();
            Header.Reserved2 = buffer.ReadUInt32();

            if (Header.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
            {
                Dx10Header.Format = (DxGiFormat) buffer.ReadUInt32();
                Dx10Header.ResourceDimension = (TexDimension) buffer.ReadUInt32();
                Dx10Header.MiscFlag = (TexMiscFlag) buffer.ReadUInt32();
                Dx10Header.ArraySize = buffer.ReadUInt32();
                Dx10Header.MiscFlags2 = (TexMiscFlag2) buffer.ReadUInt32();
            }
            
            Metadata.Width = Header.Width;
            Metadata.Height = Header.Height;
            Metadata.Depth = Header.Depth;
            Metadata.MipLevels = Header.MipMapCount;

            if (Header.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
            {
                Metadata.ArraySize = Dx10Header.ArraySize;
                Metadata.MiscFlags = Dx10Header.MiscFlag;
                Metadata.MiscFlags2 = Dx10Header.MiscFlags2;
                Metadata.Format = Dx10Header.Format;
                Metadata.Dimension = Dx10Header.ResourceDimension;
            }
            else
            {
                Metadata.ArraySize = 1;
                if (Header.Flags.HasFlag(DdsHeaderFlags.Depth))
                {
                    Metadata.Dimension = TexDimension.Texture3D;
                }
                else
                {
                    if (Header.Caps2.HasFlag(DdsCaps2.CubeMap))
                    {
                        // We require all six faces to be defined
                        if (!Header.Caps2.HasFlag(DdsCaps2.CubeMapAllFaces))
                        {
                            // error
                            return;
                        }

                        Metadata.ArraySize = 6;
                        Metadata.MiscFlags |= TexMiscFlag.TextureCube;
                    }

                    Metadata.Dimension = TexDimension.Texture2D;
                }

                Metadata.Format = DxTexUtil.GetDxGiFormat(Header, Header.PixelFormat, ddsFlags,
                    out ConversionFlags convFlags);
            }


            if (!DxTexUtil.DetermineImageArray(Metadata,
                    CpFlags.None,
                    out ulong numberImages,
                    out ulong pixelSize))
            {
                return;
            }

            if (!DxTexUtil.SetupImageArray(out Image[] outImages,
                    pixelSize,
                    Metadata,
                    CpFlags.None,
                    numberImages))
            {
                return;
            }

            Images = outImages;

            for (int i = 0; i < Images.Length; i++)
            {
                Images[i].Data = buffer.ReadBytes((int) Images[i].PixelsSize);
            }

            if (buffer.Size != buffer.Position)
            {
                Logger.Error($"buffer.Size()({buffer.Size}) != buffer.Position({buffer.Position})");
            }
        }

        protected override void WriteResource(IBuffer buffer)
        {
            buffer.WriteUInt32(Header.Size);
            buffer.WriteUInt32((uint) Header.Flags);
            buffer.WriteUInt32(Header.Height);
            buffer.WriteUInt32(Header.Width);
            buffer.WriteUInt32(Header.PitchOrLinearSize);
            buffer.WriteUInt32(Header.Depth);
            buffer.WriteUInt32(Header.MipMapCount);
            for (int i = 0; i < 11; i++)
            {
                buffer.WriteUInt32(Header.Reserved1[i]);
            }

            buffer.WriteUInt32(Header.PixelFormat.Size);
            buffer.WriteUInt32((uint) Header.PixelFormat.Flags);
            buffer.WriteUInt32(Header.PixelFormat.FourCc);
            buffer.WriteUInt32(Header.PixelFormat.RgbBitCount);
            buffer.WriteUInt32(Header.PixelFormat.RBitMask);
            buffer.WriteUInt32(Header.PixelFormat.GBitMask);
            buffer.WriteUInt32(Header.PixelFormat.BBitMask);
            buffer.WriteUInt32(Header.PixelFormat.ABitMask);
            buffer.WriteUInt32((uint) Header.Caps);
            buffer.WriteUInt32((uint) Header.Caps2);
            buffer.WriteUInt32(Header.Caps3);
            buffer.WriteUInt32(Header.Caps4);
            buffer.WriteUInt32(Header.Reserved2);

            if (Header.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
            {
                buffer.WriteUInt32((uint) Dx10Header.Format);
                buffer.WriteUInt32((uint) Dx10Header.ResourceDimension);
                buffer.WriteUInt32((uint) Dx10Header.MiscFlag);
                buffer.WriteUInt32(Dx10Header.ArraySize);
                buffer.WriteUInt32((uint) Dx10Header.MiscFlags2);
            }

            for (int i = 0; i < Images.Length; i++)
            {
                buffer.WriteBytes(Images[i].Data);
            }
        }

        public void Save(string path)
        {
            StreamBuffer buffer = new StreamBuffer();
            Write(buffer);
            File.WriteAllBytes(path, buffer.GetAllBytes());
        }
    }
}
