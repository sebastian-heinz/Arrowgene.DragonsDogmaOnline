using System;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Enemy
    {       
        public Enemy() {
            DropsTable = new DropsTable();
        }

        public uint Id { get; set; }
        public TimeSpan Time { get; set; }

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
        public uint BloodOrbs { get; set; }
        public uint HighOrbs { get; set; }
        public string SpawnTime { get; set; }
        public uint Experience { get; set; }
        public DropsTable DropsTable { get; set; }
  

        public CDataStageLayoutEnemyPresetEnemyInfoClient asCDataStageLayoutEnemyPresetEnemyInfoClient()
        {
            return new CDataStageLayoutEnemyPresetEnemyInfoClient()
            {
                EnemyId = EnemyId,
                NamedEnemyParamsId = NamedEnemyParamsId,
                RaidBossId = RaidBossId,
                Scale = Scale,
                Lv = Lv,
                HmPresetNo = HmPresetNo,
                StartThinkTblNo = StartThinkTblNo,
                RepopNum = RepopNum,
                RepopCount = RepopCount,
                EnemyTargetTypesId = EnemyTargetTypesId,
                MontageFixNo = MontageFixNo,
                SetType = SetType,
                InfectionType = InfectionType,
                IsBossGauge = IsBossGauge,
                IsBossBGM = IsBossBGM,
                IsManualSet = IsManualSet,
                IsAreaBoss = IsAreaBoss,
                IsBloodEnemy = BloodOrbs > 0,
                IsHighOrbEnemy = HighOrbs > 0,
                SpawnTime = SpawnTime,
            };
        }
    }
}
