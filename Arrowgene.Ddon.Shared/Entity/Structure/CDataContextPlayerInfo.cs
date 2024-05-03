using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextPlayerInfo
    {
        public CDataContextPlayerInfo()
        {
            JobList=new List<CDataContextJobData>();
            ChargeEffectList=new List<CDataCommonU32>();
            OcdActiveList=new List<CDataOcdActive>();
        }

        public static CDataContextPlayerInfo FromPawn(Pawn pawn)
        {
            CDataContextPlayerInfo obj = new CDataContextPlayerInfo()
            {
                Job = pawn.Job,
                HP = pawn.StatusInfo.HP,
                MaxHP = pawn.StatusInfo.MaxHP,
                WhiteHP = pawn.StatusInfo.WhiteHP,
                Stamina = pawn.StatusInfo.Stamina,
                MaxStamina = pawn.StatusInfo.MaxStamina,
                // Weight
                Lv = (ushort) pawn.ActiveCharacterJobData.Lv,
                Exp = pawn.ActiveCharacterJobData.Exp,
                Atk = pawn.ActiveCharacterJobData.Atk,
                Def = pawn.ActiveCharacterJobData.Def,
                MAtk = pawn.ActiveCharacterJobData.MAtk,
                MDef = pawn.ActiveCharacterJobData.MDef,
                Strength = pawn.ActiveCharacterJobData.Strength,
                DownPower = pawn.ActiveCharacterJobData.DownPower,
                ShakePower = pawn.ActiveCharacterJobData.ShakePower,
                StanPower = pawn.ActiveCharacterJobData.StunPower,
                Constitution = pawn.ActiveCharacterJobData.Consitution,
                Guts = pawn.ActiveCharacterJobData.Guts,
                JobPoint = pawn.ActiveCharacterJobData.JobPoint,
                GainHp = pawn.StatusInfo.GainHP,
                GainStamina = pawn.StatusInfo.GainStamina,
                GainAttack = pawn.StatusInfo.GainAttack,
                GainDefense = pawn.StatusInfo.GainDefense,
                GainMagicAttack = pawn.StatusInfo.GainMagicAttack,
                GainMagicDefense = pawn.StatusInfo.GainMagicDefense,
                ActNo = 0,
                RevivePoint = 0,
                CustomSkillGroup = 0,
                JobList = CDataContextJobData.FromCDataCharacterJobData(pawn.CharacterJobDataList)
                // ChargeEffectList
                // OcdActiveList
            };
            return obj;
        }

        public JobId Job { get; set; }
        public float HP { get; set; }
        public float MaxHP { get; set; }
        public float WhiteHP { get; set; }
        public float Stamina { get; set; }
        public float MaxStamina { get; set; }
        public float Weight { get; set; }
        public ushort Lv { get; set; }
        public ulong Exp { get; set; }
        public uint Atk { get; set; }
        public uint Def { get; set; }
        public uint MAtk { get; set; }
        public uint MDef { get; set; }
        public uint Strength { get; set; }
        public uint DownPower { get; set; }
        public uint ShakePower { get; set; }
        public uint StanPower { get; set; }
        public uint Constitution { get; set; }
        public uint Guts { get; set; }
        public ulong JobPoint { get; set; }
        public uint GainHp { get; set; }
        public uint GainStamina { get; set; }
        public uint GainAttack { get; set; }
        public uint GainDefense { get; set; }
        public uint GainMagicAttack { get; set; }
        public uint GainMagicDefense { get; set; }
        public uint ActNo { get; set; }
        public byte RevivePoint { get; set; }
        public byte CustomSkillGroup { get; set; }
        public List<CDataContextJobData> JobList { get; set; }
        public List<CDataCommonU32> ChargeEffectList { get; set; }
        public List<CDataOcdActive> OcdActiveList { get; set; }
        public double CliffX { get; set; }
        public float CliffY { get; set; }
        public double CliffZ { get; set; }
        public float CliffNormalX { get; set; }
        public float CliffNormalY { get; set; }
        public float CliffNormalZ { get; set; }
        public double CliffStartX { get; set; }
        public float CliffStartY { get; set; }
        public double CliffStartZ { get; set; }
        public double CliffStartOldX { get; set; }
        public float CliffStartOldY { get; set; }
        public double CliffStartOldZ { get; set; }
        public byte CatchType { get; set; }
        public byte CatchJointNo { get; set; }
        public ulong Unk0 { get; set; } // Probably CatchTargetUID, in the PS4 it was an uint
        public ushort CustomWork { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataContextPlayerInfo>
        {
            public override void Write(IBuffer buffer, CDataContextPlayerInfo obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteFloat(buffer, obj.HP);
                WriteFloat(buffer, obj.MaxHP);
                WriteFloat(buffer, obj.WhiteHP);
                WriteFloat(buffer, obj.Stamina);
                WriteFloat(buffer, obj.MaxStamina);
                WriteFloat(buffer, obj.Weight);
                WriteUInt16(buffer, obj.Lv);
                WriteUInt64(buffer, obj.Exp);
                WriteUInt32(buffer, obj.Atk);
                WriteUInt32(buffer, obj.Def);
                WriteUInt32(buffer, obj.MAtk);
                WriteUInt32(buffer, obj.MDef);
                WriteUInt32(buffer, obj.Strength);
                WriteUInt32(buffer, obj.DownPower);
                WriteUInt32(buffer, obj.ShakePower);
                WriteUInt32(buffer, obj.StanPower);
                WriteUInt32(buffer, obj.Constitution);
                WriteUInt32(buffer, obj.Guts);
                WriteUInt64(buffer, obj.JobPoint);
                WriteUInt32(buffer, obj.GainHp);
                WriteUInt32(buffer, obj.GainStamina);
                WriteUInt32(buffer, obj.GainAttack);
                WriteUInt32(buffer, obj.GainDefense);
                WriteUInt32(buffer, obj.GainMagicAttack);
                WriteUInt32(buffer, obj.GainMagicDefense);
                WriteUInt32(buffer, obj.ActNo);
                WriteByte(buffer, obj.RevivePoint);
                WriteByte(buffer, obj.CustomSkillGroup);
                WriteEntityList<CDataContextJobData>(buffer, obj.JobList);
                WriteEntityList<CDataCommonU32>(buffer, obj.ChargeEffectList);
                WriteEntityList<CDataOcdActive>(buffer, obj.OcdActiveList);
                WriteDouble(buffer, obj.CliffX);
                WriteFloat(buffer, obj.CliffY);
                WriteDouble(buffer, obj.CliffZ);
                WriteFloat(buffer, obj.CliffNormalX);
                WriteFloat(buffer, obj.CliffNormalY);
                WriteFloat(buffer, obj.CliffNormalZ);
                WriteDouble(buffer, obj.CliffStartX);
                WriteFloat(buffer, obj.CliffStartY);
                WriteDouble(buffer, obj.CliffStartZ);
                WriteDouble(buffer, obj.CliffStartOldX);
                WriteFloat(buffer, obj.CliffStartOldY);
                WriteDouble(buffer, obj.CliffStartOldZ);
                WriteByte(buffer, obj.CatchType);
                WriteByte(buffer, obj.CatchJointNo);
                WriteUInt64(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.CustomWork);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
            }

            public override CDataContextPlayerInfo Read(IBuffer buffer)
            {
                CDataContextPlayerInfo obj = new CDataContextPlayerInfo();
                obj.Job = (JobId) ReadByte(buffer);
                obj.HP = ReadFloat(buffer);
                obj.MaxHP = ReadFloat(buffer);
                obj.WhiteHP = ReadFloat(buffer);
                obj.Stamina = ReadFloat(buffer);
                obj.MaxStamina = ReadFloat(buffer);
                obj.Weight = ReadFloat(buffer);
                obj.Lv = ReadUInt16(buffer);
                obj.Exp = ReadUInt64(buffer);
                obj.Atk = ReadUInt32(buffer);
                obj.Def = ReadUInt32(buffer);
                obj.MAtk = ReadUInt32(buffer);
                obj.MDef = ReadUInt32(buffer);
                obj.Strength = ReadUInt32(buffer);
                obj.DownPower = ReadUInt32(buffer);
                obj.ShakePower = ReadUInt32(buffer);
                obj.StanPower = ReadUInt32(buffer);
                obj.Constitution = ReadUInt32(buffer);
                obj.Guts = ReadUInt32(buffer);
                obj.JobPoint = ReadUInt64(buffer);
                obj.GainHp = ReadUInt32(buffer);
                obj.GainStamina = ReadUInt32(buffer);
                obj.GainAttack = ReadUInt32(buffer);
                obj.GainDefense = ReadUInt32(buffer);
                obj.GainMagicAttack = ReadUInt32(buffer);
                obj.GainMagicDefense = ReadUInt32(buffer);
                obj.ActNo = ReadUInt32(buffer);
                obj.RevivePoint = ReadByte(buffer);
                obj.CustomSkillGroup = ReadByte(buffer);
                obj.JobList = ReadEntityList<CDataContextJobData>(buffer);
                obj.ChargeEffectList = ReadEntityList<CDataCommonU32>(buffer);
                obj.OcdActiveList = ReadEntityList<CDataOcdActive>(buffer);
                obj.CliffX = ReadDouble(buffer);
                obj.CliffY = ReadFloat(buffer);
                obj.CliffZ = ReadDouble(buffer);
                obj.CliffNormalX = ReadFloat(buffer);
                obj.CliffNormalY = ReadFloat(buffer);
                obj.CliffNormalZ = ReadFloat(buffer);
                obj.CliffStartX = ReadDouble(buffer);
                obj.CliffStartY = ReadFloat(buffer);
                obj.CliffStartZ = ReadDouble(buffer);
                obj.CliffStartOldX = ReadDouble(buffer);
                obj.CliffStartOldY = ReadFloat(buffer);
                obj.CliffStartOldZ = ReadDouble(buffer);
                obj.CatchType = ReadByte(buffer);
                obj.CatchJointNo = ReadByte(buffer);
                obj.Unk0 = ReadUInt64(buffer);
                obj.CustomWork = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
