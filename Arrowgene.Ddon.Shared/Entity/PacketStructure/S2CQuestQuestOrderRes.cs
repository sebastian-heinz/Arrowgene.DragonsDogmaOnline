using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestOrderRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_QUEST_ORDER_RES;

        public S2CQuestQuestOrderRes()
        {
            QuestProcessStateList = new List<CDataQuestProcessState>();
        }

        public List<CDataQuestProcessState> QuestProcessStateList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestOrderRes>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestOrderRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataQuestProcessState>(buffer, obj.QuestProcessStateList);
            }

            public override S2CQuestQuestOrderRes Read(IBuffer buffer)
            {
                S2CQuestQuestOrderRes obj = new S2CQuestQuestOrderRes();
                ReadServerResponse(buffer, obj);
                obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
                return obj;
            }
        }
    }
}