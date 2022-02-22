using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client
{
    public class ArcArchive : ClientFile
    {
        private const string Key = "ABB(DF2I8[{Y-oS_CCMy(@<}qR}WYX11M)w[5V.~CbjwM5q<F1Iab+-";
        private const int EntrySize = 80;

        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ResourceFile));

        private static BlowFish BlowFish
            = new BlowFish(Encoding.UTF8.GetBytes(Key), true);

        public string MagicTag { get; set; }
        public ushort MagicId { get; set; }

        protected override void Read(IBuffer buffer)
        {
            if (buffer.Size < 6)
            {
                Logger.Error($"Not enough data to parse ArcArchive (Size:{buffer.Size} < 8)");
                return;
            }

            byte[] magicTag = buffer.ReadBytes(4);
            MagicTag = Encoding.UTF8.GetString(magicTag);
            MagicId = ReadUInt16(buffer);
            if (MagicTag != "ARCC" || MagicId != 0x07)
            {
                Logger.Error("Invalid .arc File");
            }

            int count = ReadInt16(buffer);

            for (int i = 0; i < count; i++)
            {
                byte[] entry = buffer.ReadBytes(EntrySize);
                entry = BlowFish.Decrypt_ECB(entry);
                IBuffer entryBuffer = new StreamBuffer(entry);
                entryBuffer.Position = 0;
                string fileName = entryBuffer.ReadFixedString(64);
            }
            
            if (buffer.Position != buffer.Size)
            {
                Logger.Debug(
                    $"It looks like there is more data available (Position:{buffer.Position} != Size:{buffer.Size})");
            }
        }
    }
}
