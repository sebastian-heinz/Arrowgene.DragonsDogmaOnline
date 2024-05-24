using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Text.Json.Serialization;
using System;
using Arrowgene.Ddon.GameServer.Quests;
using System.Collections;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using static Arrowgene.Ddon.Shared.Csv.GmdCsv;
using System.Security.Cryptography;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class QuestAssetDeserializer : IAssetDeserializer<QuestAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestAssetDeserializer));

        public QuestAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            var questAssets = new QuestAsset();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            if (!Enum.TryParse(document.RootElement.GetProperty("state_machine").GetString(), true, out QuestStateMachineType questStateMachineType))
            {
                Logger.Error($"Expected key 'state_machine' not in the root of the document. Unable to parse {path}.");
                return questAssets;
            }

            if (questStateMachineType != QuestStateMachineType.GenericStateMachine)
            {
                Logger.Error($"Unsupported QuestStateMachineType '{questStateMachineType}'. Unable to parse {path}.");
                return questAssets;
            }

            foreach (var quest in document.RootElement.GetProperty("quests").EnumerateArray())
            {
                QuestAssetData assetData = new QuestAssetData();

                if (!Enum.TryParse(quest.GetProperty("type").GetString(), true, out QuestType questType))
                {
                    Logger.Error($"Unable to parse the quest type. Skipping.");
                    continue;
                }

                assetData.Type = questType;
                assetData.QuestId = (QuestId) quest.GetProperty("quest_id").GetUInt32();
                assetData.BaseLevel = quest.GetProperty("base_level").GetUInt16();
                assetData.MinimumItemRank = quest.GetProperty("minimum_item_rank").GetByte();
                assetData.Discoverable = quest.GetProperty("discoverable").GetBoolean();

                ParseRewards(assetData, quest);
                if (!ParseBlocks(assetData, quest))
                {
                    Logger.Error($"Unable to create the quest '{assetData.QuestId}'. Skipping.");
                    continue;
                }

                questAssets.Quests.Add(assetData);
            }

            return questAssets;
        }

        private void ParseRewards(QuestAssetData assetData, JsonElement quest)
        {
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

        private bool ParseBlocks(QuestAssetData assetData, JsonElement quest)
        {
            ushort blockIndex = 1;
            foreach (var block in quest.GetProperty("blocks").EnumerateArray())
            {
                QuestBlock questBlock = new QuestBlock();

                if (!Enum.TryParse(block.GetProperty("type").GetString(), true, out QuestBlockType questBlockType))
                {
                    Logger.Error($"Unable to parse the quest block type @ index {blockIndex - 1}.");
                    return false;
                }

                questBlock.BlockType = questBlockType;
                questBlock.BlockNo = blockIndex;
                questBlock.AnnounceType = QuestAnnounceType.None;

                if (block.TryGetProperty("announce_type", out JsonElement jUpdateAnnounce))
                {
                    if (!Enum.TryParse(jUpdateAnnounce.GetString(), true, out QuestAnnounceType announceType))
                    {
                        Logger.Error($"Unable to parse the quest announce type @ index {blockIndex - 1}.");
                        return false;
                    }
                    questBlock.AnnounceType = announceType;
                }

                questBlock.StageId = ParseStageId(block.GetProperty("stage_id"));
                questBlock.SubGroupId = 0;
                if (block.TryGetProperty("subgroup_no", out JsonElement jSubGroupNo))
                {
                    questBlock.SubGroupId = jSubGroupNo.GetUInt16();
                }

                if (block.TryGetProperty("layout_flags_on", out JsonElement jLayoutFlagsOn))
                {
                    foreach (var jLayoutFlag in jLayoutFlagsOn.EnumerateArray())
                    {
                        questBlock.QuestLayoutFlagsOn.Add(jLayoutFlag.GetUInt32());
                    }
                }

                if (block.TryGetProperty("layout_flags_off", out JsonElement jLayoutFlagsOff))
                {
                    foreach (var jLayoutFlag in jLayoutFlagsOff.EnumerateArray())
                    {
                        questBlock.QuestLayoutFlagsOff.Add(jLayoutFlag.GetUInt32());
                    }
                }

                switch (questBlockType)
                {
                    case QuestBlockType.Accept:
                        questBlock.AnnounceType = QuestAnnounceType.Accept;
                        break;
                    case QuestBlockType.IsStageNo:
                        break;
                    case QuestBlockType.DummyBlock:
                        break;
                    case QuestBlockType.DiscoverEnemy:
                    case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                        break;
                    case QuestBlockType.NpcTalkAndOrder:
                        {
                            if (!Enum.TryParse(block.GetProperty("npc_id").GetString(), true, out NpcId npcId))
                            {
                                Logger.Error($"Unable to parse the npc_id in block @ index {blockIndex - 1}.");
                                return false;
                            }
                            questBlock.NpcOrderDetails.NpcId = npcId;
                            questBlock.NpcOrderDetails.MsgId = block.GetProperty("message_id").GetInt32();
                        }
                        break;
                    case QuestBlockType.KillGroup:
                        questBlock.QuestLayoutFlag = GetQuestLayoutFlag(assetData.QuestId, questBlock.StageId, questBlock.SubGroupId, questBlock.ProcessNo, questBlock.BlockNo);
                        questBlock.StartingIndex = 0;

                        if (block.TryGetProperty("starting_index", out JsonElement jStartingIndex))
                        {
                            questBlock.StartingIndex = jStartingIndex.GetUInt32();
                        }

                        foreach (var enemy in block.GetProperty("group").EnumerateArray())
                        {
                            bool IsBoss = enemy.GetProperty("is_boss").GetBoolean();
                            var questEnemy = new Enemy()
                            {
                                EnemyId = Convert.ToUInt32(enemy.GetProperty("enemy_id").GetString(), 16),
                                Lv = enemy.GetProperty("level").GetUInt16(),
                                Experience = enemy.GetProperty("exp").GetUInt32(),
                                IsBossBGM = IsBoss,
                                IsBossGauge = IsBoss,
                                Scale = 100,
                                EnemyTargetTypesId = 4
                            };

                            ApplyOptionalEnemyKeys(enemy, questEnemy);
                            // ApplyEnemyDropTable

                            questBlock.Enemies.Add(questEnemy);
                        }
                        break;
                    case QuestBlockType.TalkToNpc:
                        {
                            if (!Enum.TryParse(block.GetProperty("npc_id").GetString(), true, out NpcId npcId))
                            {
                                Logger.Error($"Unable to parse the npc_id in block @ index {blockIndex - 1}.");
                                return false;
                            }
                            questBlock.NpcOrderDetails.NpcId = npcId;
                            questBlock.NpcOrderDetails.MsgId = block.GetProperty("message_id").GetInt32();
                        }
                        break;
                    case QuestBlockType.CollectItem:
                        {
                            questBlock.ShowMarker = true;
                            if (block.TryGetProperty("show_marker", out JsonElement jShowMarker))
                            {
                                questBlock.ShowMarker = jShowMarker.GetBoolean();
                            }
                        }
                        break;
                    case QuestBlockType.DeliverItems:
                        foreach (var item in block.GetProperty("items").EnumerateArray())
                        {
                            questBlock.DeliveryRequests.Add(new QuestDeliveryItem()
                            {
                                ItemId = item.GetProperty("id").GetUInt32(),
                                Amount = item.GetProperty("amount").GetUInt32()
                            });
                        }
                        // DeliverItems is a bit more complicated
                        // It needs to be split into 3 different actions
                        // 1. Item check to see if you have the items
                        // 2. Delivery of items
                        // 3. Removal of items from inventory?
                        break;
                    default:
                        Logger.Error($"Unsupported QuestBlockType {questBlockType} @ index {blockIndex - 1}.");
                        return false;
                }

                assetData.Blocks.Add(questBlock);

                blockIndex += 1;
            }

            // Add an implicit EndBlock
            assetData.Blocks.Add(new QuestBlock()
            {
                BlockType = QuestBlockType.End,
                BlockNo = blockIndex,
                SequenceNo = 1,
            });

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
            
            uint groupId = jStageId.GetProperty("group_id").GetUInt32();
            return new StageId(id, layerNo, groupId);
        }

        private void ApplyOptionalEnemyKeys(JsonElement enemy, Enemy questEnemey)
        {
            if (enemy.TryGetProperty("named_enemy_params_id", out JsonElement jNamedEnemyParamsId))
            {
                questEnemey.NamedEnemyParamsId = jNamedEnemyParamsId.GetUInt32();
            }

            if (enemy.TryGetProperty("named_enemy_params_id", out JsonElement jRaidBossId))
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

        private uint GetQuestLayoutFlag(QuestId questId, StageId stageId, uint subGroupNo, uint processNo, uint blockNo)
        {
            IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
            hash.AppendData(BitConverter.GetBytes((uint)questId));
            hash.AppendData(BitConverter.GetBytes(processNo));
            hash.AppendData(BitConverter.GetBytes(blockNo));
            hash.AppendData(BitConverter.GetBytes(stageId.Id));
            hash.AppendData(BitConverter.GetBytes(stageId.GroupId));
            hash.AppendData(BitConverter.GetBytes(stageId.LayerNo));
            hash.AppendData(BitConverter.GetBytes(subGroupNo));

            return Convert.ToUInt32(BitConverter.ToString(hash.GetHashAndReset()).Replace("-", string.Empty).Substring(0, 8), 16);
        }
    }
}
