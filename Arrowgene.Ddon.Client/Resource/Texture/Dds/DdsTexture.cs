using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource.Texture.Dds
{
    /// <summary>
    /// https://github.com/microsoft/DirectXTex/blob/9c72f2c6cdbe3cc9e33ec55f1b15cee356c4ecf6/DirectXTex/DirectXTexImage.cpp#L82
    /// https://github.com/microsoft/DirectXTex/blob/9c72f2c6cdbe3cc9e33ec55f1b15cee356c4ecf6/DirectXTex/DirectXTexUtil.cpp
    /// </summary>
    public class DdsTexture : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdsTexture));

        public DdsHeader DdsHeader;
        public Dx10Header Dx10Header;
        public Image[] Images;

        protected override void ReadResource(IBuffer buffer)
        {
            DdsFlags ddsFlags = DdsFlags.None;

            if (Magic != DdsHeader.Magic)
            {
                Logger.Error("DdsHeader bytes mismatch");
                return;
            }

            DdsHeader.Size = buffer.ReadUInt32();
            if (DdsHeader.Size != 124)
            {
                return;
            }

            DdsHeader.Flags = (DdsHeaderFlags) buffer.ReadUInt32();
            DdsHeader.Height = buffer.ReadUInt32();
            DdsHeader.Width = buffer.ReadUInt32();
            DdsHeader.PitchOrLinearSize = buffer.ReadUInt32();
            DdsHeader.Depth = buffer.ReadUInt32();
            DdsHeader.MipMapCount = buffer.ReadUInt32();
            DdsHeader.Reserved1 = new uint[11];
            for (int i = 0; i < 11; i++)
            {
                DdsHeader.Reserved1[i] = buffer.ReadUInt32();
            }

            DdsHeader.PixelFormat.Size = buffer.ReadUInt32();
            DdsHeader.PixelFormat.Flags = (DdsPixelFormatFlag) buffer.ReadUInt32();
            DdsHeader.PixelFormat.FourCc = buffer.ReadUInt32();
            DdsHeader.PixelFormat.RgbBitCount = buffer.ReadUInt32();
            DdsHeader.PixelFormat.RBitMask = buffer.ReadUInt32();
            DdsHeader.PixelFormat.GBitMask = buffer.ReadUInt32();
            DdsHeader.PixelFormat.BBitMask = buffer.ReadUInt32();
            DdsHeader.PixelFormat.ABitMask = buffer.ReadUInt32();
            DdsHeader.Caps = (DdsCaps) buffer.ReadUInt32();
            DdsHeader.Caps2 = (DdsCaps2) buffer.ReadUInt32();
            DdsHeader.Caps3 = buffer.ReadUInt32();
            DdsHeader.Caps4 = buffer.ReadUInt32();
            DdsHeader.Reserved2 = buffer.ReadUInt32();

            if (DdsHeader.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
            {
                Dx10Header.Format = (DxGiFormat) buffer.ReadUInt32();
                Dx10Header.ResourceDimension = (TexDimension) buffer.ReadUInt32();
                Dx10Header.MiscFlag = (TexMiscFlag) buffer.ReadUInt32();
                Dx10Header.ArraySize = buffer.ReadUInt32();
                Dx10Header.MiscFlags2 = (TexMiscFlag2) buffer.ReadUInt32();
            }

            TexMetadata metadata = new TexMetadata();
            metadata.Width = DdsHeader.Width;
            metadata.Height = DdsHeader.Height;
            metadata.Depth = DdsHeader.Depth;
            metadata.MipLevels = DdsHeader.MipMapCount;

            if (DdsHeader.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
            {
                metadata.ArraySize = Dx10Header.ArraySize;
                metadata.MiscFlags = Dx10Header.MiscFlag;
                metadata.MiscFlags2 = Dx10Header.MiscFlags2;
                metadata.Format = Dx10Header.Format;
                metadata.Dimension = Dx10Header.ResourceDimension;
                LoadArray(metadata);
            }
            else
            {
                metadata.ArraySize = 1;
                if (DdsHeader.Flags.HasFlag(DdsHeaderFlags.Volume))
                {
                    metadata.Dimension = TexDimension.Texture3D;
                }
                else
                {
                    if (DdsHeader.Caps2.HasFlag(DdsCaps2.CubeMap))
                    {
                        // We require all six faces to be defined
                        if (!DdsHeader.Caps2.HasFlag(DdsCaps2.CubeMapAllFaces))
                        {
                            // error
                            return;
                        }

                        metadata.ArraySize = 6;
                        metadata.MiscFlags |= TexMiscFlag.TextureCube;
                    }

                    metadata.Dimension = TexDimension.Texture2D;
                }

                metadata.Format = DirectXTexUtility.GetDxGiFormat(DdsHeader, DdsHeader.PixelFormat, ddsFlags,
                    out ConversionFlags convFlags);
            }
            
            
            if (!DirectXTexUtility.DetermineImageArray(metadata,
                    CpFlags.None,
                    out ulong numberImages,
                    out ulong pixelSize))
            {
                return;
            }

            if (!DirectXTexUtility.SetupImageArray(out Image[] outImages,
                    pixelSize,
                    metadata,
                    CpFlags.None,
                    numberImages))
            {
                return;
            }

            Images = outImages;
            
        }

        private void LoadArray(TexMetadata metadata)
        {
            if (!DirectXTexUtility.DetermineImageArray(metadata,
                    CpFlags.None,
                    out ulong numberImages,
                    out ulong pixelSize))
            {
                return;
            }

            if (!DirectXTexUtility.SetupImageArray(out Image[] outImages,
                    pixelSize,
                    metadata,
                    CpFlags.None,
                    numberImages))
            {
                return;
            }

            Images = outImages;
        }

        private void LoadImage()
        {
        }

        public void Save(string path)
        {
            StreamBuffer buffer = new StreamBuffer();
            buffer.WriteString(DdsHeader.Magic);
            buffer.WriteUInt32(DdsHeader.Size);
            buffer.WriteUInt32((uint) DdsHeader.Flags);
            buffer.WriteUInt32(DdsHeader.Height);
            buffer.WriteUInt32(DdsHeader.Width);
            buffer.WriteUInt32(DdsHeader.PitchOrLinearSize);
            buffer.WriteUInt32(DdsHeader.Depth);
            buffer.WriteUInt32(DdsHeader.MipMapCount);
            for (int i = 0; i < 11; i++)
            {
                buffer.WriteUInt32(DdsHeader.Reserved1[i]);
            }

            buffer.WriteUInt32(DdsHeader.PixelFormat.Size);
            buffer.WriteUInt32((uint) DdsHeader.PixelFormat.Flags);
            buffer.WriteUInt32(DdsHeader.PixelFormat.FourCc);
            buffer.WriteUInt32(DdsHeader.PixelFormat.RgbBitCount);
            buffer.WriteUInt32(DdsHeader.PixelFormat.RBitMask);
            buffer.WriteUInt32(DdsHeader.PixelFormat.GBitMask);
            buffer.WriteUInt32(DdsHeader.PixelFormat.BBitMask);
            buffer.WriteUInt32(DdsHeader.PixelFormat.ABitMask);
            buffer.WriteUInt32((uint) DdsHeader.Caps);
            buffer.WriteUInt32((uint) DdsHeader.Caps2);
            buffer.WriteUInt32(DdsHeader.Caps3);
            buffer.WriteUInt32(DdsHeader.Caps4);
            buffer.WriteUInt32(DdsHeader.Reserved2);

            if (DdsHeader.PixelFormat.FourCc == DDSPixelFormat.DX10.FourCc)
            {
                buffer.WriteUInt32((uint) Dx10Header.Format);
                buffer.WriteUInt32((uint) Dx10Header.ResourceDimension);
                buffer.WriteUInt32((uint) Dx10Header.MiscFlag);
                buffer.WriteUInt32(Dx10Header.ArraySize);
                buffer.WriteUInt32((uint) Dx10Header.MiscFlags2);
            }

            // todo image data

            File.WriteAllBytes(path, buffer.GetAllBytes());
        }
    }
}
