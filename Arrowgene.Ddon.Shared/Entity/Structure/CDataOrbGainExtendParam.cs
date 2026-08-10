using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOrbGainExtendParam
    {
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

        public static CDataOrbGainExtendParam operator +(CDataOrbGainExtendParam a, CDataOrbGainExtendParam b)
        {
            return new CDataOrbGainExtendParam()
            {
                HpMax = (ushort)(a.HpMax + b.HpMax),
                StaminaMax = (ushort)(a.StaminaMax + b.StaminaMax),
                Attack = (ushort)(a.Attack + b.Attack),
                Defence = (ushort)(a.Defence + b.Defence),
                MagicAttack = (ushort)(a.MagicAttack + b.MagicAttack),
                MagicDefence = (ushort)(a.MagicDefence + b.MagicDefence),
                AbilityCost = (ushort)(a.AbilityCost + b.AbilityCost),
                JewelrySlot = (ushort)(a.JewelrySlot + b.JewelrySlot),
                UseItemSlot = (ushort)(a.UseItemSlot + b.UseItemSlot),
                MaterialItemSlot = (ushort)(a.MaterialItemSlot + b.MaterialItemSlot),
                EquipItemSlot = (ushort)(a.EquipItemSlot + b.EquipItemSlot),
                MainPawnSlot = (ushort)(a.MainPawnSlot + b.MainPawnSlot),
                SupportPawnSlot = (ushort)(a.SupportPawnSlot + b.SupportPawnSlot),
            };
        }

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
