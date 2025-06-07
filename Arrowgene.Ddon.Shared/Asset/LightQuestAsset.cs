using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class LightQuestAsset
    {
        public Dictionary<QuestAreaId, HashSet<uint>> LestaniaEnemyNodes { get; set; } = [];
        public Dictionary<QuestAreaId, HashSet<uint>> LestaniaGatheringNodes { get; set; } = [];
        public List<LightQuestGeneratingAsset> GeneratingAssets { get; set; } = [];
    }

    public class LightQuestGeneratingAsset
    {
        public string Name { get; set; } = string.Empty;
        public QuestBoardBaseId BoardId { get; set; }
        public LightQuestType Type { get; set; }

        public bool AllowNormalQuests { get; set; } = true;
        public bool AllowAreaOrders { get; set; } = false;

        /// <summary>
        /// If true and BiasRerolls > 0, takes the lowest of [BiasReroll+1] quests for each slot.
        /// If false and BiasRerolls > 0, takes the highest of [BiasReroll+1] quests for each slot.
        /// </summary>
        public bool BiasLower { get; set; } = true;
        public uint BiasRerolls { get; set; } = 0;

        public int MinQuests { get; set; } = 3;
        public int MaxQuests { get; set; } = 3;
        public int MinCount { get; set; } = 2;
        public int MaxCount { get; set; } = 4;
        public uint MinLevel { get; set; } = 0;
        public uint MaxLevel { get; set; } = 90;

    }
}
