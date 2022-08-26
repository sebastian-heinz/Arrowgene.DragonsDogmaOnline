using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnCraftSkill
    {
        public byte Type { get; set; }
        public uint Level { get; set; }

        public class Serializer : EntitySerializer<CDataPawnCraftSkill>
        {
            public override void Write(IBuffer buffer, CDataPawnCraftSkill obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.Level);
            }

            public override CDataPawnCraftSkill Read(IBuffer buffer)
            {
                CDataPawnCraftSkill obj = new CDataPawnCraftSkill();
                obj.Type = ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}