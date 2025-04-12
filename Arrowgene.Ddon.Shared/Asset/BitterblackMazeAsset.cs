using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class BitterblackMazeAsset
    {
        public BitterblackMazeAsset()
        {
            Stages = new Dictionary<StageLayoutId, BitterblackMazeConfig>();
            StarterEquipment = new Dictionary<JobId, Dictionary<EquipType, List<Item>>>();
            StarterJobEquipment = new Dictionary<JobId, List<Item>>();
            StarterJobItems = new List<(uint ItemId, uint Amount)>();
            RareItemAppraisalList = new List<CDataCommonU32>();
            ItemTakeawayList = new List<CDataCommonU32>();
            StageProgressionList = new List<CDataBattleContentStageProgression>();
            LootRanges = new Dictionary<uint, LootRange>();

            LowQualityWeapons = new Dictionary<JobId, List<uint>>();
            HighQualityWeapons = new Dictionary<JobId, List<uint>>();
            LowQualityArmors = new Dictionary<BitterblackMazeEquipmentClass, List<uint>>();
            HighQualityArmors = new Dictionary<BitterblackMazeEquipmentClass, List<uint>>();
            LowQualityOther = new List<uint>();
            HighQualityOther = new List<uint>();
            RotundaRare = new List<uint>();
            AbyssRare = new List<uint>();
            ChestTrash = new List<(uint ItemId, uint Amount)>();
        }

        public class LootRange
        {
            public (uint Min, uint Max) NormalRange;
            public (uint Min, uint Max) SealedRange;
            public double RareChance { get; set; }
            public double JewelryChance { get; set; }
            public (uint Gold, uint Silver, uint Red) Marks { get; set; }
        }

        public Dictionary<StageLayoutId, BitterblackMazeConfig> Stages { get; set; }
        public Dictionary<uint, LootRange> LootRanges { get; set; }
        public Dictionary<JobId, Dictionary<EquipType, List<Item>>> StarterEquipment {  get; set; }
        public Dictionary<JobId, List<Item>> StarterJobEquipment { get; set; }
        public List<(uint ItemId, uint Amount)>  StarterJobItems { get; set; }
        public List<CDataCommonU32> RareItemAppraisalList { get; set; }
        public List<CDataCommonU32> ItemTakeawayList {  get; set; }
        public List<CDataBattleContentStageProgression> StageProgressionList {  get; set; }

        public Dictionary<JobId, List<uint>> LowQualityWeapons {  get; set; }
        public Dictionary<JobId, List<uint>> HighQualityWeapons { get; set; }
        public Dictionary<BitterblackMazeEquipmentClass, List<uint>> LowQualityArmors { get; set; }
        public Dictionary<BitterblackMazeEquipmentClass, List<uint>> HighQualityArmors { get; set; }
        public List<uint> LowQualityOther {  get; set; }
        public List<uint> HighQualityOther {  get; set; }

        public List<uint> RotundaRare { get; set; }
        public List<uint> AbyssRare { get; set; }
        public List<(uint ItemId, uint Amount)> ChestTrash { get; set; }

        public Dictionary<JobId, Dictionary<EquipType, List<Item>>> GenerateStarterEquipment()
        {
            var result = new Dictionary<JobId, Dictionary<EquipType, List<Item>>>();

            foreach (var (jobId, equipMap) in StarterEquipment)
            {
                result[jobId] = new Dictionary<EquipType, List<Item>>();
                foreach (var (equipType, items) in equipMap)
                {
                    result[jobId][equipType] = new List<Item>();
                    foreach (var item in items)
                    {
                        result[jobId][equipType].Add((item == null) ? null : new Item(item));
                    }
                }
            }

            return result;
        }

        public Dictionary<JobId, List<Item>> GenerateStarterJobEquipment()
        {
            var result = new Dictionary<JobId, List<Item>>();

            foreach (var (jobId, items) in StarterJobEquipment)
            {
                result[jobId] = new List<Item>();
                foreach (var item in items)
                {
                    result[jobId].Add((item == null) ? null : new Item(item));
                }
            }

            return result;
        }

        //Hunter arrows and Alchemist elixirs
        public List<(uint ItemId, uint Amount)> GenerateStarterJobItems() 
        {
            var result = new List<(uint ItemId, uint Amount)>();
            foreach(var (itemId, quantity) in StarterJobItems) 
            {
                result.Add((itemId, quantity));
            }

            return result;
        }
    }
}
