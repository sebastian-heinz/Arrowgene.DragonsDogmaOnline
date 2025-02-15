using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRankingRankListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RANKING_RANK_LIST_RES;

        public uint Rank { get; set; }
        public List<CDataRankingData> RankingData { get; set; } = new();
        public DateTimeOffset Tallied { get; set; }

        public class Serializer : PacketEntitySerializer<S2CRankingRankListRes>
        {
            public override void Write(IBuffer buffer, S2CRankingRankListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Rank);
                WriteEntityList(buffer, obj.RankingData);
                WriteInt64(buffer, obj.Tallied.ToUnixTimeSeconds());
            }

            public override S2CRankingRankListRes Read(IBuffer buffer)
            {
                S2CRankingRankListRes obj = new S2CRankingRankListRes();
                ReadServerResponse(buffer, obj);
                obj.Rank = ReadUInt32(buffer);
                obj.RankingData = ReadEntityList<CDataRankingData>(buffer);
                obj.Tallied = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
