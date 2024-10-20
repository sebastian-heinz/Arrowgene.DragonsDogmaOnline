using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanScoutEntrySearchRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SCOUT_ENTRY_SEARCH_RES;

        public S2CClanClanScoutEntrySearchRes()
        {
            SearchResult = new List<CDataClanScoutEntrySearchResult>();
        }

        public List<CDataClanScoutEntrySearchResult> SearchResult;

        public class Serializer : PacketEntitySerializer<S2CClanClanScoutEntrySearchRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanScoutEntrySearchRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanScoutEntrySearchResult>(buffer, obj.SearchResult);
            }

            public override S2CClanClanScoutEntrySearchRes Read(IBuffer buffer)
            {
                S2CClanClanScoutEntrySearchRes obj = new S2CClanClanScoutEntrySearchRes();
                ReadServerResponse(buffer, obj);
                obj.SearchResult = ReadEntityList<CDataClanScoutEntrySearchResult>(buffer);
                return obj;
            }
        }
    }
}
