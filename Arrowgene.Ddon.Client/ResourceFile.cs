using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client
{
    public abstract class ResourceFile : ClientFile
    {
        public string MagicTag { get; set; }
        public uint MagicId { get; set; }

        protected override void Read(IBuffer buffer)
        {
            MagicTag = buffer.ReadFixedString(4);
            MagicId = buffer.ReadUInt32(Endianness.Little);
            ReadResource(buffer);
        }

        protected abstract void ReadResource(IBuffer buffer);
    }
}
