using System;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
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
