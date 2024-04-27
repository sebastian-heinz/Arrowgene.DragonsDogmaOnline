using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    /// <summary>
    /// CDataStageLayoutEnemyPresetEnemyInfoClient
    /// </summary>
    public class CDataStageLayoutEnemyPresetEnemyInfoClient
    {
        public CDataStageLayoutEnemyPresetEnemyInfoClient()
        {
            EnemyId = 0;
            NamedEnemyParamsId = 0;
            RaidBossId = 0;
            Scale = 0;
            Lv = 0;
            HmPresetNo = 0;
            StartThinkTblNo = 0;
            RepopNum = 0;
            RepopCount = 0;
            EnemyTargetTypesId = 0;
            MontageFixNo = 0;
            SetType = 0;
            InfectionType = 0;
            IsBossGauge = false;
            IsBossBGM = false;
            IsManualSet = false;
            IsAreaBoss = false;
            IsBloodEnemy = false;
            IsHighOrbEnemy = false;
        }

        public uint EnemyId { get; set; }
        public uint NamedEnemyParamsId { get; set; }
        public uint RaidBossId { get; set; }
        public ushort Scale { get; set; } // Scale as a percentage, 100(%) is normal
        public ushort Lv { get; set; } // Level
        public ushort HmPresetNo { get; set; }
        public byte StartThinkTblNo { get; set; } // Start Think Table Number???
        public byte RepopNum { get; set; }
        public byte RepopCount { get; set; }
        public byte EnemyTargetTypesId { get; set; }
        public byte MontageFixNo { get; set; }
        public byte SetType { get; set; }
        public byte InfectionType { get; set; }
        public bool IsBossGauge { get; set; }
        public bool IsBossBGM { get; set; }
        public bool IsManualSet { get; set; }
        public bool IsAreaBoss { get; set; }
        public bool IsBloodEnemy { get; set; }
        public bool IsHighOrbEnemy { get; set; }

        public class Serializer : EntitySerializer<CDataStageLayoutEnemyPresetEnemyInfoClient>
        {
            public override void Write(IBuffer buffer, CDataStageLayoutEnemyPresetEnemyInfoClient obj)
            {
                WriteUInt32(buffer, obj.EnemyId);
                WriteUInt32(buffer, obj.NamedEnemyParamsId);
                WriteUInt32(buffer, obj.RaidBossId);
                WriteUInt16(buffer, obj.Scale);
                WriteUInt16(buffer, obj.Lv);
                WriteUInt16(buffer, obj.HmPresetNo);
                WriteByte(buffer, obj.StartThinkTblNo);
                WriteByte(buffer, obj.RepopCount);
                WriteByte(buffer, obj.RepopNum);
                WriteByte(buffer, obj.EnemyTargetTypesId);
                WriteByte(buffer, obj.MontageFixNo);
                WriteByte(buffer, obj.SetType);
                WriteByte(buffer, obj.InfectionType);
                WriteBool(buffer, obj.IsBossGauge);
                WriteBool(buffer, obj.IsBossBGM);
                WriteBool(buffer, obj.IsManualSet);
                WriteBool(buffer, obj.IsAreaBoss);
                WriteBool(buffer, obj.IsBloodEnemy);
                WriteBool(buffer, obj.IsHighOrbEnemy);
            }

            public override CDataStageLayoutEnemyPresetEnemyInfoClient Read(IBuffer buffer)
            {
                CDataStageLayoutEnemyPresetEnemyInfoClient obj = new CDataStageLayoutEnemyPresetEnemyInfoClient();
                obj.EnemyId = ReadUInt32(buffer);
                obj.NamedEnemyParamsId = ReadUInt32(buffer);
                obj.RaidBossId = ReadUInt32(buffer);
                obj.Scale = ReadUInt16(buffer);
                obj.Lv = ReadUInt16(buffer);
                obj.HmPresetNo = ReadUInt16(buffer);
                obj.StartThinkTblNo = ReadByte(buffer);
                obj.RepopCount = ReadByte(buffer);
                obj.RepopNum = ReadByte(buffer);
                obj.EnemyTargetTypesId = ReadByte(buffer);
                obj.MontageFixNo = ReadByte(buffer);
                obj.SetType = ReadByte(buffer);
                obj.InfectionType = ReadByte(buffer);
                obj.IsBossGauge = ReadBool(buffer);
                obj.IsBossBGM = ReadBool(buffer);
                obj.IsManualSet = ReadBool(buffer);
                obj.IsAreaBoss = ReadBool(buffer);
                obj.IsBloodEnemy = ReadBool(buffer);
                obj.IsHighOrbEnemy = ReadBool(buffer);
                return obj;
            }
        }
    }
}
