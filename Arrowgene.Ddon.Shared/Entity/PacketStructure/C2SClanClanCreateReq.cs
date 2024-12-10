using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanCreateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_CREATE_REQ;

        public CDataClanUserParam CreateParam { get; set; }

        public C2SClanClanCreateReq()
        {
            CreateParam = new CDataClanUserParam();
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanCreateReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanCreateReq obj)
            {
                WriteEntity<CDataClanUserParam>(buffer, obj.CreateParam);
            }

            public override C2SClanClanCreateReq Read(IBuffer buffer)
            {
                C2SClanClanCreateReq obj = new C2SClanClanCreateReq();
                obj.CreateParam = ReadEntity<CDataClanUserParam>(buffer);
                return obj;
            }
        }
    }
}
