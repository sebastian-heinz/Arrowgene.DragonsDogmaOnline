using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRankingRankListByCharacterIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RANKING_RANK_LIST_BY_CHARACTER_ID_RES;

        public List<CDataRankingData> RankingData { get; set; } = new();
        public DateTimeOffset Tallied { get; set; }

        public class Serializer : PacketEntitySerializer<S2CRankingRankListByCharacterIdRes>
        {
            public override void Write(IBuffer buffer, S2CRankingRankListByCharacterIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RankingData);
                WriteInt64(buffer, obj.Tallied.ToUnixTimeSeconds());
            }

            public override S2CRankingRankListByCharacterIdRes Read(IBuffer buffer)
            {
                S2CRankingRankListByCharacterIdRes obj = new S2CRankingRankListByCharacterIdRes();
                ReadServerResponse(buffer, obj);
                obj.RankingData = ReadEntityList<CDataRankingData>(buffer);
                obj.Tallied = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
