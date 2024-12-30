using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetAreaBonusListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_AREA_BONUS_LIST_RES;

        public S2CQuestGetAreaBonusListRes()
        {
            AreaBonusList = new();
        }

        public List<CDataAreaBonus> AreaBonusList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetAreaBonusListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetAreaBonusListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.AreaBonusList);
            }

            public override S2CQuestGetAreaBonusListRes Read(IBuffer buffer)
            {
                S2CQuestGetAreaBonusListRes obj = new S2CQuestGetAreaBonusListRes();
                ReadServerResponse(buffer, obj);
                obj.AreaBonusList = ReadEntityList<CDataAreaBonus>(buffer);
                return obj;
            }
        }
    }
}
