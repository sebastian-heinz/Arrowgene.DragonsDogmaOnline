using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAbilityLevelParam
    {
        public byte Lv { get; set; }
        public uint RequireJobLevel { get; set; }
        public uint RequireJobPoint { get; set; }
        public bool IsRelease { get; set; }

        public class Serializer : EntitySerializer<CDataAbilityLevelParam>
        {
            public override void Write(IBuffer buffer, CDataAbilityLevelParam obj)
            {
                WriteByte(buffer, obj.Lv);
                WriteUInt32(buffer, obj.RequireJobLevel);
                WriteUInt32(buffer, obj.RequireJobPoint);
                WriteBool(buffer, obj.IsRelease);
            }

            public override CDataAbilityLevelParam Read(IBuffer buffer)
            {
                CDataAbilityLevelParam obj = new CDataAbilityLevelParam();
                obj.Lv = ReadByte(buffer);
                obj.RequireJobLevel = ReadUInt32(buffer);
                obj.RequireJobPoint = ReadUInt32(buffer);
                obj.IsRelease = ReadBool(buffer);
                return obj;
            }
        }
    }
}