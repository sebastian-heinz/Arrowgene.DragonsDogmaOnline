using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class EnemySpawnCsv : CsvReaderWriter<EnemySpawn>
    {
        public EnemySpawnCsv() : base()
        {
            
        }
        
        protected override int NumExpectedItems => 5;

        protected override EnemySpawn CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint stageId)) return null;
            if (!byte.TryParse(properties[1], out byte layerNo)) return null;
            if (!uint.TryParse(properties[2], out uint groupNo)) return null;
            if (!byte.TryParse(properties[3], out byte subGroupNo)) return null;
            if (!TryParseHexUInt(properties[4], out uint enemyId)) return null;
            if (!TryParseHexUInt(properties[5], out uint namedEnemyParamsId)) return null;
            if (!uint.TryParse(properties[6], out uint raidBossId)) return null;
            if (!ushort.TryParse(properties[7], out ushort scale)) return null;
            if (!ushort.TryParse(properties[8], out ushort lv)) return null;
            if (!ushort.TryParse(properties[9], out ushort hmPresetNo)) return null;
            if (!byte.TryParse(properties[10], out byte startThinkTblNo)) return null;
            if (!byte.TryParse(properties[11], out byte repopNum)) return null;
            if (!byte.TryParse(properties[12], out byte repopCount)) return null;
            if (!byte.TryParse(properties[13], out byte enemyTargetTypesId)) return null;
            if (!byte.TryParse(properties[14], out byte montageFixNo)) return null;
            if (!byte.TryParse(properties[15], out byte setType)) return null;
            if (!byte.TryParse(properties[16], out byte infectionType)) return null;
            if (!bool.TryParse(properties[17], out bool isBossGauge)) return null;
            if (!bool.TryParse(properties[18], out bool isBossBGM)) return null;
            if (!bool.TryParse(properties[19], out bool isManualSet)) return null;
            if (!bool.TryParse(properties[20], out bool isAreaBoss)) return null;
            if (!bool.TryParse(properties[21], out bool isBloodEnemy)) return null;
            if (!bool.TryParse(properties[22], out bool isHighOrbEnemy)) return null;

            return new EnemySpawn
            {
                Id = 0,
                StageId = new StageId(stageId, layerNo, groupNo),
                SubGroupId = subGroupNo,
                Enemy = new CDataStageLayoutEnemyPresetEnemyInfoClient()
                {
                    EnemyId = enemyId,
                    NamedEnemyParamsId = namedEnemyParamsId,
                    RaidBossId = raidBossId,
                    Scale = scale,
                    Lv = lv,
                    HmPresetNo = hmPresetNo,
                    StartThinkTblNo = startThinkTblNo,
                    RepopNum = repopNum,
                    RepopCount = repopCount,
                    EnemyTargetTypesId = enemyTargetTypesId,
                    MontageFixNo = montageFixNo,
                    SetType = setType,
                    InfectionType = infectionType,
                    IsBossGauge = isBossGauge,
                    IsBossBGM = isBossBGM,
                    IsManualSet = isManualSet,
                    IsAreaBoss = isAreaBoss,
                    IsBloodEnemy = isBloodEnemy,
                    IsHighOrbEnemy = isHighOrbEnemy,
                }
            };
        }
    }
}
