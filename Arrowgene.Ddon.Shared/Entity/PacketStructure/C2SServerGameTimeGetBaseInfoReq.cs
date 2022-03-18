using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SServerGameTimeGetBaseInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SERVER_GAME_TIME_GET_BASEINFO_REQ;

        public class Serializer : PacketEntitySerializer<C2SServerGameTimeGetBaseInfoReq>
        {
            public override void Write(IBuffer buffer, C2SServerGameTimeGetBaseInfoReq obj)
            {
            }

            public override C2SServerGameTimeGetBaseInfoReq Read(IBuffer buffer)
            {
                return new C2SServerGameTimeGetBaseInfoReq();
            }
        }
    }
}