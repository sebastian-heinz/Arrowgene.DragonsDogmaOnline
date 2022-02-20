using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataNamedEnemyParamClient
    {
        public CDataNamedEnemyParamClient()
        {
            Id = 0;
            RateHp = 0;
            RateHpPart = 0;
            RatePhyAttackBase = 0;
            RatePhyAttackEQ = 0;
            RatePhyDefenceBase = 0;
            RatePhyDefenceEq = 0;
            RateMagAttackBase = 0;
            RateMagAttackEQ = 0;
            RateMagDefenceBase = 0;
            RateMagDefenceEq = 0;
            RateStr = 0;
            RateGuardAtk = 0;
            RateGuardDefBase = 0;
            RateGuardDefEq = 0;
            RateShrinkDef = 0;
            RateBlowDef = 0;
            RateDownDef = 0;
            RateShakeDef = 0;
            RateShrinkDefPart = 0;
            RateBlowDefPart = 0;
            RateOcdDef = 0;
            RateOcdAtk = 0;
            NameTypeId = 0;
        }

        public uint Id { get; set; }
        public ushort RateHp { get; set; }
        public ushort RateHpPart { get; set; }
        public ushort RatePhyAttackBase { get; set; }
        public ushort RatePhyAttackEQ { get; set; }
        public ushort RatePhyDefenceBase { get; set; }
        public ushort RatePhyDefenceEq { get; set; }
        public ushort RateMagAttackBase { get; set; }
        public ushort RateMagAttackEQ { get; set; }
        public ushort RateMagDefenceBase { get; set; }
        public ushort RateMagDefenceEq { get; set; }
        public ushort RateStr { get; set; }
        public ushort RateGuardAtk { get; set; }
        public ushort RateGuardDefBase { get; set; }
        public ushort RateGuardDefEq { get; set; }
        public ushort RateShrinkDef { get; set; }
        public ushort RateBlowDef { get; set; }
        public ushort RateDownDef { get; set; }
        public ushort RateShakeDef { get; set; }
        public ushort RateShrinkDefPart { get; set; }
        public ushort RateBlowDefPart { get; set; }
        public ushort RateOcdDef { get; set; }
        public ushort RateOcdAtk { get; set; }
        public byte NameTypeId { get; set; }

        public class Serializer : EntitySerializer<CDataNamedEnemyParamClient>
        {
            public override void Write(IBuffer buffer, CDataNamedEnemyParamClient obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt16(buffer, obj.RateHp);
                WriteUInt16(buffer, obj.RateHpPart);
                WriteUInt16(buffer, obj.RatePhyAttackBase);
                WriteUInt16(buffer, obj.RatePhyAttackEQ);
                WriteUInt16(buffer, obj.RatePhyDefenceBase);
                WriteUInt16(buffer, obj.RatePhyDefenceEq);
                WriteUInt16(buffer, obj.RateMagAttackBase);
                WriteUInt16(buffer, obj.RateMagAttackEQ);
                WriteUInt16(buffer, obj.RateMagDefenceBase);
                WriteUInt16(buffer, obj.RateMagDefenceEq);
                WriteUInt16(buffer, obj.RateStr);
                WriteUInt16(buffer, obj.RateGuardAtk);
                WriteUInt16(buffer, obj.RateGuardDefBase);
                WriteUInt16(buffer, obj.RateGuardDefEq);
                WriteUInt16(buffer, obj.RateShrinkDef);
                WriteUInt16(buffer, obj.RateBlowDef);
                WriteUInt16(buffer, obj.RateDownDef);
                WriteUInt16(buffer, obj.RateShakeDef);
                WriteUInt16(buffer, obj.RateShrinkDefPart);
                WriteUInt16(buffer, obj.RateBlowDefPart);
                WriteUInt16(buffer, obj.RateOcdDef);
                WriteUInt16(buffer, obj.RateOcdAtk);
                WriteByte(buffer, obj.NameTypeId);
            }

            public override CDataNamedEnemyParamClient Read(IBuffer buffer)
            {
                CDataNamedEnemyParamClient obj = new CDataNamedEnemyParamClient();
                obj.Id = ReadUInt32(buffer);
                obj.RateHp = ReadUInt16(buffer);
                obj.RateHpPart = ReadUInt16(buffer);
                obj.RatePhyAttackBase = ReadUInt16(buffer);
                obj.RatePhyAttackEQ = ReadUInt16(buffer);
                obj.RatePhyDefenceBase = ReadUInt16(buffer);
                obj.RatePhyDefenceEq = ReadUInt16(buffer);
                obj.RateMagAttackBase = ReadUInt16(buffer);
                obj.RateMagAttackEQ = ReadUInt16(buffer);
                obj.RateMagDefenceBase = ReadUInt16(buffer);
                obj.RateMagDefenceEq = ReadUInt16(buffer);
                obj.RateStr = ReadUInt16(buffer);
                obj.RateGuardAtk = ReadUInt16(buffer);
                obj.RateGuardDefBase = ReadUInt16(buffer);
                obj.RateGuardDefEq = ReadUInt16(buffer);
                obj.RateShrinkDef = ReadUInt16(buffer);
                obj.RateBlowDef = ReadUInt16(buffer);
                obj.RateDownDef = ReadUInt16(buffer);
                obj.RateShakeDef = ReadUInt16(buffer);
                obj.RateShrinkDefPart = ReadUInt16(buffer);
                obj.RateBlowDefPart = ReadUInt16(buffer);
                obj.RateOcdDef = ReadUInt16(buffer);
                obj.RateOcdAtk = ReadUInt16(buffer);
                obj.NameTypeId = ReadByte(buffer);
                return obj;
            }
        }
    }
}
