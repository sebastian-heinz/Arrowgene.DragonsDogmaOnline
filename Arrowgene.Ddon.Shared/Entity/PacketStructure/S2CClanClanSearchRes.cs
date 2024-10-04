using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanSearchRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SEARCH_RES;

        public S2CClanClanSearchRes()
        {
            ClanList = new List<CDataClanSearchResult>();
        }

        public List<CDataClanSearchResult> ClanList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanSearchRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanSearchRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanSearchResult>(buffer, obj.ClanList);
            }

            public override S2CClanClanSearchRes Read(IBuffer buffer)
            {
                S2CClanClanSearchRes obj = new S2CClanClanSearchRes();
                ReadServerResponse(buffer, obj);
                obj.ClanList = ReadEntityList<CDataClanSearchResult>(buffer);
                return obj;
            }
        }
    }
}
