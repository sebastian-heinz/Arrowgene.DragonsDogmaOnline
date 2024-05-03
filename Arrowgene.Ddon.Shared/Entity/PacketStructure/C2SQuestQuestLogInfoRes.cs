using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestQuestLogInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_QUEST_LOG_INFO_RES;

        public C2SQuestQuestLogInfoRes()
        {
            ClanQuestClearNumList = new List<CDataLightQuestClearList>();
        }

        public List<CDataLightQuestClearList> ClanQuestClearNumList {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestQuestLogInfoRes>
        {
            public override void Write(IBuffer buffer, C2SQuestQuestLogInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataLightQuestClearList>(buffer, obj.ClanQuestClearNumList);
            }

            public override C2SQuestQuestLogInfoRes Read(IBuffer buffer)
            {
                C2SQuestQuestLogInfoRes obj = new C2SQuestQuestLogInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ClanQuestClearNumList = ReadEntityList<CDataLightQuestClearList>(buffer);
                return obj;
            }
        }
    }
}
