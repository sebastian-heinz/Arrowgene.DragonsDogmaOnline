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
        public const ushort DdonTexHeaderVersion = 0x9D;
        public const ushort DddaTexHeaderVersion = 0x99;

        public TexHeader Header;
        public TexSphericalHarmonics SphericalHarmonics;
        public TexLayer[] Layers;

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

            Layers = new TexLayer[Header.LayerCount];
            for (int layerIndex = 0; layerIndex < Header.LayerCount; layerIndex++)
            {
                Layers[layerIndex].Offset = ReadUInt32(buffer);
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

        public void Save(string path)
        {
            if (Header.Version == DddaTexHeaderVersion)
            {
                Logger.Info("Saving TEX for DDDA");
            }
            else if (Header.Version == DdonTexHeaderVersion)
            {
                Logger.Info("Saving TEX for DDON");
            }
            else
            {
                Logger.Error("Invalid Version");
                return;
            }

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
                if (!SphericalHarmonics.Loaded)
                {
                    Logger.Error(
                        "SphericalHarmonics requested by TexHeader, but structure not marked as Loaded, writing anyways");
                }

                sb.WriteBytes(SphericalHarmonics.Encode());
            }

            if (Layers == null)
            {
                Logger.Error("No Layers");
                return;
            }

            if (Header.LayerCount != Layers.Length)
            {
                Logger.Error($"Header.LayerCount{Header.LayerCount} != Layers.Length{Layers.Length}");
                return;
            }

            int offsetByteLength = (int) Header.LayerCount * 4;
            int offsetBytePosition = sb.Position;
            sb.WriteBytes(new byte[offsetByteLength]);
            for (int layerIndex = 0;
                 layerIndex < Header.LayerCount;
                 layerIndex++)
            {
                Layers[layerIndex].Offset = (uint) sb.Position;
                sb.WriteBytes(Layers[layerIndex].Data);
            }

            sb.Position = offsetBytePosition;
            for (int layerIndex = 0;
                 layerIndex < Header.LayerCount;
                 layerIndex++)
            {
                sb.WriteUInt32(Layers[layerIndex].Offset);
            }

            File.WriteAllBytes(path, sb.GetAllBytes());
        }
    }
}
