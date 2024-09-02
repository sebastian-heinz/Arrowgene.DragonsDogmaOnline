using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestProgressNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_PROGRESS_NTC;

        public S2CQuestQuestProgressNtc()
        {
            QuestProcessStateList = new List<CDataQuestProcessState>();
        }

        public uint ProgressCharacterId { get; set; }
        public uint QuestScheduleId { get; set; }
        public List<CDataQuestProcessState> QuestProcessStateList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestProgressNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestProgressNtc obj)
            {
                WriteUInt32(buffer, obj.ProgressCharacterId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteEntityList<CDataQuestProcessState>(buffer, obj.QuestProcessStateList);
            }

            public override S2CQuestQuestProgressNtc Read(IBuffer buffer)
            {
                S2CQuestQuestProgressNtc obj = new S2CQuestQuestProgressNtc();
                obj.ProgressCharacterId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
                return obj;
            }
        }
    }
}
