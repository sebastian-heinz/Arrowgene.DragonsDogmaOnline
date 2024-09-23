using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_INFO_RES;

        public S2CClanClanGetInfoRes()
        {
            ClanParam = new CDataClanParam();
        }
        public CDataClanParam ClanParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanGetInfoRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataClanParam>(buffer, obj.ClanParam);
            }

            public override S2CClanClanGetInfoRes Read(IBuffer buffer)
            {
                S2CClanClanGetInfoRes obj = new S2CClanClanGetInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ClanParam = ReadEntity<CDataClanParam>(buffer);
                return obj;
            }
        }
    }
}
