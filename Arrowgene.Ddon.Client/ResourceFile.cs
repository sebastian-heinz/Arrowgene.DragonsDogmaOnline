using System;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client
{
    public abstract class ResourceFile : ClientFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ResourceFile));

        public string Magic { get; set; }

        // TODO Magic Validation
        // protected abstract string ExpectedMagic { get; }

        public override void Read(IBuffer buffer)
        {
            if (buffer.Size < 8)
            {
                Logger.Error($"Not enough data to parse ResourceFile (Size:{buffer.Size} < 8)");
                return;
            }

            byte[] magicTag = buffer.ReadBytes(4);
            Magic = Encoding.UTF8.GetString(magicTag);
            // TODO Magic Validation
            ReadResource(buffer);
            if (buffer.Position != buffer.Size)
            {
                Logger.Debug(
                    $"It looks like there is more data available (Position:{buffer.Position} != Size:{buffer.Size})");
            }
        }

        public override void Write(IBuffer buffer)
        {
            byte[] magicTag = Encoding.UTF8.GetBytes(Magic);
            buffer.WriteBytes(magicTag);
            WriteResource(buffer);
        }

        protected abstract void ReadResource(IBuffer buffer);

        protected virtual void WriteResource(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
