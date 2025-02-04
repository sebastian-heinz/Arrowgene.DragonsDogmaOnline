using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetLevelBonusListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_LEVEL_BONUS_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestGetLevelBonusListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetLevelBonusListReq obj)
            {
            }

            public override C2SQuestGetLevelBonusListReq Read(IBuffer buffer)
            {
                C2SQuestGetLevelBonusListReq obj = new C2SQuestGetLevelBonusListReq();
                return obj;
            }
        }
    }
}
