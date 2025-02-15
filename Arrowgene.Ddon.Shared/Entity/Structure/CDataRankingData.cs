using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRankingData
    {
        public uint Rank { get; set; }
        public uint Serial { get; set; }
        public long Score { get; set; }
        public CDataCommunityCharacterBaseInfo CommunityCharacterBaseInfo { get; set; } = new();

        public class Serializer : EntitySerializer<CDataRankingData>
        {
            public override void Write(IBuffer buffer, CDataRankingData obj)
            {
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Serial);
                WriteInt64(buffer, obj.Score);
                WriteEntity(buffer, obj.CommunityCharacterBaseInfo);
            }

            public override CDataRankingData Read(IBuffer buffer)
            {
                CDataRankingData obj = new CDataRankingData();
                obj.Rank = ReadUInt32(buffer);
                obj.Serial = ReadUInt32(buffer);
                obj.Score = ReadInt64(buffer);
                obj.CommunityCharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                return obj;
            }
        }
    }
}
