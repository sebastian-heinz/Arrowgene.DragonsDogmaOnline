using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStampBonusCheckReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAMP_BONUS_CHECK_REQ;

        public class Serializer : PacketEntitySerializer<C2SStampBonusCheckReq>
        {
            public override void Write(IBuffer buffer, C2SStampBonusCheckReq obj)
            {
            }

            public override C2SStampBonusCheckReq Read(IBuffer buffer)
            {
                C2SStampBonusCheckReq obj = new C2SStampBonusCheckReq();
                return obj;
            }
        }
    }
}
