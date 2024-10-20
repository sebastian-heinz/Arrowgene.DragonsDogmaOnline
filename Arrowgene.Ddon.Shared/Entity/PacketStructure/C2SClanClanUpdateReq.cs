using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanUpdateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_UPDATE_REQ;

        public CDataClanUserParam CreateParam { get; set; }

        public C2SClanClanUpdateReq()
        {
            CreateParam = new CDataClanUserParam();
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanUpdateReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanUpdateReq obj)
            {
                WriteEntity<CDataClanUserParam>(buffer, obj.CreateParam);
            }

            public override C2SClanClanUpdateReq Read(IBuffer buffer)
            {
                C2SClanClanUpdateReq obj = new C2SClanClanUpdateReq();
                obj.CreateParam = ReadEntity<CDataClanUserParam>(buffer);
                return obj;
            }
        }
    }
}
