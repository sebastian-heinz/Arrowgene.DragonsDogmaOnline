using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestLeaderQuestProgressRequestRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_LEADER_QUEST_PROGRESS_REQUEST_RES;

        public byte QuestProgressResult { get; set; }
        public uint QuestScheduleId { get; set; }
        public ushort ProcessNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestLeaderQuestProgressRequestRes>
        {
            public override void Write(IBuffer buffer, S2CQuestLeaderQuestProgressRequestRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.QuestProgressResult);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
            }

            public override S2CQuestLeaderQuestProgressRequestRes Read(IBuffer buffer)
            {
                S2CQuestLeaderQuestProgressRequestRes obj = new S2CQuestLeaderQuestProgressRequestRes();
                ReadServerResponse(buffer, obj);
                obj.QuestProgressResult = ReadByte(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}