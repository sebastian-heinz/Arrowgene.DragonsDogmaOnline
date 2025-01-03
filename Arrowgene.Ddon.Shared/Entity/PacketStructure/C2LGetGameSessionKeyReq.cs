using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LGetGameSessionKeyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_GET_GAME_SESSION_KEY_REQ;

        public class Serializer : PacketEntitySerializer<C2LGetGameSessionKeyReq>
        {

            public override void Write(IBuffer buffer, C2LGetGameSessionKeyReq obj)
            {
            }

            public override C2LGetGameSessionKeyReq Read(IBuffer buffer)
            {
                C2LGetGameSessionKeyReq obj = new C2LGetGameSessionKeyReq();
                return obj;
            }
        }
    }
}
