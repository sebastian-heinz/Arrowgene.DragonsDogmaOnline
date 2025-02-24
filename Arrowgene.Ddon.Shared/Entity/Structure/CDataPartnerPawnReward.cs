using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartnerPawnReward
    {

        public byte Type { get; set; }
        public uint ParamTypeId { get; set; }
        public uint UID { get; set; }

        public class Serializer : EntitySerializer<CDataPartnerPawnReward>
        {
            public override void Write(IBuffer buffer, CDataPartnerPawnReward obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.ParamTypeId);
                WriteUInt32(buffer, obj.UID);
            }

            public override CDataPartnerPawnReward Read(IBuffer buffer)
            {
                CDataPartnerPawnReward obj = new CDataPartnerPawnReward();
                obj.Type = ReadByte(buffer);
                obj.ParamTypeId = ReadUInt32(buffer);
                obj.UID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
