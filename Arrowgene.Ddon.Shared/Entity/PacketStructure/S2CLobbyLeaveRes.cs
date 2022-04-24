using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyLeaveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_LOBBY_LOBBY_LEAVE_RES;

        public class Serializer : PacketEntitySerializer<S2CLobbyLeaveRes>
        {
            public override void Write(IBuffer buffer, S2CLobbyLeaveRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CLobbyLeaveRes Read(IBuffer buffer)
            {
                S2CLobbyLeaveRes obj = new S2CLobbyLeaveRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }        
    }
}