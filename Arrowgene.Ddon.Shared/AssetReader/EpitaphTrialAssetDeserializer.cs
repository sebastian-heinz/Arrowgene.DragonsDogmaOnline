using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class EpitaphTrialAssetDeserializer
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EpitaphTrialAssetDeserializer));

        private AssetCommonDeserializer _CommonEnemyDeserializer;
        private QuestDropItemAsset _QuestDrops;

        public EpitaphTrialAssetDeserializer(AssetCommonDeserializer commonEnemyDeserializer, QuestDropItemAsset questDrops)
        {
            _CommonEnemyDeserializer = commonEnemyDeserializer;
            _QuestDrops = questDrops;
        }

        public bool LoadTrialsFromDirectory(string path, EpitaphTrialAsset trialAssets)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                Logger.Error($"The directory '{path}' does not exist");
                return false;
            }

            Logger.Info($"Reading epitaph road trial files from {path}");
            foreach (var file in info.EnumerateFiles())
            {
                Logger.Info($"{file.FullName}");

                string json = Util.ReadAllText(file.FullName);
                JsonDocument document = JsonDocument.Parse(json);

                var assetData = new EpitaphTrial();
                if (!ParseTrial(assetData, document.RootElement))
                {
                    Logger.Error($"Unable to parse '{file.FullName}'. Skipping.");
                    continue;
                }

                if (!trialAssets.Trials.ContainsKey(assetData.OmLayoutId))
                {
                    trialAssets.Trials[assetData.OmLayoutId] = new List<EpitaphTrial>();
                }

                trialAssets.Trials[assetData.OmLayoutId].Add(assetData);
                trialAssets.EpitahObjects[assetData.EpitaphId] = assetData;

                foreach (var option in assetData.Options)
                {
                    if (trialAssets.EpitahObjects.ContainsKey(option.EpitaphId))
                    {
                        Logger.Error($"Multiple Epitaph trials options have TrialId={option.EpitaphId}");
                    }
                    trialAssets.EpitahObjects[option.EpitaphId] = option;
                }
            }

            return true;
        }

        public bool ParseTrial(EpitaphTrial assetData, JsonElement jTrial)
        {
            assetData.OmLayoutId = AssetCommonDeserializer.ParseStageId(jTrial.GetProperty("stage_id"));
            assetData.PosId = jTrial.GetProperty("pos_id").GetUInt32();
            assetData.EpitaphId = EpitaphId.GenerateTrialId(assetData.OmLayoutId, assetData.PosId, 0);

            foreach (var jItem in jTrial.GetProperty("unlock_cost").EnumerateArray())
            {
                var item = new CDataSoulOrdealItem()
                {
                    ItemId = jItem.GetProperty("item_id").GetUInt32(),
                    Num = jItem.GetProperty("amount").GetUInt16(),
                };
                assetData.UnlockCost.Add(item);
            }

            foreach (var jUnlock in jTrial.GetProperty("unlocks").EnumerateArray())
            {
                var stageId = jUnlock.GetProperty("stage_id").GetUInt32();
                var groupId = jUnlock.GetProperty("group_id").GetUInt32();
                var posId = jUnlock.GetProperty("pos_id").GetUInt32();
                assetData.Unlocks.Add((new StageLayoutId(stageId, 0, groupId), posId));
            }

            uint i = 1;
            foreach (var jOption in jTrial.GetProperty("options").EnumerateArray())
            {
                if (i > EpitaphId.GetIndexWidth())
                {
                    Logger.Error($"Too many trial options defined for epitaph trial {assetData.EpitaphId} ({i} > {EpitaphId.GetIndexWidth()}). Skipping.");
                    return false;
                }

                var trialOption = new EpitaphTrialOption()
                {
                    EpitaphId = EpitaphId.GenerateTrialId(assetData.OmLayoutId, assetData.PosId, i++),
                    TrialName = jOption.GetProperty("trial_name").GetString(),
                    StageId = assetData.OmLayoutId,
                    PosId = assetData.PosId,
                    Parent = assetData
                };

                foreach (var jItem in jOption.GetProperty("cost").EnumerateArray())
                {
                    var item = new CDataSoulOrdealItem()
                    {
                        ItemId = jItem.GetProperty("item_id").GetUInt32(),
                        Num = jItem.GetProperty("amount").GetUInt16(),
                    };
                    trialOption.EntryCost.Add(item);
                }

                if (!_CommonEnemyDeserializer.ParseEnemyGroups(0, _QuestDrops, trialOption.EnemyGroups, jOption))
                {
                    Logger.Error($"Unable to parse enemies for epitah trial {trialOption.TrialName}:{trialOption.EpitaphId}. Skipping.");
                    return false;
                }

                foreach (var jConstraint in jOption.GetProperty("constraints").EnumerateArray())
                {
                    var constraint = ParseConstraint(jConstraint);
                    if (constraint == null)
                    {
                        Logger.Error($"Unable to parse constraint for epitah trial {trialOption.TrialName}:{trialOption.EpitaphId}. Skipping.");
                        return false;
                    }

                    trialOption.Objectives.Add(constraint.Type, constraint);
                }

                foreach (var enemyGroup in trialOption.EnemyGroups.Values)
                {
                    trialOption.EnemyGroupsByStageId[(enemyGroup.StageLayoutId, enemyGroup.SubGroupId)] = enemyGroup;
                }

                foreach (var jItemReward in jOption.GetProperty("item_rewards").EnumerateArray())
                {
                    var reward = EpitaphAssetCommonDeserializer.ParseReward(jItemReward);
                    if (reward == null)
                    {
                        Logger.Error($"Failed to parse reward for epitah trial {trialOption.TrialName}:{trialOption.EpitaphId}. Skipping.");
                    }
                    trialOption.ItemRewards.Add(reward);
                }

                assetData.Options.Add(trialOption);
            }

            return true;
        }

        private EpitaphObjective ParseConstraint(JsonElement jConstraint)
        {
            var result = new EpitaphObjective();

            if (!Enum.TryParse(jConstraint.GetProperty("type").GetString(), true, out result.Type))
            {
                return null;
            }

            if (!Enum.TryParse(jConstraint.GetProperty("priority").GetString(), true, out result.Priority))
            {
                return null;
            }

            if (result.Type == SoulOrdealObjective.CompleteConditionsWithinTimeLimit ||
                result.Type == SoulOrdealObjective.CannotBeAffectedByAbnormalStatus ||
                result.Type == SoulOrdealObjective.DefeatEnemyCount ||
                result.Type == SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount ||
                result.Type == SoulOrdealObjective.InflictAbnormalStatusCount)
            {
                result.Param1 = jConstraint.GetProperty("Param1").GetUInt32();
                if (result.Type == SoulOrdealObjective.CompleteConditionsWithinTimeLimit)
                {
                    result.Param2 = 1;
                }
                else
                {
                    result.Param2 = 0;
                }
            }
            else
            {
                result.Param1 = 1;
                result.Param2 = 1;
            }

            return result;
        }
    }
}

