using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSubstoryMissionAllCompleteNtc : IPacketStructure
    {
        public S2CQuestSubstoryMissionAllCompleteNtc()
        {
        }

        public uint SubstoryGroupId { get; set; }

        public PacketId Id => PacketId.S2C_QUEST_11_94_16_NTC;

        public class Serializer : PacketEntitySerializer<S2CQuestSubstoryMissionAllCompleteNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestSubstoryMissionAllCompleteNtc obj)
            {
                WriteUInt32(buffer, obj.SubstoryGroupId);
            }

            public override S2CQuestSubstoryMissionAllCompleteNtc Read(IBuffer buffer)
            {
                var obj = new S2CQuestSubstoryMissionAllCompleteNtc();
                obj.SubstoryGroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
