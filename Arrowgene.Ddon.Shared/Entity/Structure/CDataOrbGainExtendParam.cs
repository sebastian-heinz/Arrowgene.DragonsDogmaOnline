using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOrbGainExtendParam
    {
        public CDataOrbGainExtendParam()
        {
            HpMax = 0;
            StaminaMax = 0;
            Attack = 0;
            Defence = 0;
            MagicAttack = 0;
            MagicDefence = 0;
            AbilityCost = 0;
            JewelrySlot = 0;
            UseItemSlot = 0;
            MaterialItemSlot = 0;
            EquipItemSlot = 0;
            MainPawnSlot = 0;
            SupportPawnSlot = 0;
        }

        public ushort HpMax { get; set; }
        public ushort StaminaMax { get; set; }
        public ushort Attack { get; set; }
        public ushort Defence { get; set; }
        public ushort MagicAttack { get; set; }
        public ushort MagicDefence { get; set; }
        public ushort AbilityCost { get; set; }
        public ushort JewelrySlot { get; set; }
        public ushort UseItemSlot { get; set; }
        public ushort MaterialItemSlot { get; set; }
        public ushort EquipItemSlot { get; set; }
        public ushort MainPawnSlot { get; set; }
        public ushort SupportPawnSlot { get; set; }

        public class Serializer : EntitySerializer<CDataOrbGainExtendParam>
        {
            public override void Write(IBuffer buffer, CDataOrbGainExtendParam obj)
            {
                WriteUInt16(buffer, obj.HpMax);
                WriteUInt16(buffer, obj.StaminaMax);
                WriteUInt16(buffer, obj.Attack);
                WriteUInt16(buffer, obj.Defence);
                WriteUInt16(buffer, obj.MagicAttack);
                WriteUInt16(buffer, obj.MagicDefence);
                WriteUInt16(buffer, obj.AbilityCost);
                WriteUInt16(buffer, obj.JewelrySlot);
                WriteUInt16(buffer, obj.UseItemSlot);
                WriteUInt16(buffer, obj.MaterialItemSlot);
                WriteUInt16(buffer, obj.EquipItemSlot);
                WriteUInt16(buffer, obj.MainPawnSlot);
                WriteUInt16(buffer, obj.SupportPawnSlot);
            }

            public override CDataOrbGainExtendParam Read(IBuffer buffer)
            {
                CDataOrbGainExtendParam obj = new CDataOrbGainExtendParam();
                obj.HpMax = ReadUInt16(buffer);
                obj.StaminaMax = ReadUInt16(buffer);
                obj.Attack = ReadUInt16(buffer);
                obj.Defence = ReadUInt16(buffer);
                obj.MagicAttack = ReadUInt16(buffer);
                obj.MagicDefence = ReadUInt16(buffer);
                obj.AbilityCost = ReadUInt16(buffer);
                obj.JewelrySlot = ReadUInt16(buffer);
                obj.UseItemSlot = ReadUInt16(buffer);
                obj.MaterialItemSlot = ReadUInt16(buffer);
                obj.EquipItemSlot = ReadUInt16(buffer);
                obj.MainPawnSlot = ReadUInt16(buffer);
                obj.SupportPawnSlot = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
