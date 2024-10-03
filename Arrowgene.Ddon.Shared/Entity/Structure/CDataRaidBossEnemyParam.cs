using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRaidBossEnemyParam
    {
        public CDataRaidBossEnemyParam()
        {
        }

        public uint RaidBossId { get; set; }
        public uint RateHp { get; set; }
        public ushort RateHpPart { get; set; }
        public ushort RateShrinkDef { get; set; }
        public ushort RateShrinkDefPart { get; set; }
        public ushort RateBlowDef { get; set; }
        public ushort RateBlowDefPart { get; set; }
        public ushort RateOcdDef { get; set; }
        public ushort RateOcdAtk { get; set; }
        public ushort RateShakeDef { get; set; }
        public ushort RateDownDef { get; set; }
        public ushort RateStr { get; set; }
        public ushort RatePhyAtkBase { get; set; }
        public ushort RateMagAtkBase { get; set; }
        public ushort RatePhyDefBase { get; set; }
        public ushort RateMagDefBase { get; set; }

        public class Serializer : EntitySerializer<CDataRaidBossEnemyParam>
        {
            public override void Write(IBuffer buffer, CDataRaidBossEnemyParam obj)
            {
                WriteUInt32(buffer, obj.RaidBossId);
                WriteUInt32(buffer, obj.RateHp);
                WriteUInt16(buffer, obj.RateHpPart);
                WriteUInt16(buffer, obj.RateShrinkDef);
                WriteUInt16(buffer, obj.RateShrinkDefPart);
                WriteUInt16(buffer, obj.RateBlowDef);
                WriteUInt16(buffer, obj.RateBlowDefPart);
                WriteUInt16(buffer, obj.RateOcdDef);
                WriteUInt16(buffer, obj.RateOcdAtk);
                WriteUInt16(buffer, obj.RateShakeDef);
                WriteUInt16(buffer, obj.RateDownDef);
                WriteUInt16(buffer, obj.RateStr);
                WriteUInt16(buffer, obj.RatePhyAtkBase);
                WriteUInt16(buffer, obj.RateMagAtkBase);
                WriteUInt16(buffer, obj.RatePhyDefBase);
                WriteUInt16(buffer, obj.RateMagDefBase);
            }

            public override CDataRaidBossEnemyParam Read(IBuffer buffer)
            {
                CDataRaidBossEnemyParam obj = new CDataRaidBossEnemyParam();
                obj.RaidBossId = ReadUInt32(buffer);
                obj.RateHp = ReadUInt32(buffer);
                obj.RateHpPart = ReadUInt16(buffer);
                obj.RateShrinkDef = ReadUInt16(buffer);
                obj.RateShrinkDefPart = ReadUInt16(buffer);
                obj.RateBlowDef = ReadUInt16(buffer);
                obj.RateBlowDefPart = ReadUInt16(buffer);
                obj.RateOcdDef = ReadUInt16(buffer);
                obj.RateOcdAtk = ReadUInt16(buffer);
                obj.RateShakeDef = ReadUInt16(buffer);
                obj.RateDownDef = ReadUInt16(buffer);
                obj.RateStr = ReadUInt16(buffer);
                obj.RatePhyAtkBase = ReadUInt16(buffer);
                obj.RateMagAtkBase = ReadUInt16(buffer);
                obj.RatePhyDefBase = ReadUInt16(buffer);
                obj.RateMagDefBase = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
