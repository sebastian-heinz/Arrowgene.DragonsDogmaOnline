using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceQuestDropManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceQuestDropManager));
        // <dropEntryId, gathering items>
        private Dictionary<uint, List<InstancedGatheringItem>> QuestEnemyDropsTable;
        public InstanceQuestDropManager()
        {
            QuestEnemyDropsTable = new();
            LastDropIdQuery = 0;
        }

        private uint LastDropIdQuery { get; set; }

        private List<InstancedGatheringItem> InstanceAssets(List<GatheringItem> originals)
        {
            return originals.Select(item => new InstancedGatheringItem(item))
                .Where(instancedAsset => instancedAsset.ItemNum > 0)
                .ToList();
        }

        private uint GetDropId(CDataStageLayoutId layoutId, uint setId)
        {
            return layoutId.GroupId + (setId * 2) +
            (layoutId.StageId + layoutId.GroupId + layoutId.LayerNo);
        }

        public bool IsQuestDrop(CDataStageLayoutId layoutId, uint setId)
        {
            uint dropEntryId = GetDropId(layoutId, setId);

            if (QuestEnemyDropsTable.ContainsKey(dropEntryId))
            {
                // Stores the drop Id if we return true. This saves on an unnecessary call within FetchEnemyLoot
                LastDropIdQuery = dropEntryId;
                return true;
            }
            return false;
        }
        public List<InstancedGatheringItem> FetchEnemyLoot(CDataStageLayoutId layoutId, uint setId)
        {
            if (QuestEnemyDropsTable.ContainsKey(LastDropIdQuery))
            {
                return QuestEnemyDropsTable[LastDropIdQuery];

            }
            return new List<InstancedGatheringItem>();
        }

        public List<InstancedGatheringItem> RollEnemyLoot(InstancedEnemy enemy, CDataStageLayoutId layoutId, uint setId)
        {   
            uint dropEntryId = GetDropId(layoutId, setId);

            if (!QuestEnemyDropsTable.ContainsKey(dropEntryId))
            {
                // Check to see if a drop table exists
                if (enemy.DropsTable != null)
                {
                    List<InstancedGatheringItem> items = InstanceAssets(enemy.DropsTable.Items);
                    
                    QuestEnemyDropsTable[dropEntryId] = items;
                    
                    // Return the list of drop items
                    return items;
                }
            }
                // Return an empty list of gathering items for no loot.
                return new List<InstancedGatheringItem>();
        }

        public void Clear()
        {
            LastDropIdQuery = 0;
            QuestEnemyDropsTable.Clear();
        }
    }

}
