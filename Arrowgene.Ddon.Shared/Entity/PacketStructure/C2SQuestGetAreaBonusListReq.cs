using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetAreaBonusListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_AREA_BONUS_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetAreaBonusListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetAreaBonusListReq obj)
            {
            }

            public override C2SQuestGetAreaBonusListReq Read(IBuffer buffer)
            {
                C2SQuestGetAreaBonusListReq obj = new C2SQuestGetAreaBonusListReq();
                return obj;
            }
        }
    }
}
