using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_INFO_REQ;

        public uint ClanId { get; set; }

        public C2SClanClanGetInfoReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanGetInfoReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanGetInfoReq obj)
            {
                WriteUInt32(buffer, obj.ClanId);
            }

            public override C2SClanClanGetInfoReq Read(IBuffer buffer)
            {
                C2SClanClanGetInfoReq obj = new C2SClanClanGetInfoReq();
                obj.ClanId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
