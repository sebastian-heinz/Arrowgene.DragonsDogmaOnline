using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LDecideCancelCharacterReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_DECIDE_CANCEL_CHARACTER_REQ;

        public class Serializer : PacketEntitySerializer<C2LDecideCancelCharacterReq>
        {
            public override void Write(IBuffer buffer, C2LDecideCancelCharacterReq obj)
            {
            }

            public override C2LDecideCancelCharacterReq Read(IBuffer buffer)
            {
                C2LDecideCancelCharacterReq obj = new C2LDecideCancelCharacterReq();
                return obj;
            }
        }
    }
}
