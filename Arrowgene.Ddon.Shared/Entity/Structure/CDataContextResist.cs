using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextResist
    {

        public CDataContextResist(Character character)
        {
            CDataCharacterJobData characterJobData = character.CharacterInfo.CharacterJobDataList
                .Where(x => x.Job == character.CharacterInfo.Job)
                .Single();

            FireResist = characterJobData.FireResist;
            IceResist = characterJobData.IceResist;
            ThunderResist = characterJobData.ThunderResist;
            HolyResist = characterJobData.HolyResist;
            DarkResist = characterJobData.DarkResist;
            SpreadResist = characterJobData.SpreadResist;
            FreezeResist = characterJobData.FreezeResist;
            ShockResist = characterJobData.ShockResist;
            AbsorbResist = characterJobData.AbsorbResist;
            DarkElmResist = characterJobData.DarkElmResist;
            PoisonResist = characterJobData.PoisonResist;
            SlowResist = characterJobData.SlowResist;
            SleepResist = characterJobData.SleepResist;
            StunResist = characterJobData.StunResist;
            WetResist = characterJobData.WetResist;
            OilResist = characterJobData.OilResist;
            SealResist = characterJobData.SealResist;
            CurseResist = characterJobData.CurseResist;
            SoftResist = characterJobData.SoftResist;
            StoneResist = characterJobData.StoneResist;
            GoldResist = characterJobData.GoldResist;
            FireReduceResist = characterJobData.FireReduceResist;
            IceReduceResist = characterJobData.IceReduceResist;
            ThunderReduceResist = characterJobData.ThunderReduceResist;
            HolyReduceResist = characterJobData.HolyReduceResist;
            DarkReduceResist = characterJobData.DarkReduceResist;
            AtkDownResist = characterJobData.AtkDownResist;
            DefDownResist = characterJobData.DefDownResist;
            MAtkDownResist = characterJobData.MAtkDownResist;
            MDefDownResist = characterJobData.MDefDownResist;
        }

        public CDataContextResist()
        {
        }

        public byte FireResist { get; set; }
        public byte IceResist { get; set; }
        public byte ThunderResist { get; set; }
        public byte HolyResist { get; set; }
        public byte DarkResist { get; set; }
        public byte SpreadResist { get; set; }
        public byte FreezeResist { get; set; }
        public byte ShockResist { get; set; }
        public byte AbsorbResist { get; set; }
        public byte DarkElmResist { get; set; }
        public byte PoisonResist { get; set; }
        public byte SlowResist { get; set; }
        public byte SleepResist { get; set; }
        public byte StunResist { get; set; }
        public byte WetResist { get; set; }
        public byte OilResist { get; set; }
        public byte SealResist { get; set; }
        public byte CurseResist { get; set; }
        public byte SoftResist { get; set; }
        public byte StoneResist { get; set; }
        public byte GoldResist { get; set; }
        public byte FireReduceResist { get; set; }
        public byte IceReduceResist { get; set; }
        public byte ThunderReduceResist { get; set; }
        public byte HolyReduceResist { get; set; }
        public byte DarkReduceResist { get; set; }
        public byte AtkDownResist { get; set; }
        public byte DefDownResist { get; set; }
        public byte MAtkDownResist { get; set; }
        public byte MDefDownResist { get; set; }

        public class Serializer : EntitySerializer<CDataContextResist>
        {
            public override void Write(IBuffer buffer, CDataContextResist obj)
            {
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

            public override CDataContextResist Read(IBuffer buffer)
            {
                CDataContextResist obj = new CDataContextResist();
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