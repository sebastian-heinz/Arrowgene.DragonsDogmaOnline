using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class EventItem
    {
        public EventItem()
        {
            QuestIds = new HashSet<uint>();
            StageIds = new HashSet<uint>();
            EnemyIds = new HashSet<uint>();
            RequiredItemsEquipped = new HashSet<uint>();
            ItemConstraint = EventItemConstraint.None;
        }

        public uint ItemId { get; set; }
        public uint MinAmount { get; set; }
        public uint MaxAmount { get; set; }
        public double Chance { get; set; }

        public HashSet<uint> QuestIds { get; set; }
        public HashSet<uint> StageIds { get; set; }
        public HashSet<uint> EnemyIds { get; set; }

        public EventItemConstraint EmClassConstraint; // Boss or Regular Enemy

        public EventItemConstraint EmLvConstraint;
        public (uint Lv, uint MinLv, uint MaxLv) EmLvConstraintParams { get; set; }

        public bool RequiresLanternLit { get; set; }

        public EventItemConstraint ItemConstraint { get; set; }
        public HashSet<uint> RequiredItemsEquipped { get; set; }
    }

    public class EventDropsAsset
    {
        public EventDropsAsset()
        {
            EventItems = new List<EventItem>();
        }

        public List<EventItem> EventItems { get; set; }
    }
}
