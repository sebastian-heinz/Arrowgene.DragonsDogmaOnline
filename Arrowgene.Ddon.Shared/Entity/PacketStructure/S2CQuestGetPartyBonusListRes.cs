using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetPartyBonusListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_PARTY_BONUS_LIST_RES;

        public S2CQuestGetPartyBonusListRes()
        {
            PartyBonusList = new List<CDataSetQuestBonusList>();
        }

        public List<CDataSetQuestBonusList> PartyBonusList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetPartyBonusListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetPartyBonusListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSetQuestBonusList>(buffer, obj.PartyBonusList);
            }

            public override S2CQuestGetPartyBonusListRes Read(IBuffer buffer)
            {
                S2CQuestGetPartyBonusListRes obj = new S2CQuestGetPartyBonusListRes();
                ReadServerResponse(buffer, obj);
                obj.PartyBonusList = ReadEntityList<CDataSetQuestBonusList>(buffer);
                return obj;
            }
        }
    }
}
