using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemCrestInheritanceBaseChance
    {
        public byte Slot { get; set; }
        public byte BaseChanceAmount { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemCrestInheritanceBaseChance>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemCrestInheritanceBaseChance obj)
            {
                WriteByte(buffer, obj.Slot);
                WriteByte(buffer, obj.BaseChanceAmount);
            }

            public override CDataJobEmblemCrestInheritanceBaseChance Read(IBuffer buffer)
            {
                CDataJobEmblemCrestInheritanceBaseChance obj = new CDataJobEmblemCrestInheritanceBaseChance();
                obj.Slot = ReadByte(buffer);
                obj.BaseChanceAmount = ReadByte(buffer);
                return obj;
            }
        }
    }
}

