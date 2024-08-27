using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentInfoListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_INFO_LIST_REQ;

        public C2SBattleContentInfoListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SBattleContentInfoListReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentInfoListReq obj)
            {
            }

            public override C2SBattleContentInfoListReq Read(IBuffer buffer)
            {
                C2SBattleContentInfoListReq obj = new C2SBattleContentInfoListReq();
                return obj;
            }
        }

    }
}
