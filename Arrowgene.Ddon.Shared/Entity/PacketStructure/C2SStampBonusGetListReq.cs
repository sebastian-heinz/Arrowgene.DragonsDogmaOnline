using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStampBonusGetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAMP_BONUS_GET_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SStampBonusGetListReq>
        {
            public override void Write(IBuffer buffer, C2SStampBonusGetListReq obj)
            {
            }

            public override C2SStampBonusGetListReq Read(IBuffer buffer)
            {
                C2SStampBonusGetListReq obj = new C2SStampBonusGetListReq();
                return obj;
            }
        }
    }
}

