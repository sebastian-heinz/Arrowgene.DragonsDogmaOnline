using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaQuestHintListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_QUEST_HINT_LIST_RES;

        public S2CAreaGetAreaQuestHintListRes()
        {
            AreaQuestHintList = new();
        }

        public List<CDataAreaQuestHint> AreaQuestHintList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaQuestHintListRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaQuestHintListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.AreaQuestHintList);
            }

            public override S2CAreaGetAreaQuestHintListRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaQuestHintListRes obj = new S2CAreaGetAreaQuestHintListRes();
                ReadServerResponse(buffer, obj);
                obj.AreaQuestHintList = ReadEntityList<CDataAreaQuestHint>(buffer);
                return obj;
            }
        }
    }
}
