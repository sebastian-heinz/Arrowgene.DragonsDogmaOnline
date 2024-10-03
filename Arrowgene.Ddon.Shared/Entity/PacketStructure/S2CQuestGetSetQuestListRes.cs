using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetSetQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_SET_QUEST_LIST_RES;

        public S2CQuestGetSetQuestListRes()
        {
            SetQuestList = new List<CDataSetQuestList>();
        }

        public QuestAreaId DistributeId { get; set; }
        public List<CDataSetQuestList> SetQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetSetQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetSetQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, (uint)obj.DistributeId);
                WriteEntityList<CDataSetQuestList>(buffer, obj.SetQuestList);
            }

            public override S2CQuestGetSetQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetSetQuestListRes obj = new S2CQuestGetSetQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.DistributeId = (QuestAreaId)ReadUInt32(buffer);
                obj.SetQuestList = ReadEntityList<CDataSetQuestList>(buffer);
                return obj;
            }
        }
    }
}
