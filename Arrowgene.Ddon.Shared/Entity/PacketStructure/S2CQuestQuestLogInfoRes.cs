using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestLogInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_QUEST_LOG_INFO_RES;

        public S2CQuestQuestLogInfoRes()
        {
            ClanQuestClearNumList = new List<CDataLightQuestClearList>();
        }

        public List<CDataLightQuestClearList> ClanQuestClearNumList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestLogInfoRes>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestLogInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataLightQuestClearList>(buffer, obj.ClanQuestClearNumList);
            }

            public override S2CQuestQuestLogInfoRes Read(IBuffer buffer)
            {
                S2CQuestQuestLogInfoRes obj = new S2CQuestQuestLogInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ClanQuestClearNumList = ReadEntityList<CDataLightQuestClearList>(buffer);
                return obj;
            }
        }
    }
}
