using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class BonusDungeonInfo
    {
        public BonusDungeonInfo()
        {
            EventName = string.Empty;
            EntryCostList = new List<CDataStageDungeonItem>();
        }

        public uint DungeonId { get; set; }
        public string EventName { get; set; }
        public uint StageId { get; set; }
        public uint StartingPos { get; set; }
        public bool SyncMonsterLevel { get; set; }

        public List<CDataStageDungeonItem> EntryCostList { get; set; }
    }

    public class BonusDungeonCategory
    {
        public BonusDungeonCategory()
        {
            DungeonInformation = new Dictionary<uint, BonusDungeonInfo>();
        }

        public uint CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Dictionary<uint, BonusDungeonInfo> DungeonInformation { get; set; }
    }

    public class BonusDungeonAsset
    {
        public BonusDungeonAsset()
        {
            DungeonCategories = new Dictionary<uint, BonusDungeonCategory>();
            DungeonInfo = new Dictionary<uint, BonusDungeonInfo>();
        }

        public Dictionary<uint, BonusDungeonCategory> DungeonCategories { get; set; }
        public Dictionary<uint, BonusDungeonInfo> DungeonInfo { get; set; }
    }
}
