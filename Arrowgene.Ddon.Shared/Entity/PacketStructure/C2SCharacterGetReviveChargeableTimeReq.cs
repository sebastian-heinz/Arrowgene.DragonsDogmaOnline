using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterGetReviveChargeableTimeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_REQ;

        public class Serializer : PacketEntitySerializer<C2SCharacterGetReviveChargeableTimeReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterGetReviveChargeableTimeReq obj)
            {
            }

            public override C2SCharacterGetReviveChargeableTimeReq Read(IBuffer buffer)
            {
                C2SCharacterGetReviveChargeableTimeReq obj = new C2SCharacterGetReviveChargeableTimeReq();   
                return obj;
            }
        }

    }
}