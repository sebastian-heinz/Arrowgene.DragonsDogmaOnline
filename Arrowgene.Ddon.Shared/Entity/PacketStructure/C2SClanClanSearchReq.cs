using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanSearchReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SEARCH_REQ;

        public CDataClanSearchParam SearchParam { get; set; }

        public C2SClanClanSearchReq()
        {
            SearchParam = new CDataClanSearchParam();
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanSearchReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanSearchReq obj)
            {
                WriteEntity<CDataClanSearchParam>(buffer, obj.SearchParam);
            }

            public override C2SClanClanSearchReq Read(IBuffer buffer)
            {
                C2SClanClanSearchReq obj = new C2SClanClanSearchReq();
                obj.SearchParam = ReadEntity<CDataClanSearchParam>(buffer);
                return obj;
            }
        }
    }
}
