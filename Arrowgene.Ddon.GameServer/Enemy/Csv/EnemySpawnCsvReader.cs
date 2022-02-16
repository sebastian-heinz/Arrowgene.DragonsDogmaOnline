using System.Globalization;
using System;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemy.Csv
{
    public class EnemySpawnCsvReader : CsvReader<EnemySpawn>
    {
        protected override int NumExpectedItems => 6;

        protected override EnemySpawn CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint stageId)) return null;
            if (!byte.TryParse(properties[1], out byte layerNo)) return null;
            if (!uint.TryParse(properties[2], out uint groupId)) return null;
            if (!byte.TryParse(properties[3], out byte subGroupId)) return null;
            if (!uint.TryParse(properties[4], out uint positionIndex)) return null;
            if (!uint.TryParse(properties[5].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint enemyId)) return null;
            
            EnemySpawn enemySpawn = new EnemySpawn();
            enemySpawn.StageId = new StageId(stageId, layerNo, groupId);
            enemySpawn.SubGroupId = subGroupId;
            enemySpawn.Enemy.EnemyId = enemyId;
            enemySpawn.Enemy.Lv = 20;
            enemySpawn.Enemy.Scale = 100;
            enemySpawn.Enemy.NamedEnemyParamsId = 0x8FA;
            enemySpawn.Enemy.EnemyTargetTypesId = 1;
            return enemySpawn;
        }
    }
}