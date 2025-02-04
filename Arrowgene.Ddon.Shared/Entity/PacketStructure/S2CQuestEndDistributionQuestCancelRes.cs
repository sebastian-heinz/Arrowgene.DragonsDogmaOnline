using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestEndDistributionQuestCancelRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_END_DISTRIBUTION_QUEST_CANCEL_RES;

        public S2CQuestEndDistributionQuestCancelRes()
        {
            ExpiredQuestList = new();
        }

        public List<CDataExpiredQuestList> ExpiredQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestEndDistributionQuestCancelRes>
        {
            public override void Write(IBuffer buffer, S2CQuestEndDistributionQuestCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ExpiredQuestList);
            }

            public override S2CQuestEndDistributionQuestCancelRes Read(IBuffer buffer)
            {
                S2CQuestEndDistributionQuestCancelRes obj = new S2CQuestEndDistributionQuestCancelRes();
                ReadServerResponse(buffer, obj);
                obj.ExpiredQuestList = ReadEntityList<CDataExpiredQuestList>(buffer);
                return obj;
            }
        }
    }
}
