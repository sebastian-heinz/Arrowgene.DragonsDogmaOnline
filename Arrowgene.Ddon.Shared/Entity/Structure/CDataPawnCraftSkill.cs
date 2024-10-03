using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnCraftSkill
    {
        public CraftSkillType Type { get; set; }
        public uint Level { get; set; }

        public class Serializer : EntitySerializer<CDataPawnCraftSkill>
        {
            public override void Write(IBuffer buffer, CDataPawnCraftSkill obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.Level);
            }

            public override CDataPawnCraftSkill Read(IBuffer buffer)
            {
                CDataPawnCraftSkill obj = new CDataPawnCraftSkill();
                obj.Type = (CraftSkillType) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
