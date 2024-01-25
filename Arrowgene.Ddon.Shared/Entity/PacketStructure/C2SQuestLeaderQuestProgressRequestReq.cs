using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestLeaderQuestProgressRequestReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_LEADER_QUEST_PROGRESS_REQUEST_REQ;

        public uint KeyId { get; set; }
        public uint QuestScheduleId { get; set; }
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo { get; set; }
        public ushort BlockNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestLeaderQuestProgressRequestReq>
        {
            public override void Write(IBuffer buffer, C2SQuestLeaderQuestProgressRequestReq obj)
            {
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
                WriteUInt16(buffer, obj.SequenceNo);
                WriteUInt16(buffer, obj.BlockNo);
            }

            public override C2SQuestLeaderQuestProgressRequestReq Read(IBuffer buffer)
            {
                C2SQuestLeaderQuestProgressRequestReq obj = new C2SQuestLeaderQuestProgressRequestReq();
                obj.KeyId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                obj.SequenceNo = ReadUInt16(buffer);
                obj.BlockNo = ReadUInt16(buffer);
                return obj;
            }
        }

    }
}