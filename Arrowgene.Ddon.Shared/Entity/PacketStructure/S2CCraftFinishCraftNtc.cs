using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftFinishCraftNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CRAFT_FINISH_CRAFT_NTC;

        public S2CCraftFinishCraftNtc()
        {
        }

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftFinishCraftNtc>
        {
            public override void Write(IBuffer buffer, S2CCraftFinishCraftNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override S2CCraftFinishCraftNtc Read(IBuffer buffer)
            {
                S2CCraftFinishCraftNtc obj = new S2CCraftFinishCraftNtc();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
