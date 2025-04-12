using System.IO;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client.Resource.Texture.Tex
{
    public class TexTexture : ResourceFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(TexTexture));

        public const string TexHeaderMagic = "TEX\0";

        public TexHeader Header;
        public TexSphericalHarmonics SphericalHarmonics;
        public TexImage[] Images;

        protected override void ReadResource(IBuffer buffer)
        {
            if (Magic != TexHeaderMagic)
            {
                Logger.Error("TexHeader bytes mismatch");
                return;
            }

            byte[] texHeader = ReadBytes(buffer, TexHeader.Size);
            Header = new TexHeader();
            Header.Decode(texHeader);

            if (Header.HasSphericalHarmonicsFactor)
            {
                byte[] sphericalHarmonics = ReadBytes(buffer, TexSphericalHarmonics.Size);
                SphericalHarmonics = new TexSphericalHarmonics();
                SphericalHarmonics.Decode(sphericalHarmonics);
            }

            Images = new TexImage[Header.LayerCount];
            for (int layerIndex = 0; layerIndex < Header.LayerCount; layerIndex++)
            {
                Images[layerIndex].Offset = ReadUInt32(buffer);
            }

            for (int layerIndex = 0; layerIndex < Header.LayerCount - 1; layerIndex++)
            {
                Images[layerIndex].Size = Images[layerIndex + 1].Offset - Images[layerIndex].Offset;
                Images[layerIndex].Data = ReadBytes(buffer, (int) Images[layerIndex].Size);
            }

            Images[Header.LayerCount - 1].Size = (uint) buffer.Size - Images[Header.LayerCount - 1].Offset;
            Images[Header.LayerCount - 1].Data = ReadBytes(buffer, (int) Images[Header.LayerCount - 1].Size);

            // https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-cubic-environment-maps
            // TODO differentiate between cube map, and texture layers, cube map has faces.

            if (buffer.Size != buffer.Position)
            {
                Logger.Error($"buffer.Size()({buffer.Size}) != buffer.Position({buffer.Position})");
            }
        }

        protected override void WriteResource(IBuffer buffer)
        {
            if (Header.Version == TexHeaderVersion.Ddda)
            {
                Logger.Info("Saving TEX for DDDA");
            }
            else if (Header.Version == TexHeaderVersion.Ddon)
            {
                Logger.Info("Saving TEX for DDON");
            }
            else
            {
                Logger.Error("Invalid Version");
                return;
            }

            buffer.WriteBytes(Header.Encode());

            if (Header.HasSphericalHarmonicsFactor)
            {
                if (!SphericalHarmonics.Loaded)
                {
                    Logger.Error(
                        "SphericalHarmonics requested by TexHeader, but structure not marked as Loaded, writing anyways");
                }

                buffer.WriteBytes(SphericalHarmonics.Encode());
            }

            if (Images == null)
            {
                Logger.Error("No Layers");
                return;
            }

            if (Header.LayerCount != Images.Length)
            {
                Logger.Error($"Header.LayerCount{Header.LayerCount} != Layers.Length{Images.Length}");
                return;
            }

            int offsetByteLength = (int) Header.LayerCount * 4;
            int offsetBytePosition = buffer.Position;
            buffer.WriteBytes(new byte[offsetByteLength]);
            for (int layerIndex = 0;
                 layerIndex < Header.LayerCount;
                 layerIndex++)
            {
                Images[layerIndex].Offset = (uint) buffer.Position;
                buffer.WriteBytes(Images[layerIndex].Data);
            }

            buffer.Position = offsetBytePosition;
            for (int layerIndex = 0;
                 layerIndex < Header.LayerCount;
                 layerIndex++)
            {
                buffer.WriteUInt32(Images[layerIndex].Offset);
            }
        }

        public void Save(string path)
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
            Write(sb);
            File.WriteAllBytes(path, sb.GetAllBytes());
        }
    }
}
