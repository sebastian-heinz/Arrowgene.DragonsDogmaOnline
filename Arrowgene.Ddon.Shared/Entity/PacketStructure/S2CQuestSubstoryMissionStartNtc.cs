using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSubstoryMissionStartNtc : IPacketStructure
    {
        public S2CQuestSubstoryMissionStartNtc()
        {
        }

        public uint SubstoryGroupId { get; set; }
        public uint SeqNo { get; set; }

        public PacketId Id => PacketId.S2C_QUEST_11_92_16_NTC;

        public class Serializer : PacketEntitySerializer<S2CQuestSubstoryMissionStartNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestSubstoryMissionStartNtc obj)
            {
                WriteUInt32(buffer, obj.SubstoryGroupId);
                WriteUInt32(buffer, obj.SeqNo);
            }

            public override S2CQuestSubstoryMissionStartNtc Read(IBuffer buffer)
            {
                var obj = new S2CQuestSubstoryMissionStartNtc();
                obj.SubstoryGroupId = ReadUInt32(buffer);
                obj.SeqNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
