using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarProceedsNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_PROCEEDS_NTC;

        public ulong BazaarId { get; set; }
        public uint ItemId { get; set; }
        public uint Proceeds { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarProceedsNtc>
        {
            public override void Write(IBuffer buffer, S2CBazaarProceedsNtc obj)
            {
                WriteUInt64(buffer,obj.BazaarId);
                WriteUInt32(buffer,obj.ItemId);
                WriteUInt32(buffer,obj.Proceeds);
            }

            public override S2CBazaarProceedsNtc Read(IBuffer buffer)
            {
                S2CBazaarProceedsNtc obj = new S2CBazaarProceedsNtc();
                obj.BazaarId = ReadUInt64(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Proceeds = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}