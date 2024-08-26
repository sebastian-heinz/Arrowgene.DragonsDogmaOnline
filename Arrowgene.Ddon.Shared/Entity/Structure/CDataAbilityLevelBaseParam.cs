using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAbilityLevelBaseParam
    {
        public uint AbilityNo { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : EntitySerializer<CDataAbilityLevelBaseParam>
        {
            public override void Write(IBuffer buffer, CDataAbilityLevelBaseParam obj)
            {
                WriteUInt32(buffer, obj.AbilityNo);
                WriteByte(buffer, obj.AbilityLv);
            }

            public override CDataAbilityLevelBaseParam Read(IBuffer buffer)
            {
                CDataAbilityLevelBaseParam obj = new CDataAbilityLevelBaseParam();
                obj.AbilityNo = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}
