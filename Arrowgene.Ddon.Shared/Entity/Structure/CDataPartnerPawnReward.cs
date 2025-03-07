using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartnerPawnReward
    {

        public byte Type { get; set; }
        public CDataPartnerPawnRewardParam Value { get; set; } = new();

        public class Serializer : EntitySerializer<CDataPartnerPawnReward>
        {
            public override void Write(IBuffer buffer, CDataPartnerPawnReward obj)
            {
                WriteByte(buffer, obj.Type);
                WriteEntity<CDataPartnerPawnRewardParam>(buffer, obj.Value);
            }

            public override CDataPartnerPawnReward Read(IBuffer buffer)
            {
                CDataPartnerPawnReward obj = new CDataPartnerPawnReward();
                obj.Type = ReadByte(buffer);
                obj.Value = ReadEntity<CDataPartnerPawnRewardParam>(buffer);
                return obj;
            }
        }
    }
}
