using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetMyInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_MY_INFO_REQ;

        public C2SClanClanGetMyInfoReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanGetMyInfoReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanGetMyInfoReq obj)
            {
            }

            public override C2SClanClanGetMyInfoReq Read(IBuffer buffer)
            {
                C2SClanClanGetMyInfoReq obj = new C2SClanClanGetMyInfoReq();
                return obj;
            }
        }
    }
}
