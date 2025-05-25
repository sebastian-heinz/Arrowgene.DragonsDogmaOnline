using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemSlotRestriction
    {
        public ushort LevelUnlocked { get; set; } // Shows up as Expansion at Emblem LV.x
        public byte Unk0 { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemSlotRestriction>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemSlotRestriction obj)
            {
                WriteUInt16(buffer, obj.LevelUnlocked);
                WriteByte(buffer, obj.Unk0);
            }

            public override CDataJobEmblemSlotRestriction Read(IBuffer buffer)
            {
                CDataJobEmblemSlotRestriction obj = new CDataJobEmblemSlotRestriction();
                obj.LevelUnlocked = ReadUInt16(buffer);
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}

