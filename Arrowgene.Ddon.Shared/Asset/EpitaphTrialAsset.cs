using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class EpitaphItemReward
    {
        public EpitaphItemReward()
        {
            Items = new List<(uint ItemId, uint Amount, SoulOrdealRewardType Type, bool IsHidden, double Chance)>();
        }

        public SoulOrdealRewardType Type;
        public uint Rolls { get; set; }
        public List<(uint ItemId, uint Amount, SoulOrdealRewardType Type, bool IsHidden, double Chance)> Items { get; set; }

        public List<CDataGatheringItemElement> AsCDataGatheringItemElement()
        {
            var results = new List<CDataGatheringItemElement>();
            for (uint i = 0; i < Rolls; i++)
            {
                var item = Items[Random.Shared.Next(Items.Count)];
                if (item.Chance >= Random.Shared.NextDouble())
                {
                    results.Add(new CDataGatheringItemElement()
                    {
                        ItemId = item.ItemId,
                        ItemNum = (item.Type == SoulOrdealRewardType.Fixed) ? item.Amount : (uint)Random.Shared.Next(1, (int)(item.Amount + 1)),
                        IsHidden = item.IsHidden
                    });
                }
            }
            return results;
        }

        public List<CDataSeasonDungeonRewardItemViewEntry> AsCDataSeasonDungeonRewardItemViewEntry()
        {
            var results = new List<CDataSeasonDungeonRewardItemViewEntry>();

            foreach (var item in Items)
            {
                results.Add(new CDataSeasonDungeonRewardItemViewEntry()
                {
                    ItemId = item.ItemId,
                    MinAmount = (item.Type == SoulOrdealRewardType.Fixed && item.Chance == 1) ? item.Amount : 0,
                    MaxAmount = item.Amount
                });
            }

            return results;
        }
    }

    public class EpitaphObjective
    {
        public EpitaphObjective()
        {
        }

        public EpitaphObjective(EpitaphObjective that)
        {
            this.Type = that.Type;
            this.Priority = that.Priority;
            this.Param1 = that.Param1;
            this.Param2 = that.Param2;
        }

        public SoulOrdealObjective Type;
        public SoulOrdealObjectivePriority Priority;
        public uint Param1;
        public uint Param2;

        // TODO: Come up with a way to internationalize these strings
        public string Label()
        {
            uint counter = 0;

            string label;
            switch (Type)
            {
                case SoulOrdealObjective.DefeatEnemyCount:
                    label = $"Defeat {Param1} or more enemies {Param2}/{Param1}";
                    break;
                case SoulOrdealObjective.CannotDieMoreThanOnce:
                    counter = (Param2 == 1) ? 0U : 1U;
                    label = $"Cannot die more than once {counter}/1";
                    break;
                case SoulOrdealObjective.CannotBeAffectedByAbnormalStatus:
                    label = $"Cannot be affected by abnormal status more than {Param1} times {Param2}/{Param1}";
                    break;
                case SoulOrdealObjective.InflictAbnormalStatusCount:
                    label = $"Inflict abnormal status effects {Param1} or more times {Param2}/{Param1}";
                    break;
                case SoulOrdealObjective.ItemNoteUsedMoreThanOnce:
                    counter = (Param2 == 1) ? 0U : 1U;
                    label = $"Items must not be used more than once {counter}/1";
                    break;
                case SoulOrdealObjective.EliminateTheEnemy:
                    label = "Eliminate the enemy";
                    break;
                case SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount:
                    label = $"Defeat at least {Param1} enemies with an abnormal status on them {Param2}/{Param1}";
                    break;
                case SoulOrdealObjective.CompleteConditionsWithinTimeLimit:
                    label = "Complete the conditions within the time limit";
                    break;
                default:
                    label = $"TODO: Implement label for objective: {Type}";
                    break;
            }

            return label;
        }

        public bool IsObjectiveMet()
        {
            switch (Type)
            {
                case SoulOrdealObjective.DefeatEnemyCount:
                case SoulOrdealObjective.CannotBeAffectedByAbnormalStatus:
                case SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount:
                case SoulOrdealObjective.InflictAbnormalStatusCount:
                    return Param2 >= Param1;
                case SoulOrdealObjective.ItemNoteUsedMoreThanOnce:
                    return Param2 >= 1;
                default:
                    // Logger.Error($"Attempting to check if objective type '{Type}' is met but there is no handler.");
                    return false;
            }
        }

        public CDataSoulOrdealObjective AsCDataSoulOrdealObjective()
        {
            return new CDataSoulOrdealObjective()
            {
                Type = Type,
                Priority = Priority,
                Param1 = Param1,
                Param2 = Param2,
                ObjectiveLabel = Label()
            };
        }
    }

    public class EpitaphTrialOption : EpitaphObject
    {
        public EpitaphTrialOption()
        {
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
            EnemyGroupsByStageId = new Dictionary<(StageLayoutId StageId, byte SubGroupId), QuestEnemyGroup>();
            EntryCost = new List<CDataSoulOrdealItem>();
            Objectives = new Dictionary<SoulOrdealObjective, EpitaphObjective>();
            ItemRewards = new List<EpitaphItemReward>();
            TrialName = string.Empty;
            Parent = new EpitaphTrial();
        }

        public string TrialName { get; set; }
        public Dictionary<uint, QuestEnemyGroup> EnemyGroups { get; set; }
        public Dictionary<(StageLayoutId StageId, byte SubGroupId), QuestEnemyGroup> EnemyGroupsByStageId { get; set; }
        public List<CDataSoulOrdealItem> EntryCost { get; set; }
        public Dictionary<SoulOrdealObjective, EpitaphObjective> Objectives {get; set;}
        public StageLayoutId StageId { get; set; }
        public uint PosId { get; set; }
        public List<EpitaphItemReward> ItemRewards { get; set; }
        public EpitaphTrial Parent { get; set; }

        public Dictionary<SoulOrdealObjective, EpitaphObjective> CreateNewObjectiveState()
        {
            var results = new Dictionary<SoulOrdealObjective, EpitaphObjective>();
            foreach (var constraint in Objectives.Values)
            {
                results.Add(constraint.Type, new EpitaphObjective(constraint));
            }
            return results;
        }

        public CDataSoulOrdealElementParam AsCDataSoulOrdealElementParam()
        {
            return new CDataSoulOrdealElementParam()
            {
                TrialId = EpitaphId,
                TrialName = TrialName,
                TrialCost = EntryCost
            };
        }
    }

    public class EpitaphTrial : EpitaphObject
    {
        public EpitaphTrial()
        {
            Options = new List<EpitaphTrialOption>();
            UnlockCost = new List<CDataSoulOrdealItem>();
            Unlocks = new List<(StageLayoutId StageId, uint SubGroupId)>();
        }

        public StageLayoutId OmLayoutId { get; set; }
        public uint PosId { get; set; }
        public List<EpitaphTrialOption> Options { get; set; }
        public List<CDataSoulOrdealItem> UnlockCost { get; set; }
        public List<(StageLayoutId StageId, uint PosId)> Unlocks { get; set; }

        public List<CDataSoulOrdealElementParam> SoulOrdealOptions()
        {
            List<CDataSoulOrdealElementParam> results = new List<CDataSoulOrdealElementParam>();

            foreach (var option in Options)
            {
                results.Add(option.AsCDataSoulOrdealElementParam());
            }

            return results;
        }
    }
    
    public class EpitaphTrialAsset
    {
        public EpitaphTrialAsset()
        {
            Trials = new Dictionary<StageLayoutId, List<EpitaphTrial>>();
            EpitahObjects = new Dictionary<uint, EpitaphObject>();
        }
        public Dictionary<StageLayoutId, List<EpitaphTrial>> Trials { get; set; }
        public Dictionary<uint, EpitaphObject> EpitahObjects { get; set; }
    }
}
