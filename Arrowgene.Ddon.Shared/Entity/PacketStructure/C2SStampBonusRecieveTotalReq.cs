using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStampBonusRecieveTotalReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAMP_BONUS_RECIEVE_TOTAL_REQ;

        public class Serializer : PacketEntitySerializer<C2SStampBonusRecieveTotalReq>
        {
            public override void Write(IBuffer buffer, C2SStampBonusRecieveTotalReq obj)
            {
            }

            public override C2SStampBonusRecieveTotalReq Read(IBuffer buffer)
            {
                C2SStampBonusRecieveTotalReq obj = new C2SStampBonusRecieveTotalReq();
                return obj;
            }
        }
    }
}
