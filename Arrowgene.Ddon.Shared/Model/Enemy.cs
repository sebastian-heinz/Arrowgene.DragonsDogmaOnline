using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Enemy
    {       
        public Enemy()
        {
            NamedEnemyParams = NamedParam.DEFAULT_NAMED_PARAM;
            DropsTable = new DropsTable();
        }

        public Enemy(Enemy enemy)
        {
            EnemyId = enemy.EnemyId;
            NamedEnemyParams = enemy.NamedEnemyParams;
            RaidBossId = enemy.RaidBossId;
            Scale = enemy.Scale;
            Lv = enemy.Lv;
            HmPresetNo = enemy.HmPresetNo;
            StartThinkTblNo = enemy.StartThinkTblNo;
            RepopNum = enemy.RepopNum;
            RepopCount = enemy.RepopCount;
            EnemyTargetTypesId = enemy.EnemyTargetTypesId;
            MontageFixNo = enemy.MontageFixNo;
            SetType = enemy.SetType;
            InfectionType = enemy.InfectionType;
            IsBossGauge = enemy.IsBossGauge;
            IsBossBGM = enemy.IsBossBGM;
            IsManualSet = enemy.IsManualSet;
            IsAreaBoss = enemy.IsAreaBoss;
            BloodOrbs = enemy.BloodOrbs;
            HighOrbs = enemy.HighOrbs;
            SpawnTimeStart = enemy.SpawnTimeStart;
            SpawnTimeEnd = enemy.SpawnTimeEnd;
            Experience = enemy.Experience;
            DropsTable = enemy.DropsTable;
            Subgroup = enemy.Subgroup;
            PPDrop = enemy.PPDrop;
            IsBloodOrbEnemy = enemy.IsBloodOrbEnemy;
            IsHighOrbEnemy = enemy.IsHighOrbEnemy;
        }

        public EnemyId EnemyId { get; set; }
        public NamedParam NamedEnemyParams { get; set; }
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
        public bool IsBloodOrbEnemy { get; set; }
        public uint HighOrbs { get; set; }
        public bool IsHighOrbEnemy { get; set; }
        public long SpawnTimeStart { get; set;}
        public long SpawnTimeEnd { get; set; }
        public uint Experience { get; set; }
        public uint RaidPoints { get; set; }
        public DropsTable DropsTable { get; set; }
        public uint PPDrop { get; set; }
        public EnemyUIId UINameId { get 
            {
                return EnemyId.GetUIId();
            } 
        }
        public byte Subgroup { get; set; }

        public uint GetDroppedExperience()
        {
            return Experience * (NamedEnemyParams.Experience / 100);
        }

        public uint GetDroppedPlayPoints()
        {
            return PPDrop;
        }

        public CDataStageLayoutEnemyPresetEnemyInfoClient AsCDataStageLayoutEnemyPresetEnemyInfoClient()
        {
            return new CDataStageLayoutEnemyPresetEnemyInfoClient()
            {
                EnemyId = EnemyId,
                NamedEnemyParamsId = NamedEnemyParams.Id,
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
                IsBloodEnemy = IsBloodOrbEnemy,
                IsHighOrbEnemy = IsHighOrbEnemy,
            };
        }        
    }
}
