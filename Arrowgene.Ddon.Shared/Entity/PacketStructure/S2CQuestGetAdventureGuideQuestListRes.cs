using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetAdventureGuideQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_RES;

        public S2CQuestGetAdventureGuideQuestListRes()
        {
            QuestList = new();
        }

        public List<CDataQuestAdventureGuideList> QuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetAdventureGuideQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetAdventureGuideQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.QuestList);
            }

            public override S2CQuestGetAdventureGuideQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetAdventureGuideQuestListRes obj = new S2CQuestGetAdventureGuideQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.QuestList = ReadEntityList<CDataQuestAdventureGuideList>(buffer);
                return obj;
            }
        }
    }
}
