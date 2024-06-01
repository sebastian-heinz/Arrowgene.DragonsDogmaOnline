using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestQuestProgressReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_QUEST_PROGRESS_REQ;

        public UInt32 KeyId { get; set; }
        public UInt32 ProgressCharacterId { get; set; }
        public UInt32 QuestScheduleId { get; set; }
        public UInt16 ProcessNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestQuestProgressReq>
        {
            public override void Write(IBuffer buffer, C2SQuestQuestProgressReq obj)
            {
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.ProgressCharacterId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
            }

            public override C2SQuestQuestProgressReq Read(IBuffer buffer)
            {
                C2SQuestQuestProgressReq obj = new C2SQuestQuestProgressReq();
                obj.KeyId = ReadUInt32(buffer);
                obj.ProgressCharacterId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                return obj;
            }
        }

    }
}
