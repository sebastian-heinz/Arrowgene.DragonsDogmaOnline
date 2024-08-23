using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemCrestInheritenceBaseChance
    {
        public byte Slot { get; set; }
        public byte BaseChanceAmount { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemCrestInheritenceBaseChance>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemCrestInheritenceBaseChance obj)
            {
                WriteByte(buffer, obj.Slot);
                WriteByte(buffer, obj.BaseChanceAmount);
            }

            public override CDataJobEmblemCrestInheritenceBaseChance Read(IBuffer buffer)
            {
                CDataJobEmblemCrestInheritenceBaseChance obj = new CDataJobEmblemCrestInheritenceBaseChance();
                obj.Slot = ReadByte(buffer);
                obj.BaseChanceAmount = ReadByte(buffer);
                return obj;
            }
        }
    }
}

