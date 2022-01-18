using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionMoveInServerReq : IPacketStructure
    {
        public string GameToken { get; set; }

        public PacketId Id => PacketId.C2S_CONNECTION_MOVE_IN_SERVER_REQ;
    }

    public class C2SConnectionMoveInServerReqSerializer : EntitySerializer<C2SConnectionMoveInServerReq>
    {
        public override void Write(IBuffer buffer, C2SConnectionMoveInServerReq obj)
        {
            WriteMtString(buffer, obj.GameToken);
        }

        public override C2SConnectionMoveInServerReq Read(IBuffer buffer)
        {
            C2SConnectionMoveInServerReq obj = new C2SConnectionMoveInServerReq();
            obj.GameToken = ReadMtString(buffer);
            return obj;
        }
    }
}
