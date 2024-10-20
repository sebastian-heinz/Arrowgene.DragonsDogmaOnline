using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetHistoryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_HISTORY_RES;

        public S2CClanClanGetHistoryRes()
        {
            ClanHistoryList = new List<CDataClanHistoryElement>();
        }

        public List<CDataClanHistoryElement> ClanHistoryList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanGetHistoryRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetHistoryRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanHistoryElement>(buffer, obj.ClanHistoryList);
            }

            public override S2CClanClanGetHistoryRes Read(IBuffer buffer)
            {
                S2CClanClanGetHistoryRes obj = new S2CClanClanGetHistoryRes();
                ReadServerResponse(buffer, obj);
                obj.ClanHistoryList = ReadEntityList<CDataClanHistoryElement>(buffer);
                return obj;
            }
        }
    }
}
