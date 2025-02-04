using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterJobData
    {
        public JobId Job;
        public uint Exp;
        public uint JobPoint;
        public uint Lv;
        public ushort Atk;
        public ushort Def;
        public ushort MAtk;
        public ushort MDef;
        public ushort Strength;
        public ushort DownPower;
        public ushort ShakePower;
        public ushort StunPower;
        public ushort Consitution;
        public ushort Guts;
        public byte FireResist;
        public byte IceResist;
        public byte ThunderResist;
        public byte HolyResist;
        public byte DarkResist;
        public byte SpreadResist;
        public byte FreezeResist;
        public byte ShockResist;
        public byte AbsorbResist;
        public byte DarkElmResist;
        public byte PoisonResist;
        public byte SlowResist;
        public byte SleepResist;
        public byte StunResist;
        public byte WetResist;
        public byte OilResist;
        public byte SealResist;
        public byte CurseResist;
        public byte SoftResist;
        public byte StoneResist;
        public byte GoldResist;
        public byte FireReduceResist;
        public byte IceReduceResist;
        public byte ThunderReduceResist;
        public byte HolyReduceResist;
        public byte DarkReduceResist;
        public byte AtkDownResist;
        public byte DefDownResist;
        public byte MAtkDownResist;
        public byte MDefDownResist;

        public class Serializer : EntitySerializer<CDataCharacterJobData>
        {
            public override void Write(IBuffer buffer, CDataCharacterJobData obj)
            {
                WriteByte(buffer, (byte)obj.Job);
                WriteUInt32(buffer, obj.Exp);
                WriteUInt32(buffer, obj.JobPoint);
                WriteUInt32(buffer, obj.Lv);
                WriteUInt16(buffer, obj.Atk);
                WriteUInt16(buffer, obj.Def);
                WriteUInt16(buffer, obj.MAtk);
                WriteUInt16(buffer, obj.MDef);
                WriteUInt16(buffer, obj.Strength);
                WriteUInt16(buffer, obj.DownPower);
                WriteUInt16(buffer, obj.ShakePower);
                WriteUInt16(buffer, obj.StunPower);
                WriteUInt16(buffer, obj.Consitution);
                WriteUInt16(buffer, obj.Guts);
                WriteByte(buffer, obj.FireResist);
                WriteByte(buffer, obj.IceResist);
                WriteByte(buffer, obj.ThunderResist);
                WriteByte(buffer, obj.HolyResist);
                WriteByte(buffer, obj.DarkResist);
                WriteByte(buffer, obj.SpreadResist);
                WriteByte(buffer, obj.FreezeResist);
                WriteByte(buffer, obj.ShockResist);
                WriteByte(buffer, obj.AbsorbResist);
                WriteByte(buffer, obj.DarkElmResist);
                WriteByte(buffer, obj.PoisonResist);
                WriteByte(buffer, obj.SlowResist);
                WriteByte(buffer, obj.SleepResist);
                WriteByte(buffer, obj.StunResist);
                WriteByte(buffer, obj.WetResist);
                WriteByte(buffer, obj.OilResist);
                WriteByte(buffer, obj.SealResist);
                WriteByte(buffer, obj.CurseResist);
                WriteByte(buffer, obj.SoftResist);
                WriteByte(buffer, obj.StoneResist);
                WriteByte(buffer, obj.GoldResist);
                WriteByte(buffer, obj.FireReduceResist);
                WriteByte(buffer, obj.IceReduceResist);
                WriteByte(buffer, obj.ThunderReduceResist);
                WriteByte(buffer, obj.HolyReduceResist);
                WriteByte(buffer, obj.DarkReduceResist);
                WriteByte(buffer, obj.AtkDownResist);
                WriteByte(buffer, obj.DefDownResist);
                WriteByte(buffer, obj.MAtkDownResist);
                WriteByte(buffer, obj.MDefDownResist);
            }

            public override CDataCharacterJobData Read(IBuffer buffer)
            {
                CDataCharacterJobData obj = new CDataCharacterJobData();
                obj.Job = (JobId)ReadByte(buffer);
                obj.Exp = ReadUInt32(buffer);
                obj.JobPoint = ReadUInt32(buffer);
                obj.Lv = ReadUInt32(buffer);
                obj.Atk = ReadUInt16(buffer);
                obj.Def = ReadUInt16(buffer);
                obj.MAtk = ReadUInt16(buffer);
                obj.MDef = ReadUInt16(buffer);
                obj.Strength = ReadUInt16(buffer);
                obj.DownPower = ReadUInt16(buffer);
                obj.ShakePower = ReadUInt16(buffer);
                obj.StunPower = ReadUInt16(buffer);
                obj.Consitution = ReadUInt16(buffer);
                obj.Guts = ReadUInt16(buffer);
                obj.FireResist = ReadByte(buffer);
                obj.IceResist = ReadByte(buffer);
                obj.ThunderResist = ReadByte(buffer);
                obj.HolyResist = ReadByte(buffer);
                obj.DarkResist = ReadByte(buffer);
                obj.SpreadResist = ReadByte(buffer);
                obj.FreezeResist = ReadByte(buffer);
                obj.ShockResist = ReadByte(buffer);
                obj.AbsorbResist = ReadByte(buffer);
                obj.DarkElmResist = ReadByte(buffer);
                obj.PoisonResist = ReadByte(buffer);
                obj.SlowResist = ReadByte(buffer);
                obj.SleepResist = ReadByte(buffer);
                obj.StunResist = ReadByte(buffer);
                obj.WetResist = ReadByte(buffer);
                obj.OilResist = ReadByte(buffer);
                obj.SealResist = ReadByte(buffer);
                obj.CurseResist = ReadByte(buffer);
                obj.SoftResist = ReadByte(buffer);
                obj.StoneResist = ReadByte(buffer);
                obj.GoldResist = ReadByte(buffer);
                obj.FireReduceResist = ReadByte(buffer);
                obj.IceReduceResist = ReadByte(buffer);
                obj.ThunderReduceResist = ReadByte(buffer);
                obj.HolyReduceResist = ReadByte(buffer);
                obj.DarkReduceResist = ReadByte(buffer);
                obj.AtkDownResist = ReadByte(buffer);
                obj.DefDownResist = ReadByte(buffer);
                obj.MAtkDownResist = ReadByte(buffer);
                obj.MDefDownResist = ReadByte(buffer);
                return obj;
            }
        }
    }
}
