using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static Arrowgene.Ddon.Shared.Csv.GmdCsv;


namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class QuestAssetDeserializer
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestAssetDeserializer));

        private Dictionary<uint, NamedParam> namedParams;
        private QuestDropItemAsset questDrops;

        public QuestAssetDeserializer(Dictionary<uint, NamedParam> namedParams, QuestDropItemAsset questDrops)
        {
            this.namedParams = namedParams;
            this.questDrops = questDrops;
        }

        public bool LoadQuestsFromDirectory(string path, QuestAsset questAssets)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                Logger.Error($"The directory '{path}' does not exist");
                return false;
            }

            Logger.Info($"Reading quest files from {path}");
            foreach (var file in info.EnumerateFiles())
            {
                Logger.Info($"{file.FullName}");

                string json = File.ReadAllText(file.FullName);
                JsonDocument document = JsonDocument.Parse(json);

                var jQuest = document.RootElement;
                if (!Enum.TryParse(jQuest.GetProperty("state_machine").GetString(), true, out QuestStateMachineType questStateMachineType))
                {
                    Logger.Error($"Expected key 'state_machine' not in the root of the document. Unable to parse {file.FullName}.");
                    continue;
                }

                if (questStateMachineType != QuestStateMachineType.GenericStateMachine)
                {
                    Logger.Error($"Unsupported QuestStateMachineType '{questStateMachineType}'. Unable to parse {file.FullName}.");
                    continue;
                }

                QuestAssetData assetData = new QuestAssetData();
                if (!ParseQuest(assetData, jQuest))
                {
                    Logger.Error($"Unable to parse '{file.FullName}'. Skipping.");
                    continue;
                }

                questAssets.Quests.Add(assetData);
            }

            return true;
        }

        private bool ParseQuest(QuestAssetData assetData, JsonElement jQuest)
        {
            if (!Enum.TryParse(jQuest.GetProperty("type").GetString(), true, out QuestType questType))
            {
                Logger.Error($"Unable to parse the quest type. Skipping.");
                return false;
            }

            assetData.Type = questType;
            assetData.QuestId = (QuestId)jQuest.GetProperty("quest_id").GetUInt32();
            assetData.BaseLevel = jQuest.GetProperty("base_level").GetUInt16();
            assetData.MinimumItemRank = jQuest.GetProperty("minimum_item_rank").GetByte();
            assetData.Discoverable = jQuest.GetProperty("discoverable").GetBoolean();

            assetData.QuestAreaId = QuestAreaId.None;
            if (jQuest.TryGetProperty("area_id", out JsonElement jAreaId)
                && Enum.TryParse(jAreaId.GetString(), true, out QuestAreaId areaId))
            {
                assetData.QuestAreaId = areaId;
            }

            assetData.StageId = StageId.Invalid;
            if (questType == QuestType.Tutorial)
            {
                assetData.StageId = ParseStageId(jQuest.GetProperty("stage_id"));
            }

            assetData.NewsImageId = 0;
            if (jQuest.TryGetProperty("news_image", out JsonElement jNewsImage))
            {
                assetData.NewsImageId = jNewsImage.GetUInt32();
            }

            assetData.NextQuestId = 0;
            if (jQuest.TryGetProperty("next_quest", out JsonElement jNextQuest))
            {
                assetData.NextQuestId = (QuestId)jNextQuest.GetUInt32();
            }

            assetData.QuestScheduleId = (uint) assetData.QuestId;
            if (jQuest.TryGetProperty("quest_schedule_id", out JsonElement jQuestScheduleId))
            {
                assetData.QuestScheduleId = jQuestScheduleId.GetUInt32();
            }

            if (jQuest.TryGetProperty("quest_layout_set_info_flags", out JsonElement jLayoutSetInfoFlags))
            {
                foreach (var layoutFlag in jLayoutSetInfoFlags.EnumerateArray())
                {
                    assetData.QuestLayoutSetInfoFlags.Add(QuestLayoutFlagSetInfo.FromQuestAssetJson(layoutFlag));
                }
            }

            if (jQuest.TryGetProperty("quest_layout_flags", out JsonElement jLayoutFlags))
            {
                foreach (var layoutFlag in jLayoutFlags.EnumerateArray())
                {
                    assetData.QuestLayoutFlags.Add(new QuestLayoutFlag() { FlagId = layoutFlag.GetUInt32() });
                }
            }

            assetData.ResetPlayerAfterQuest = false;
            if (jQuest.TryGetProperty("reset_player_after_quest", out JsonElement jResetPlayerAfterQuest))
            {
                assetData.ResetPlayerAfterQuest = true;
            }

            if (questType == QuestType.ExtremeMission)
            {
                if (!jQuest.TryGetProperty("mission_params", out JsonElement jMissionParams))
                {
                    Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Missing 'mission_params'. Skipping.");
                    return false;
                }

                ParseMissionParams(assetData, jMissionParams);
            }

            assetData.QuestOrderBackgroundImage = 0;
            if (questType == QuestType.WildHunt)
            {
                if (!jQuest.TryGetProperty("order_background_id", out JsonElement jOrderBackgroundId))
                {
                    Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Missing 'order_background_id'. Skipping.");
                    return false;
                }

                assetData.QuestOrderBackgroundImage = jOrderBackgroundId.GetUInt32();
                if (assetData.QuestOrderBackgroundImage == 0)
                {
                    Logger.Error($"The value of 'order_background_id' must be > 0, for the quest '{assetData.QuestId}'. Skipping.");
                    return false;
                }
            }

            assetData.Enabled = true;
            if (jQuest.TryGetProperty("enabled", out JsonElement jQuestEnabled))
            {
                assetData.Enabled = jQuestEnabled.GetBoolean();
            }

            if (questType == QuestType.Light)
            {
                if (!jQuest.TryGetProperty("light_quest_details", out JsonElement jLightQuestDetails))
                {
                    Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Missing 'light_quest_details'. Skipping.");
                    return false;
                }

                ParseLightQuestDetails(assetData, jLightQuestDetails);
            }

            ParseRewards(assetData, jQuest);

            if (!ParseServerActions(assetData, jQuest))
            {
                return false;
            }

            if (!ParseOrderCondition(assetData, jQuest))
            {
                return false;
            }

            if (!ParseEnemyGroups(assetData, jQuest))
            {
                Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Skipping.");
                return false;
            }

            if (jQuest.TryGetProperty("blocks", out JsonElement jBlocksV1))
            {
                QuestProcess questProcess = new QuestProcess()
                {
                    ProcessNo = 0
                };

                if (!ParseBlocks(questProcess, jBlocksV1))
                {
                    Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Skipping.");
                    return false;
                }
                assetData.Processes.Add(questProcess);
            }
            else if (jQuest.TryGetProperty("processes", out JsonElement jProcesses))
            {
                ushort ProcessNo = 0;
                foreach (var jProcess in jProcesses.EnumerateArray())
                {
                    QuestProcess questProcess = new QuestProcess()
                    {
                        ProcessNo = ProcessNo
                    };

                    var jBlocks = jProcess.GetProperty("blocks");
                    if (!ParseBlocks(questProcess, jBlocks))
                    {
                        Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Skipping.");
                        return false;
                    }
                    assetData.Processes.Add(questProcess);
                    ProcessNo += 1;
                }
            }

            return true;
        }

        private bool ParseOrderCondition(QuestAssetData assetData, JsonElement quest)
        {
            if (!quest.TryGetProperty("order_conditions", out JsonElement jOrderConditions))
            {
                return true;
            }

            foreach (var condition in jOrderConditions.EnumerateArray())
            {
                if (!Enum.TryParse(condition.GetProperty("type").GetString(), true, out QuestOrderConditionType orderConditionType))
                {
                    Logger.Error($"Unable to parse order condition type for '{assetData.QuestId}'. Skipping.");
                    return false;
                }

                QuestOrderCondition questOrderCondition = new QuestOrderCondition()
                {
                    Type = orderConditionType
                };

                if (condition.TryGetProperty("Param1", out JsonElement jParam1))
                {
                    questOrderCondition.Param01 = jParam1.GetInt32();
                }

                if (condition.TryGetProperty("Param2", out JsonElement jParam2))
                {
                    questOrderCondition.Param02 = jParam2.GetInt32();
                }

                assetData.OrderConditions.Add(questOrderCondition);
            }

            return true;
        }

        private void ParseRewards(QuestAssetData assetData, JsonElement quest)
        {
            uint randomRewards = 0;
            foreach (var reward in quest.GetProperty("rewards").EnumerateArray())
            {
                var rewardType = reward.GetProperty("type").GetString();
                switch (rewardType)
                {
                    case "fixed":
                    case "random":
                    case "select":
                    case "TimeBased":
                        if (!Enum.TryParse(reward.GetProperty("type").GetString(), true, out QuestRewardType questRewardType))
                        {
                            continue;
                        }

                        QuestRewardItem rewardItem = null;
                        if (questRewardType == QuestRewardType.Random)
                        {
                            if (randomRewards >= 4)
                            {
                                Logger.Error("Client only supports a maximum of 4 random rewards per quest. Skipping.");
                                continue;
                            }

                            // Keep track of random rewards for the quest
                            randomRewards += 1;

                            rewardItem = new QuestRandomRewardItem();
                            foreach (var item in reward.GetProperty("loot_pool").EnumerateArray())
                            {
                                rewardItem.LootPool.Add(new RandomLootPoolItem()
                                {
                                    ItemId = item.GetProperty("item_id").GetUInt32(),
                                    Num = item.GetProperty("num").GetUInt16(),
                                    Chance = item.GetProperty("chance").GetDouble()
                                });
                            }
                        }
                        else if (questRewardType == QuestRewardType.Select)
                        {
                            rewardItem = new QuestSelectRewardItem();
                            foreach (var item in reward.GetProperty("loot_pool").EnumerateArray())
                            {
                                rewardItem.LootPool.Add(new SelectLootPoolItem()
                                {
                                    ItemId = item.GetProperty("item_id").GetUInt32(),
                                    Num = item.GetProperty("num").GetUInt16(),
                                });
                            }
                        }
                        else if (questRewardType == QuestRewardType.Fixed)
                        {
                            rewardItem = new QuestFixedRewardItem();
                            foreach (var item in reward.GetProperty("loot_pool").EnumerateArray())
                            {
                                rewardItem.LootPool.Add(new FixedLootPoolItem()
                                {
                                    ItemId = item.GetProperty("item_id").GetUInt32(),
                                    Num = item.GetProperty("num").GetUInt16(),
                                });
                            };
                        }
                        else
                        {
                            Logger.Error($"The reward type '{rewardType}' is not implemented. Skipping.");
                        }

                        if (rewardItem != null)
                        {
                            assetData.RewardItems.Add(rewardItem);
                        }
                        break;
                    case "exp":
                        assetData.PointRewards.Add(new PointReward()
                        {
                            ExpType = ExpType.ExperiencePoints,
                            ExpReward = reward.GetProperty("amount").GetUInt32()
                        });
                        break;
                    case "pp":
                        assetData.PointRewards.Add(new PointReward()
                        {
                            ExpType = ExpType.PlayPoints,
                            ExpReward = reward.GetProperty("amount").GetUInt32()
                        });
                        break;
                    case "jp":
                        assetData.PointRewards.Add(new PointReward()
                        {
                            ExpType = ExpType.JobPoints,
                            ExpReward = reward.GetProperty("amount").GetUInt32()
                        });
                        break;
                    case "wallet":
                        if (!Enum.TryParse(reward.GetProperty("wallet_type").GetString(), true, out WalletType walletType))
                        {
                            continue;
                        }
                        assetData.RewardCurrency.Add(new QuestRewardCurrency()
                        {
                            WalletType = walletType,
                            Amount = reward.GetProperty("amount").GetUInt32()
                        });
                        break;
                    default:
                        /* NOT IMPLEMENTED */
                        break;
                }
            }
        }

        private bool ParseBlocks(QuestProcess questProcess, JsonElement jBlocks)
        {
            ushort blockIndex = 1;
            foreach (var jblock in jBlocks.EnumerateArray())
            {
                QuestBlock questBlock = new QuestBlock();

                if (!Enum.TryParse(jblock.GetProperty("type").GetString(), true, out QuestBlockType questBlockType))
                {
                    Logger.Error($"Unable to parse the quest block type @ index {blockIndex - 1}.");
                    return false;
                }

                questBlock.ProcessNo = questProcess.ProcessNo;
                questBlock.BlockType = questBlockType;
                questBlock.BlockNo = blockIndex;
                questBlock.AnnounceType = QuestAnnounceType.None;

                questBlock.ShouldStageJump = false;
                if (jblock.TryGetProperty("stage_jump", out JsonElement jStageJump))
                {
                    questBlock.ShouldStageJump = jStageJump.GetBoolean();
                }

                if (jblock.TryGetProperty("announce_type", out JsonElement jUpdateAnnounce))
                {
                    if (!Enum.TryParse(jUpdateAnnounce.GetString(), true, out QuestAnnounceType announceType))
                    {
                        Logger.Error($"Unable to parse the quest announce type @ index {blockIndex - 1}.");
                        return false;
                    }
                    questBlock.AnnounceType = announceType;
                }

                ParseAnnoucementSubtypes(questBlock, jblock);

                questBlock.IsCheckpoint = false;
                if (jblock.TryGetProperty("checkpoint", out JsonElement jCheckpoint))
                {
                    questBlock.IsCheckpoint = jCheckpoint.GetBoolean();
                }

                if (jblock.TryGetProperty("stage_id", out JsonElement jStageId))
                {
                    questBlock.StageId = ParseStageId(jStageId);
                }

                questBlock.SubGroupId = 0;
                if (jblock.TryGetProperty("subgroup_id", out JsonElement jSubGroupId))
                {
                    questBlock.SubGroupId = jSubGroupId.GetUInt16();
                }

                if (jblock.TryGetProperty("hand_items", out JsonElement jHandItems))
                {
                    foreach (var item in jHandItems.EnumerateArray())
                    {
                        questBlock.HandPlayerItems.Add(new QuestItem()
                        {
                            ItemId = item.GetProperty("id").GetUInt32(),
                            Amount = item.GetProperty("amount").GetUInt32()
                        });
                    }
                }

                if (jblock.TryGetProperty("consume_items", out JsonElement jConsumeItems))
                {
                    foreach (var item in jConsumeItems.EnumerateArray())
                    {
                        questBlock.ConsumePlayerItems.Add(new QuestItem()
                        {
                            ItemId = item.GetProperty("id").GetUInt32(),
                            Amount = item.GetProperty("amount").GetUInt32()
                        });
                    }
                }

                questBlock.BgmStop = false;
                if (jblock.TryGetProperty("bgm_stop", out JsonElement jBgmStop))
                {
                    questBlock.BgmStop = jBgmStop.GetBoolean();
                }

                if (jblock.TryGetProperty("flags", out JsonElement jFlags))
                {
                    // {"type": "MyQst", "operation": "Set", "value": 4}
                    foreach (var jFlag in jFlags.EnumerateArray())
                    {
                        var questFlag = ParseQuestFlag(jFlag);
                        if (questFlag == null)
                        {
                            Logger.Error($"Unable to parse the quest flags @ index {blockIndex - 1}.");
                            return false;
                        }
                        questBlock.QuestFlags.Add(questFlag);
                    }
                }

                if (jblock.TryGetProperty("checkpoint_flags", out JsonElement jCheckpointFlags))
                {
                    // {"type": "MyQst", "operation": "Set", "value": 4}
                    foreach (var jFlag in jCheckpointFlags.EnumerateArray())
                    {
                        var questFlag = ParseQuestFlag(jFlag);
                        if (questFlag == null)
                        {
                            Logger.Error($"Unable to parse the checkpoint quest flags @ index {blockIndex - 1}.");
                            return false;
                        }
                        questBlock.CheckpointQuestFlags.Add(questFlag);
                    }
                }

                questBlock.QuestCameraEvent.HasCameraEvent = false;
                if (jblock.TryGetProperty("camera_event", out JsonElement jCameraEvent))
                {
                    questBlock.QuestCameraEvent.HasCameraEvent = true;
                    questBlock.QuestCameraEvent.EventNo = jCameraEvent.GetProperty("event_no").GetInt32();
                }

                switch (questBlockType)
                {
                    case QuestBlockType.IsStageNo:
                        {
                            questBlock.ShowMarker = true;
                            if (jblock.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }
                        }
                        break;
                    case QuestBlockType.NpcTalkAndOrder:
                    case QuestBlockType.NewNpcTalkAndOrder:
                        {
                            if (!Enum.TryParse(jblock.GetProperty("npc_id").GetString(), true, out NpcId npcId))
                            {
                                Logger.Error($"Unable to parse the npc_id in block @ index {blockIndex - 1}.");
                                return false;
                            }
                            questBlock.NpcOrderDetails.Add(new QuestNpcOrder()
                            {
                                NpcId = npcId,
                                MsgId = jblock.GetProperty("message_id").GetInt32(),
                                StageId = questBlock.StageId
                            });
                        }
                        break;
                    case QuestBlockType.PartyGather:
                        {
                            var jLocation = jblock.GetProperty("location");
                            questBlock.PartyGatherPoint.x = jLocation.GetProperty("x").GetInt32();
                            questBlock.PartyGatherPoint.y = jLocation.GetProperty("y").GetInt32();
                            questBlock.PartyGatherPoint.z = jLocation.GetProperty("z").GetInt32();
                        }
                        break;
                    case QuestBlockType.IsGatherPartyInStage:
                        break;
                    case QuestBlockType.DiscoverEnemy:
                    case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                    case QuestBlockType.KillGroup:
                    case QuestBlockType.SpawnGroup:
                    case QuestBlockType.WeakenGroup:
                    case QuestBlockType.DestroyGroup:
                        {
                            questBlock.ResetGroup = true;
                            if (jblock.TryGetProperty("reset_group", out JsonElement jResetGroup))
                            {
                                questBlock.ResetGroup = jResetGroup.GetBoolean();
                            }

                            if (jblock.TryGetProperty("percent", out JsonElement jPercent))
                            {
                                questBlock.EnemyHpPrecent = jPercent.GetInt32();
                            }

                            foreach (var groupId in jblock.GetProperty("groups").EnumerateArray())
                            {
                                questBlock.EnemyGroupIds.Add(groupId.GetUInt32());
                            }
                        }
                        break;
                    case QuestBlockType.TalkToNpc:
                        {
                            if (!Enum.TryParse(jblock.GetProperty("npc_id").GetString(), true, out NpcId npcId))
                            {
                                Logger.Error($"Unable to parse the npc_id in block @ index {blockIndex - 1}.");
                                return false;
                            }

                            questBlock.ShowMarker = true;
                            if (jblock.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }

                            questBlock.NpcOrderDetails.Add(new QuestNpcOrder()
                            {
                                NpcId = npcId,
                                MsgId = jblock.GetProperty("message_id").GetInt32(),
                                StageId = ParseStageId(jblock.GetProperty("stage_id"))
                            });
                        }
                        break;
                    case QuestBlockType.NewTalkToNpc:
                        {
                            if (!Enum.TryParse(jblock.GetProperty("npc_id").GetString(), true, out NpcId npcId))
                            {
                                Logger.Error($"Unable to parse the npc_id in block @ index {blockIndex - 1}.");
                                return false;
                            }

                            questBlock.ShowMarker = true;
                            if (jblock.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }

                            questBlock.NpcOrderDetails.Add(new QuestNpcOrder()
                            {
                                NpcId = npcId,
                                MsgId = jblock.GetProperty("message_id").GetInt32(),
                                StageId = ParseStageId(jblock.GetProperty("stage_id"))
                            });
                        }
                        break;
                    case QuestBlockType.IsQuestOrdered:
                        {
                            if (!Enum.TryParse(jblock.GetProperty("quest_type").GetString(), true, out QuestType questType))
                            {
                                Logger.Error($"Unable to parse the quest type in block @ index {blockIndex - 1}.");
                                return false;
                            }

                            questBlock.QuestOrderDetails.QuestType = questType;

                            questBlock.QuestOrderDetails.QuestId = QuestId.None;
                            if (jblock.TryGetProperty("quest_id", out JsonElement jOrderQuestId))
                            {
                                questBlock.QuestOrderDetails.QuestId = (QuestId)jOrderQuestId.GetUInt32();
                            }
                        }
                        break;
                    case QuestBlockType.MyQstFlags:
                        {
                            if (jblock.TryGetProperty("set_flags", out JsonElement jSetFlags))
                            {
                                foreach (var jMyQstFlag in jSetFlags.EnumerateArray())
                                {
                                    questBlock.MyQstSetFlags.Add(jMyQstFlag.GetUInt32());
                                }
                            }

                            if (jblock.TryGetProperty("check_flags", out JsonElement jCheckFlags))
                            {
                                foreach (var jMyQstFlag in jCheckFlags.EnumerateArray())
                                {
                                    questBlock.MyQstCheckFlags.Add(jMyQstFlag.GetUInt32());
                                }
                            }
                        }
                        break;
                    case QuestBlockType.CollectItem:
                        {
                            questBlock.ShowMarker = true;
                            if (jblock.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }
                        }
                        break;
                    case QuestBlockType.OmInteractEvent:
                        {
                            if (!Enum.TryParse(jblock.GetProperty("quest_type").GetString(), true, out OmQuestType questType))
                            {
                                Logger.Error($"Unable to parse the quest type in block @ index {blockIndex - 1}.");
                                return false;
                            }
                            questBlock.OmInteractEvent.QuestType = questType;

                            if (!Enum.TryParse(jblock.GetProperty("interact_type").GetString(), true, out OmInteractType interactType))
                            {
                                Logger.Error($"Unable to parse the quest type in block @ index {blockIndex - 1}.");
                                return false;
                            }
                            questBlock.OmInteractEvent.InteractType = interactType;

                            questBlock.ShowMarker = true;
                            if (jblock.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }

                            if (jblock.TryGetProperty("quest_id", out JsonElement jQuestId))
                            {
                                questBlock.OmInteractEvent.QuestId = (QuestId)jQuestId.GetUInt32();
                            }
                        }
                        break;
                    case QuestBlockType.DeliverItems:
                    case QuestBlockType.NewDeliverItems:
                    {
                            if (!Enum.TryParse(jblock.GetProperty("npc_id").GetString(), true, out NpcId npcId))
                            {
                                Logger.Error($"Unable to parse the npc_id in block @ index {blockIndex - 1}.");
                                return false;
                            }

                            questBlock.NpcOrderDetails.Add(new QuestNpcOrder()
                            {
                                NpcId = npcId,
                                MsgId = jblock.GetProperty("message_id").GetInt32(),
                                StageId = ParseStageId(jblock.GetProperty("stage_id"))
                            });

                            foreach (var item in jblock.GetProperty("items").EnumerateArray())
                            {
                                questBlock.DeliveryRequests.Add(new QuestItem()
                                {
                                    ItemId = item.GetProperty("id").GetUInt32(),
                                    Amount = item.GetProperty("amount").GetUInt32()
                                });
                            }
                        }
                        break;
                    case QuestBlockType.ExtendTime:
                        {
                            questBlock.TimeAmount = jblock.GetProperty("amount").GetUInt32();
                        }
                        break;
                    case QuestBlockType.PlayEvent:
                        {
                            questBlock.QuestEvent.EventId = jblock.GetProperty("event_id").GetInt32();

                            questBlock.QuestEvent.JumpType = QuestJumpType.After;
                            if (jblock.TryGetProperty("jump_type", out JsonElement jJumpType))
                            {
                                if (!Enum.TryParse(jJumpType.GetString(), true, out QuestJumpType jumpType))
                                {
                                    Logger.Error($"Unable to parse the event jump type in block @ index {blockIndex - 1}.");
                                    return false;
                                }
                                questBlock.QuestEvent.JumpType = jumpType;
                            }

                            if (jblock.TryGetProperty("jump_stage_id", out JsonElement jStageJumpId))
                            {
                                questBlock.QuestEvent.JumpStageId = ParseStageId(jStageJumpId);
                            }

                            if (jblock.TryGetProperty("start_pos_no", out JsonElement jStartPosNo))
                            {
                                questBlock.QuestEvent.StartPosNo = jStartPosNo.GetInt32();
                            }
                        }
                        break;
                    case QuestBlockType.KillTargetEnemies:
                        {
                            // The KillTargetEnemies/EmDieLight machinery expects to be given enemy name IDs, not raw enemy IDs.
                            // Quest writers can provide either; if they give a hex number it will be converted to the proper enemy name ID automatically. 
                            var enemyIdString = jblock.GetProperty("enemy_id").GetString();
                            if (enemyIdString.Contains('x'))
                            {
                                var enemyId = Convert.ToUInt32(enemyIdString, 16);
                                questBlock.TargetEnemy.EnemyId = Enemy.NameMap[enemyId];
                            }
                            else
                            {
                                questBlock.TargetEnemy.EnemyId = Convert.ToUInt32(enemyIdString);
                            }

                            questBlock.TargetEnemy.Level = jblock.GetProperty("level").GetUInt32();
                            questBlock.TargetEnemy.Amount = jblock.GetProperty("amount").GetUInt32();
                        }
                        break;
                    case QuestBlockType.ReturnCheckpoint:
                        {
                            questBlock.CheckpointDetails.ProcessNo = jblock.GetProperty("process_no").GetUInt16();
                            questBlock.CheckpointDetails.BlockNo = jblock.GetProperty("block_no").GetUInt16();
                        }
                        break;
                    case QuestBlockType.DeliverItemsLight:
                        {
                            foreach (var item in jblock.GetProperty("items").EnumerateArray())
                            {
                                questBlock.DeliveryRequests.Add(new QuestItem()
                                {
                                    ItemId = item.GetProperty("id").GetUInt32(),
                                    Amount = item.GetProperty("amount").GetUInt32()
                                });
                            }
                        }
                        break;
                    case QuestBlockType.Raw:
                        break;
                    case QuestBlockType.DummyBlock:
                        /* Filler block which might do some meta things like announce or set/check flags */
                        break;
                    default:
                        Logger.Error($"Unsupported QuestBlockType {questBlockType} @ index {blockIndex - 1}.");
                        return false;
                }

                if (!ParseRawBlock(jblock, questBlock))
                {
                    Logger.Error($"Unable to parse RawBlock commands in block @ index {blockIndex - 1}.");
                    return false;
                }

                questProcess.Blocks.Add(questBlock);

                blockIndex += 1;
            }

            if (questProcess.ProcessNo == 0)
            {
                // Add an implicit EndBlock
                questProcess.Blocks.Add(new QuestBlock()
                {
                    BlockType = QuestBlockType.End,
                    ProcessNo = questProcess.ProcessNo,
                    BlockNo = blockIndex,
                    SequenceNo = 1,
                    AnnounceType = QuestAnnounceType.None
                });
            }
            else
            {
                // Add a block which does nothing
                questProcess.Blocks.Add(new QuestBlock()
                {
                    ProcessNo = questProcess.ProcessNo,
                    BlockType = QuestBlockType.None,
                    BlockNo = blockIndex,
                    SequenceNo = 1,
                    AnnounceType = QuestAnnounceType.None
                });
            }

            return true;
        }

        private StageId ParseStageId(JsonElement jStageId)
        {
            uint id = jStageId.GetProperty("id").GetUInt32();

            byte layerNo = 0;
            if (jStageId.TryGetProperty("layer_no", out JsonElement jLayerNo))
            {
                layerNo = jLayerNo.GetByte();
            }

            uint groupId = 0;
            if (jStageId.TryGetProperty("group_id", out JsonElement jGroupId))
            {
                groupId = jGroupId.GetUInt32();
            }

            return new StageId(id, layerNo, groupId);
        }

        private void ParseAnnoucementSubtypes(QuestBlock questBlock, JsonElement jBlock)
        {
            var announcements = questBlock.Announcements;

            announcements.GeneralAnnounceId = 0;
            if (jBlock.TryGetProperty("general_announce", out JsonElement jGeneralAnnounce))
            {
                announcements.GeneralAnnounceId = jGeneralAnnounce.GetInt32();
            }

            announcements.StageStart = 0;
            if (jBlock.TryGetProperty("stage_start", out JsonElement jStageStart))
            {
                announcements.StageStart = jStageStart.GetInt32();
            }

            announcements.StageClear = 0;
            if (jBlock.TryGetProperty("stage_clear", out JsonElement jStageClear))
            {
                announcements.StageClear = jStageClear.GetInt32();
            }

            announcements.EndContentsPurpose = 0;
            if (jBlock.TryGetProperty("end_contents_purpose", out JsonElement jEndContentsPurpose))
            {
                announcements.EndContentsPurpose = jEndContentsPurpose.GetInt32();
            }
        }

        private QuestFlag ParseQuestFlag(JsonElement jFlag)
        {
            QuestFlag questFlag = new QuestFlag();

            if (!Enum.TryParse(jFlag.GetProperty("type").GetString(), true, out QuestFlagType type))
            {
                Logger.Error($"Invalid QuestFlagType");
                return null;
            }

            if (!Enum.TryParse(jFlag.GetProperty("action").GetString(), true, out QuestFlagAction action))
            {
                Logger.Error($"Invalid QuestFlagAction");
                return null;
            }

            if (jFlag.TryGetProperty("quest_id", out JsonElement jQuestId))
            {
                questFlag.QuestId = jQuestId.GetInt32();
            }

            questFlag.Type = type;
            questFlag.Action = action;
            questFlag.Value = jFlag.GetProperty("value").GetInt32();

            return questFlag;
        }

        private bool ParseMissionParams(QuestAssetData assetData, JsonElement jMissionParams)
        {

            if (!jMissionParams.TryGetProperty("group", out JsonElement jGroup))
            {
                Logger.Error($"Missing required member 'group' from ExtremeMission config.");
                return false;
            }
            assetData.MissionParams.Group = jGroup.GetUInt32();

            if (!jMissionParams.TryGetProperty("phase_groups", out JsonElement jPhaseGroups))
            {
                Logger.Error($"Missing required member 'phase_groups' from ExtremeMission config.");
                return false;
            }

            foreach (var element in jPhaseGroups.EnumerateArray())
            {
                assetData.MissionParams.QuestPhaseGroupIdList.Add(new CDataCommonU32() { Value = element.GetUInt32() });
            }

            assetData.MissionParams.StartPos = 0;
            if (jMissionParams.TryGetProperty("start_pos", out JsonElement jStartPos))
            {
                assetData.MissionParams.StartPos = jStartPos.GetByte();
            }

            assetData.MissionParams.MinimumMembers = 4;
            if (jMissionParams.TryGetProperty("minimum_members", out JsonElement jMinimumMembers))
            {
                assetData.MissionParams.MinimumMembers = jMinimumMembers.GetUInt32();
            }

            assetData.MissionParams.MaximumMembers = 4;
            if (jMissionParams.TryGetProperty("maximum_members", out JsonElement jMaximumMembers))
            {
                assetData.MissionParams.MaximumMembers = jMaximumMembers.GetUInt32();
            }

            assetData.MissionParams.LootDistribution = QuestLootDistribution.Normal;
            if (jMissionParams.TryGetProperty("loot_distribution", out JsonElement jLootDistribution))
            {
                if (!Enum.TryParse(jLootDistribution.GetString(), true, out QuestLootDistribution lootDistribution))
                {
                    Logger.Error("Invalid 'loot_distribution' from ExtremeMission config.");
                    return false;
                }
                assetData.MissionParams.LootDistribution = lootDistribution;
            }

            assetData.MissionParams.PlaytimeInSeconds = 1200;
            if (jMissionParams.TryGetProperty("playtime", out JsonElement jPlaytime))
            {
                assetData.MissionParams.PlaytimeInSeconds = jPlaytime.GetUInt32();
            }

            assetData.MissionParams.IsSolo = false;
            if (jMissionParams.TryGetProperty("solo_only", out JsonElement jIsSoloOnly))
            {
                assetData.MissionParams.IsSolo = jIsSoloOnly.GetBoolean();
            }

            assetData.MissionParams.MaxPawns = 3;
            if (jMissionParams.TryGetProperty("max_pawns", out JsonElement jMaxPawns))
            {
                assetData.MissionParams.MaxPawns = jMaxPawns.GetUInt32();
            }

            assetData.MissionParams.ArmorAllowed = true;
            assetData.MissionParams.JewelryAllowed = true;
            if (jMissionParams.TryGetProperty("restrictions", out JsonElement jRestrictions))
            {
                if (jMissionParams.TryGetProperty("armor", out JsonElement jArmorAllowed))
                {
                    assetData.MissionParams.ArmorAllowed = jArmorAllowed.GetBoolean();
                }

                if (jMissionParams.TryGetProperty("jewelry", out JsonElement jJewelryAllowed))
                {
                    assetData.MissionParams.JewelryAllowed = jJewelryAllowed.GetBoolean();
                }
            }

            return true;
        }

        private bool ParseServerActions(QuestAssetData assetData, JsonElement quest)
        {
            if (!quest.TryGetProperty("server_actions", out JsonElement jServerActions))
            {
                // It is optional to provide this list
                return true;
            }

            foreach (var jServerAction in jServerActions.EnumerateArray())
            {
                var action = new QuestServerAction();

                if (!jServerAction.TryGetProperty("action_type", out JsonElement jActionType))
                {
                    Logger.Error("Unable to find the server action type. Exiting.");
                    return false;
                }

                if (!Enum.TryParse(jActionType.GetString(), true, out QuestSeverActionType actionType))
                {
                    Logger.Error("Unable to decode the server action type. Exiting.");
                    return false;
                }

                action.ActionType = actionType;
                if (actionType == QuestSeverActionType.OmSetInstantValue)
                {
                    if (!jServerAction.TryGetProperty("instant_value_action", out JsonElement jInstantValueAction))
                    {
                        Logger.Error("Failed to locate the instant_value_action field. Exiting.");
                        return false;
                    }

                    if (!Enum.TryParse(jInstantValueAction.ToString(), true, out OmInstantValueAction instantValueAction))
                    {
                        Logger.Error("Failed to decode the instant_value_action field. Exiting.");
                    }
                    action.OmInstantValueAction = instantValueAction;
                    action.Key = jServerAction.GetProperty("key").GetUInt64();
                    action.Value = jServerAction.GetProperty("value").GetUInt32();
                    action.StageId = ParseStageId(jServerAction.GetProperty("stage_id"));
                }

                assetData.ServerActions.Add(action);
            }

            return true;
        }

        private bool ParseEnemyGroups(QuestAssetData assetData, JsonElement quest)
        {
            if (!quest.TryGetProperty("enemy_groups", out JsonElement jGroups))
            {
                // No Enemy groups to parse
                return true;
            }

            uint groupId = 0;
            foreach (var jGroup in jGroups.EnumerateArray())
            {
                QuestEnemyGroup enemyGroup = new QuestEnemyGroup();

                if (!jGroup.TryGetProperty("stage_id", out JsonElement jStageId))
                {
                    Logger.Info("Required stage_id field for enemy group not found.");
                    return false;
                }

                enemyGroup.StageId = ParseStageId(jStageId);

                enemyGroup.SubGroupId = 0;
                if (jGroup.TryGetProperty("subgroup_id", out JsonElement jSubGroupId))
                {
                    enemyGroup.SubGroupId = jSubGroupId.GetUInt32();
                }

                if (jGroup.TryGetProperty("starting_index", out JsonElement jStartingIndex))
                {
                    enemyGroup.StartingIndex = jStartingIndex.GetUInt32();
                }

                enemyGroup.PlacementType = QuestEnemyPlacementType.Automatic;
                if (jGroup.TryGetProperty("placement_type", out JsonElement jPlacementType))
                {
                    if (!Enum.TryParse(jPlacementType.GetString(), true, out QuestEnemyPlacementType placementType))
                    {
                        Logger.Error($"Invalid Quest Enemy Placement Type");
                        return false;
                    }

                    enemyGroup.PlacementType = placementType;
                }

                foreach (var enemy in jGroup.GetProperty("enemies").EnumerateArray())
                {
                    bool isBoss = false;
                    if (enemy.TryGetProperty("is_boss", out JsonElement jIsBoss))
                    {
                        isBoss = jIsBoss.GetBoolean();
                    }

                    byte index = 0;
                    if (enemyGroup.PlacementType == QuestEnemyPlacementType.Manual)
                    {
                        if (!enemy.TryGetProperty("index", out JsonElement jEnemyIndex))
                        {
                            Logger.Error($"Manual placed enemy group requires an index value. Unable to parse.");
                            return false;
                        }
                        index = jEnemyIndex.GetByte();
                    }

                    bool isRequired = true;
                    if (enemy.TryGetProperty("is_required", out JsonElement jIsRequired))
                    {
                        isRequired = jIsRequired.GetBoolean();
                    }

                    uint repopWaitSecond = 0;
                    if (enemy.TryGetProperty("repop_wait_second", out JsonElement jRepopWaitSecond))
                    {
                        repopWaitSecond = jRepopWaitSecond.GetUInt32();
                    }

                    // Look for custom drops here
                    bool customDropItems = false;

                    // Setting default values in case a custom table is defined.
                    DropsTable customTable = new()
                    {
                        Id = 0,
                        MdlType = 0,
                    };

                    if (enemy.TryGetProperty("drop_items", out JsonElement itemsList))
                    {
                        customDropItems = true;
                        var list = itemsList.EnumerateArray();

                        foreach (var items in list)
                        {
                            GatheringItem dropItems = new()
                            {
                                ItemId = items.GetProperty("item_id").GetUInt32(),
                                ItemNum = items.GetProperty("item_min").GetUInt32(),
                                MaxItemNum = items.GetProperty("item_max").GetUInt32(),
                                Quality = items.GetProperty("quality").GetUInt32(),
                                IsHidden = false,
                                DropChance = items.GetProperty("drop_chance").GetDouble()
                            };

                            customTable.Items.Add(dropItems);
                        }

                    }

                    var questEnemy = new InstancedEnemy()
                    {
                        EnemyId = Convert.ToUInt32(enemy.GetProperty("enemy_id").GetString(), 16),
                        Lv = enemy.GetProperty("level").GetUInt16(),
                        Experience = enemy.GetProperty("exp").GetUInt32(),
                        IsBossBGM = isBoss,
                        IsBossGauge = isBoss,
                        Scale = 100,
                        EnemyTargetTypesId = (byte)(isRequired ? 4 : 1),
                        Index = index,
                        IsRequired = isRequired,
                        RepopWaitSecond = repopWaitSecond,
                    };

                    ApplyOptionalEnemyKeys(enemy, questEnemy);

                    // ApplyEnemyDropTable
                    if (customDropItems)
                    {
                        questEnemy.DropsTable = customTable;
                    }
                    else
                    {
                        // Get default drops for this enemy id and level.
                        DropsTable lootTable = questDrops.GetDropTable(questEnemy.EnemyId, questEnemy.Lv);

                        questEnemy.DropsTable = lootTable;
                    }

                    enemyGroup.Enemies.Add(questEnemy);
                }

                assetData.EnemyGroups[groupId++] = enemyGroup;
            }

            return true;
        }

        private void ApplyOptionalEnemyKeys(JsonElement enemy, Enemy questEnemey)
        {
            if (enemy.TryGetProperty("pp", out JsonElement jPpAmount))
            {
                questEnemey.PPDrop = jPpAmount.GetUInt32();
            }

            if (enemy.TryGetProperty("named_enemy_params_id", out JsonElement jNamedEnemyParamsId))
            {
                questEnemey.NamedEnemyParams = this.namedParams.GetValueOrDefault(jNamedEnemyParamsId.GetUInt32(), NamedParam.DEFAULT_NAMED_PARAM);
            }

            if (enemy.TryGetProperty("raid_boss_id", out JsonElement jRaidBossId))
            {
                questEnemey.RaidBossId = jRaidBossId.GetUInt32();
            }

            if (enemy.TryGetProperty("scale", out JsonElement jScale))
            {
                questEnemey.Scale = jScale.GetUInt16();
            }

            if (enemy.TryGetProperty("hm_present_no", out JsonElement jHmPresetNo))
            {
                questEnemey.HmPresetNo = jHmPresetNo.GetUInt16();
            }

            if (enemy.TryGetProperty("start_think_tbl_no", out JsonElement jStartThinkTblNo))
            {
                questEnemey.StartThinkTblNo = jStartThinkTblNo.GetByte();
            }

            if (enemy.TryGetProperty("repop_num", out JsonElement jRepopNum))
            {
                questEnemey.RepopNum = jRepopNum.GetByte();
            }

            if (enemy.TryGetProperty("repop_count", out JsonElement jRepopCount))
            {
                questEnemey.RepopCount = jRepopCount.GetByte();
            }

            if (enemy.TryGetProperty("enemy_target_types_id", out JsonElement jEnemyTargetTypesId))
            {
                questEnemey.EnemyTargetTypesId = jEnemyTargetTypesId.GetByte();
            }

            if (enemy.TryGetProperty("montage_fix_no", out JsonElement jMontageFixNo))
            {
                questEnemey.MontageFixNo = jMontageFixNo.GetByte();
            }

            if (enemy.TryGetProperty("set_type", out JsonElement jSetType))
            {
                questEnemey.SetType = jSetType.GetByte();
            }

            if (enemy.TryGetProperty("infection_type", out JsonElement jInfectionType))
            {
                questEnemey.InfectionType = jInfectionType.GetByte();
            }

            if (enemy.TryGetProperty("is_boss_gauge", out JsonElement jIsBossGauge))
            {
                questEnemey.IsBossGauge = jIsBossGauge.GetBoolean();
            }

            if (enemy.TryGetProperty("is_boss_bgm", out JsonElement jIsBossBGM))
            {
                questEnemey.IsBossBGM = jIsBossBGM.GetBoolean();
            }

            if (enemy.TryGetProperty("is_manual_set", out JsonElement jIsManualSet))
            {
                questEnemey.IsManualSet = jIsManualSet.GetBoolean();
            }

            if (enemy.TryGetProperty("is_area_boss", out JsonElement jIsAreaBoss))
            {
                questEnemey.IsAreaBoss = jIsAreaBoss.GetBoolean();
            }

            if (enemy.TryGetProperty("blood_orbs", out JsonElement jBloodOrbs))
            {
                questEnemey.BloodOrbs = jBloodOrbs.GetUInt32();
            }

            if (enemy.TryGetProperty("high_orbs", out JsonElement jHighOrbs))
            {
                questEnemey.HighOrbs = jHighOrbs.GetUInt32();
            }

            if (enemy.TryGetProperty("spawn_time_start", out JsonElement jSpawnTimeStart))
            {
                questEnemey.SpawnTimeStart = jSpawnTimeStart.GetUInt32();
            }

            if (enemy.TryGetProperty("spawn_time_end", out JsonElement jSpawnTimeEnd))
            {
                questEnemey.SpawnTimeEnd = jSpawnTimeEnd.GetUInt32();
            }
        }

        private bool ParseRawBlock(JsonElement jBlock, QuestBlock questBlock)
        {
            if (jBlock.TryGetProperty("check_commands", out JsonElement jCheckCommands))
            {
                var jCheckCommandList = jCheckCommands.EnumerateArray().ToList();
                if (jCheckCommandList.Count > 0)
                {
                    if (jCheckCommands[0].ValueKind == JsonValueKind.Array)
                    {
                        // New way which supports OR conditions
                        foreach (var jCheckGroup in jCheckCommandList)
                        {
                            List<CDataQuestCommand> checkCommands = new List<CDataQuestCommand>();
                            foreach (var jCheckCommand in jCheckGroup.EnumerateArray())
                            {
                                CDataQuestCommand command = new CDataQuestCommand();
                                if (!Enum.TryParse(jCheckCommand.GetProperty("type").GetString(), true, out QuestCommandCheckType type))
                                {
                                    return false;
                                }

                                command.Command = (ushort)type;
                                ParseCommandParams(jCheckCommand, command);

                                checkCommands.Add(command);
                            }

                            if (checkCommands.Count > 0)
                            {
                                questBlock.CheckCommands.Add(checkCommands);
                            }
                        }
                    }
                    else
                    {
                        // Legacy Way
                        List<CDataQuestCommand> checkCommands = new List<CDataQuestCommand>();
                        foreach (var jCheckCommand in jCheckCommandList)
                        {
                            CDataQuestCommand command = new CDataQuestCommand();
                            if (!Enum.TryParse(jCheckCommand.GetProperty("type").GetString(), true, out QuestCommandCheckType type))
                            {
                                return false;
                            }

                            command.Command = (ushort)type;
                            ParseCommandParams(jCheckCommand, command);

                            checkCommands.Add(command);
                        }

                        if (checkCommands.Count > 0)
                        {
                            questBlock.CheckCommands.Add(checkCommands);
                        }
                    }
                }
            }

            if (jBlock.TryGetProperty("result_commands", out JsonElement jResultCommands))
            {
                foreach (var jResultCommand in jResultCommands.EnumerateArray())
                {
                    CDataQuestCommand command = new CDataQuestCommand();
                    if (!Enum.TryParse(jResultCommand.GetProperty("type").GetString(), true, out QuestResultCommand type))
                    {
                        return false;
                    }

                    command.Command = (ushort)type;
                    ParseCommandParams(jResultCommand, command);

                    questBlock.ResultCommands.Add(command);
                }
            }

            return true;
        }

        private bool ParseAltConditions(JsonElement jAltConditions, QuestBlock questBlock)
        {
            return true;
        }

        private void ParseCommandParams(JsonElement jCommand, CDataQuestCommand command)
        {
            List<string> commandParams = new List<string>() { "Param1", "Param2", "Param3", "Param4" };
            for (int i = 0; i < commandParams.Count; i++)
            {
                int paramValue = 0;
                if (jCommand.TryGetProperty(commandParams[i], out JsonElement jParam))
                {
                    paramValue = jParam.GetInt32();
                }

                switch (i)
                {
                    case 0:
                        command.Param01 = paramValue;
                        break;
                    case 1:
                        command.Param02 = paramValue;
                        break;
                    case 2:
                        command.Param03 = paramValue;
                        break;
                    case 3:
                        command.Param04 = paramValue;
                        break;
                }
            }
        }
   
        private bool ParseLightQuestDetails(QuestAssetData assetData, JsonElement jLightQuestDetails)
        {
            if(!jLightQuestDetails.TryGetProperty("area_id", out JsonElement jAreaId))
            {
                Logger.Error($"Missing required member 'area_id' from LightQuest config.");
                return false;
            }
            assetData.LightQuestDetail.AreaId = jAreaId.GetUInt32();

            if (!jLightQuestDetails.TryGetProperty("board_type", out JsonElement jBoardType))
            {
                Logger.Error($"Missing required member 'board_type' from LightQuest config.");
                return false;
            }
            assetData.LightQuestDetail.BoardType = jAreaId.GetUInt32();

            if (!jLightQuestDetails.TryGetProperty("board_id", out JsonElement jBoardId))
            {
                Logger.Error($"Missing required member 'board_id' from LightQuest config.");
                return false;
            }
            assetData.LightQuestDetail.BaseAreaPoint = jBoardId.GetUInt32();

            return true;
        }
    }
}
