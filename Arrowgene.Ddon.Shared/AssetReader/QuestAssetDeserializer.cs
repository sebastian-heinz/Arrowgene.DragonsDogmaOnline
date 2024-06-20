using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using Arrowgene.Ddon.Shared.Model.Quest;
using YamlDotNet.Core.Tokens;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class QuestAssetDeserializer
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestAssetDeserializer));

        private Dictionary<uint, NamedParam> namedParams;

        public QuestAssetDeserializer(Dictionary<uint, NamedParam> namedParams)
        {
            this.namedParams = namedParams;
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

            assetData.NextQuestId = 0;
            if (jQuest.TryGetProperty("next_quest", out JsonElement jNextQuest))
            {
                assetData.NextQuestId = (QuestId)jNextQuest.GetUInt32();
            }

            assetData.QuestScheduleId = assetData.QuestId;
            if (jQuest.TryGetProperty("quest_schedule_id", out JsonElement jQuestScheduleId))
            {
                assetData.QuestScheduleId = (QuestId)jQuestScheduleId.GetUInt32();
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

            ParseRewards(assetData, jQuest);
            
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
                            var item = reward.GetProperty("loot_pool").EnumerateArray().ToList()[0];
                            rewardItem = new QuestFixedRewardItem()
                            {
                                LootPool = new List<LootPoolItem>()
                                {
                                    new FixedLootPoolItem()
                                    {
                                        ItemId = item.GetProperty("item_id").GetUInt32(),
                                        Num = item.GetProperty("num").GetUInt16(),
                                    }
                                }
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
                        assetData.ExpType = ExpType.ExperiencePoints;
                        assetData.ExpReward = reward.GetProperty("amount").GetUInt32();
                        break;
                    case "pp":
                        assetData.ExpType = ExpType.PlayPoints;
                        assetData.ExpReward = reward.GetProperty("amount").GetUInt32();
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
                                StageId = ParseStageId(jblock.GetProperty("stage_id"))
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
                            questBlock.QuestOrderDetails.QuestId = (QuestId) jblock.GetProperty("quest_id").GetUInt32();
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

                            if (!Enum.TryParse(jblock.GetProperty("interact_type").GetString(), true, out OmInteractType interactType))
                            {
                                Logger.Error($"Unable to parse the quest typ in block @ index {blockIndex - 1}.");
                                return false;
                            }

                            questBlock.ShowMarker = true;
                            if (jblock.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }

                            if (jblock.TryGetProperty("quest_id", out JsonElement jQuestId))
                            {
                                questBlock.OmInteractEvent.QuestId = (QuestId) jQuestId.GetUInt32();
                            }

                            questBlock.OmInteractEvent.QuestType = questType;
                            questBlock.OmInteractEvent.InteractType = interactType;
                        }
                        break;
                    case QuestBlockType.DeliverItems:
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
                        IsRequired = isRequired
                    };

                    ApplyOptionalEnemyKeys(enemy, questEnemy);
                    // ApplyEnemyDropTable

                    enemyGroup.Enemies.Add(questEnemy);
                }

                assetData.EnemyGroups[groupId++] = enemyGroup;
            }

            return true;
        }

        private void ApplyOptionalEnemyKeys(JsonElement enemy, Enemy questEnemey)
        {
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

            if (enemy.TryGetProperty("mondatge_fix_no", out JsonElement jMontageFixNo))
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
    }
}
