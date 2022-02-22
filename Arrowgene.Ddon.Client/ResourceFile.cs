using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client
{
    public abstract class ResourceFile : ClientFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ResourceFile));

        public string MagicTag { get; set; }
        public uint MagicId { get; set; }

        protected override void Read(IBuffer buffer)
        {
            if (buffer.Size < 8)
            {
                Logger.Error($"Not enough data to parse ResourceFile (Size:{buffer.Size} < 8)");
                return;
            }

            byte[] magicTag = buffer.ReadBytes(4);
            MagicTag = Encoding.UTF8.GetString(magicTag);
            MagicId = buffer.ReadUInt32(Endianness.Little);
            ReadResource(buffer);
            if (buffer.Position != buffer.Size)
            {
                Logger.Debug($"It looks like there is more data available (Position:{buffer.Position} != Size:{buffer.Size})");
            }
        }

        protected abstract void ReadResource(IBuffer buffer);
    }
}
