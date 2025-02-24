using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartnerPawnRewardParam
    {
        public uint ParamTypeId { get; set; }
        public uint UID { get; set; }

        public class Serializer : EntitySerializer<CDataPartnerPawnRewardParam>
        {
            public override void Write(IBuffer buffer, CDataPartnerPawnRewardParam obj)
            {
                WriteUInt32(buffer, obj.ParamTypeId);
                WriteUInt32(buffer, obj.UID);
            }

            public override CDataPartnerPawnRewardParam Read(IBuffer buffer)
            {
                CDataPartnerPawnRewardParam obj = new CDataPartnerPawnRewardParam();
                obj.ParamTypeId = ReadUInt32(buffer);
                obj.UID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
