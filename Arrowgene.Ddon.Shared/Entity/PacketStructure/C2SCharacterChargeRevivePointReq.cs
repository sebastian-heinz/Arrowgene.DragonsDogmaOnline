using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterChargeRevivePointReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARGE_REVIVE_POINT_REQ;

        public class Serializer : PacketEntitySerializer<C2SCharacterChargeRevivePointReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterChargeRevivePointReq obj)
            {
            }

            public override C2SCharacterChargeRevivePointReq Read(IBuffer buffer)
            {
                C2SCharacterChargeRevivePointReq obj = new C2SCharacterChargeRevivePointReq();   
                return obj;
            }
        }

    }
}