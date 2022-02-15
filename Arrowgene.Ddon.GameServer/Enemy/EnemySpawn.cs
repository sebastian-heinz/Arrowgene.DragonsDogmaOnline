using System;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemy
{
    public class EnemySpawn
    {
        public EnemySpawn()
        {
            Enemy = new CDataStageLayoutEnemyPresetEnemyInfoClient();
        }
        
        public uint Id { get; set; }
        public TimeSpan Time { get; set; }
        public StageId StageId { get; set; }
        public CDataStageLayoutEnemyPresetEnemyInfoClient Enemy { get; set; }
        public byte SubGroupId { get; set; }
    }
}
