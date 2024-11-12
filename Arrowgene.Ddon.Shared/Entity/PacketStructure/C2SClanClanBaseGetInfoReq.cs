using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanBaseGetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_BASE_GET_INFO_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanBaseGetInfoReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanBaseGetInfoReq obj)
            {
            }

            public override C2SClanClanBaseGetInfoReq Read(IBuffer buffer)
            {
                C2SClanClanBaseGetInfoReq obj = new C2SClanClanBaseGetInfoReq();
                return obj;
            }
        }
    }
}
