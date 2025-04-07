using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class BitterblackMazeManager
    {
        private static readonly uint BitterblackBraceletItemId = 21651;
        private static readonly uint BitterblackEarringItemId = 23711;

        public static readonly byte BBM_ROTUNDA_LV_CAP = 50;
        public static readonly byte BBM_ABYSS_LV_CAP = 55;

        private static uint ShouldReportProgress(BitterblackMazeProgress progress)
        {
            return (uint) ((progress.ContentId == 0 && progress.StartTime > 0) ? 1 : 0);
        }

        private static uint ShouldReportSearchResults(BitterblackMazeProgress progress, BitterblackMazeRewards rewards)
        {
            bool rewardPresent = (rewards.GoldMarks > 0 || rewards.SilverMarks > 0 || rewards.RedMarks > 0);
            return (uint) ((rewardPresent && progress.Tier == 0 && progress.StartTime > 0) ? 1 : 0);
        }

        public static uint LevelCap(BitterblackMazeProgress progress)
        {
            uint levelCap = 0;
            switch (progress.ContentMode)
            {
                case BattleContentMode.Rotunda:
                    levelCap = BBM_ROTUNDA_LV_CAP;
                    break;
                case BattleContentMode.Abyss:
                    levelCap = BBM_ABYSS_LV_CAP;
                    break;
            }
            return levelCap;
        }

        public static CDataBattleContentStatus GetUpdatedContentStatus(DdonGameServer server, Character character, DbConnection? connectionIn = null)
        {
            var progress = character.BbmProgress;

            var rewards = server.Database.SelectBBMRewards(character.CharacterId, connectionIn);

            var availableRewards = new List<CDataBattleContentAvailableRewards>();
            var trackedRewards = server.Database.SelectBBMContentTreasure(character.CharacterId, connectionIn);
            foreach (var stage in server.AssetRepository.BitterblackMazeAsset.Stages)
            {
                var matches = trackedRewards.Select(x => x.ContentId == stage.Value.ContentId).ToList();
                if (matches.Count == 0)
                {
                    availableRewards.Add(new CDataBattleContentAvailableRewards()
                    {
                        Id = stage.Value.ContentId,
                        Amount = 1,
                    });
                }
                else
                {
                    availableRewards.Add(new CDataBattleContentAvailableRewards()
                    {
                        Id = stage.Value.ContentId,
                        Amount = 0,
                    });
                }
            }

            var contentStatus = new CDataBattleContentStatus()
            {
                BattleContentSituationData = new CDataBattleContentSituationData()
                {
                    GameMode = GameMode.BitterblackMaze,
                    ContentId = progress.ContentId,
                    StartTime = progress.StartTime,
                    RewardBonus = BattleContentRewardBonus.Normal,
                    RewardReceived = availableRewards.All(x => x.Amount == 0),
                    ReportSearchResults = ShouldReportSearchResults(progress, rewards), // This needs to be set after killing last boss? (or maybe between tiers if you exit?)
                    ReportReset = ShouldReportProgress(progress),
                    Unk7 = 23, // Value from pcap, not sure what it does
                },
                BattleContentAvailableRewardsList = availableRewards,
            };
            return contentStatus;
        }

        public static PacketQueue HandleTierClear(DdonGameServer server, GameClient client, Character character, StageLayoutId stageId, DbConnection? connectionIn = null)
        {
            PacketQueue packets = new();
            
            var progress = character.BbmProgress;

            var match = server.AssetRepository.BitterblackMazeAsset.Stages.Select(x => x.Value).Where(x => x.ContentId == progress.ContentId).FirstOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL); // TODO: Better error code.

            var stageProgression = server.AssetRepository.BitterblackMazeAsset.StageProgressionList.Where(x => x.Id == match.ContentId).FirstOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL); // TODO: Better error code.

            if (stageProgression.ConnectionList.Count > 0)
            {
                var clearNtc = new S2CBattleContentClearTierNtc()
                {
                    Unk0 = progress.ContentId,
                    TierName = match.ContentName
                };
                client.Enqueue(clearNtc, packets);

                progress.ContentId = stageProgression.ConnectionList[0].Value;
                progress.Tier += 1;
            }
            else
            {
                // All tiers cleared
                var clearNtc = new S2CBattleContentClearContentNtc()
                {
                    Unk0 = progress.ContentId,
                    ContentName = (match.ContentMode == BattleContentMode.Rotunda) ? "Bitterblack Maze Rotunda" : "Bitterblack Maze Abyss",
                    ClearTime = (ulong) (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (long) progress.StartTime)
                };
                client.Enqueue(clearNtc, packets);

                progress.ContentId = 0;
                progress.Tier = 0;

                if (match.ContentMode == BattleContentMode.Abyss)
                {
                    // Clearing Abyss counts for both kinds of achievements.
                    packets.AddRange(server.AchievementManager.HandleClearBBM(client, true, connectionIn));
                }
                packets.AddRange(server.AchievementManager.HandleClearBBM(client, false, connectionIn));
            }
            server.Database.UpdateBBMProgress(character.CharacterId, progress, connectionIn);

            var rewards = server.Database.SelectBBMRewards(character.CharacterId, connectionIn);
            // TODO: handle BattleContentRewardBonus.Up (some sort of reward bonus)
            // TODO: Is there a reason we wouldn't get a reward here?
            var marks = GetMarksForStage(server.AssetRepository.BitterblackMazeAsset, stageId);
            rewards.GoldMarks += marks.Gold;
            rewards.SilverMarks += marks.Silver;
            rewards.RedMarks += marks.Red;
            server.Database.UpdateBBMRewards(character.CharacterId, rewards, connectionIn);

            // Update the situation information
            S2CBattleContentProgressNtc progressNtc = new S2CBattleContentProgressNtc();
            progressNtc.BattleContentStatusList.Add(BitterblackMazeManager.GetUpdatedContentStatus(server, character, connectionIn));
            client.Enqueue(progressNtc, packets);

            return packets;
        }

        public static bool IsMazeReward(uint itemId)
        {
            return itemId == BitterblackMazeManager.BitterblackBraceletItemId || itemId == BitterblackMazeManager.BitterblackEarringItemId;
        }

        public static Item ApplyCrest(IDatabase database, Character character, Item item, DbConnection? connectionIn = null)
        {
            // Don't allow crests to be applied if it's already gotten one by the Dispel Handler.
            if (item.EquipElementParamList.Any())
            {
                return item;
            }

            if (item.ItemId == BitterblackMazeManager.BitterblackBraceletItemId)
            {
                uint crestId = BitterBlackMazeRewards.BraceletRolls[Random.Shared.Next(BitterBlackMazeRewards.BraceletRolls.Count)];
                item.EquipElementParamList.Add(new CDataEquipElementParam()
                {
                    SlotNo = 1,
                    CrestId = crestId,
                });
                database.InsertCrest(character.CommonId, item.UId, 1, crestId, 0, connectionIn);
            }
            else
            {
                var rolls = BitterBlackMazeRewards.EarringRolls[character.Job];
                uint crestId = rolls[Random.Shared.Next(rolls.Count)];
                ushort add = AppraisalManager.RollBitterBlackMazeEarringPercent(character.Job);
                item.EquipElementParamList.Add(new CDataEquipElementParam()
                {
                    SlotNo = 1,
                    CrestId = crestId,
                    Add = add
                });
                database.InsertCrest(character.CommonId, item.UId, 1, crestId, add, connectionIn);
            }

            return item;
        }

        internal enum ChestType
        {
            Normal, // Random unsealed chests
            Orange, // Sealed chests from stage objectives
            Purple, // Sealed chests from tier boss
            Bracelet, // Sealed chests from tier boss
            Earring, // Sealed chests from tier boss
        }

        private static readonly Dictionary<(StageLayoutId, uint), ChestType> gSealedChestDrops = new Dictionary<(StageLayoutId, uint), ChestType>()
        {
            // Rotunda Sealed Chests
            [(new StageLayoutId(610, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(610, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(610, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(610, 0, 30), 3)] = ChestType.Orange,
            [(new StageLayoutId(611, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(611, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(611, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(612, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(612, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(612, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(614, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(614, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(615, 0, 29), 0)] = ChestType.Orange,
            [(new StageLayoutId(615, 0, 29), 1)] = ChestType.Orange,
            [(new StageLayoutId(615, 0, 29), 2)] = ChestType.Orange,
            [(new StageLayoutId(616, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(616, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(616, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(617, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(617, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(617, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(617, 0, 30), 3)] = ChestType.Orange,
            [(new StageLayoutId(617, 0, 30), 4)] = ChestType.Orange,
            [(new StageLayoutId(617, 0, 30), 5)] = ChestType.Orange,
            [(new StageLayoutId(618, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(618, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(618, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(618, 0, 30), 3)] = ChestType.Orange,
            [(new StageLayoutId(618, 0, 30), 4)] = ChestType.Orange,
            [(new StageLayoutId(619, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(619, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(619, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(619, 0, 30), 3)] = ChestType.Orange,
            [(new StageLayoutId(620, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(620, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(620, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(620, 0, 30), 3)] = ChestType.Orange,
            [(new StageLayoutId(620, 0, 30), 4)] = ChestType.Orange,
            [(new StageLayoutId(620, 0, 30), 5)] = ChestType.Orange,
            [(new StageLayoutId(621, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(621, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(621, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(621, 0, 30), 3)] = ChestType.Orange,
            [(new StageLayoutId(621, 0, 30), 4)] = ChestType.Orange,
            [(new StageLayoutId(622, 0, 30), 0)] = ChestType.Orange,
            [(new StageLayoutId(622, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(622, 0, 30), 2)] = ChestType.Orange,
            [(new StageLayoutId(622, 0, 30), 3)] = ChestType.Orange,
            // Rotunda Bosses
            [(new StageLayoutId(603, 0, 30), 0)] = ChestType.Bracelet,
            [(new StageLayoutId(603, 0, 30), 1)] = ChestType.Orange,
            [(new StageLayoutId(603, 0, 30), 2)] = ChestType.Purple,
            [(new StageLayoutId(604, 0, 30), 0)] = ChestType.Bracelet,
            [(new StageLayoutId(604, 0, 30), 1)] = ChestType.Purple,
            [(new StageLayoutId(605, 0, 30), 0)] = ChestType.Bracelet,
            // Abyss Sealed Chests
            [(new StageLayoutId(686, 0, 199), 0)] = ChestType.Orange,
            [(new StageLayoutId(687, 0, 201), 0)] = ChestType.Orange,
            [(new StageLayoutId(687, 0, 201), 1)] = ChestType.Orange,
            [(new StageLayoutId(688, 0, 205), 0)] = ChestType.Orange,
            [(new StageLayoutId(689, 0, 199), 0)] = ChestType.Orange,
            [(new StageLayoutId(690, 0, 201), 0)] = ChestType.Orange,
            [(new StageLayoutId(691, 0, 211), 0)] = ChestType.Orange,
            [(new StageLayoutId(692, 0, 201), 0)] = ChestType.Orange,
            [(new StageLayoutId(692, 0, 201), 1)] = ChestType.Orange,
            [(new StageLayoutId(693, 0, 200), 0)] = ChestType.Orange,
            [(new StageLayoutId(693, 0, 200), 1)] = ChestType.Orange,
            [(new StageLayoutId(694, 0, 210), 0)] = ChestType.Orange,
            [(new StageLayoutId(694, 0, 210), 1)] = ChestType.Orange,
            [(new StageLayoutId(695, 0, 203), 0)] = ChestType.Orange,
            [(new StageLayoutId(695, 0, 203), 1)] = ChestType.Orange,
            [(new StageLayoutId(696, 0, 201), 0)] = ChestType.Orange,
            [(new StageLayoutId(696, 0, 201), 1)] = ChestType.Orange,
            [(new StageLayoutId(697, 0, 203), 0)] = ChestType.Orange,
            [(new StageLayoutId(697, 0, 203), 1)] = ChestType.Orange,
            [(new StageLayoutId(715, 0, 203), 0)] = ChestType.Orange,
            [(new StageLayoutId(715, 0, 203), 1)] = ChestType.Orange,
            [(new StageLayoutId(716, 0, 201), 0)] = ChestType.Orange,
            [(new StageLayoutId(716, 0, 201), 1)] = ChestType.Orange,
            [(new StageLayoutId(717, 0, 203), 0)] = ChestType.Orange,
            [(new StageLayoutId(717, 0, 203), 1)] = ChestType.Orange,
            // Abyss Bosses
            [(new StageLayoutId(682, 0, 199), 0)] = ChestType.Orange,
            [(new StageLayoutId(682, 0, 201), 0)] = ChestType.Earring,
            [(new StageLayoutId(682, 0, 201), 1)] = ChestType.Purple,
            [(new StageLayoutId(683, 0, 201), 0)] = ChestType.Earring,
            [(new StageLayoutId(683, 0, 201), 1)] = ChestType.Purple,
            [(new StageLayoutId(684, 0, 201), 0)] = ChestType.Earring,
            [(new StageLayoutId(684, 0, 201), 1)] = ChestType.Purple,
            [(new StageLayoutId(685, 0, 200), 0)] = ChestType.Earring,
        };

        public static List<InstancedGatheringItem> RollChestLoot(DdonGameServer server, Character character, StageLayoutId stageId, uint pos)
        {
            JobId jobId = character.ActiveCharacterJobData.Job;
            var results = new List<InstancedGatheringItem>();

            var assets = server.AssetRepository.BitterblackMazeAsset;

            var chestType = gSealedChestDrops.ContainsKey((stageId, pos)) ? gSealedChestDrops[(stageId, pos)] : ChestType.Normal;

            if (chestType == ChestType.Purple || chestType == ChestType.Bracelet || chestType == ChestType.Earring)
            {
                uint rareItem = RollRareArmor(assets, stageId);
                if (rareItem > 0)
                {
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = (ItemId) rareItem,
                        ItemNum = 1,
                        Quality = 1,
                    });
                }
            }

            if (chestType == ChestType.Bracelet || chestType == ChestType.Earring)
            {
                // Check to see if player claimed loot already
                // If not, populate it in the chest loot table
                var treasure = server.Database.SelectBBMContentTreasure(character.CharacterId).Where(x => x.ContentId == character.BbmProgress.ContentId).ToList();
                if (treasure.Count == 0)
                {
                    uint itemId;
                    uint quality;
                    if (RollRewardChance(DetermineJewelryChance(assets, stageId)))
                    {
                        itemId = (chestType == ChestType.Bracelet) ? BitterblackMazeManager.BitterblackBraceletItemId : BitterblackMazeManager.BitterblackEarringItemId;
                        quality = 1;
                    }
                    else
                    {
                        var items = BitterblackMazeManager.SelectGearType(assets.HighQualityWeapons[jobId], DetermineEquipClass(assets.HighQualityArmors, jobId), assets.HighQualityOther);
                        itemId = BitterblackMazeManager.SelectGear(server, items, chestType, stageId);
                        quality = 0;
                    }
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = (ItemId) itemId,
                        ItemNum = 1,
                        Quality = quality
                    });
                }
            }
            else
            {
                uint picks = (uint)Random.Shared.Next(4);
                for (int i = 0; i < picks; i++)
                {
                    List<uint> items = new List<uint>();
                    items.AddRange(BitterblackMazeManager.SelectGearType(assets.LowQualityWeapons[jobId], DetermineEquipClass(assets.LowQualityArmors, jobId), assets.LowQualityOther));
                    switch (chestType)
                    {
                        case ChestType.Orange:
                        case ChestType.Purple:
                            items.AddRange(BitterblackMazeManager.SelectGearType(assets.HighQualityWeapons[jobId], DetermineEquipClass(assets.HighQualityArmors, jobId), assets.HighQualityOther));
                            break;
                    }

                    uint itemId = BitterblackMazeManager.SelectGear(server, items, chestType, stageId);
                    if (itemId > 0)
                    {
                        results.Add(new InstancedGatheringItem()
                        {
                            ItemId = (ItemId)itemId,
                            ItemNum = 1,
                        });
                    }
                }
            }

            if (results.Count == 0 && (chestType == ChestType.Orange || chestType == ChestType.Purple))
            {
                // Sealed chests should have at least one piece of equipment
                var items = BitterblackMazeManager.SelectGearType(assets.HighQualityWeapons[jobId], DetermineEquipClass(assets.HighQualityArmors, jobId), assets.HighQualityOther);
                uint itemId = BitterblackMazeManager.SelectGear(server, items, chestType, stageId);
                results.Add(new InstancedGatheringItem()
                {
                    ItemId = (ItemId) itemId,
                    ItemNum = 1,
                });
            }

            if (chestType != ChestType.Earring && chestType != ChestType.Bracelet)
            {
                foreach (var item in assets.ChestTrash)
                {
                    if (Random.Shared.Next(5) < 4)
                    {
                        // 1/5 chance
                        continue;
                    }

                    uint numItems = (uint)Random.Shared.Next((int)(item.Amount + 1));
                    if (numItems > 0)
                    {
                        // Stick consumable in the front of the list
                        results.Insert(0, new InstancedGatheringItem()
                        {
                            ItemId = (ItemId) item.Item1,
                            ItemNum = numItems
                        });
                    }
                }
            }

            if (results.Count == 0)
            {
                // Stick something in the chest so it is not empty
                var item = assets.ChestTrash[Random.Shared.Next(assets.ChestTrash.Count)];
                uint numItems = (uint)Random.Shared.Next((int)(item.Amount + 1));
                results.Add(new InstancedGatheringItem()
                {
                    ItemId = (ItemId) item.ItemId,
                    ItemNum = numItems > 0 ? numItems : 1
                });
            }

            return results;
        }

        private static List<uint> DetermineEquipClass(Dictionary<BitterblackMazeEquipmentClass, List<uint>> items, JobId jobId)
        {
            switch (jobId)
            {
                case JobId.Fighter:
                case JobId.Warrior:
                    return items[BitterblackMazeEquipmentClass.HeavyMelee];
                case JobId.SpiritLancer:
                case JobId.Hunter:
                case JobId.Seeker:
                    return items[BitterblackMazeEquipmentClass.LightMelee];
                case JobId.Sorcerer:
                case JobId.Priest:
                case JobId.ElementArcher:
                    return items[BitterblackMazeEquipmentClass.Mage];
                case JobId.ShieldSage:
                case JobId.Alchemist:
                case JobId.HighScepter:
                    return items[BitterblackMazeEquipmentClass.Tank];
                default:
                    return null;
            }
        }

        private static List<uint> SelectGearType(List<uint> weapons, List<uint> armors, List<uint> capes)
        {
            int itemType = Random.Shared.Next(3);
            switch (itemType)
            {
                case 0:
                    return weapons;
                case 1:
                    return armors;
                case 2:
                    return capes;
            }
            return weapons;
        }

        private static (uint Min, uint Max) DetermineItemTier(DdonGameServer server, ChestType chestType, StageLayoutId stageId)
        {
            var lootRange = server.AssetRepository.BitterblackMazeAsset.LootRanges[stageId.Id];
            switch (chestType)
            {
                case ChestType.Orange:
                case ChestType.Purple:
                case ChestType.Bracelet:
                case ChestType.Earring:
                    return lootRange.SealedRange;
                default:
                    return lootRange.NormalRange;
            }
        }

        private static double DetermineRareChance(BitterblackMazeAsset assets, StageLayoutId stageId)
        {
            return assets.LootRanges[stageId.Id].RareChance;
        }

        private static double DetermineJewelryChance(BitterblackMazeAsset assets, StageLayoutId stageId)
        {
            return assets.LootRanges[stageId.Id].JewelryChance;
        }

        private static (uint Gold, uint Silver, uint Red) GetMarksForStage(BitterblackMazeAsset assets, StageLayoutId stageId)
        {
            return assets.LootRanges[stageId.Id].Marks;
        }

        private static uint SelectGear(DdonGameServer server, List<uint> items, ChestType chestType, StageLayoutId stageId)
        {
            if (items.Count == 0)
            {
                return 0;
            }

            uint attempts = 0;
            uint itemId = 0;
            var itemRankRange = DetermineItemTier(server, chestType, stageId);
            do
            {
                itemId = items[Random.Shared.Next(items.Count)];

                var itemRank = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, itemId).Rank;
                if (itemRank >= itemRankRange.Min && itemRank <= itemRankRange.Max)
                {
                    break;
                }

            } while (attempts++ < 100);

            if (attempts > 100)
            {
                itemId = items[0];
            }

            return itemId;
        }

        private static uint RollRareArmor(BitterblackMazeAsset assets, StageLayoutId stageId)
        {
            uint itemId = 0;
            var dropTable = StageManager.IsBitterBlackMazeBossStageId(stageId) ? assets.RotundaRare : assets.AbyssRare;
            if (RollRewardChance(DetermineRareChance(assets, stageId)))
            {
                itemId = dropTable[Random.Shared.Next(dropTable.Count)];
            }
            return itemId;
        }

        private static bool RollRewardChance(double chance)
        {
            return chance >= Random.Shared.NextDouble();
        }
    }
}
