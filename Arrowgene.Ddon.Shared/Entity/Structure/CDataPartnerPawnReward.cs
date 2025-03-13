using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartnerPawnReward
    {

        public PawnLikabilityRewardType Type { get; set; }
        public CDataPartnerPawnRewardParam Value { get; set; } = new();

        public class Serializer : EntitySerializer<CDataPartnerPawnReward>
        {
            public override void Write(IBuffer buffer, CDataPartnerPawnReward obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteEntity<CDataPartnerPawnRewardParam>(buffer, obj.Value);
            }

            public override CDataPartnerPawnReward Read(IBuffer buffer)
            {
                CDataPartnerPawnReward obj = new CDataPartnerPawnReward();
                obj.Type = (PawnLikabilityRewardType) ReadByte(buffer);
                obj.Value = ReadEntity<CDataPartnerPawnRewardParam>(buffer);
                return obj;
            }
        }
    }
}
