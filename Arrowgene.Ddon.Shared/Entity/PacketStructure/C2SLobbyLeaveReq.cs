using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLobbyLeaveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_LOBBY_LOBBY_LEAVE_REQ;
        
        public class Serializer : PacketEntitySerializer<C2SLobbyLeaveReq>
        {
            public override void Write(IBuffer buffer, C2SLobbyLeaveReq obj)
            {
            }

            public override C2SLobbyLeaveReq Read(IBuffer buffer)
            {
                return new C2SLobbyLeaveReq();
            }
        }
    }
}