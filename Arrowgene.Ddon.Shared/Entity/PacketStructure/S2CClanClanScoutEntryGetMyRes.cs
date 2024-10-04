using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanScoutEntryGetMyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SCOUT_ENTRY_GET_MY_RES;

        public S2CClanClanScoutEntryGetMyRes()
        {
            MyEntries = new List<CDataClanScoutEntrySearchResult>();
        }

        public List<CDataClanScoutEntrySearchResult> MyEntries { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanScoutEntryGetMyRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanScoutEntryGetMyRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanScoutEntrySearchResult>(buffer, obj.MyEntries);
            }

            public override S2CClanClanScoutEntryGetMyRes Read(IBuffer buffer)
            {
                S2CClanClanScoutEntryGetMyRes obj = new S2CClanClanScoutEntryGetMyRes();
                ReadServerResponse(buffer, obj);
                obj.MyEntries = ReadEntityList<CDataClanScoutEntrySearchResult>(buffer);
                return obj;
            }
        }
    }
}
