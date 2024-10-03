using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetQuestCompleteListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_QUEST_COMPLETE_LIST_RES;

        public S2CQuestGetQuestCompleteListRes()
        {
            QuestIdList = new List<CDataQuestId>();
        }

        public byte QuestType {  get; set; }
        public List<CDataQuestId> QuestIdList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CQuestGetQuestCompleteListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetQuestCompleteListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.QuestType);
                WriteEntityList<CDataQuestId>(buffer, obj.QuestIdList);
            }

            public override S2CQuestGetQuestCompleteListRes Read(IBuffer buffer)
            {
                S2CQuestGetQuestCompleteListRes obj = new S2CQuestGetQuestCompleteListRes();
                ReadServerResponse(buffer, obj);
                obj.QuestType = ReadByte(buffer);
                obj.QuestIdList = ReadEntityList<CDataQuestId>(buffer);
                return obj;
            }
        }
    }
}

