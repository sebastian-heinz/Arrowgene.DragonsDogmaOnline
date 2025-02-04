using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStatusInfo
    {
        public uint HP;
        public uint Stamina;
        public byte RevivePoint;
        public uint MaxHP;
        public uint MaxStamina;
        public uint WhiteHP;
        public uint GainHP;
        public uint GainStamina;
        public uint GainAttack;
        public uint GainDefense;
        public uint GainMagicAttack;
        public uint GainMagicDefense;

        public class Serializer : EntitySerializer<CDataStatusInfo>
        {
            public override void Write(IBuffer buffer, CDataStatusInfo obj)
            {
                WriteUInt32(buffer, obj.HP);
                WriteUInt32(buffer, obj.Stamina);
                WriteByte(buffer, obj.RevivePoint);
                WriteUInt32(buffer, obj.MaxHP);
                WriteUInt32(buffer, obj.MaxStamina);
                WriteUInt32(buffer, obj.WhiteHP);
                WriteUInt32(buffer, obj.GainHP);
                WriteUInt32(buffer, obj.GainStamina);
                WriteUInt32(buffer, obj.GainAttack);
                WriteUInt32(buffer, obj.GainDefense);
                WriteUInt32(buffer, obj.GainMagicAttack);
                WriteUInt32(buffer, obj.GainMagicDefense);
            }

            public override CDataStatusInfo Read(IBuffer buffer)
            {
                CDataStatusInfo obj = new CDataStatusInfo();
                obj.HP = ReadUInt32(buffer);
                obj.Stamina = ReadUInt32(buffer);
                obj.RevivePoint = ReadByte(buffer);
                obj.MaxHP = ReadUInt32(buffer);
                obj.MaxStamina = ReadUInt32(buffer);
                obj.WhiteHP = ReadUInt32(buffer);
                obj.GainHP = ReadUInt32(buffer);
                obj.GainStamina = ReadUInt32(buffer);
                obj.GainAttack = ReadUInt32(buffer);
                obj.GainDefense = ReadUInt32(buffer);
                obj.GainMagicAttack = ReadUInt32(buffer);
                obj.GainMagicDefense = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
