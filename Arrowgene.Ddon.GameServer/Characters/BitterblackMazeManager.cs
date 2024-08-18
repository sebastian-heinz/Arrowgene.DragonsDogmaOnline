using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using YamlDotNet.Core;

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

        public static CDataBattleContentStatus GetUpdatedContentStatus(DdonGameServer server, Character character)
        {
            var progress = character.BbmProgress;

            var rewards = server.Database.SelectBBMRewards(character.NormalCharacterId);

            var availableRewards = new List<CDataBattleContentAvailableRewards>();
            var trackedRewards = server.Database.SelectBBMContentTreasure(character.NormalCharacterId);
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

        public static void HandleTierClear(DdonGameServer server, GameClient client, Character character, StageId stageId)
        {
            var progress = character.BbmProgress;

            var match = server.AssetRepository.BitterblackMazeAsset.Stages.Select(x => x.Value).Where(x => x.ContentId == progress.ContentId).FirstOrDefault();

            var stageProgression = server.AssetRepository.BitterblackMazeAsset.StageProgressionList.Where(x => x.Id == match.ContentId).FirstOrDefault();
            if (stageProgression.ConnectionList.Count > 0)
            {
                var clearNtc = new S2CBattleContentClearTierNtc()
                {
                    Unk0 = progress.ContentId,
                    TierName = match.ContentName
                };
                client.Send(clearNtc);

                progress.ContentId = stageProgression.ConnectionList[0].Value;
                progress.Tier += 1;
            }
            else
            {
                // All tiers cleared
                var clearNtc = new S2CBattleContentClearContentNtc()
                {
                    Unk0 = progress.ContentId,
                    ContentName = (match.ContentMode == BattleContentMode.Rotunda) ? "Bitterblack Maze Rotunda Cleared" : "Bitterblack Maze Abyss Cleared",
                    ClearTime = (ulong) (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (long) progress.StartTime)
                };
                client.Send(clearNtc);

                progress.ContentId = 0;
                progress.Tier = 0;
            }
            server.Database.UpdateBBMProgress(character.NormalCharacterId, progress);

            var rewards = server.Database.SelectBBMRewards(character.NormalCharacterId);
            // TODO: handle BattleContentRewardBonus.Up (some sort of reward bonus)
            // TODO: Is there a reason we wouldn't get a reward here?
            rewards.GoldMarks += 1;
            rewards.SilverMarks += 5;
            rewards.RedMarks += 15;
            server.Database.UpdateBBMRewards(character.NormalCharacterId, rewards);

            // Update the situation information
            S2CBattleContentProgressNtc progressNtc = new S2CBattleContentProgressNtc();
            progressNtc.BattleContentStatusList.Add(BitterblackMazeManager.GetUpdatedContentStatus(server, character));
            client.Send(progressNtc);
        }

        public static bool IsMazeReward(uint itemId)
        {
            return itemId == BitterblackMazeManager.BitterblackBraceletItemId || itemId == BitterblackMazeManager.BitterblackEarringItemId;
        }

        public static Item ApplyCrest(IDatabase database, Character character, Item item, DbConnection? connectionIn = null)
        {
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

        private static readonly Dictionary<(StageId, uint), ChestType> gSealedChestDrops = new Dictionary<(StageId, uint), ChestType>()
        {
            // Rotunda Sealed Chests
            [(new StageId(610, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(610, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(610, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(610, 0, 30), 3)] = ChestType.Orange,
            [(new StageId(611, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(611, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(611, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(612, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(612, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(612, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(614, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(614, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(615, 0, 29), 0)] = ChestType.Orange,
            [(new StageId(615, 0, 29), 1)] = ChestType.Orange,
            [(new StageId(615, 0, 29), 2)] = ChestType.Orange,
            [(new StageId(616, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(616, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(616, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(617, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(617, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(617, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(617, 0, 30), 3)] = ChestType.Orange,
            [(new StageId(617, 0, 30), 4)] = ChestType.Orange,
            [(new StageId(617, 0, 30), 5)] = ChestType.Orange,
            [(new StageId(618, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(618, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(618, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(618, 0, 30), 3)] = ChestType.Orange,
            [(new StageId(618, 0, 30), 4)] = ChestType.Orange,
            [(new StageId(619, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(619, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(619, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(619, 0, 30), 3)] = ChestType.Orange,
            [(new StageId(620, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(620, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(620, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(620, 0, 30), 3)] = ChestType.Orange,
            [(new StageId(620, 0, 30), 4)] = ChestType.Orange,
            [(new StageId(620, 0, 30), 5)] = ChestType.Orange,
            [(new StageId(621, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(621, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(621, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(621, 0, 30), 3)] = ChestType.Orange,
            [(new StageId(621, 0, 30), 4)] = ChestType.Orange,
            [(new StageId(622, 0, 30), 0)] = ChestType.Orange,
            [(new StageId(622, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(622, 0, 30), 2)] = ChestType.Orange,
            [(new StageId(622, 0, 30), 3)] = ChestType.Orange,
            // Rotunda Bosses
            [(new StageId(603, 0, 30), 0)] = ChestType.Bracelet,
            [(new StageId(603, 0, 30), 1)] = ChestType.Orange,
            [(new StageId(603, 0, 30), 2)] = ChestType.Purple,
            [(new StageId(604, 0, 30), 0)] = ChestType.Bracelet,
            [(new StageId(604, 0, 30), 1)] = ChestType.Purple,
            [(new StageId(605, 0, 30), 0)] = ChestType.Bracelet,
            // Abyss Sealed Chests
            [(new StageId(686, 0, 199), 0)] = ChestType.Orange,
            [(new StageId(687, 0, 201), 0)] = ChestType.Orange,
            [(new StageId(687, 0, 201), 1)] = ChestType.Orange,
            [(new StageId(688, 0, 205), 0)] = ChestType.Orange,
            [(new StageId(689, 0, 199), 0)] = ChestType.Orange,
            [(new StageId(690, 0, 201), 0)] = ChestType.Orange,
            [(new StageId(691, 0, 211), 0)] = ChestType.Orange,
            [(new StageId(692, 0, 201), 0)] = ChestType.Orange,
            [(new StageId(692, 0, 201), 1)] = ChestType.Orange,
            [(new StageId(693, 0, 200), 0)] = ChestType.Orange,
            [(new StageId(693, 0, 200), 1)] = ChestType.Orange,
            [(new StageId(694, 0, 210), 0)] = ChestType.Orange,
            [(new StageId(694, 0, 210), 1)] = ChestType.Orange,
            [(new StageId(695, 0, 203), 0)] = ChestType.Orange,
            [(new StageId(695, 0, 203), 1)] = ChestType.Orange,
            [(new StageId(696, 0, 201), 0)] = ChestType.Orange,
            [(new StageId(696, 0, 201), 1)] = ChestType.Orange,
            [(new StageId(697, 0, 203), 0)] = ChestType.Orange,
            [(new StageId(697, 0, 203), 1)] = ChestType.Orange,
            [(new StageId(715, 0, 203), 0)] = ChestType.Orange,
            [(new StageId(715, 0, 203), 1)] = ChestType.Orange,
            [(new StageId(716, 0, 201), 0)] = ChestType.Orange,
            [(new StageId(716, 0, 201), 1)] = ChestType.Orange,
            [(new StageId(717, 0, 203), 0)] = ChestType.Orange,
            [(new StageId(717, 0, 203), 1)] = ChestType.Orange,
            // Abyss Bosses
            [(new StageId(682, 0, 199), 0)] = ChestType.Orange,
            [(new StageId(682, 0, 201), 0)] = ChestType.Earring,
            [(new StageId(682, 0, 201), 1)] = ChestType.Purple,
            [(new StageId(683, 0, 201), 0)] = ChestType.Earring,
            [(new StageId(683, 0, 201), 1)] = ChestType.Purple,
            [(new StageId(684, 0, 201), 0)] = ChestType.Earring,
            [(new StageId(684, 0, 201), 1)] = ChestType.Purple,
            [(new StageId(685, 0, 200), 0)] = ChestType.Earring,
        };

        public static List<InstancedGatheringItem> RollChestLoot(DdonGameServer server, Character character, StageId stageId, uint pos)
        {
            JobId jobId = character.ActiveCharacterJobData.Job;

            var results = new List<InstancedGatheringItem>();

            var chestType = gSealedChestDrops.ContainsKey((stageId, pos)) ? gSealedChestDrops[(stageId, pos)] : ChestType.Normal;

            if (chestType == ChestType.Purple || chestType == ChestType.Bracelet || chestType == ChestType.Earring)
            {
                uint rareItem = RollRareArmor(stageId);
                if (rareItem > 0)
                {
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = rareItem,
                        ItemNum = 1,
                    });
                }
            }

            if (chestType == ChestType.Bracelet || chestType == ChestType.Earring)
            {
                // Check to see if player claimed loot already
                // If not, populate it in the chest loot table
                var treasure = server.Database.SelectBBMContentTreasure(character.NormalCharacterId).Where(x => x.ContentId == character.BbmProgress.ContentId).ToList();
                if (treasure.Count == 0)
                {
                    uint itemId = (chestType == ChestType.Bracelet) ? BitterblackMazeManager.BitterblackBraceletItemId : BitterblackMazeManager.BitterblackEarringItemId;
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = itemId,
                        ItemNum = 1,
                        Quality = 1
                    });
                }
            }
            else
            {
                uint picks = (uint)Random.Shared.Next(4);
                for (int i = 0; i < picks; i++)
                {
                    List<uint> items = new List<uint>();
                    switch (chestType)
                    {
                        case ChestType.Orange:
                        case ChestType.Purple:
                            items.AddRange(BitterblackMazeManager.SelectGearType(gHighQualityWeapons[jobId], gHighQualityArmors[jobId], gHighQualityCapes));
                            break;
                        case ChestType.Normal:
                            items.AddRange(BitterblackMazeManager.SelectGearType(gLowQualityWeapons[jobId], gLowQualityArmors[jobId], gLowQualityCapes));
                            break;
                    }

                    uint itemId = BitterblackMazeManager.SelectGear(server, character, items);
                    if (itemId > 0)
                    {
                        results.Add(new InstancedGatheringItem()
                        {
                            ItemId = itemId,
                            ItemNum = 1,
                        });
                    }
                }
            }

            if (results.Count == 0 && (chestType == ChestType.Orange || chestType == ChestType.Purple))
            {
                // Sealed chests should have at least one piece of equipment
                var items = BitterblackMazeManager.SelectGearType(gHighQualityWeapons[jobId], gHighQualityArmors[jobId], gHighQualityCapes);
                uint itemId = BitterblackMazeManager.SelectGear(server, character, items);
                results.Add(new InstancedGatheringItem()
                {
                    ItemId = itemId,
                    ItemNum = 1,
                });
            }

            if (chestType != ChestType.Earring && chestType != ChestType.Bracelet)
            {
                foreach (var item in gChestTrash)
                {
                    if (Random.Shared.Next(5) < 4)
                    {
                        // 1/5 chance
                        continue;
                    }

                    uint numItems = (uint)Random.Shared.Next((int)(item.Item2 + 1));
                    if (numItems > 0)
                    {
                        // Stick consumable in the front of the list
                        results.Insert(0, new InstancedGatheringItem()
                        {
                            ItemId = item.Item1,
                            ItemNum = numItems
                        });
                    }
                }
            }

            if (results.Count == 0)
            {
                // Stick something in the chest so it is not empty
                var item = gChestTrash[Random.Shared.Next(gChestTrash.Count)];
                uint numItems = (uint)Random.Shared.Next((int)(item.Item2 + 1));
                results.Add(new InstancedGatheringItem()
                {
                    ItemId = item.Item1,
                    ItemNum = numItems > 0 ? numItems : 1
                });
            }

            return results;
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

        private static (uint, uint) DetermineItemTier(Character character)
        {
            var progress = character.BbmProgress;
            switch (progress.ContentMode)
            {
                case BattleContentMode.Rotunda:
                    switch (progress.Tier)
                    {
                        case 1:
                            return (1, 4);
                        case 2:
                            return (4, 5);
                        case 3:
                            return (6, 10);
                    }
                    break;
                case BattleContentMode.Abyss:
                    switch (progress.Tier)
                    {
                        case 1:
                            return (1, 4);
                        case 2:
                            return (5, 6);
                        case 3:
                            return (7, 11);
                        case 4:
                            return (11, 11);
                    }
                    break;
            }
            return (1, 1);
        }

        private static uint SelectGear(DdonGameServer server, Character character, List<uint> items)
        {
            if (items.Count == 0)
            {
                return 0;
            }

            uint attempts = 0;
            uint itemId = 0;
            var itemRankRange = DetermineItemTier(character);
            do
            {
                itemId = items[Random.Shared.Next(items.Count)];

                var itemRank = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, itemId).Rank;
                if (itemRank >= itemRankRange.Item1 && itemRank <= itemRankRange.Item2)
                {
                    break;
                }

            } while (attempts++ < 20);

            if (attempts > 20)
            {
                itemId = items[0];
            }

            return itemId;
        }

        private static uint RollRareArmor(StageId stageId)
        {
            uint itemId = 0;
            var dropTable = StageManager.IsBitterBlackMazeBossStageId(stageId) ? gRareNormalDrops : gRareAbyssNormalDrops;
            if (Random.Shared.Next(100) > 98)
            {
                itemId = dropTable[Random.Shared.Next(dropTable.Count)];
            }
            return itemId;
        }

        private static readonly List<(uint, uint)> gChestTrash = new List<(uint, uint)>()
        {
            // ItemId, Max
            (55,   5), // Lantern Kindling
            (9361, 2), // Quality Gala Extract
            (41,   1), // Panacea
            (7552, 2), // Healing Elixer
        };

        private static readonly List<uint> gRareNormalDrops = new List<uint>()
        {
            21396, // カースドヘルム,Cursed Helm,
            21397, // カースドアーマー,Cursed Armor,
            21398, // カースドアーム,Cursed Arms,
            21399, // カースドキュイス,Cursed Cuisses,
            21400, // ケイオスマスク,Chaos Mask,
            21401, // ケイオスローブ,Chaos Robe,
            21402, // ケイオスグローブ,Chaos Gloves,
            21403, // ケイオスレッグス,Chaos Legs,
        };

        private static readonly List<uint> gRareAbyssNormalDrops = new List<uint>()
        {
            24601, // プレアードヘルム,Pleiades Helm,
            24605, // ヴァルキリアヘルム,Valkyrian Helm,
            24609, // プレアードローブ,Pleiades Robe,
            24614, // ヴァルキリアアーマー,Valkyrian Armor,
            24615, // プレアードウェア,Pleiades Clothing,
            24616, // ヴァルキリアアーム,Valkyrian Arms,
            24617, // プレアードパンツ,Pleiades Pants,
            24618, // ヴァルキリアレッグス,Valkyrian Legs,
        };

        private static readonly Dictionary<JobId, List<uint>> gLowQualityWeapons = new Dictionary<JobId, List<uint>>()
        {
            [JobId.Fighter] = new List<uint>()
            {
                // Weapons
                62, // 3,0,1,1,Bronze Sword,201,1,1,1,0,1
                1674, // 3,8,1,1,Bronze Sword,201,1,1,1,1,1
                2697, // 3,8,1,1,Bronze Sword,201,1,1,1,2,1
                63, // 3,104,1,2,Landing Sword,201,5,1,1,0,2
                1675, // 3,104,1,2,Landing Sword,201,5,1,1,1,2
                2698, // 3,104,1,2,Landing Sword,201,5,1,1,2,2
                64, // 3,260,1,2,Mace,201,8,1,1,0,3
                1676, // 3,260,1,2,Mace,201,8,1,1,1,3
                2699, // 3,260,1,2,Mace,201,8,1,1,2,3
                65, // 3,488,1,3,Makhaira,201,11,1,1,0,1
                1677, // 3,488,1,3,Makhaira,201,11,1,1,1,1
                2700, // 3,488,1,3,Makhaira,201,11,1,1,2,1
                66, // 3,1160,1,4,Thousand Troops,201,17,1,1,0,1
                1678, // 3,1160,1,4,Thousand Troops,201,17,1,1,1,1
                2701, // 3,1160,1,4,Thousand Troops,201,17,1,1,2,1
                67, // 3,788,1,3,Strong Hatchet,201,14,1,1,0,1
                1679, // 3,788,1,3,Strong Hatchet,201,14,1,1,1,1
                2702, // 3,788,1,3,Strong Hatchet,201,14,1,1,2,1
                68, // 3,1448,1,4,Iron Sword,201,19,1,1,0,1
                1680, // 3,1448,1,4,Iron Sword,201,19,1,1,1,1
                2703, // 3,1448,1,4,Iron Sword,201,19,1,1,2,1
                134, // 3,1768,1,5,Cutlass,201,21,1,1,0,1
                1746, // 3,1768,1,5,Cutlass,201,21,1,1,1,1
                2769, // 3,1768,1,5,Cutlass,201,21,1,1,2,1
                136, // 3,2308,1,5,Bastard Sword,201,25,1,1,0,1
                1748, // 3,2308,1,5,Bastard Sword,201,25,1,1,1,1
                2771, // 3,2308,1,5,Bastard Sword,201,25,1,1,2,1
                137, // 3,2708,1,6,Broadsword,201,27,1,1,0,1
                1749, // 3,2708,1,6,Broadsword,201,27,1,1,1,1
                2772, // 3,2708,1,6,Broadsword,201,27,1,1,2,1
                138, // 3,3848,1,7,Tight Cinquedea,201,31,1,1,0,1
                1750, // 3,3848,1,7,Tight Cinquedea,201,31,1,1,1,1
                2773, // 3,3848,1,7,Tight Cinquedea,201,31,1,1,2,1
                139, // 3,3140,1,6,Morgenstern,201,29,1,1,0,1
                1751, // 3,3140,1,6,Morgenstern,201,29,1,1,1,1
                2774, // 3,3140,1,6,Morgenstern,201,29,1,1,2,1
                140, // 3,5188,1,8,Captain's Soul,201,36,1,1,0,1
                1752, // 3,5188,1,8,Captain's Soul,201,36,1,1,1,1
                2775, // 3,5188,1,8,Captain's Soul,201,36,1,1,2,1
                141, // 3,5480,1,8,Bud of Destruction,201,37,1,1,0,1
                1753, // 3,5480,1,8,Bud of Destruction,201,37,1,1,1,1
                2776, // 3,5480,1,8,Bud of Destruction,201,37,1,1,2,1
                142, // 3,4628,1,7,Falcata,201,34,1,1,0,1
                1754, // 3,4628,1,7,Falcata,201,34,1,1,1,1
                2777, // 3,4628,1,7,Falcata,201,34,1,1,2,1
                143, // 3,8104,1,9,Thousand Stings,201,45,1,1,0,1
                1755, // 3,8104,1,9,Thousand Stings,201,45,1,1,1,1
                2778, // 3,8104,1,9,Thousand Stings,201,45,1,1,2,1
                144, // 3,6088,1,8,Sense of Wound,201,40,1,1,0,1
                1756, // 3,6088,1,8,Sense of Wound,201,40,1,1,1,1
                2779, // 3,6088,1,8,Sense of Wound,201,40,1,1,2,1
                145, // 3,4100,1,7,Biting Club,201,32,1,1,0,1
                1757, // 3,4100,1,7,Biting Club,201,32,1,1,1,1
                2780, // 3,4100,1,7,Biting Club,201,32,1,1,2,1
                146, // 3,5780,1,8,Silver Rapier,201,38,1,1,0,1
                1758, // 3,5780,1,8,Silver Rapier,201,38,1,1,1,1
                2781, // 3,5780,1,8,Silver Rapier,201,38,1,1,2,1
                152, // 3,8468,1,10,Brute Killer,201,47,1,1,0,1
                1764, // 3,8468,1,10,Brute Killer,201,47,1,1,1,1
                2787, // 3,8468,1,10,Brute Killer,201,47,1,1,2,1
                153, // 3,12104,1,11,Dark Intention,201,55,1,1,0,1
                1765, // 3,12104,1,11,Dark Intention,201,55,1,1,1,1
                2788, // 3,12104,1,11,Dark Intention,201,55,1,1,2,1
                154, // 3,9220,1,11,Francisca,201,51,1,1,0,1
                1766, // 3,9220,1,11,Francisca,201,51,1,1,1,1
                2789, // 3,9220,1,11,Francisca,201,51,1,1,2,1
                155, // 3,8840,1,10,Ingot Club,201,49,1,1,0,1
                1767, // 3,8840,1,10,Ingot Club,201,49,1,1,1,1
                2790, // 3,8840,1,10,Ingot Club,201,49,1,1,2,1
                // Shields
                73, // 3,0,1,1,Round Shield,202,1,1,0,0,1
                1685, // 3,4,1,1,Round Shield,202,1,1,0,1,1
                2708, // 3,4,1,1,Round Shield,202,1,1,0,2,1
                69, // 3,74,1,2,Pelta,202,6,1,0,0,1
                1681, // 3,74,1,2,Pelta,202,6,1,0,1,1
                2704, // 3,74,1,2,Pelta,202,6,1,0,2,1
                9933, // 3,244,1,3,Novice Shield,202,11,1,0,0,1
                9934, // 3,244,1,3,Novice Shield,202,11,1,0,1,1
                9935, // 3,244,1,3,Novice Shield,202,11,1,0,2,1
                71, // 3,514,1,4,Muscular Wall,202,16,1,0,0,1
                1683, // 3,514,1,4,Muscular Wall,202,16,1,0,1,1
                2706, // 3,514,1,4,Muscular Wall,202,16,1,0,2,1
                9049, // 3,1060,1,5,Shield of the White Knights,202,23,1,0,0,1
                9050, // 3,1060,1,5,Shield of the White Knights,202,23,1,0,1,1
                9051, // 3,1060,1,5,Shield of the White Knights,202,23,1,0,2,1
                9943, // 3,1570,1,6,Alpine Pelta,202,28,1,0,0,1
                9944, // 3,1570,1,6,Alpine Pelta,202,28,1,0,1,1
                9945, // 3,1570,1,6,Alpine Pelta,202,28,1,0,2,1
                165, // 3,2180,1,7,Silver Thorn,202,33,1,0,0,1
                1777, // 3,2180,1,7,Silver Thorn,202,33,1,0,1,1
                2800, // 3,2180,1,7,Silver Thorn,202,33,1,0,2,1
                167, // 3,2594,1,8,Mark of the Chimera,202,36,1,0,0,1
                1779, // 3,2594,1,8,Mark of the Chimera,202,36,1,0,1,1
                2802, // 3,2594,1,8,Mark of the Chimera,202,36,1,0,2,1
                169, // 3,3364,1,9,Shine Targe,202,41,1,0,0,1
                1781, // 3,3364,1,9,Shine Targe,202,41,1,0,1,1
                2804, // 3,3364,1,9,Shine Targe,202,41,1,0,2,1
                170, // 3,5410,1,11,Stark Stolz,202,53,1,0,0,1
                1782, // 3,5410,1,11,Stark Stolz,202,53,1,0,1,1
                2805, // 3,5410,1,11,Stark Stolz,202,53,1,0,2,1
                171, // 3,4610,1,10,Boldness,202,49,1,0,0,1
                1783, // 3,4610,1,10,Boldness,202,49,1,0,1,1
                2806, // 3,4610,1,10,Boldness,202,49,1,0,2,1
                172, // 3,6052,1,11,Shield of the Divine Protector,202,55,1,0,0,1
                1784, // 3,6052,1,11,Shield of the Divine Protector,202,55,1,0,1,1
                2807, // 3,6052,1,11,Shield of the Divine Protector,202,55,1,0,2,1
            },
            [JobId.Hunter] = new List<uint>()
            {
                72, // 3,0,1,1,Shortbow,207,1,3,1,0,1
                1684, // 3,10,1,1,Shortbow,207,1,3,1,1,1
                2707, // 3,10,1,1,Shortbow,207,1,3,1,2,1
                74, // 3,130,1,2,Stalking Bow,207,5,3,1,0,1
                1686, // 3,130,1,2,Stalking Bow,207,5,3,1,1,1
                2709, // 3,130,1,2,Stalking Bow,207,5,3,1,2,1
                75, // 3,610,1,3,Strain,207,11,3,1,0,1
                1687, // 3,610,1,3,Strain,207,11,3,1,1,1
                2710, // 3,610,1,3,Strain,207,11,3,1,2,1
                76, // 3,325,1,2,Composite Longbow,207,8,3,1,0,1
                1688, // 3,325,1,2,Composite Longbow,207,8,3,1,1,1
                2711, // 3,325,1,2,Composite Longbow,207,8,3,1,2,1
                78, // 3,1450,1,4,White Wolf Bow,207,17,3,1,0,1
                1690, // 3,1450,1,4,White Wolf Bow,207,17,3,1,1,1
                2713, // 3,1450,1,4,White Wolf Bow,207,17,3,1,2,1
                178, // 3,2885,1,5,Fluted Bow,207,25,3,1,0,1
                1790, // 3,2885,1,5,Fluted Bow,207,25,3,1,1,1
                2813, // 3,2885,1,5,Fluted Bow,207,25,3,1,2,1
                180, // 3,3925,1,6,Liberty Bow,207,29,3,1,0,1
                1792, // 3,3925,1,6,Liberty Bow,207,29,3,1,1,1
                2815, // 3,3925,1,6,Liberty Bow,207,29,3,1,2,1
                184, // 3,5785,1,7,Lustre Acanthus,207,34,3,1,0,1
                1796, // 3,5785,1,7,Lustre Acanthus,207,34,3,1,1,1
                2819, // 3,5785,1,7,Lustre Acanthus,207,34,3,1,2,1
                182, // 3,7225,1,8,Bezel Crown,207,38,3,1,0,1
                1794, // 3,7225,1,8,Bezel Crown,207,38,3,1,1,1
                2817, // 3,7225,1,8,Bezel Crown,207,38,3,1,2,1
                11117, // 3,8410,1,9,Nactoral Bow,207,41,3,1,0,1
                11118, // 3,8410,1,9,Nactoral Bow,207,41,3,1,1,1
                11119, // 3,8410,1,9,Nactoral Bow,207,41,3,1,2,1
                190, // 3,10130,1,9,Scroll Hilt Bow,207,45,3,1,0,1
                1802, // 3,10130,1,9,Scroll Hilt Bow,207,45,3,1,1,1
                2825, // 3,10130,1,9,Scroll Hilt Bow,207,45,3,1,2,1
                196, // 3,10585,1,10,Brier Shooter,207,47,3,1,0,1
                1808, // 3,10585,1,10,Brier Shooter,207,47,3,1,1,1
                2831, // 3,10585,1,10,Brier Shooter,207,47,3,1,2,1
                193, // 3,11050,1,10,Ex Machina,207,49,3,1,0,1
                1805, // 3,11050,1,10,Ex Machina,207,49,3,1,1,1
                2828, // 3,11050,1,10,Ex Machina,207,49,3,1,2,1
                194, // 3,11525,1,11,Elder's Anima,207,51,3,1,0,1
                1806, // 3,11525,1,11,Elder's Anima,207,51,3,1,1,1
                2829, // 3,11525,1,11,Elder's Anima,207,51,3,1,2,1
                198, // 3,13010,1,11,Sturdy Bow,207,53,3,1,0,1
                1810, // 3,13010,1,11,Sturdy Bow,207,53,3,1,1,1
                2833, // 3,13010,1,11,Sturdy Bow,207,53,3,1,2,1
                200, // 3,14050,1,11,Auto-Adjust Bow,207,55,3,1,0,1
                1812, // 3,14050,1,11,Auto-Adjust Bow,207,55,3,1,1,1
                2835, // 3,14050,1,11,Auto-Adjust Bow,207,55,3,1,2,1
            },
            [JobId.Priest] = new List<uint>()
            {
                80, // 3,0,1,1,Wood Staff,211,1,4,1,0,1
                1692, // 3,10,1,1,Wood Staff,211,1,4,1,1,1
                2715, // 3,10,1,1,Wood Staff,211,1,4,1,2,1
                81, // 3,130,1,2,Blue Cane,211,5,4,1,0,1
                1693, // 3,130,1,2,Blue Cane,211,5,4,1,1,1
                2716, // 3,130,1,2,Blue Cane,211,5,4,1,2,1
                82, // 3,610,1,3,Hermit Wand,211,11,4,1,0,1
                1694, // 3,610,1,3,Hermit Wand,211,11,4,1,1,1
                2717, // 3,610,1,3,Hermit Wand,211,11,4,1,2,1
                85, // 3,1450,1,4,Staff of Magickal Restraint,211,17,4,1,0,1
                1697, // 3,1450,1,4,Staff of Magickal Restraint,211,17,4,1,1,1
                2720, // 3,1450,1,4,Staff of Magickal Restraint,211,17,4,1,2,1
                207, // 3,2885,1,5,Illusionist,211,25,4,1,0,1
                1819, // 3,2885,1,5,Illusionist,211,25,4,1,1,1
                2842, // 3,2885,1,5,Illusionist,211,25,4,1,2,1
                208, // 3,3385,1,6,Sky Anchor,211,27,4,1,0,1
                1820, // 3,3385,1,6,Sky Anchor,211,27,4,1,1,1
                2843, // 3,3385,1,6,Sky Anchor,211,27,4,1,2,1
                209, // 3,3925,1,6,Pilgrim's Cane,211,29,4,1,0,1
                1821, // 3,3925,1,6,Pilgrim's Cane,211,29,4,1,1,1
                2844, // 3,3925,1,6,Pilgrim's Cane,211,29,4,1,2,1
                213, // 3,5785,1,7,Portal Indicator,211,34,4,1,0,1
                1825, // 3,5785,1,7,Portal Indicator,211,34,4,1,1,1
                2848, // 3,5785,1,7,Portal Indicator,211,34,4,1,2,1
                211, // 3,7225,1,8,Staff of Beyond the Rift,211,38,4,1,0,1
                1823, // 3,7225,1,8,Staff of Beyond the Rift,211,38,4,1,1,1
                2846, // 3,7225,1,8,Staff of Beyond the Rift,211,38,4,1,2,1
                11147, // 3,8410,1,9,Signposter,211,41,4,1,0,1
                11148, // 3,8410,1,9,Signposter,211,41,4,1,1,1
                11149, // 3,8410,1,9,Signposter,211,41,4,1,2,1
                221, // 3,10130,1,9,Staff of Magickal Destruction,211,45,4,1,0,1
                1833, // 3,10130,1,9,Staff of Magickal Destruction,211,45,4,1,1,1
                2856, // 3,10130,1,9,Staff of Magickal Destruction,211,45,4,1,2,1
                222, // 3,10585,1,10,Lela Daphne,211,47,4,1,0,1
                1834, // 3,10585,1,10,Lela Daphne,211,47,4,1,1,1
                2857, // 3,10585,1,10,Lela Daphne,211,47,4,1,2,1
                223, // 3,11525,1,11,Heart of Darkness,211,51,4,1,0,1
                1835, // 3,11525,1,11,Heart of Darkness,211,51,4,1,1,1
                2858, // 3,11525,1,11,Heart of Darkness,211,51,4,1,2,1
                224, // 3,15130,1,11,Etupirka,211,55,4,1,0,1
                1836, // 3,15130,1,11,Etupirka,211,55,4,1,1,1
                2859, // 3,15130,1,11,Etupirka,211,55,4,1,2,1
                225, // 3,11050,1,10,Staff of the Pathfinder,211,49,4,1,0,1
                1837, // 3,11050,1,10,Staff of the Pathfinder,211,49,4,1,1,1
                2860, // 3,11050,1,10,Staff of the Pathfinder,211,49,4,1,2,1
            },
            [JobId.ShieldSage] = new List<uint>()
            {
                91, // 3,0,1,1,Large Wood,204,1,5,1,0,1
                1703, // 3,8,1,1,Large Wood,204,1,5,1,1,1
                2726, // 3,8,1,1,Large Wood,204,1,5,1,2,1
                92, // 3,104,1,2,Oval Shield,204,5,5,1,0,1
                1704, // 3,104,1,2,Oval Shield,204,5,5,1,1,1
                2727, // 3,104,1,2,Oval Shield,204,5,5,1,2,1
                93, // 3,488,1,3,Monolith,204,11,5,1,0,1
                1705, // 3,488,1,3,Monolith,204,11,5,1,1,1
                2728, // 3,488,1,3,Monolith,204,11,5,1,2,1
                94, // 3,260,1,2,Battle Shield,204,8,5,1,0,1
                1706, // 3,260,1,2,Battle Shield,204,8,5,1,1,1
                2729, // 3,260,1,2,Battle Shield,204,8,5,1,2,1
                95, // 3,788,1,3,Knight's Honor,204,14,5,1,0,1
                1707, // 3,788,1,3,Knight's Honor,204,14,5,1,1,1
                2730, // 3,788,1,3,Knight's Honor,204,14,5,1,2,1
                96, // 3,1448,1,4,Scutum,204,19,5,1,0,1
                1708, // 3,1448,1,4,Scutum,204,19,5,1,1,1
                2731, // 3,1448,1,4,Scutum,204,19,5,1,2,1
                97, // 3,1160,1,4,Woody Magick,204,17,5,1,0,1
                1709, // 3,1160,1,4,Woody Magick,204,17,5,1,1,1
                2732, // 3,1160,1,4,Woody Magick,204,17,5,1,2,1
                250, // 3,2308,1,5,Megalith,204,25,5,1,0,1
                1862, // 3,2308,1,5,Megalith,204,25,5,1,1,1
                2885, // 3,2308,1,5,Megalith,204,25,5,1,2,1
                251, // 3,2708,1,6,Blue Kite,204,27,5,1,0,1
                1863, // 3,2708,1,6,Blue Kite,204,27,5,1,1,1
                2886, // 3,2708,1,6,Blue Kite,204,27,5,1,2,1
                252, // 3,3140,1,6,Setzschild,204,29,5,1,0,1
                1864, // 3,3140,1,6,Setzschild,204,29,5,1,1,1
                2887, // 3,3140,1,6,Setzschild,204,29,5,1,2,1
                253, // 3,3848,1,7,Embossed Head,204,31,5,1,0,1
                1865, // 3,3848,1,7,Embossed Head,204,31,5,1,1,1
                2888, // 3,3848,1,7,Embossed Head,204,31,5,1,2,1
                254, // 3,5188,1,8,Valor Glare,204,36,5,1,0,1
                1866, // 3,5188,1,8,Valor Glare,204,36,5,1,1,1
                2889, // 3,5188,1,8,Valor Glare,204,36,5,1,2,1
                255, // 3,4100,1,7,Bastion,204,32,5,1,0,1
                1867, // 3,4100,1,7,Bastion,204,32,5,1,1,1
                2890, // 3,4100,1,7,Bastion,204,32,5,1,2,1
                256, // 3,4628,1,7,Mark of the Advance,204,34,5,1,0,1
                1868, // 3,4628,1,7,Mark of the Advance,204,34,5,1,1,1
                2891, // 3,4628,1,7,Mark of the Advance,204,34,5,1,2,1
                259, // 3,5780,1,8,Chained Sepulchre,204,38,5,1,0,1
                1871, // 3,5780,1,8,Chained Sepulchre,204,38,5,1,1,1
                2894, // 3,5780,1,8,Chained Sepulchre,204,38,5,1,2,1
                11087, // 3,6728,1,9,Black Wall,204,41,5,1,0,1
                11088, // 3,6728,1,9,Black Wall,204,41,5,1,1,1
                11089, // 3,6728,1,9,Black Wall,204,41,5,1,2,1
                264, // 3,8104,1,9,Byzantine Guard,204,45,5,1,0,1
                1876, // 3,8104,1,9,Byzantine Guard,204,45,5,1,1,1
                2899, // 3,8104,1,9,Byzantine Guard,204,45,5,1,2,1
                268, // 3,8468,1,10,Heaven's Door,204,47,5,1,0,1
                1880, // 3,8468,1,10,Heaven's Door,204,47,5,1,1,1
                2903, // 3,8468,1,10,Heaven's Door,204,47,5,1,2,1
                270, // 3,10408,1,11,Unbreakable,204,53,5,1,0,1
                1882, // 3,10408,1,11,Unbreakable,204,53,5,1,1,1
                2905, // 3,10408,1,11,Unbreakable,204,53,5,1,2,1
                271, // 3,11240,1,11,Black Aegis,204,55,5,1,0,1
                1883, // 3,11240,1,11,Black Aegis,204,55,5,1,1,1
                2906, // 3,11240,1,11,Black Aegis,204,55,5,1,2,1
                87, // 3,0,1,1,Bronze Rod,205,1,5,0,0,1
                1699, // 3,4,1,1,Bronze Rod,205,1,5,0,1,1
                2722, // 3,4,1,1,Bronze Rod,205,1,5,0,2,1
                88, // 3,74,1,2,Spike Rod,205,6,5,0,0,1
                1700, // 3,74,1,2,Spike Rod,205,6,5,0,1,1
                2723, // 3,74,1,2,Spike Rod,205,6,5,0,2,1
                9998, // 3,244,1,3,Strike Rod,205,11,5,0,0,1
                9999, // 3,244,1,3,Strike Rod,205,11,5,0,1,1
                10000, // 3,244,1,3,Strike Rod,205,11,5,0,2,1
                90, // 3,514,1,4,Power Rod,205,16,5,0,0,1
                1702, // 3,514,1,4,Power Rod,205,16,5,0,1,1
                2725, // 3,514,1,4,Power Rod,205,16,5,0,2,1
                9968, // 3,1060,1,5,Crowned Head,205,23,5,0,0,1
                9969, // 3,1060,1,5,Crowned Head,205,23,5,0,1,1
                9970, // 3,1060,1,5,Crowned Head,205,23,5,0,2,1
                9973, // 3,1570,1,6,Beacon of Mahler,205,28,5,0,0,1
                9974, // 3,1570,1,6,Beacon of Mahler,205,28,5,0,1,1
                9975, // 3,1570,1,6,Beacon of Mahler,205,28,5,0,2,1
                9978, // 3,2314,1,7,Supernova,205,33,5,0,0,1
                9979, // 3,2314,1,7,Supernova,205,33,5,0,1,1
                9980, // 3,2314,1,7,Supernova,205,33,5,0,2,1
                240, // 3,2594,1,8,Patriot Stone,205,36,5,0,0,1
                1852, // 3,2594,1,8,Patriot Stone,205,36,5,0,1,1
                2875, // 3,2594,1,8,Patriot Stone,205,36,5,0,2,1
                10057, // 3,3364,1,9,Punishment,205,41,5,0,0,1
                10058, // 3,3364,1,9,Punishment,205,41,5,0,1,1
                10059, // 3,3364,1,9,Punishment,205,41,5,0,2,1
                10052, // 3,4052,1,9,Lethal Pile,205,45,5,0,0,1
                10053, // 3,4052,1,9,Lethal Pile,205,45,5,0,1,1
                10054, // 3,4052,1,9,Lethal Pile,205,45,5,0,2,1
                243, // 3,4234,1,10,Savior Borne,205,47,5,0,0,1
                1855, // 3,4234,1,10,Savior Borne,205,47,5,0,1,1
                2878, // 3,4234,1,10,Savior Borne,205,47,5,0,2,1
                244, // 3,4610,1,10,Gaia Scepter,205,49,5,0,0,1
                1856, // 3,4610,1,10,Gaia Scepter,205,49,5,0,1,1
                2879, // 3,4610,1,10,Gaia Scepter,205,49,5,0,2,1
                245, // 3,6052,1,11,Hands of Rain,205,55,5,0,0,1
                1857, // 3,6052,1,11,Hands of Rain,205,55,5,0,1,1
                2880, // 3,6052,1,11,Hands of Rain,205,55,5,0,2,1
            },
            [JobId.Seeker] = new List<uint>()
            {
                98,   // キャヴァリーアームス,Cavalry Arms,
                1710, // キャヴァリーアームス,Cavalry Arms,
                2733, // キャヴァリーアームス,Cavalry Arms,
                99,   // シーフブレード,Thief's Blades,
                1711, // シーフブレード,Thief's Blades,
                2734, // シーフブレード,Thief's Blades,
                100,  // ブリゼペ,Brise-épéé,
                1712, // ブリゼペ,Brise-épéé,
                2735, // ブリゼペ,Brise-épéé,
                104,  // グラディウス,Gladius,
                1716, // グラディウス,Gladius,
                2739, // グラディウス,Gladius,
                279,  // スティレット,Stilettos,
                1891, // スティレット,Stilettos,
                2914, // スティレット,Stilettos,
                281,  // ダキアエネシス,Daciaensis,
                1893, // ダキアエネシス,Daciaensis,
                2916, // ダキアエネシス,Daciaensis,
                285,  // トリスケーレ,Triskele,
                1897, // トリスケーレ,Triskele,
                2920, // トリスケーレ,Triskele,
                283,  // シニストラ,Sinistra,
                1895, // シニストラ,Sinistra,
                2918, // シニストラ,Sinistra,
                11102, // トゥーエッジ,Two-Edged,
                11103, // トゥーエッジ,Two-Edged,
                11104, // トゥーエッジ,Two-Edged,
                290, // トータルエングレイバー,Total Engravers,
                1902, // トータルエングレイバー,Total Engravers,
                2925, // トータルエングレイバー,Total Engravers,
                294, // プレージュドー,Prejuízo,
                1906, // プレージュドー,Prejuízo,
                2929, // プレージュドー,Prejuízo,
            },
            [JobId.Sorcerer] = new List<uint>()
            {
                105, // 3,0,1,1,Guide Wand,212,1,6,1,0,1
                1717, // 3,10,1,1,Guide Wand,212,1,6,1,1,1
                2740, // 3,10,1,1,Guide Wand,212,1,6,1,2,1
                106, // 3,130,1,2,Ferocious Horn,212,5,6,1,0,1
                1718, // 3,130,1,2,Ferocious Horn,212,5,6,1,1,1
                2741, // 3,130,1,2,Ferocious Horn,212,5,6,1,2,1
                107, // 3,610,1,3,Magician's Thought,212,11,6,1,0,1
                1719, // 3,610,1,3,Magician's Thought,212,11,6,1,1,1
                2742, // 3,610,1,3,Magician's Thought,212,11,6,1,2,1
                110, // 3,1450,1,4,Spiral Wand,212,17,6,1,0,1
                1722, // 3,1450,1,4,Spiral Wand,212,17,6,1,1,1
                2745, // 3,1450,1,4,Spiral Wand,212,17,6,1,2,1
                111, // 3,1810,1,4,Mage Wand,212,19,6,1,0,1
                1723, // 3,1810,1,4,Mage Wand,212,19,6,1,1,1
                2746, // 3,1810,1,4,Mage Wand,212,19,6,1,2,1
                308, // 3,2885,1,5,Magickal Branch,212,25,6,1,0,1
                1920, // 3,2885,1,5,Magickal Branch,212,25,6,1,1,1
                2943, // 3,2885,1,5,Magickal Branch,212,25,6,1,2,1
                310, // 3,3925,1,6,Sorcerer's Wand,212,29,6,1,0,1
                1922, // 3,3925,1,6,Sorcerer's Wand,212,29,6,1,1,1
                2945, // 3,3925,1,6,Sorcerer's Wand,212,29,6,1,2,1
                311, // 3,5785,1,7,Soul Harvest,212,34,6,1,0,1
                1923, // 3,5785,1,7,Soul Harvest,212,34,6,1,1,1
                2946, // 3,5785,1,7,Soul Harvest,212,34,6,1,2,1
                312, // 3,7225,1,8,Lunar Saga,212,38,6,1,0,1
                1924, // 3,7225,1,8,Lunar Saga,212,38,6,1,1,1
                2947, // 3,7225,1,8,Lunar Saga,212,38,6,1,2,1
                11162, // 3,8410,1,9,Valiant Helix,212,41,6,1,0,1
                11163, // 3,8410,1,9,Valiant Helix,212,41,6,1,1,1
                11164, // 3,8410,1,9,Valiant Helix,212,41,6,1,2,1
                11167, // 3,9250,1,9,Sealed Calamity,212,43,6,1,0,1
                11168, // 3,9250,1,9,Sealed Calamity,212,43,6,1,1,1
                11169, // 3,9250,1,9,Sealed Calamity,212,43,6,1,2,1
                315, // 3,10130,1,9,Grabbed Heart,212,45,6,1,0,1
                1927, // 3,10130,1,9,Grabbed Heart,212,45,6,1,1,1
                2950, // 3,10130,1,9,Grabbed Heart,212,45,6,1,2,1
                316, // 3,7610,1,8,Evil Eye Specimen,212,40,6,1,0,1
                1928, // 3,7610,1,8,Evil Eye Specimen,212,40,6,1,1,1
                2951, // 3,7610,1,8,Evil Eye Specimen,212,40,6,1,2,1
                317, // 3,6850,1,8,Ancient Horn,212,37,6,1,0,1
                1929, // 3,6850,1,8,Ancient Horn,212,37,6,1,1,1
                2952, // 3,6850,1,8,Ancient Horn,212,37,6,1,2,1
                318, // 3,6485,1,8,Pious Roots,212,36,6,1,0,1
                1930, // 3,6485,1,8,Pious Roots,212,36,6,1,1,1
                2953, // 3,6485,1,8,Pious Roots,212,36,6,1,2,1
                323, // 3,11525,1,11,Grace of Apollo,212,51,6,1,0,1
                1935, // 3,11525,1,11,Grace of Apollo,212,51,6,1,1,1
                2958, // 3,11525,1,11,Grace of Apollo,212,51,6,1,2,1
                324, // 3,10585,1,10,Ruthless Enforcer,212,47,6,1,0,1
                1936, // 3,10585,1,10,Ruthless Enforcer,212,47,6,1,1,1
                2959, // 3,10585,1,10,Ruthless Enforcer,212,47,6,1,2,1
                325, // 3,15130,1,11,Abyss Dweller,212,55,6,1,0,1
                1937, // 3,15130,1,11,Abyss Dweller,212,55,6,1,1,1
                2960, // 3,15130,1,11,Abyss Dweller,212,55,6,1,2,1
                326, // 3,11050,1,10,Sign of Menace,212,49,6,1,0,1
                1938, // 3,11050,1,10,Sign of Menace,212,49,6,1,1,1
                2961, // 3,11050,1,10,Sign of Menace,212,49,6,1,2,1
            },
            [JobId.ElementArcher] = new List<uint>()
            {
                112, // 3,0,1,1,Magick Bow,209,1,8,1,0,1
                1724, // 3,10,1,1,Magick Bow,209,1,8,1,1,1
                2747, // 3,10,1,1,Magick Bow,209,1,8,1,2,1
                113, // 3,130,1,2,Gnosir,209,5,8,1,0,1
                1725, // 3,130,1,2,Gnosir,209,5,8,1,1,1
                2748, // 3,130,1,2,Gnosir,209,5,8,1,2,1
                114, // 3,610,1,3,Kenn-brecher,209,11,8,1,0,1
                1726, // 3,610,1,3,Kenn-brecher,209,11,8,1,1,1
                2749, // 3,610,1,3,Kenn-brecher,209,11,8,1,2,1
                117, // 3,1450,1,4,Mageia Kyklos,209,17,8,1,0,1
                1729, // 3,1450,1,4,Mageia Kyklos,209,17,8,1,1,1
                2752, // 3,1450,1,4,Mageia Kyklos,209,17,8,1,2,1
                118, // 3,1810,1,4,Quellenkraft,209,19,8,1,0,1
                1730, // 3,1810,1,4,Quellenkraft,209,19,8,1,1,1
                2753, // 3,1810,1,4,Quellenkraft,209,19,8,1,2,1
                335, // 3,2885,1,5,Schild und Sühne,209,25,8,1,0,1
                1947, // 3,2885,1,5,Schild und Sühne,209,25,8,1,1,1
                2970, // 3,2885,1,5,Schild und Sühne,209,25,8,1,2,1
                336, // 3,2425,1,5,Mondnacht Schütteln,209,23,8,1,0,1
                1948, // 3,2425,1,5,Mondnacht Schütteln,209,23,8,1,1,1
                2971, // 3,2425,1,5,Mondnacht Schütteln,209,23,8,1,2,1
                337, // 3,2210,1,5,Morgenglühen,209,21,8,1,0,1
                1949, // 3,2210,1,5,Morgenglühen,209,21,8,1,1,1
                2972, // 3,2210,1,5,Morgenglühen,209,21,8,1,2,1
                338, // 3,3385,1,6,Glücksrad,209,27,8,1,0,1
                1950, // 3,3385,1,6,Glücksrad,209,27,8,1,1,1
                2973, // 3,3385,1,6,Glücksrad,209,27,8,1,2,1
                339, // 3,3925,1,6,Aspro Neraida,209,29,8,1,0,1
                1951, // 3,3925,1,6,Aspro Neraida,209,29,8,1,1,1
                2974, // 3,3925,1,6,Aspro Neraida,209,29,8,1,2,1
                340, // 3,4810,1,7,Vortex Flow,209,31,8,1,0,1
                1952, // 3,4810,1,7,Vortex Flow,209,31,8,1,1,1
                2975, // 3,4810,1,7,Vortex Flow,209,31,8,1,2,1
                341, // 3,7610,1,8,Gunvoltzeit,209,36,8,1,0,1
                1953, // 3,7610,1,8,Gunvoltzeit,209,36,8,1,1,1
                2976, // 3,7610,1,8,Gunvoltzeit,209,36,8,1,2,1
                342, // 3,5125,1,7,Zauberkraft,209,32,8,1,0,1
                1954, // 3,5125,1,7,Zauberkraft,209,32,8,1,1,1
                2977, // 3,5125,1,7,Zauberkraft,209,32,8,1,2,1
                343, // 3,5785,1,7,Weiß Merkmale,209,34,8,1,0,1
                1955, // 3,5785,1,7,Weiß Merkmale,209,34,8,1,1,1
                2978, // 3,5785,1,7,Weiß Merkmale,209,34,8,1,2,1
                344, // 3,10585,1,9,Dämmerungslicht,209,45,8,1,0,1
                1956, // 3,10585,1,9,Dämmerungslicht,209,45,8,1,1,1
                2979, // 3,10585,1,9,Dämmerungslicht,209,45,8,1,2,1
                345, // 3,6485,1,8,Metempsychosis,209,40,8,1,0,1
                1957, // 3,6485,1,8,Metempsychosis,209,40,8,1,1,1
                2980, // 3,6485,1,8,Metempsychosis,209,40,8,1,2,1
                347, // 3,7225,1,8,Siegstraße,209,38,8,1,0,1
                1959, // 3,7225,1,8,Siegstraße,209,38,8,1,1,1
                2982, // 3,7225,1,8,Siegstraße,209,38,8,1,2,1
                348, // 3,10130,1,9,Kunstraum,209,45,8,1,0,1
                1960, // 3,10130,1,9,Kunstraum,209,45,8,1,1,1
                2983, // 3,10130,1,9,Kunstraum,209,45,8,1,2,1
                352, // 3,10585,1,10,Großglauben,209,47,8,1,0,1
                1964, // 3,10585,1,10,Großglauben,209,47,8,1,1,1
                2987, // 3,10585,1,10,Großglauben,209,47,8,1,2,1
                353, // 3,11050,1,10,Farsighted Eye,209,49,8,1,0,1
                1965, // 3,11050,1,10,Farsighted Eye,209,49,8,1,1,1
                2988, // 3,11050,1,10,Farsighted Eye,209,49,8,1,2,1
                355, // 3,11525,1,11,Krifo Proseuché,209,51,8,1,0,1
                1967, // 3,11525,1,11,Krifo Proseuché,209,51,8,1,1,1
                2990, // 3,11525,1,11,Krifo Proseuché,209,51,8,1,2,1
                356, // 3,13010,1,11,Reize Hoffnung,209,53,8,1,0,1
                1968, // 3,13010,1,11,Reize Hoffnung,209,53,8,1,1,1
                2991, // 3,13010,1,11,Reize Hoffnung,209,53,8,1,2,1
                357, // 3,15130,1,11,Vredesmos,209,55,8,1,0,1
                1969, // 3,15130,1,11,Vredesmos,209,55,8,1,1,1
                2992, // 3,15130,1,11,Vredesmos,209,55,8,1,2,1
                358, // 3,14050,1,11,Galant Bogen,209,55,8,1,0,1
                1970, // 3,14050,1,11,Galant Bogen,209,55,8,1,1,1
                2993, // 3,14050,1,11,Galant Bogen,209,55,8,1,2,1
            },
            [JobId.Warrior] = new List<uint>()
            {
                119, // 3,0,1,1,Two-Handed Sword,203,1,7,1,0,1
                1731, // 3,10,1,1,Two-Handed Sword,203,1,7,1,1,1
                2754, // 3,10,1,1,Two-Handed Sword,203,1,7,1,2,1
                120, // 3,130,1,2,Battle Sword,203,5,7,1,0,1
                1732, // 3,130,1,2,Battle Sword,203,5,7,1,1,1
                2755, // 3,130,1,2,Battle Sword,203,5,7,1,2,1
                121, // 3,610,1,3,Claymore,203,11,7,1,0,1
                1733, // 3,610,1,3,Claymore,203,11,7,1,1,1
                2756, // 3,610,1,3,Claymore,203,11,7,1,2,1
                122, // 3,1810,1,4,Greatsword,203,20,7,1,0,1
                1734, // 3,1810,1,4,Greatsword,203,20,7,1,1,1
                2757, // 3,1810,1,4,Greatsword,203,20,7,1,2,1
                366, // 3,1450,1,4,Sharp Column,203,17,7,1,0,1
                1978, // 3,1450,1,4,Sharp Column,203,17,7,1,1,1
                3001, // 3,1450,1,4,Sharp Column,203,17,7,1,2,1
                367, // 3,2425,1,6,Iron Urchin,203,26,7,1,0,1
                1979, // 3,2425,1,6,Iron Urchin,203,26,7,1,1,1
                3002, // 3,2425,1,6,Iron Urchin,203,26,7,1,2,1
                368, // 3,3385,1,7,Flamberge,203,32,7,1,0,1
                1980, // 3,3385,1,7,Flamberge,203,32,7,1,1,1
                3003, // 3,3385,1,7,Flamberge,203,32,7,1,2,1
                369, // 3,4810,1,8,Lance Sword,203,38,7,1,0,1
                1981, // 3,4810,1,8,Lance Sword,203,38,7,1,1,1
                3004, // 3,4810,1,8,Lance Sword,203,38,7,1,2,1
                370, // 3,3925,1,7,Massive Hammer,203,35,7,1,0,1
                1982, // 3,3925,1,7,Massive Hammer,203,35,7,1,1,1
                3005, // 3,3925,1,7,Massive Hammer,203,35,7,1,2,1
                371, // 3,15685,1,12,Dissuader,203,55,7,1,0,1
                1983, // 3,15685,1,12,Dissuader,203,55,7,1,1,1
                3006, // 3,15685,1,12,Dissuader,203,55,7,1,2,1
                372, // 3,7225,1,10,Foreign Flamberge,203,47,7,1,0,1
                1984, // 3,7225,1,10,Foreign Flamberge,203,47,7,1,1,1
                3007, // 3,7225,1,10,Foreign Flamberge,203,47,7,1,2,1
                373, // 3,5785,1,9,Elfenkrieger,203,43,7,1,0,1
                1985, // 3,5785,1,9,Elfenkrieger,203,43,7,1,1,1
                3008, // 3,5785,1,9,Elfenkrieger,203,43,7,1,2,1
                374, // 3,8410,1,11,Glyph Victory,203,53,7,1,0,1
                1986, // 3,8410,1,11,Glyph Victory,203,53,7,1,1,1
                3009, // 3,8410,1,11,Glyph Victory,203,53,7,1,2,1
                375, // 3,7610,1,11,D D D,203,51,7,1,0,1
                1987, // 3,7610,1,11,D D D,203,51,7,1,1,1
                3010, // 3,7610,1,11,D D D,203,51,7,1,2,1
                376, // 3,8005,1,8,Noblesse Oblige,203,40,7,1,0,1
                1988, // 3,8005,1,8,Noblesse Oblige,203,40,7,1,1,1
                3011, // 3,8005,1,8,Noblesse Oblige,203,40,7,1,2,1
            },
            [JobId.Alchemist] = new List<uint>()
            {
                126, // 3,0,1,1,Primus,208,1,9,1,0,1
                1738, // 3,10,1,1,Primus,208,1,9,1,1,1
                2761, // 3,10,1,1,Primus,208,1,9,1,2,1
                127, // 3,130,1,2,Mergitur,208,5,9,1,0,1
                1739, // 3,130,1,2,Mergitur,208,5,9,1,1,1
                2762, // 3,130,1,2,Mergitur,208,5,9,1,2,1
                128, // 3,1285,1,4,Fremitu,208,16,9,1,0,1
                1740, // 3,1285,1,4,Fremitu,208,16,9,1,1,1
                2763, // 3,1285,1,4,Fremitu,208,16,9,1,2,1
                129, // 3,2210,1,5,Sinceritatis,208,21,9,1,0,1
                1741, // 3,2210,1,5,Sinceritatis,208,21,9,1,1,1
                2764, // 3,2210,1,5,Sinceritatis,208,21,9,1,2,1
                130, // 3,4810,1,7,Benedico Sole,208,31,9,1,0,1
                1742, // 3,4810,1,7,Benedico Sole,208,31,9,1,1,1
                2765, // 3,4810,1,7,Benedico Sole,208,31,9,1,2,1
                131, // 3,6485,1,8,Locus Solus,208,36,9,1,0,1
                1743, // 3,6485,1,8,Locus Solus,208,36,9,1,1,1
                2766, // 3,6485,1,8,Locus Solus,208,36,9,1,2,1
                132, // 3,610,1,3,Solitus,208,11,9,1,0,1
                1744, // 3,610,1,3,Solitus,208,11,9,1,1,1
                2767, // 3,610,1,3,Solitus,208,11,9,1,2,1
                395, // 3,8410,1,9,Callositas,208,41,9,1,0,1
                2007, // 3,8410,1,9,Callositas,208,41,9,1,1,1
                3030, // 3,8410,1,9,Callositas,208,41,9,1,2,1
                396, // 3,3385,1,6,Ferrum Gravis,208,26,9,1,0,1
                2008, // 3,3385,1,6,Ferrum Gravis,208,26,9,1,1,1
                3031, // 3,3385,1,6,Ferrum Gravis,208,26,9,1,2,1
                397, // 3,12010,1,10,Memento Mori,208,49,9,1,0,1
                2009, // 3,12010,1,10,Memento Mori,208,49,9,1,1,1
                3032, // 3,12010,1,10,Memento Mori,208,49,9,1,2,1
                398, // 3,11050,1,10,Robustum,208,47,9,1,0,1
                2010, // 3,11050,1,10,Robustum,208,47,9,1,1,1
                3033, // 3,11050,1,10,Robustum,208,47,9,1,2,1
                399, // 3,14050,1,11,Aeternitas,208,53,9,1,0,1
                2011, // 3,14050,1,11,Aeternitas,208,53,9,1,1,1
                3034, // 3,14050,1,11,Aeternitas,208,53,9,1,2,1
                401, // 3,13010,1,11,Honestatis,208,51,9,1,0,1
                2013, // 3,13010,1,11,Honestatis,208,51,9,1,1,1
                3036, // 3,13010,1,11,Honestatis,208,51,9,1,2,1
                402, // 3,15685,1,12,Bellator,208,55,9,1,0,1
                2014, // 3,15685,1,12,Bellator,208,55,9,1,1,1
                3037, // 3,15685,1,12,Bellator,208,55,9,1,2,1
            },
            [JobId.SpiritLancer] = new List<uint>()
            {
                14737, // Novice Spear,213,1,10,1,0,1
                14738, // Novice Spear,213,1,10,1,1,1
                14739, // Novice Spear,213,1,10,1,2,1
                14742,
                14743,
                14744,
                14747,
                14748,
                14749,
                14752,
                14753,
                14754,
                14757,
                14758,
                14759,
                14762,
                14763,
                14764,
            },
            [JobId.HighScepter] = new List<uint>()
            {
                20027, // 3,0,1,1,Scimitar,215,1,11,1,0,1
                20028, // 3,0,1,2,Scimitar,215,1,11,1,1,1
                20029, // 3,0,1,3,Scimitar,215,1,11,1,2,1
                20032, // イリケス,Iriketh,
                20033, // イリケス,Iriketh
                20034, // イリケス,Iriketh
                20037, // クリス,Kris
                20038, // クリス,Kris
                20039, // クリス,Kris
                20042, // マギアスソード,Magias Sword,
                20043, // マギアスソード,Magias Sword,
                20044, // マギアスソード,Magias Sword,
                20047, // フォールントゥルース,Fallen Truth,
                20048, // フォールントゥルース,Fallen Truth,
                20049, // フォールントゥルース,Fallen Truth,

            }
        };

        private static readonly Dictionary<JobId, List<uint>> gHighQualityWeapons = new Dictionary<JobId, List<uint>>()
        {
            [JobId.Fighter] = new List<uint>()
            {
                3720, // 3,8,1,1,Bronze Sword,201,1,1,1,3,1
                4743, // 3,8,1,1,Bronze Sword,201,1,1,1,4,1
                3721, // 3,104,1,2,Landing Sword,201,5,1,1,3,2
                4744, // 3,104,1,2,Landing Sword,201,5,1,1,4,2
                3722, // 3,260,1,2,Mace,201,8,1,1,3,3
                4745, // 3,260,1,2,Mace,201,8,1,1,4,3
                3723, // 3,488,1,3,Makhaira,201,11,1,1,3,1
                4746, // 3,488,1,3,Makhaira,201,11,1,1,4,1
                3724, // 3,1160,1,4,Thousand Troops,201,17,1,1,3,1
                4747, // 3,1160,1,4,Thousand Troops,201,17,1,1,4,1
                3725, // 3,788,1,3,Strong Hatchet,201,14,1,1,3,1
                4748, // 3,788,1,3,Strong Hatchet,201,14,1,1,4,1
                3726, // 3,1448,1,4,Iron Sword,201,19,1,1,3,1
                4749, // 3,1448,1,4,Iron Sword,201,19,1,1,4,1
                3792, // 3,1768,1,5,Cutlass,201,21,1,1,3,1
                4815, // 3,1768,1,5,Cutlass,201,21,1,1,4,1
                3794, // 3,2308,1,5,Bastard Sword,201,25,1,1,3,1
                4817, // 3,2308,1,5,Bastard Sword,201,25,1,1,4,1
                3795, // 3,2708,1,6,Broadsword,201,27,1,1,3,1
                4818, // 3,2708,1,6,Broadsword,201,27,1,1,4,1
                3796, // 3,3848,1,7,Tight Cinquedea,201,31,1,1,3,1
                4819, // 3,3848,1,7,Tight Cinquedea,201,31,1,1,4,1
                3797, // 3,3140,1,6,Morgenstern,201,29,1,1,3,1
                4820, // 3,3140,1,6,Morgenstern,201,29,1,1,4,1
                3798, // 3,5188,1,8,Captain's Soul,201,36,1,1,3,1
                4821, // 3,5188,1,8,Captain's Soul,201,36,1,1,4,1
                3799, // 3,5480,1,8,Bud of Destruction,201,37,1,1,3,1
                4822, // 3,5480,1,8,Bud of Destruction,201,37,1,1,4,1
                3800, // 3,4628,1,7,Falcata,201,34,1,1,3,1
                4823, // 3,4628,1,7,Falcata,201,34,1,1,4,1
                3801, // 3,8104,1,9,Thousand Stings,201,45,1,1,3,1
                4824, // 3,8104,1,9,Thousand Stings,201,45,1,1,4,1
                3802, // 3,6088,1,8,Sense of Wound,201,40,1,1,3,1
                4825, // 3,6088,1,8,Sense of Wound,201,40,1,1,4,1
                3803, // 3,4100,1,7,Biting Club,201,32,1,1,3,1
                4826, // 3,4100,1,7,Biting Club,201,32,1,1,4,1
                3804, // 3,5780,1,8,Silver Rapier,201,38,1,1,3,1
                4827, // 3,5780,1,8,Silver Rapier,201,38,1,1,4,1
                3810, // 3,8468,1,10,Brute Killer,201,47,1,1,3,1
                4833, // 3,8468,1,10,Brute Killer,201,47,1,1,4,1
                3811, // 3,12104,1,11,Dark Intention,201,55,1,1,3,1
                4834, // 3,12104,1,11,Dark Intention,201,55,1,1,4,1
                3812, // 3,9220,1,11,Francisca,201,51,1,1,3,1
                4835, // 3,9220,1,11,Francisca,201,51,1,1,4,1
                3813, // 3,8840,1,10,Ingot Club,201,49,1,1,3,1
                4836, // 3,8840,1,10,Ingot Club,201,49,1,1,4,1
                3731, // 3,4,1,1,Round Shield,202,1,1,0,3,1
                4754, // 3,4,1,1,Round Shield,202,1,1,0,4,1
                3727, // 3,74,1,2,Pelta,202,6,1,0,3,1
                4750, // 3,74,1,2,Pelta,202,6,1,0,4,1
                9936, // 3,244,1,3,Novice Shield,202,11,1,0,3,1
                9937, // 3,244,1,3,Novice Shield,202,11,1,0,4,1
                3729, // 3,514,1,4,Muscular Wall,202,16,1,0,3,1
                4752, // 3,514,1,4,Muscular Wall,202,16,1,0,4,1
                9052, // 3,1060,1,5,Shield of the White Knights,202,23,1,0,3,1
                9053, // 3,1060,1,5,Shield of the White Knights,202,23,1,0,4,1
                9946, // 3,1570,1,6,Alpine Pelta,202,28,1,0,3,1
                9947, // 3,1570,1,6,Alpine Pelta,202,28,1,0,4,1
                3823, // 3,2180,1,7,Silver Thorn,202,33,1,0,3,1
                4846, // 3,2180,1,7,Silver Thorn,202,33,1,0,4,1
                3825, // 3,2594,1,8,Mark of the Chimera,202,36,1,0,3,1
                4848, // 3,2594,1,8,Mark of the Chimera,202,36,1,0,4,1
                3827, // 3,3364,1,9,Shine Targe,202,41,1,0,3,1
                4850, // 3,3364,1,9,Shine Targe,202,41,1,0,4,1
                3828, // 3,5410,1,11,Stark Stolz,202,53,1,0,3,1
                4851, // 3,5410,1,11,Stark Stolz,202,53,1,0,4,1
                3829, // 3,4610,1,10,Boldness,202,49,1,0,3,1
                4852, // 3,4610,1,10,Boldness,202,49,1,0,4,1
                3830, // 3,6052,1,11,Shield of the Divine Protector,202,55,1,0,3,1
                4853, // 3,6052,1,11,Shield of the Divine Protector,202,55,1,0,4,1
            },
            [JobId.Hunter] = new List<uint>()
            {
                3730, // 3,10,1,1,Shortbow,207,1,3,1,3,1
                4753, // 3,10,1,1,Shortbow,207,1,3,1,4,1
                3732, // 3,130,1,2,Stalking Bow,207,5,3,1,3,1
                4755, // 3,130,1,2,Stalking Bow,207,5,3,1,4,1
                3733, // 3,610,1,3,Strain,207,11,3,1,3,1
                4756, // 3,610,1,3,Strain,207,11,3,1,4,1
                3734, // 3,325,1,2,Composite Longbow,207,8,3,1,3,1
                4757, // 3,325,1,2,Composite Longbow,207,8,3,1,4,1
                3736, // 3,1450,1,4,White Wolf Bow,207,17,3,1,3,1
                4759, // 3,1450,1,4,White Wolf Bow,207,17,3,1,4,1
                3836, // 3,2885,1,5,Fluted Bow,207,25,3,1,3,1
                4859, // 3,2885,1,5,Fluted Bow,207,25,3,1,4,1
                3838, // 3,3925,1,6,Liberty Bow,207,29,3,1,3,1
                4861, // 3,3925,1,6,Liberty Bow,207,29,3,1,4,1
                3842, // 3,5785,1,7,Lustre Acanthus,207,34,3,1,3,1
                4865, // 3,5785,1,7,Lustre Acanthus,207,34,3,1,4,1
                3840, // 3,7225,1,8,Bezel Crown,207,38,3,1,3,1
                4863, // 3,7225,1,8,Bezel Crown,207,38,3,1,4,1
                11120, // 3,8410,1,9,Nactoral Bow,207,41,3,1,3,1
                11121, // 3,8410,1,9,Nactoral Bow,207,41,3,1,4,1
                3848, // 3,10130,1,9,Scroll Hilt Bow,207,45,3,1,3,1
                4871, // 3,10130,1,9,Scroll Hilt Bow,207,45,3,1,4,1
                3854, // 3,10585,1,10,Brier Shooter,207,47,3,1,3,1
                4877, // 3,10585,1,10,Brier Shooter,207,47,3,1,4,1
                3851, // 3,11050,1,10,Ex Machina,207,49,3,1,3,1
                4874, // 3,11050,1,10,Ex Machina,207,49,3,1,4,1
                3852, // 3,11525,1,11,Elder's Anima,207,51,3,1,3,1
                4875, // 3,11525,1,11,Elder's Anima,207,51,3,1,4,1
                3856, // 3,13010,1,11,Sturdy Bow,207,53,3,1,3,1
                4879, // 3,13010,1,11,Sturdy Bow,207,53,3,1,4,1
                3858, // 3,14050,1,11,Auto-Adjust Bow,207,55,3,1,3,1
                4881, // 3,14050,1,11,Auto-Adjust Bow,207,55,3,1,4,1
            },
            [JobId.Priest] = new List<uint>()
            {
                3738, // 3,10,1,1,Wood Staff,211,1,4,1,3,1
                4761, // 3,10,1,1,Wood Staff,211,1,4,1,4,1
                3739, // 3,130,1,2,Blue Cane,211,5,4,1,3,1
                4762, // 3,130,1,2,Blue Cane,211,5,4,1,4,1
                3740, // 3,610,1,3,Hermit Wand,211,11,4,1,3,1
                4763, // 3,610,1,3,Hermit Wand,211,11,4,1,4,1
                3743, // 3,1450,1,4,Staff of Magickal Restraint,211,17,4,1,3,1
                4766, // 3,1450,1,4,Staff of Magickal Restraint,211,17,4,1,4,1
                3865, // 3,2885,1,5,Illusionist,211,25,4,1,3,1
                4888, // 3,2885,1,5,Illusionist,211,25,4,1,4,1
                3866, // 3,3385,1,6,Sky Anchor,211,27,4,1,3,1
                4889, // 3,3385,1,6,Sky Anchor,211,27,4,1,4,1
                3867, // 3,3925,1,6,Pilgrim's Cane,211,29,4,1,3,1
                4890, // 3,3925,1,6,Pilgrim's Cane,211,29,4,1,4,1
                3871, // 3,5785,1,7,Portal Indicator,211,34,4,1,3,1
                4894, // 3,5785,1,7,Portal Indicator,211,34,4,1,4,1
                3869, // 3,7225,1,8,Staff of Beyond the Rift,211,38,4,1,3,1
                4892, // 3,7225,1,8,Staff of Beyond the Rift,211,38,4,1,4,1
                11150, // 3,8410,1,9,Signposter,211,41,4,1,3,1
                11151, // 3,8410,1,9,Signposter,211,41,4,1,4,1
                3879, // 3,10130,1,9,Staff of Magickal Destruction,211,45,4,1,3,1
                4902, // 3,10130,1,9,Staff of Magickal Destruction,211,45,4,1,4,1
                3880, // 3,10585,1,10,Lela Daphne,211,47,4,1,3,1
                4903, // 3,10585,1,10,Lela Daphne,211,47,4,1,4,1
                3881, // 3,11525,1,11,Heart of Darkness,211,51,4,1,3,1
                4904, // 3,11525,1,11,Heart of Darkness,211,51,4,1,4,1
                3882, // 3,15130,1,11,Etupirka,211,55,4,1,3,1
                4905, // 3,15130,1,11,Etupirka,211,55,4,1,4,1
                3883, // 3,11050,1,10,Staff of the Pathfinder,211,49,4,1,3,1
                4906, // 3,11050,1,10,Staff of the Pathfinder,211,49,4,1,4,1
            },
            [JobId.ShieldSage] = new List<uint>()
            {
                3749, // 3,8,1,1,Large Wood,204,1,5,1,3,1
                4772, // 3,8,1,1,Large Wood,204,1,5,1,4,1
                3750, // 3,104,1,2,Oval Shield,204,5,5,1,3,1
                4773, // 3,104,1,2,Oval Shield,204,5,5,1,4,1
                3751, // 3,488,1,3,Monolith,204,11,5,1,3,1
                4774, // 3,488,1,3,Monolith,204,11,5,1,4,1
                3752, // 3,260,1,2,Battle Shield,204,8,5,1,3,1
                4775, // 3,260,1,2,Battle Shield,204,8,5,1,4,1
                3753, // 3,788,1,3,Knight's Honor,204,14,5,1,3,1
                4776, // 3,788,1,3,Knight's Honor,204,14,5,1,4,1
                3754, // 3,1448,1,4,Scutum,204,19,5,1,3,1
                4777, // 3,1448,1,4,Scutum,204,19,5,1,4,1
                3755, // 3,1160,1,4,Woody Magick,204,17,5,1,3,1
                4778, // 3,1160,1,4,Woody Magick,204,17,5,1,4,1
                3908, // 3,2308,1,5,Megalith,204,25,5,1,3,1
                4931, // 3,2308,1,5,Megalith,204,25,5,1,4,1
                3909, // 3,2708,1,6,Blue Kite,204,27,5,1,3,1
                4932, // 3,2708,1,6,Blue Kite,204,27,5,1,4,1
                3910, // 3,3140,1,6,Setzschild,204,29,5,1,3,1
                4933, // 3,3140,1,6,Setzschild,204,29,5,1,4,1
                3911, // 3,3848,1,7,Embossed Head,204,31,5,1,3,1
                4934, // 3,3848,1,7,Embossed Head,204,31,5,1,4,1
                3912, // 3,5188,1,8,Valor Glare,204,36,5,1,3,1
                4935, // 3,5188,1,8,Valor Glare,204,36,5,1,4,1
                3913, // 3,4100,1,7,Bastion,204,32,5,1,3,1
                4936, // 3,4100,1,7,Bastion,204,32,5,1,4,1
                3914, // 3,4628,1,7,Mark of the Advance,204,34,5,1,3,1
                4937, // 3,4628,1,7,Mark of the Advance,204,34,5,1,4,1
                3917, // 3,5780,1,8,Chained Sepulchre,204,38,5,1,3,1
                4940, // 3,5780,1,8,Chained Sepulchre,204,38,5,1,4,1
                11090, // 3,6728,1,9,Black Wall,204,41,5,1,3,1
                11091, // 3,6728,1,9,Black Wall,204,41,5,1,4,1
                3922, // 3,8104,1,9,Byzantine Guard,204,45,5,1,3,1
                4945, // 3,8104,1,9,Byzantine Guard,204,45,5,1,4,1
                3926, // 3,8468,1,10,Heaven's Door,204,47,5,1,3,1
                4949, // 3,8468,1,10,Heaven's Door,204,47,5,1,4,1
                3928, // 3,10408,1,11,Unbreakable,204,53,5,1,3,1
                4951, // 3,10408,1,11,Unbreakable,204,53,5,1,4,1
                3929, // 3,11240,1,11,Black Aegis,204,55,5,1,3,1
                4952, // 3,11240,1,11,Black Aegis,204,55,5,1,4,1
                3745, // 3,4,1,1,Bronze Rod,205,1,5,0,3,1
                4768, // 3,4,1,1,Bronze Rod,205,1,5,0,4,1
                3746, // 3,74,1,2,Spike Rod,205,6,5,0,3,1
                4769, // 3,74,1,2,Spike Rod,205,6,5,0,4,1
                10001, // 3,244,1,3,Strike Rod,205,11,5,0,3,1
                10002, // 3,244,1,3,Strike Rod,205,11,5,0,4,1
                3748, // 3,514,1,4,Power Rod,205,16,5,0,3,1
                4771, // 3,514,1,4,Power Rod,205,16,5,0,4,1
                9971, // 3,1060,1,5,Crowned Head,205,23,5,0,3,1
                9972, // 3,1060,1,5,Crowned Head,205,23,5,0,4,1
                9976, // 3,1570,1,6,Beacon of Mahler,205,28,5,0,3,1
                9977, // 3,1570,1,6,Beacon of Mahler,205,28,5,0,4,1
                9981, // 3,2314,1,7,Supernova,205,33,5,0,3,1
                9982, // 3,2314,1,7,Supernova,205,33,5,0,4,1
                3898, // 3,2594,1,8,Patriot Stone,205,36,5,0,3,1
                4921, // 3,2594,1,8,Patriot Stone,205,36,5,0,4,1
                10060, // 3,3364,1,9,Punishment,205,41,5,0,3,1
                10061, // 3,3364,1,9,Punishment,205,41,5,0,4,1
                10055, // 3,4052,1,9,Lethal Pile,205,45,5,0,3,1
                10056, // 3,4052,1,9,Lethal Pile,205,45,5,0,4,1
                3901, // 3,4234,1,10,Savior Borne,205,47,5,0,3,1
                4924, // 3,4234,1,10,Savior Borne,205,47,5,0,4,1
                3902, // 3,4610,1,10,Gaia Scepter,205,49,5,0,3,1
                4925, // 3,4610,1,10,Gaia Scepter,205,49,5,0,4,1
                3903, // 3,6052,1,11,Hands of Rain,205,55,5,0,3,1
                4926, // 3,6052,1,11,Hands of Rain,205,55,5,0,4,1
            },
            [JobId.Seeker] = new List<uint>()
            {
                3756, // キャヴァリーアームス,Cavalry Arms,
                4779, // キャヴァリーアームス,Cavalry Arms,
                3757, // シーフブレード,Thief's Blades,
                4780, // シーフブレード,Thief's Blades,
                3758, // ブリゼペ,Brise-épéé,
                4781, // ブリゼペ,Brise-épéé,
                3762, // グラディウス,Gladius,
                4785, // グラディウス,Gladius,
                3937, // スティレット,Stilettos,
                4960, // スティレット,Stilettos,
                3939, // ダキアエネシス,Daciaensis,
                4962, // ダキアエネシス,Daciaensis,
                3943, // トリスケーレ,Triskele,
                4966, // トリスケーレ,Triskele,
                3941, // シニストラ,Sinistra,
                4964, // シニストラ,Sinistra,
                11105, // トゥーエッジ,Two-Edged,
                11106, // トゥーエッジ,Two-Edged,
                3948, // トータルエングレイバー,Total Engravers,
                4971, // トータルエングレイバー,Total Engravers,
                3952, // プレージュドー,Prejuízo,
                4975, // プレージュドー,Prejuízo,
            },
            [JobId.Sorcerer] = new List<uint>()
            {
                3763, // 3,10,1,1,Guide Wand,212,1,6,1,3,1
                4786, // 3,10,1,1,Guide Wand,212,1,6,1,4,1
                3764, // 3,130,1,2,Ferocious Horn,212,5,6,1,3,1
                4787, // 3,130,1,2,Ferocious Horn,212,5,6,1,4,1
                3765, // 3,610,1,3,Magician's Thought,212,11,6,1,3,1
                4788, // 3,610,1,3,Magician's Thought,212,11,6,1,4,1
                3768, // 3,1450,1,4,Spiral Wand,212,17,6,1,3,1
                4791, // 3,1450,1,4,Spiral Wand,212,17,6,1,4,1
                3769, // 3,1810,1,4,Mage Wand,212,19,6,1,3,1
                4792, // 3,1810,1,4,Mage Wand,212,19,6,1,4,1
                3966, // 3,2885,1,5,Magickal Branch,212,25,6,1,3,1
                4989, // 3,2885,1,5,Magickal Branch,212,25,6,1,4,1
                3968, // 3,3925,1,6,Sorcerer's Wand,212,29,6,1,3,1
                4991, // 3,3925,1,6,Sorcerer's Wand,212,29,6,1,4,1
                3969, // 3,5785,1,7,Soul Harvest,212,34,6,1,3,1
                4992, // 3,5785,1,7,Soul Harvest,212,34,6,1,4,1
                3970, // 3,7225,1,8,Lunar Saga,212,38,6,1,3,1
                4993, // 3,7225,1,8,Lunar Saga,212,38,6,1,4,1
                11165, // 3,8410,1,9,Valiant Helix,212,41,6,1,3,1
                11166, // 3,8410,1,9,Valiant Helix,212,41,6,1,4,1
                11170, // 3,9250,1,9,Sealed Calamity,212,43,6,1,3,1
                11171, // 3,9250,1,9,Sealed Calamity,212,43,6,1,4,1
                3973, // 3,10130,1,9,Grabbed Heart,212,45,6,1,3,1
                4996, // 3,10130,1,9,Grabbed Heart,212,45,6,1,4,1
                3974, // 3,7610,1,8,Evil Eye Specimen,212,40,6,1,3,1
                4997, // 3,7610,1,8,Evil Eye Specimen,212,40,6,1,4,1
                3975, // 3,6850,1,8,Ancient Horn,212,37,6,1,3,1
                4998, // 3,6850,1,8,Ancient Horn,212,37,6,1,4,1
                3976, // 3,6485,1,8,Pious Roots,212,36,6,1,3,1
                4999, // 3,6485,1,8,Pious Roots,212,36,6,1,4,1
                3981, // 3,11525,1,11,Grace of Apollo,212,51,6,1,3,1
                5004, // 3,11525,1,11,Grace of Apollo,212,51,6,1,4,1
                3982, // 3,10585,1,10,Ruthless Enforcer,212,47,6,1,3,1
                5005, // 3,10585,1,10,Ruthless Enforcer,212,47,6,1,4,1
                3983, // 3,15130,1,11,Abyss Dweller,212,55,6,1,3,1
                5006, // 3,15130,1,11,Abyss Dweller,212,55,6,1,4,1
                3984, // 3,11050,1,10,Sign of Menace,212,49,6,1,3,1
                5007, // 3,11050,1,10,Sign of Menace,212,49,6,1,4,1
            },
            [JobId.ElementArcher] = new List<uint>()
            {
                3770, // 3,10,1,1,Magick Bow,209,1,8,1,3,1
                4793, // 3,10,1,1,Magick Bow,209,1,8,1,4,1
                3771, // 3,130,1,2,Gnosir,209,5,8,1,3,1
                4794, // 3,130,1,2,Gnosir,209,5,8,1,4,1
                3772, // 3,610,1,3,Kenn-brecher,209,11,8,1,3,1
                4795, // 3,610,1,3,Kenn-brecher,209,11,8,1,4,1
                3775, // 3,1450,1,4,Mageia Kyklos,209,17,8,1,3,1
                4798, // 3,1450,1,4,Mageia Kyklos,209,17,8,1,4,1
                3776, // 3,1810,1,4,Quellenkraft,209,19,8,1,3,1
                4799, // 3,1810,1,4,Quellenkraft,209,19,8,1,4,1
                3993, // 3,2885,1,5,Schild und Sühne,209,25,8,1,3,1
                5016, // 3,2885,1,5,Schild und Sühne,209,25,8,1,4,1
                3994, // 3,2425,1,5,Mondnacht Schütteln,209,23,8,1,3,1
                5017, // 3,2425,1,5,Mondnacht Schütteln,209,23,8,1,4,1
                3995, // 3,2210,1,5,Morgenglühen,209,21,8,1,3,1
                5018, // 3,2210,1,5,Morgenglühen,209,21,8,1,4,1
                3996, // 3,3385,1,6,Glücksrad,209,27,8,1,3,1
                5019, // 3,3385,1,6,Glücksrad,209,27,8,1,4,1
                3997, // 3,3925,1,6,Aspro Neraida,209,29,8,1,3,1
                5020, // 3,3925,1,6,Aspro Neraida,209,29,8,1,4,1
                3998, // 3,4810,1,7,Vortex Flow,209,31,8,1,3,1
                5021, // 3,4810,1,7,Vortex Flow,209,31,8,1,4,1
                3999, // 3,7610,1,8,Gunvoltzeit,209,36,8,1,3,1
                5022, // 3,7610,1,8,Gunvoltzeit,209,36,8,1,4,1
                4000, // 3,5125,1,7,Zauberkraft,209,32,8,1,3,1
                5023, // 3,5125,1,7,Zauberkraft,209,32,8,1,4,1
                4001, // 3,5785,1,7,Weiß Merkmale,209,34,8,1,3,1
                5024, // 3,5785,1,7,Weiß Merkmale,209,34,8,1,4,1
                4002, // 3,10585,1,9,Dämmerungslicht,209,45,8,1,3,1
                5025, // 3,10585,1,9,Dämmerungslicht,209,45,8,1,4,1
                4003, // 3,6485,1,8,Metempsychosis,209,40,8,1,3,1
                5026, // 3,6485,1,8,Metempsychosis,209,40,8,1,4,1
                4005, // 3,7225,1,8,Siegstraße,209,38,8,1,3,1
                5028, // 3,7225,1,8,Siegstraße,209,38,8,1,4,1
                4006, // 3,10130,1,9,Kunstraum,209,45,8,1,3,1
                5029, // 3,10130,1,9,Kunstraum,209,45,8,1,4,1
                4010, // 3,10585,1,10,Großglauben,209,47,8,1,3,1
                5033, // 3,10585,1,10,Großglauben,209,47,8,1,4,1
                4011, // 3,11050,1,10,Farsighted Eye,209,49,8,1,3,1
                5034, // 3,11050,1,10,Farsighted Eye,209,49,8,1,4,1
                4013, // 3,11525,1,11,Krifo Proseuché,209,51,8,1,3,1
                5036, // 3,11525,1,11,Krifo Proseuché,209,51,8,1,4,1
                4014, // 3,13010,1,11,Reize Hoffnung,209,53,8,1,3,1
                5037, // 3,13010,1,11,Reize Hoffnung,209,53,8,1,4,1
                4015, // 3,15130,1,11,Vredesmos,209,55,8,1,3,1
                5038, // 3,15130,1,11,Vredesmos,209,55,8,1,4,1
                4016, // 3,14050,1,11,Galant Bogen,209,55,8,1,3,1
                5039, // 3,14050,1,11,Galant Bogen,209,55,8,1,4,1
            },
            [JobId.Warrior] = new List<uint>()
            {
                3777, // 3,10,1,1,Two-Handed Sword,203,1,7,1,3,1
                4800, // 3,10,1,1,Two-Handed Sword,203,1,7,1,4,1
                3778, // 3,130,1,2,Battle Sword,203,5,7,1,3,1
                4801, // 3,130,1,2,Battle Sword,203,5,7,1,4,1
                3779, // 3,610,1,3,Claymore,203,11,7,1,3,1
                4802, // 3,610,1,3,Claymore,203,11,7,1,4,1
                3780, // 3,1810,1,4,Greatsword,203,20,7,1,3,1
                4803, // 3,1810,1,4,Greatsword,203,20,7,1,4,1
                4024, // 3,1450,1,4,Sharp Column,203,17,7,1,3,1
                5047, // 3,1450,1,4,Sharp Column,203,17,7,1,4,1
                4025, // 3,2425,1,6,Iron Urchin,203,26,7,1,3,1
                5048, // 3,2425,1,6,Iron Urchin,203,26,7,1,4,1
                4026, // 3,3385,1,7,Flamberge,203,32,7,1,3,1
                5049, // 3,3385,1,7,Flamberge,203,32,7,1,4,1
                4027, // 3,4810,1,8,Lance Sword,203,38,7,1,3,1
                5050, // 3,4810,1,8,Lance Sword,203,38,7,1,4,1
                4028, // 3,3925,1,7,Massive Hammer,203,35,7,1,3,1
                5051, // 3,3925,1,7,Massive Hammer,203,35,7,1,4,1
                4029, // 3,15685,1,12,Dissuader,203,55,7,1,3,1
                5052, // 3,15685,1,12,Dissuader,203,55,7,1,4,1
                4030, // 3,7225,1,10,Foreign Flamberge,203,47,7,1,3,1
                5053, // 3,7225,1,10,Foreign Flamberge,203,47,7,1,4,1
                4031, // 3,5785,1,9,Elfenkrieger,203,43,7,1,3,1
                5054, // 3,5785,1,9,Elfenkrieger,203,43,7,1,4,1
                4032, // 3,8410,1,11,Glyph Victory,203,53,7,1,3,1
                5055, // 3,8410,1,11,Glyph Victory,203,53,7,1,4,1
                4033, // 3,7610,1,11,D D D,203,51,7,1,3,1
                5056, // 3,7610,1,11,D D D,203,51,7,1,4,1
                4034, // 3,8005,1,8,Noblesse Oblige,203,40,7,1,3,1
                5057, // 3,8005,1,8,Noblesse Oblige,203,40,7,1,4,1
            },
            [JobId.Alchemist] = new List<uint>()
            {
                3784, // 3,10,1,1,Primus,208,1,9,1,3,1
                4807, // 3,10,1,1,Primus,208,1,9,1,4,1
                3785, // 3,130,1,2,Mergitur,208,5,9,1,3,1
                4808, // 3,130,1,2,Mergitur,208,5,9,1,4,1
                3786, // 3,1285,1,4,Fremitu,208,16,9,1,3,1
                4809, // 3,1285,1,4,Fremitu,208,16,9,1,4,1
                3787, // 3,2210,1,5,Sinceritatis,208,21,9,1,3,1
                4810, // 3,2210,1,5,Sinceritatis,208,21,9,1,4,1
                3788, // 3,4810,1,7,Benedico Sole,208,31,9,1,3,1
                4811, // 3,4810,1,7,Benedico Sole,208,31,9,1,4,1
                3789, // 3,6485,1,8,Locus Solus,208,36,9,1,3,1
                4812, // 3,6485,1,8,Locus Solus,208,36,9,1,4,1
                3790, // 3,610,1,3,Solitus,208,11,9,1,3,1
                4813, // 3,610,1,3,Solitus,208,11,9,1,4,1
                4053, // 3,8410,1,9,Callositas,208,41,9,1,3,1
                5076, // 3,8410,1,9,Callositas,208,41,9,1,4,1
                4054, // 3,3385,1,6,Ferrum Gravis,208,26,9,1,3,1
                5077, // 3,3385,1,6,Ferrum Gravis,208,26,9,1,4,1
                4055, // 3,12010,1,10,Memento Mori,208,49,9,1,3,1
                5078, // 3,12010,1,10,Memento Mori,208,49,9,1,4,1
                4056, // 3,11050,1,10,Robustum,208,47,9,1,3,1
                5079, // 3,11050,1,10,Robustum,208,47,9,1,4,1
                4057, // 3,14050,1,11,Aeternitas,208,53,9,1,3,1
                5080, // 3,14050,1,11,Aeternitas,208,53,9,1,4,1
                4059, // 3,13010,1,11,Honestatis,208,51,9,1,3,1
                5082, // 3,13010,1,11,Honestatis,208,51,9,1,4,1
                4060, // 3,15685,1,12,Bellator,208,55,9,1,3,1
                5083, // 3,15685,1,12,Bellator,208,55,9,1,4,1
            },
            [JobId.SpiritLancer] = new List<uint>()
            {
                14740,
                14741,
                14745,
                14746,
                14750,
                14751,
                14755,
                14756,
                14760,
                14761,
                14765,
                14766,
            },
            [JobId.HighScepter] = new List<uint>()
            {
                20030, // 3,0,1,4,Scimitar,215,1,11,1,3,1
                20031, // 3,0,1,5,Scimitar,215,1,11,1,4,1
                20035,
                20036,
                20040,
                20041,
                20045, // マギアスソード,Magias Sword,
                20046, // マギアスソード,Magias Sword,
                20050, // フォールントゥルース,Fallen Truth,
                20051, // フォールントゥルース,Fallen Truth,
            }
        };

        private static readonly List<uint> gLowQualityCapes = new List<uint>()
        {
            486, // 3,0,1,7,Benefit Muffler,309,1,0,0,0,1
            2098, // 3,0,1,7,Benefit Muffler,309,1,0,0,1,1
            3121, // 3,0,1,7,Benefit Muffler,309,1,0,0,2,1
            511, // 3,40,1,1,Leather Cloak,309,1,0,0,0,1
            2123, // 3,40,1,1,Leather Cloak,309,1,0,0,1,1
            3146, // 3,40,1,1,Leather Cloak,309,1,0,0,2,1
            512, // 3,60,1,1,Shed Cloak,309,3,0,0,0,1
            2124, // 3,60,1,1,Shed Cloak,309,3,0,0,1,1
            3147, // 3,60,1,1,Shed Cloak,309,3,0,0,2,1
            513, // 3,105,1,2,Scholar Cloak,309,6,0,0,0,1
            2125, // 3,105,1,2,Scholar Cloak,309,6,0,0,1,1
            3148, // 3,105,1,2,Scholar Cloak,309,6,0,0,2,1
            514, // 3,115,1,2,Brown Cape,309,7,0,0,0,1
            2126, // 3,115,1,2,Brown Cape,309,7,0,0,1,1
            3149, // 3,115,1,2,Brown Cape,309,7,0,0,2,1
            515, // 3,165,1,3,Knight's Cape,309,11,0,0,0,1
            2127, // 3,165,1,3,Knight's Cape,309,11,0,0,1,1
            3150, // 3,165,1,3,Knight's Cape,309,11,0,0,2,1
            1009, // 3,250,1,4,Pharmacy Mantle,309,16,0,0,0,1
            2621, // 3,250,1,4,Pharmacy Mantle,309,16,0,0,1,1
            3644, // 3,250,1,4,Pharmacy Mantle,309,16,0,0,2,1
            1010, // 3,400,1,5,Gryphus Mantle,309,21,0,0,0,1
            2622, // 3,400,1,5,Gryphus Mantle,309,21,0,0,1,1
            3645, // 3,400,1,5,Gryphus Mantle,309,21,0,0,2,1
            1011, // 3,1150,1,8,White Wings Mantle,309,40,0,0,0,1
            2623, // 3,1150,1,8,White Wings Mantle,309,40,0,0,1,1
            3646, // 3,1150,1,8,White Wings Mantle,309,40,0,0,2,1
            1012, // 3,600,1,6,Wiseman Cloak,309,26,0,0,0,1
            2624, // 3,600,1,6,Wiseman Cloak,309,26,0,0,1,1
            3647, // 3,600,1,6,Wiseman Cloak,309,26,0,0,2,1
            1013, // 3,850,1,7,Pauldron,309,31,0,0,0,1
            2625, // 3,850,1,7,Pauldron,309,31,0,0,1,1
            3648, // 3,850,1,7,Pauldron,309,31,0,0,2,1
            1014, // 3,600,1,6,Eagle Mantle,309,30,0,0,0,1
            2626, // 3,600,1,6,Eagle Mantle,309,30,0,0,1,1
            3649, // 3,600,1,6,Eagle Mantle,309,30,0,0,2,1
            1015, // 3,850,1,7,Survivor's Cape,309,32,0,0,0,1
            2627, // 3,850,1,7,Survivor's Cape,309,32,0,0,1,1
            3650, // 3,850,1,7,Survivor's Cape,309,32,0,0,2,1
            1016, // 3,400,1,5,Cloak of Nostalgia,309,25,0,0,0,1
            2628, // 3,400,1,5,Cloak of Nostalgia,309,25,0,0,1,1
            3651, // 3,400,1,5,Cloak of Nostalgia,309,25,0,0,2,1
            1017, // 3,600,1,6,Sergeant's Cloak,309,28,0,0,0,1
            2629, // 3,600,1,6,Sergeant's Cloak,309,28,0,0,1,1
            3652, // 3,600,1,6,Sergeant's Cloak,309,28,0,0,2,1
            1018, // 3,1900,1,10,White Dragon's Mantle,309,50,0,0,0,1
            2630, // 3,1900,1,10,White Dragon's Mantle,309,50,0,0,1,1
            3653, // 3,1900,1,10,White Dragon's Mantle,309,50,0,0,2,1
            1019, // 3,850,1,7,Saudade Muffler,309,34,0,0,0,1
            2631, // 3,850,1,7,Saudade Muffler,309,34,0,0,1,1
            3654, // 3,850,1,7,Saudade Muffler,309,34,0,0,2,1
            1020, // 3,850,1,7,Cavalier's Cloak,309,35,0,0,0,1
            2632, // 3,850,1,7,Cavalier's Cloak,309,35,0,0,1,1
            3655, // 3,850,1,7,Cavalier's Cloak,309,35,0,0,2,1
            1021, // 3,1500,1,9,Order Cape,309,41,0,0,0,1
            2633, // 3,1500,1,9,Order Cape,309,41,0,0,1,1
            3656, // 3,1500,1,9,Order Cape,309,41,0,0,2,1
            1022, // 3,1500,1,9,Arbitrator's Mantle,309,42,0,0,0,1
            2634, // 3,1500,1,9,Arbitrator's Mantle,309,42,0,0,1,1
            3657, // 3,1500,1,9,Arbitrator's Mantle,309,42,0,0,2,1
        };

        private static readonly List<uint> gHighQualityCapes = new List<uint>()
        {
            4144, // 3,0,1,7,Benefit Muffler,309,1,0,0,3,1
            5167, // 3,0,1,7,Benefit Muffler,309,1,0,0,4,1
            4169, // 3,40,1,1,Leather Cloak,309,1,0,0,3,1
            5192, // 3,40,1,1,Leather Cloak,309,1,0,0,4,1
            4170, // 3,60,1,1,Shed Cloak,309,3,0,0,3,1
            5193, // 3,60,1,1,Shed Cloak,309,3,0,0,4,1
            4171, // 3,105,1,2,Scholar Cloak,309,6,0,0,3,1
            5194, // 3,105,1,2,Scholar Cloak,309,6,0,0,4,1
            4172, // 3,115,1,2,Brown Cape,309,7,0,0,3,1
            5195, // 3,115,1,2,Brown Cape,309,7,0,0,4,1
            4173, // 3,165,1,3,Knight's Cape,309,11,0,0,3,1
            5196, // 3,165,1,3,Knight's Cape,309,11,0,0,4,1
            4667, // 3,250,1,4,Pharmacy Mantle,309,16,0,0,3,1
            5690, // 3,250,1,4,Pharmacy Mantle,309,16,0,0,4,1
            4668, // 3,400,1,5,Gryphus Mantle,309,21,0,0,3,1
            5691, // 3,400,1,5,Gryphus Mantle,309,21,0,0,4,1
            4669, // 3,1150,1,8,White Wings Mantle,309,40,0,0,3,1
            5692, // 3,1150,1,8,White Wings Mantle,309,40,0,0,4,1
            4670, // 3,600,1,6,Wiseman Cloak,309,26,0,0,3,1
            5693, // 3,600,1,6,Wiseman Cloak,309,26,0,0,4,1
            4671, // 3,850,1,7,Pauldron,309,31,0,0,3,1
            5694, // 3,850,1,7,Pauldron,309,31,0,0,4,1
            4672, // 3,600,1,6,Eagle Mantle,309,30,0,0,3,1
            5695, // 3,600,1,6,Eagle Mantle,309,30,0,0,4,1
            4673, // 3,850,1,7,Survivor's Cape,309,32,0,0,3,1
            5696, // 3,850,1,7,Survivor's Cape,309,32,0,0,4,1
            4674, // 3,400,1,5,Cloak of Nostalgia,309,25,0,0,3,1
            5697, // 3,400,1,5,Cloak of Nostalgia,309,25,0,0,4,1
            4675, // 3,600,1,6,Sergeant's Cloak,309,28,0,0,3,1
            5698, // 3,600,1,6,Sergeant's Cloak,309,28,0,0,4,1
            4676, // 3,1900,1,10,White Dragon's Mantle,309,50,0,0,3,1
            5699, // 3,1900,1,10,White Dragon's Mantle,309,50,0,0,4,1
            4677, // 3,850,1,7,Saudade Muffler,309,34,0,0,3,1
            5700, // 3,850,1,7,Saudade Muffler,309,34,0,0,4,1
            4678, // 3,850,1,7,Cavalier's Cloak,309,35,0,0,3,1
            5701, // 3,850,1,7,Cavalier's Cloak,309,35,0,0,4,1
            4679, // 3,1500,1,9,Order Cape,309,41,0,0,3,1
            5702, // 3,1500,1,9,Order Cape,309,41,0,0,4,1
            4680, // 3,1500,1,9,Arbitrator's Mantle,309,42,0,0,3,1
            5703, // 3,1500,1,9,Arbitrator's Mantle,309,42,0,0,4,1
        };

        // Hunter/Seeker/Mystic Spear
        private static readonly List<uint> LowQualityHunterSeekerSpiritArmor = new List<uint>()
        {
            490, // 3,0,1,1,Supply Vest,304,1,0,1,0,1
            2102, // 3,30,1,1,Supply Vest,304,1,0,1,1,1
            3125, // 3,30,1,1,Supply Vest,304,1,0,1,2,1
            493, // 3,0,1,1,Supply Bottoms,307,1,0,0,0,1
            2105, // 3,30,1,1,Supply Bottoms,307,1,0,0,1,1
            3128, // 3,30,1,1,Supply Bottoms,307,1,0,0,2,1
            438, // 3,30,1,1,Leather Cap,303,3,14,0,0,1
            2050, // 3,30,1,1,Leather Cap,303,3,14,0,1,1
            3073, // 3,30,1,1,Leather Cap,303,3,14,0,2,1
            439, // 3,60,1,2,Leather Hood,303,6,14,0,0,1
            2051, // 3,60,1,2,Leather Hood,303,6,14,0,1,1
            3074, // 3,60,1,2,Leather Hood,303,6,14,0,2,1
            8198, // 3,195,1,3,Plate Coif,303,11,14,0,0,1
            8199, // 3,195,1,3,Plate Coif,303,11,14,0,1,1
            8200, // 3,195,1,3,Plate Coif,303,11,14,0,2,1
            440, // 3,270,1,3,Traveler's Hood,303,13,14,0,0,1
            2052, // 3,270,1,3,Traveler's Hood,303,13,14,0,1,1
            3075, // 3,270,1,3,Traveler's Hood,303,13,14,0,2,1
            441, // 3,410,1,4,Red Leather Cap,303,16,14,0,0,1
            2053, // 3,410,1,4,Red Leather Cap,303,16,14,0,1,1
            3076, // 3,410,1,4,Red Leather Cap,303,16,14,0,2,1
            625, // 3,775,1,5,Assassin's Mask,303,22,14,0,0,1
            2237, // 3,775,1,5,Assassin's Mask,303,22,14,0,1,1
            3260, // 3,775,1,5,Assassin's Mask,303,22,14,0,2,1
            626, // 3,920,1,5,Wanderer's Hood,303,24,14,0,0,1
            2238, // 3,920,1,5,Wanderer's Hood,303,24,14,0,1,1
            3261, // 3,920,1,5,Wanderer's Hood,303,24,14,0,2,1
            627, // 3,2560,1,8,Drachenatem,303,40,14,0,0,1
            2239, // 3,2560,1,8,Drachenatem,303,40,14,0,1,1
            3262, // 3,2560,1,8,Drachenatem,303,40,14,0,2,1
            628, // 3,1080,1,6,Over Guard,303,26,14,0,0,1
            2240, // 3,1080,1,6,Over Guard,303,26,14,0,1,1
            3263, // 3,1080,1,6,Over Guard,303,26,14,0,2,1
            629, // 3,1255,1,6,Enamel Hood,303,28,14,0,0,1
            2241, // 3,1255,1,6,Enamel Hood,303,28,14,0,1,1
            3264, // 3,1255,1,6,Enamel Hood,303,28,14,0,2,1
            630, // 3,1440,1,6,Huntsman Cap,303,30,14,0,0,1
            2242, // 3,1440,1,6,Huntsman Cap,303,30,14,0,1,1
            3265, // 3,1440,1,6,Huntsman Cap,303,30,14,0,2,1
            631, // 3,1640,1,7,Vagrant's Mask,303,32,14,0,0,1
            2243, // 3,1640,1,7,Vagrant's Mask,303,32,14,0,1,1
            3266, // 3,1640,1,7,Vagrant's Mask,303,32,14,0,2,1
            632, // 3,1850,1,7,Infantry Helm,303,34,14,0,0,1
            2244, // 3,1850,1,7,Infantry Helm,303,34,14,0,1,1
            3267, // 3,1850,1,7,Infantry Helm,303,34,14,0,2,1
            633, // 3,2075,1,8,Beast Coif,303,36,14,0,0,1
            2245, // 3,2075,1,8,Beast Coif,303,36,14,0,1,1
            3268, // 3,2075,1,8,Beast Coif,303,36,14,0,2,1
            634, // 3,2310,1,8,Pirate's Cap,303,38,14,0,0,1
            2246, // 3,2310,1,8,Pirate's Cap,303,38,14,0,1,1
            3269, // 3,2310,1,8,Pirate's Cap,303,38,14,0,2,1
            635, // 3,2960,1,9,Shooter's Hood,303,45,14,0,0,1
            2247, // 3,2960,1,9,Shooter's Hood,303,45,14,0,1,1
            3270, // 3,2960,1,9,Shooter's Hood,303,45,14,0,2,1
            636, // 3,2820,1,9,Capture Hood,303,43,14,0,0,1
            2248, // 3,2820,1,9,Capture Hood,303,43,14,0,1,1
            3271, // 3,2820,1,9,Capture Hood,303,43,14,0,2,1
            637, // 3,2690,1,9,Officer's Cap,303,41,14,0,0,1
            2249, // 3,2690,1,9,Officer's Cap,303,41,14,0,1,1
            3272, // 3,2690,1,9,Officer's Cap,303,41,14,0,2,1
            639, // 3,3385,1,10,Aloni Headgear,303,46,14,0,0,1
            2251, // 3,3385,1,10,Aloni Headgear,303,46,14,0,1,1
            3274, // 3,3385,1,10,Aloni Headgear,303,46,14,0,2,1
            640, // 3,4840,1,11,Black Bart's Cap,303,55,14,0,0,1
            2252, // 3,4840,1,11,Black Bart's Cap,303,55,14,0,1,1
            3275, // 3,4840,1,11,Black Bart's Cap,303,55,14,0,2,1
            641, // 3,3535,1,10,Ruby Brigade Cap,303,49,14,0,0,1
            2253, // 3,3535,1,10,Ruby Brigade Cap,303,49,14,0,1,1
            3276, // 3,3535,1,10,Ruby Brigade Cap,303,49,14,0,2,1
            442, // 3,52,1,1,Breast Armor,304,3,14,1,0,1
            2054, // 3,52,1,1,Breast Armor,304,3,14,1,1,1
            3077, // 3,52,1,1,Breast Armor,304,3,14,1,2,1
            443, // 3,105,1,2,Copper Lorica,304,6,14,1,0,1
            2055, // 3,105,1,2,Copper Lorica,304,6,14,1,1,1
            3078, // 3,105,1,2,Copper Lorica,304,6,14,1,2,1
            444, // 3,475,1,3,Leather Armor,304,13,14,1,0,1
            2056, // 3,475,1,3,Leather Armor,304,13,14,1,1,1
            3079, // 3,475,1,3,Leather Armor,304,13,14,1,2,1
            445, // 3,715,1,4,Lamellar Jacket,304,16,14,1,0,1
            2057, // 3,715,1,4,Lamellar Jacket,304,16,14,1,1,1
            3080, // 3,715,1,4,Lamellar Jacket,304,16,14,1,2,1
            651, // 3,1355,1,5,Hunter Jacket,304,22,14,1,0,1
            2263, // 3,1355,1,5,Hunter Jacket,304,22,14,1,1,1
            3286, // 3,1355,1,5,Hunter Jacket,304,22,14,1,2,1
            652, // 3,1615,1,5,Wanderer's Jacket,304,24,14,1,0,1
            2264, // 3,1615,1,5,Wanderer's Jacket,304,24,14,1,1,1
            3287, // 3,1615,1,5,Wanderer's Jacket,304,24,14,1,2,1
            654, // 3,1895,1,6,Silver Lorica,304,26,14,1,0,1
            2266, // 3,1895,1,6,Silver Lorica,304,26,14,1,1,1
            3289, // 3,1895,1,6,Silver Lorica,304,26,14,1,2,1
            655, // 3,2195,1,6,Combat Guard,304,28,14,1,0,1
            2267, // 3,2195,1,6,Combat Guard,304,28,14,1,1,1
            3290, // 3,2195,1,6,Combat Guard,304,28,14,1,2,1
            656, // 3,2520,1,6,Huntsman Jacket,304,30,14,1,0,1
            2268, // 3,2520,1,6,Huntsman Jacket,304,30,14,1,1,1
            3291, // 3,2520,1,6,Huntsman Jacket,304,30,14,1,2,1
            657, // 3,2870,1,7,Vagrant Armor,304,32,14,1,0,1
            2269, // 3,2870,1,7,Vagrant Armor,304,32,14,1,1,1
            3292, // 3,2870,1,7,Vagrant Armor,304,32,14,1,2,1
            658, // 3,3235,1,7,Vagabond Armor,304,34,14,1,0,1
            2270, // 3,3235,1,7,Vagabond Armor,304,34,14,1,1,1
            3293, // 3,3235,1,7,Vagabond Armor,304,34,14,1,2,1
            659, // 3,3630,1,8,Beast Padding,304,36,14,1,0,1
            2271, // 3,3630,1,8,Beast Padding,304,36,14,1,1,1
            3294, // 3,3630,1,8,Beast Padding,304,36,14,1,2,1
            660, // 3,4045,1,8,Pirate's Jacket,304,38,14,1,0,1
            2272, // 3,4045,1,8,Pirate's Jacket,304,38,14,1,1,1
            3295, // 3,4045,1,8,Pirate's Jacket,304,38,14,1,2,1
            661, // 3,4705,1,9,Scale Jacket,304,41,14,1,0,1
            2273, // 3,4705,1,9,Scale Jacket,304,41,14,1,1,1
            3296, // 3,4705,1,9,Scale Jacket,304,41,14,1,2,1
            662, // 3,4940,1,9,Raider's Jacket,304,43,14,1,0,1
            2274, // 3,4940,1,9,Raider's Jacket,304,43,14,1,1,1
            3297, // 3,4940,1,9,Raider's Jacket,304,43,14,1,2,1
            663, // 3,5180,1,9,Hard Skin Armor,304,45,14,1,0,1
            2275, // 3,5180,1,9,Hard Skin Armor,304,45,14,1,1,1
            3298, // 3,5180,1,9,Hard Skin Armor,304,45,14,1,2,1
            665, // 3,5925,1,10,Spirit Guard,304,46,14,1,0,1
            2277, // 3,5925,1,10,Spirit Guard,304,46,14,1,1,1
            3300, // 3,5925,1,10,Spirit Guard,304,46,14,1,2,1
            666, // 3,8470,1,11,Black Bart's Jacket,304,55,14,1,0,1
            2278, // 3,8470,1,11,Black Bart's Jacket,304,55,14,1,1,1
            3301, // 3,8470,1,11,Black Bart's Jacket,304,55,14,1,2,1
            667, // 3,6185,1,10,Dandy's Jacket,304,49,14,1,0,1
            2279, // 3,6185,1,10,Dandy's Jacket,304,49,14,1,1,1
            3302, // 3,6185,1,10,Dandy's Jacket,304,49,14,1,2,1
            450, // 3,30,1,1,Worker Gloves,306,3,14,0,0,1
            2062, // 3,30,1,1,Worker Gloves,306,3,14,0,1,1
            3085, // 3,30,1,1,Worker Gloves,306,3,14,0,2,1
            451, // 3,60,1,2,Bronze Bracers,306,6,14,0,0,1
            2063, // 3,60,1,2,Bronze Bracers,306,6,14,0,1,1
            3086, // 3,60,1,2,Bronze Bracers,306,6,14,0,2,1
            452, // 3,270,1,3,Archer's Gloves,306,13,14,0,0,1
            2064, // 3,270,1,3,Archer's Gloves,306,13,14,0,1,1
            3087, // 3,270,1,3,Archer's Gloves,306,13,14,0,2,1
            453, // 3,410,1,4,Laborer Gloves,306,16,14,0,0,1
            2065, // 3,410,1,4,Laborer Gloves,306,16,14,0,1,1
            3088, // 3,410,1,4,Laborer Gloves,306,16,14,0,2,1
            705, // 3,775,1,5,Composite Sleeves,306,22,14,0,0,1
            2317, // 3,775,1,5,Composite Sleeves,306,22,14,0,1,1
            3340, // 3,775,1,5,Composite Sleeves,306,22,14,0,2,1
            706, // 3,920,1,5,Striker Arms,306,24,14,0,0,1
            2318, // 3,920,1,5,Striker Arms,306,24,14,0,1,1
            3341, // 3,920,1,5,Striker Arms,306,24,14,0,2,1
            708, // 3,1080,1,6,Victory Bracers,306,26,14,0,0,1
            2320, // 3,1080,1,6,Victory Bracers,306,26,14,0,1,1
            3343, // 3,1080,1,6,Victory Bracers,306,26,14,0,2,1
            709, // 3,1255,1,6,Ace Bracers,306,28,14,0,0,1
            2321, // 3,1255,1,6,Ace Bracers,306,28,14,0,1,1
            3344, // 3,1255,1,6,Ace Bracers,306,28,14,0,2,1
            710, // 3,1440,1,6,Huntsman Gloves,306,30,14,0,0,1
            2322, // 3,1440,1,6,Huntsman Gloves,306,30,14,0,1,1
            3345, // 3,1440,1,6,Huntsman Gloves,306,30,14,0,2,1
            711, // 3,1640,1,7,Soldier Arms,306,32,14,0,0,1
            2323, // 3,1640,1,7,Soldier Arms,306,32,14,0,1,1
            3346, // 3,1640,1,7,Soldier Arms,306,32,14,0,2,1
            712, // 3,1850,1,7,Iron Bracers,306,34,14,0,0,1
            2324, // 3,1850,1,7,Iron Bracers,306,34,14,0,1,1
            3347, // 3,1850,1,7,Iron Bracers,306,34,14,0,2,1
            713, // 3,2075,1,8,Silver Manicae,306,36,14,0,0,1
            2325, // 3,2075,1,8,Silver Manicae,306,36,14,0,1,1
            3348, // 3,2075,1,8,Silver Manicae,306,36,14,0,2,1
            714, // 3,2310,1,8,Pirate Arms,306,38,14,0,0,1
            2326, // 3,2310,1,8,Pirate Arms,306,38,14,0,1,1
            3349, // 3,2310,1,8,Pirate Arms,306,38,14,0,2,1
            715, // 3,2690,1,9,Agent Sleeves,306,41,14,0,0,1
            2327, // 3,2690,1,9,Agent Sleeves,306,41,14,0,1,1
            3350, // 3,2690,1,9,Agent Sleeves,306,41,14,0,2,1
            716, // 3,2820,1,9,Assault Spine,306,43,14,0,0,1
            2328, // 3,2820,1,9,Assault Spine,306,43,14,0,1,1
            3351, // 3,2820,1,9,Assault Spine,306,43,14,0,2,1
            717, // 3,2960,1,9,Leader Gloves,306,45,14,0,0,1
            2329, // 3,2960,1,9,Leader Gloves,306,45,14,0,1,1
            3352, // 3,2960,1,9,Leader Gloves,306,45,14,0,2,1
            719, // 3,3385,1,10,Executor Arms,306,46,14,0,0,1
            2331, // 3,3385,1,10,Executor Arms,306,46,14,0,1,1
            3354, // 3,3385,1,10,Executor Arms,306,46,14,0,2,1
            720, // 3,4840,1,11,Black Bart's Arms,306,55,14,0,0,1
            2332, // 3,4840,1,11,Black Bart's Arms,306,55,14,0,1,1
            3355, // 3,4840,1,11,Black Bart's Arms,306,55,14,0,2,1
            721, // 3,3535,1,10,Captain's Sleeves,306,49,14,0,0,1
            2333, // 3,3535,1,10,Captain's Sleeves,306,49,14,0,1,1
            3356, // 3,3535,1,10,Captain's Sleeves,306,49,14,0,2,1
            446, // 3,37,1,1,Leather Bottoms,307,3,14,0,0,1
            2058, // 3,37,1,1,Leather Bottoms,307,3,14,0,1,1
            3081, // 3,37,1,1,Leather Bottoms,307,3,14,0,2,1
            447, // 3,75,1,2,Chainmail Legs,307,6,14,0,0,1
            2059, // 3,75,1,2,Chainmail Legs,307,6,14,0,1,1
            3082, // 3,75,1,2,Chainmail Legs,307,6,14,0,2,1
            448, // 3,340,1,3,Smart Boots,307,13,14,0,0,1
            2060, // 3,340,1,3,Smart Boots,307,13,14,0,1,1
            3083, // 3,340,1,3,Smart Boots,307,13,14,0,2,1
            449, // 3,510,1,4,Composite Bottoms,307,16,14,0,0,1
            2061, // 3,510,1,4,Composite Bottoms,307,16,14,0,1,1
            3084, // 3,510,1,4,Composite Bottoms,307,16,14,0,2,1
            680, // 3,970,1,5,Assassin's Bottoms,307,22,14,0,0,1
            2292, // 3,970,1,5,Assassin's Bottoms,307,22,14,0,1,1
            3315, // 3,970,1,5,Assassin's Bottoms,307,22,14,0,2,1
            681, // 3,1150,1,5,Striker's Greaves,307,24,14,0,0,1
            2293, // 3,1150,1,5,Striker's Greaves,307,24,14,0,1,1
            3316, // 3,1150,1,5,Striker's Greaves,307,24,14,0,2,1
            683, // 3,1350,1,6,Victory Legs,307,26,14,0,0,1
            2295, // 3,1350,1,6,Victory Legs,307,26,14,0,1,1
            3318, // 3,1350,1,6,Victory Legs,307,26,14,0,2,1
            684, // 3,1570,1,6,Nature Cuisses,307,28,14,0,0,1
            2296, // 3,1570,1,6,Nature Cuisses,307,28,14,0,1,1
            3319, // 3,1570,1,6,Nature Cuisses,307,28,14,0,2,1
            685, // 3,1800,1,6,Huntsman's Cuisses,307,30,14,0,0,1
            2297, // 3,1800,1,6,Huntsman's Cuisses,307,30,14,0,1,1
            3320, // 3,1800,1,6,Huntsman's Cuisses,307,30,14,0,2,1
            686, // 3,2050,1,7,Soldier's Greaves,307,32,14,0,0,1
            2298, // 3,2050,1,7,Soldier's Greaves,307,32,14,0,1,1
            3321, // 3,2050,1,7,Soldier's Greaves,307,32,14,0,2,1
            687, // 3,2310,1,7,Ninja Bottoms,307,34,14,0,0,1
            2299, // 3,2310,1,7,Ninja Bottoms,307,34,14,0,1,1
            3322, // 3,2310,1,7,Ninja Bottoms,307,34,14,0,2,1
            688, // 3,2590,1,8,Extra Bottoms,307,36,14,0,0,1
            2300, // 3,2590,1,8,Extra Bottoms,307,36,14,0,1,1
            3323, // 3,2590,1,8,Extra Bottoms,307,36,14,0,2,1
            689, // 3,2890,1,8,Pirate's Legs,307,38,14,0,0,1
            2301, // 3,2890,1,8,Pirate's Legs,307,38,14,0,1,1
            3324, // 3,2890,1,8,Pirate's Legs,307,38,14,0,2,1
            690, // 3,3360,1,9,Backwater Waistguard,307,41,14,0,0,1
            2302, // 3,3360,1,9,Backwater Waistguard,307,41,14,0,1,1
            3325, // 3,3360,1,9,Backwater Waistguard,307,41,14,0,2,1
            691, // 3,3530,1,9,Raider's Greaves,307,43,14,0,0,1
            2303, // 3,3530,1,9,Raider's Greaves,307,43,14,0,1,1
            3326, // 3,3530,1,9,Raider's Greaves,307,43,14,0,2,1
            692, // 3,3700,1,9,Leader Leg Guards,307,45,14,0,0,1
            2304, // 3,3700,1,9,Leader Leg Guards,307,45,14,0,1,1
            3327, // 3,3700,1,9,Leader Leg Guards,307,45,14,0,2,1
            694, // 3,4230,1,10,Fur Cuisses,307,46,14,0,0,1
            2306, // 3,4230,1,10,Fur Cuisses,307,46,14,0,1,1
            3329, // 3,4230,1,10,Fur Cuisses,307,46,14,0,2,1
            695, // 3,6050,1,11,Black Bart's Legs,307,55,14,0,0,1
            2307, // 3,6050,1,11,Black Bart's Legs,307,55,14,0,1,1
            3330, // 3,6050,1,11,Black Bart's Legs,307,55,14,0,2,1
            696, // 3,4420,1,10,Slayer's Bottoms,307,49,14,0,0,1
            2308, // 3,4420,1,10,Slayer's Bottoms,307,49,14,0,1,1
            3331, // 3,4420,1,10,Slayer's Bottoms,307,49,14,0,2,1
        };

        private static readonly List<uint> HighQualityHunterSeekerSpiritArmor = new List<uint>()
        {
            4148, // 3,30,1,1,Supply Vest,304,1,0,1,3,1
            5171, // 3,30,1,1,Supply Vest,304,1,0,1,4,1
            4151, // 3,30,1,1,Supply Bottoms,307,1,0,0,3,1
            5174, // 3,30,1,1,Supply Bottoms,307,1,0,0,4,1
            4096, // 3,30,1,1,Leather Cap,303,3,14,0,3,1
            5119, // 3,30,1,1,Leather Cap,303,3,14,0,4,1
            4097, // 3,60,1,2,Leather Hood,303,6,14,0,3,1
            5120, // 3,60,1,2,Leather Hood,303,6,14,0,4,1
            8201, // 3,195,1,3,Plate Coif,303,11,14,0,3,1
            8202, // 3,195,1,3,Plate Coif,303,11,14,0,4,1
            4098, // 3,270,1,3,Traveler's Hood,303,13,14,0,3,1
            5121, // 3,270,1,3,Traveler's Hood,303,13,14,0,4,1
            4099, // 3,410,1,4,Red Leather Cap,303,16,14,0,3,1
            5122, // 3,410,1,4,Red Leather Cap,303,16,14,0,4,1
            4283, // 3,775,1,5,Assassin's Mask,303,22,14,0,3,1
            5306, // 3,775,1,5,Assassin's Mask,303,22,14,0,4,1
            4284, // 3,920,1,5,Wanderer's Hood,303,24,14,0,3,1
            5307, // 3,920,1,5,Wanderer's Hood,303,24,14,0,4,1
            4285, // 3,2560,1,8,Drachenatem,303,40,14,0,3,1
            5308, // 3,2560,1,8,Drachenatem,303,40,14,0,4,1
            4286, // 3,1080,1,6,Over Guard,303,26,14,0,3,1
            5309, // 3,1080,1,6,Over Guard,303,26,14,0,4,1
            4287, // 3,1255,1,6,Enamel Hood,303,28,14,0,3,1
            5310, // 3,1255,1,6,Enamel Hood,303,28,14,0,4,1
            4288, // 3,1440,1,6,Huntsman Cap,303,30,14,0,3,1
            5311, // 3,1440,1,6,Huntsman Cap,303,30,14,0,4,1
            4289, // 3,1640,1,7,Vagrant's Mask,303,32,14,0,3,1
            5312, // 3,1640,1,7,Vagrant's Mask,303,32,14,0,4,1
            4290, // 3,1850,1,7,Infantry Helm,303,34,14,0,3,1
            5313, // 3,1850,1,7,Infantry Helm,303,34,14,0,4,1
            4291, // 3,2075,1,8,Beast Coif,303,36,14,0,3,1
            5314, // 3,2075,1,8,Beast Coif,303,36,14,0,4,1
            4292, // 3,2310,1,8,Pirate's Cap,303,38,14,0,3,1
            5315, // 3,2310,1,8,Pirate's Cap,303,38,14,0,4,1
            4293, // 3,2960,1,9,Shooter's Hood,303,45,14,0,3,1
            5316, // 3,2960,1,9,Shooter's Hood,303,45,14,0,4,1
            4294, // 3,2820,1,9,Capture Hood,303,43,14,0,3,1
            5317, // 3,2820,1,9,Capture Hood,303,43,14,0,4,1
            4295, // 3,2690,1,9,Officer's Cap,303,41,14,0,3,1
            5318, // 3,2690,1,9,Officer's Cap,303,41,14,0,4,1
            4297, // 3,3385,1,10,Aloni Headgear,303,46,14,0,3,1
            5320, // 3,3385,1,10,Aloni Headgear,303,46,14,0,4,1
            4298, // 3,4840,1,11,Black Bart's Cap,303,55,14,0,3,1
            5321, // 3,4840,1,11,Black Bart's Cap,303,55,14,0,4,1
            4299, // 3,3535,1,10,Ruby Brigade Cap,303,49,14,0,3,1
            5322, // 3,3535,1,10,Ruby Brigade Cap,303,49,14,0,4,1
            4100, // 3,52,1,1,Breast Armor,304,3,14,1,3,1
            5123, // 3,52,1,1,Breast Armor,304,3,14,1,4,1
            4101, // 3,105,1,2,Copper Lorica,304,6,14,1,3,1
            5124, // 3,105,1,2,Copper Lorica,304,6,14,1,4,1
            4102, // 3,475,1,3,Leather Armor,304,13,14,1,3,1
            5125, // 3,475,1,3,Leather Armor,304,13,14,1,4,1
            4103, // 3,715,1,4,Lamellar Jacket,304,16,14,1,3,1
            5126, // 3,715,1,4,Lamellar Jacket,304,16,14,1,4,1
            4309, // 3,1355,1,5,Hunter Jacket,304,22,14,1,3,1
            5332, // 3,1355,1,5,Hunter Jacket,304,22,14,1,4,1
            4310, // 3,1615,1,5,Wanderer's Jacket,304,24,14,1,3,1
            5333, // 3,1615,1,5,Wanderer's Jacket,304,24,14,1,4,1
            4312, // 3,1895,1,6,Silver Lorica,304,26,14,1,3,1
            5335, // 3,1895,1,6,Silver Lorica,304,26,14,1,4,1
            4313, // 3,2195,1,6,Combat Guard,304,28,14,1,3,1
            5336, // 3,2195,1,6,Combat Guard,304,28,14,1,4,1
            4314, // 3,2520,1,6,Huntsman Jacket,304,30,14,1,3,1
            5337, // 3,2520,1,6,Huntsman Jacket,304,30,14,1,4,1
            4315, // 3,2870,1,7,Vagrant Armor,304,32,14,1,3,1
            5338, // 3,2870,1,7,Vagrant Armor,304,32,14,1,4,1
            4316, // 3,3235,1,7,Vagabond Armor,304,34,14,1,3,1
            5339, // 3,3235,1,7,Vagabond Armor,304,34,14,1,4,1
            4317, // 3,3630,1,8,Beast Padding,304,36,14,1,3,1
            5340, // 3,3630,1,8,Beast Padding,304,36,14,1,4,1
            4318, // 3,4045,1,8,Pirate's Jacket,304,38,14,1,3,1
            5341, // 3,4045,1,8,Pirate's Jacket,304,38,14,1,4,1
            4319, // 3,4705,1,9,Scale Jacket,304,41,14,1,3,1
            5342, // 3,4705,1,9,Scale Jacket,304,41,14,1,4,1
            4320, // 3,4940,1,9,Raider's Jacket,304,43,14,1,3,1
            5343, // 3,4940,1,9,Raider's Jacket,304,43,14,1,4,1
            4321, // 3,5180,1,9,Hard Skin Armor,304,45,14,1,3,1
            5344, // 3,5180,1,9,Hard Skin Armor,304,45,14,1,4,1
            4323, // 3,5925,1,10,Spirit Guard,304,46,14,1,3,1
            5346, // 3,5925,1,10,Spirit Guard,304,46,14,1,4,1
            4324, // 3,8470,1,11,Black Bart's Jacket,304,55,14,1,3,1
            5347, // 3,8470,1,11,Black Bart's Jacket,304,55,14,1,4,1
            4325, // 3,6185,1,10,Dandy's Jacket,304,49,14,1,3,1
            5348, // 3,6185,1,10,Dandy's Jacket,304,49,14,1,4,1
            4108, // 3,30,1,1,Worker Gloves,306,3,14,0,3,1
            5131, // 3,30,1,1,Worker Gloves,306,3,14,0,4,1
            4109, // 3,60,1,2,Bronze Bracers,306,6,14,0,3,1
            5132, // 3,60,1,2,Bronze Bracers,306,6,14,0,4,1
            4110, // 3,270,1,3,Archer's Gloves,306,13,14,0,3,1
            5133, // 3,270,1,3,Archer's Gloves,306,13,14,0,4,1
            4111, // 3,410,1,4,Laborer Gloves,306,16,14,0,3,1
            5134, // 3,410,1,4,Laborer Gloves,306,16,14,0,4,1
            4363, // 3,775,1,5,Composite Sleeves,306,22,14,0,3,1
            5386, // 3,775,1,5,Composite Sleeves,306,22,14,0,4,1
            4364, // 3,920,1,5,Striker Arms,306,24,14,0,3,1
            5387, // 3,920,1,5,Striker Arms,306,24,14,0,4,1
            4366, // 3,1080,1,6,Victory Bracers,306,26,14,0,3,1
            5389, // 3,1080,1,6,Victory Bracers,306,26,14,0,4,1
            4367, // 3,1255,1,6,Ace Bracers,306,28,14,0,3,1
            5390, // 3,1255,1,6,Ace Bracers,306,28,14,0,4,1
            4368, // 3,1440,1,6,Huntsman Gloves,306,30,14,0,3,1
            5391, // 3,1440,1,6,Huntsman Gloves,306,30,14,0,4,1
            4369, // 3,1640,1,7,Soldier Arms,306,32,14,0,3,1
            5392, // 3,1640,1,7,Soldier Arms,306,32,14,0,4,1
            4370, // 3,1850,1,7,Iron Bracers,306,34,14,0,3,1
            5393, // 3,1850,1,7,Iron Bracers,306,34,14,0,4,1
            4371, // 3,2075,1,8,Silver Manicae,306,36,14,0,3,1
            5394, // 3,2075,1,8,Silver Manicae,306,36,14,0,4,1
            4372, // 3,2310,1,8,Pirate Arms,306,38,14,0,3,1
            5395, // 3,2310,1,8,Pirate Arms,306,38,14,0,4,1
            4373, // 3,2690,1,9,Agent Sleeves,306,41,14,0,3,1
            5396, // 3,2690,1,9,Agent Sleeves,306,41,14,0,4,1
            4374, // 3,2820,1,9,Assault Spine,306,43,14,0,3,1
            5397, // 3,2820,1,9,Assault Spine,306,43,14,0,4,1
            4375, // 3,2960,1,9,Leader Gloves,306,45,14,0,3,1
            5398, // 3,2960,1,9,Leader Gloves,306,45,14,0,4,1
            4377, // 3,3385,1,10,Executor Arms,306,46,14,0,3,1
            5400, // 3,3385,1,10,Executor Arms,306,46,14,0,4,1
            4378, // 3,4840,1,11,Black Bart's Arms,306,55,14,0,3,1
            5401, // 3,4840,1,11,Black Bart's Arms,306,55,14,0,4,1
            4379, // 3,3535,1,10,Captain's Sleeves,306,49,14,0,3,1
            5402, // 3,3535,1,10,Captain's Sleeves,306,49,14,0,4,1
            4104, // 3,37,1,1,Leather Bottoms,307,3,14,0,3,1
            5127, // 3,37,1,1,Leather Bottoms,307,3,14,0,4,1
            4105, // 3,75,1,2,Chainmail Legs,307,6,14,0,3,1
            5128, // 3,75,1,2,Chainmail Legs,307,6,14,0,4,1
            4106, // 3,340,1,3,Smart Boots,307,13,14,0,3,1
            5129, // 3,340,1,3,Smart Boots,307,13,14,0,4,1
            4107, // 3,510,1,4,Composite Bottoms,307,16,14,0,3,1
            5130, // 3,510,1,4,Composite Bottoms,307,16,14,0,4,1
            4338, // 3,970,1,5,Assassin's Bottoms,307,22,14,0,3,1
            5361, // 3,970,1,5,Assassin's Bottoms,307,22,14,0,4,1
            4339, // 3,1150,1,5,Striker's Greaves,307,24,14,0,3,1
            5362, // 3,1150,1,5,Striker's Greaves,307,24,14,0,4,1
            4341, // 3,1350,1,6,Victory Legs,307,26,14,0,3,1
            5364, // 3,1350,1,6,Victory Legs,307,26,14,0,4,1
            4342, // 3,1570,1,6,Nature Cuisses,307,28,14,0,3,1
            5365, // 3,1570,1,6,Nature Cuisses,307,28,14,0,4,1
            4343, // 3,1800,1,6,Huntsman's Cuisses,307,30,14,0,3,1
            5366, // 3,1800,1,6,Huntsman's Cuisses,307,30,14,0,4,1
            4344, // 3,2050,1,7,Soldier's Greaves,307,32,14,0,3,1
            5367, // 3,2050,1,7,Soldier's Greaves,307,32,14,0,4,1
            4345, // 3,2310,1,7,Ninja Bottoms,307,34,14,0,3,1
            5368, // 3,2310,1,7,Ninja Bottoms,307,34,14,0,4,1
            4346, // 3,2590,1,8,Extra Bottoms,307,36,14,0,3,1
            5369, // 3,2590,1,8,Extra Bottoms,307,36,14,0,4,1
            4347, // 3,2890,1,8,Pirate's Legs,307,38,14,0,3,1
            5370, // 3,2890,1,8,Pirate's Legs,307,38,14,0,4,1
            4348, // 3,3360,1,9,Backwater Waistguard,307,41,14,0,3,1
            5371, // 3,3360,1,9,Backwater Waistguard,307,41,14,0,4,1
            4349, // 3,3530,1,9,Raider's Greaves,307,43,14,0,3,1
            5372, // 3,3530,1,9,Raider's Greaves,307,43,14,0,4,1
            4350, // 3,3700,1,9,Leader Leg Guards,307,45,14,0,3,1
            5373, // 3,3700,1,9,Leader Leg Guards,307,45,14,0,4,1
            4352, // 3,4230,1,10,Fur Cuisses,307,46,14,0,3,1
            5375, // 3,4230,1,10,Fur Cuisses,307,46,14,0,4,1
            4353, // 3,6050,1,11,Black Bart's Legs,307,55,14,0,3,1
            5376, // 3,6050,1,11,Black Bart's Legs,307,55,14,0,4,1
            4354, // 3,4420,1,10,Slayer's Bottoms,307,49,14,0,3,1
            5377, // 3,4420,1,10,Slayer's Bottoms,307,49,14,0,4,1
        };

        private static readonly List<uint> LowQualityMageGear = new List<uint>()
        {
            490, // 3,0,1,1,Supply Vest,304,1,0,1,0,1
            2102, // 3,30,1,1,Supply Vest,304,1,0,1,1,1
            3125, // 3,30,1,1,Supply Vest,304,1,0,1,2,1
            493, // 3,0,1,1,Supply Bottoms,307,1,0,0,0,1
            2105, // 3,30,1,1,Supply Bottoms,307,1,0,0,1,1
            3128, // 3,30,1,1,Supply Bottoms,307,1,0,0,2,1
            454, // 3,30,1,1,Magician's Hat,303,3,15,0,0,1
            2066, // 3,30,1,1,Magician's Hat,303,3,15,0,1,1
            3089, // 3,30,1,1,Magician's Hat,303,3,15,0,2,1
            455, // 3,60,1,2,Neophyte's Hood,303,6,15,0,0,1
            2067, // 3,60,1,2,Neophyte's Hood,303,6,15,0,1,1
            3090, // 3,60,1,2,Neophyte's Hood,303,6,15,0,2,1
            456, // 3,270,1,3,Healer's Hood,303,13,15,0,0,1
            2068, // 3,270,1,3,Healer's Hood,303,13,15,0,1,1
            3091, // 3,270,1,3,Healer's Hood,303,13,15,0,2,1
            457, // 3,410,1,4,Halo of Beginning,303,16,15,0,0,1
            2069, // 3,410,1,4,Halo of Beginning,303,16,15,0,1,1
            3092, // 3,410,1,4,Halo of Beginning,303,16,15,0,2,1
            731, // 3,775,1,5,Spike Head,303,22,15,0,0,1
            2343, // 3,775,1,5,Spike Head,303,22,15,0,1,1
            3366, // 3,775,1,5,Spike Head,303,22,15,0,2,1
            732, // 3,920,1,5,Mentor's Cap,303,24,15,0,0,1
            2344, // 3,920,1,5,Mentor's Cap,303,24,15,0,1,1
            3367, // 3,920,1,5,Mentor's Cap,303,24,15,0,2,1
            733, // 3,2560,1,8,Wyrm's Will,303,40,15,0,0,1
            2345, // 3,2560,1,8,Wyrm's Will,303,40,15,0,1,1
            3368, // 3,2560,1,8,Wyrm's Will,303,40,15,0,2,1
            734, // 3,1080,1,6,Saintly Hood,303,26,15,0,0,1
            2346, // 3,1080,1,6,Saintly Hood,303,26,15,0,1,1
            3369, // 3,1080,1,6,Saintly Hood,303,26,15,0,2,1
            735, // 3,1255,1,6,Prayer Hood,303,28,15,0,0,1
            2347, // 3,1255,1,6,Prayer Hood,303,28,15,0,1,1
            3370, // 3,1255,1,6,Prayer Hood,303,28,15,0,2,1
            736, // 3,1440,1,6,Bellwether Hood,303,30,15,0,0,1
            2348, // 3,1440,1,6,Bellwether Hood,303,30,15,0,1,1
            3371, // 3,1440,1,6,Bellwether Hood,303,30,15,0,2,1
            737, // 3,1640,1,7,Atonement Cap,303,32,15,0,0,1
            2349, // 3,1640,1,7,Atonement Cap,303,32,15,0,1,1
            3372, // 3,1640,1,7,Atonement Cap,303,32,15,0,2,1
            738, // 3,1850,1,7,Archmage's Hat,303,34,15,0,0,1
            2350, // 3,1850,1,7,Archmage's Hat,303,34,15,0,1,1
            3373, // 3,1850,1,7,Archmage's Hat,303,34,15,0,2,1
            739, // 3,2075,1,8,Crowned Hood,303,36,15,0,0,1
            2351, // 3,2075,1,8,Crowned Hood,303,36,15,0,1,1
            3374, // 3,2075,1,8,Crowned Hood,303,36,15,0,2,1
            740, // 3,2310,1,8,Circlet of Wisdom,303,38,15,0,0,1
            2352, // 3,2310,1,8,Circlet of Wisdom,303,38,15,0,1,1
            3375, // 3,2310,1,8,Circlet of Wisdom,303,38,15,0,2,1
            741, // 3,2690,1,9,Halo of the New Moon,303,41,15,0,0,1
            2353, // 3,2690,1,9,Halo of the New Moon,303,41,15,0,1,1
            3376, // 3,2690,1,9,Halo of the New Moon,303,41,15,0,2,1
            742, // 3,2820,1,9,Cathedral Cap,303,43,15,0,0,1
            2354, // 3,2820,1,9,Cathedral Cap,303,43,15,0,1,1
            3377, // 3,2820,1,9,Cathedral Cap,303,43,15,0,2,1
            743, // 3,2960,1,9,Adept's Hat,303,45,15,0,0,1
            2355, // 3,2960,1,9,Adept's Hat,303,45,15,0,1,1
            3378, // 3,2960,1,9,Adept's Hat,303,45,15,0,2,1
            745, // 3,3535,1,11,Circlet of Divine Sight,303,52,15,0,0,1
            2357, // 3,3535,1,11,Circlet of Divine Sight,303,52,15,0,1,1
            3380, // 3,3535,1,11,Circlet of Divine Sight,303,52,15,0,2,1
            746, // 3,4840,1,11,Peerless Shade,303,55,15,0,0,1
            2358, // 3,4840,1,11,Peerless Shade,303,55,15,0,1,1
            3381, // 3,4840,1,11,Peerless Shade,303,55,15,0,2,1
            747, // 3,3385,1,10,Diadem,303,49,15,0,0,1
            2359, // 3,3385,1,10,Diadem,303,49,15,0,1,1
            3382, // 3,3385,1,10,Diadem,303,49,15,0,2,1
            458, // 3,52,1,1,Apprentice's Coat,304,3,15,1,0,1
            2070, // 3,52,1,1,Apprentice's Coat,304,3,15,1,1,1
            3093, // 3,52,1,1,Apprentice's Coat,304,3,15,1,2,1
            459, // 3,105,1,2,Hallowed Robe,304,6,15,1,0,1
            2071, // 3,105,1,2,Hallowed Robe,304,6,15,1,1,1
            3094, // 3,105,1,2,Hallowed Robe,304,6,15,1,2,1
            460, // 3,475,1,3,Jongleur's Wrap,304,13,15,1,0,1
            2072, // 3,475,1,3,Jongleur's Wrap,304,13,15,1,1,1
            3095, // 3,475,1,3,Jongleur's Wrap,304,13,15,1,2,1
            461, // 3,715,1,4,Dalmatica,304,16,15,1,0,1
            2073, // 3,715,1,4,Dalmatica,304,16,15,1,1,1
            3096, // 3,715,1,4,Dalmatica,304,16,15,1,2,1
            474, // 3,52,1,1,Sage's Robe,304,3,17,1,0,1
            2086, // 3,52,1,1,Sage's Robe,304,3,17,1,1,1
            3109, // 3,52,1,1,Sage's Robe,304,3,17,1,2,1
            758, // 3,1355,1,5,Shaman's Robe,304,22,15,1,0,1
            2370, // 3,1355,1,5,Shaman's Robe,304,22,15,1,1,1
            3393, // 3,1355,1,5,Shaman's Robe,304,22,15,1,2,1
            759, // 3,1615,1,5,Ocean Robe,304,24,15,1,0,1
            2371, // 3,1615,1,5,Ocean Robe,304,24,15,1,1,1
            3394, // 3,1615,1,5,Ocean Robe,304,24,15,1,2,1
            760, // 3,4480,1,8,Wyrm's Idea,304,40,15,1,0,1
            2372, // 3,4480,1,8,Wyrm's Idea,304,40,15,1,1,1
            3395, // 3,4480,1,8,Wyrm's Idea,304,40,15,1,2,1
            761, // 3,1895,1,6,Sorcerer's Coat,304,26,15,1,0,1
            2373, // 3,1895,1,6,Sorcerer's Coat,304,26,15,1,1,1
            3396, // 3,1895,1,6,Sorcerer's Coat,304,26,15,1,2,1
            762, // 3,2195,1,6,Combat Robe,304,28,15,1,0,1
            2374, // 3,2195,1,6,Combat Robe,304,28,15,1,1,1
            3397, // 3,2195,1,6,Combat Robe,304,28,15,1,2,1
            763, // 3,2520,1,6,Bellwether Robe,304,30,15,1,0,1
            2375, // 3,2520,1,6,Bellwether Robe,304,30,15,1,1,1
            3398, // 3,2520,1,6,Bellwether Robe,304,30,15,1,2,1
            764, // 3,2870,1,7,Elder's Robe,304,32,15,1,0,1
            2376, // 3,2870,1,7,Elder's Robe,304,32,15,1,1,1
            3399, // 3,2870,1,7,Elder's Robe,304,32,15,1,2,1
            765, // 3,3235,1,7,Senior's Coat,304,34,15,1,0,1
            2377, // 3,3235,1,7,Senior's Coat,304,34,15,1,1,1
            3400, // 3,3235,1,7,Senior's Coat,304,34,15,1,2,1
            766, // 3,3630,1,8,Wizard's Robe,304,36,15,1,0,1
            2378, // 3,3630,1,8,Wizard's Robe,304,36,15,1,1,1
            3401, // 3,3630,1,8,Wizard's Robe,304,36,15,1,2,1
            767, // 3,4045,1,8,Precious Robe,304,38,15,1,0,1
            2379, // 3,4045,1,8,Precious Robe,304,38,15,1,1,1
            3402, // 3,4045,1,8,Precious Robe,304,38,15,1,2,1
            768, // 3,4705,1,9,Tide Robe,304,41,15,1,0,1
            2380, // 3,4705,1,9,Tide Robe,304,41,15,1,1,1
            3403, // 3,4705,1,9,Tide Robe,304,41,15,1,2,1
            769, // 3,4940,1,9,Divine Robe,304,43,15,1,0,1
            2381, // 3,4940,1,9,Divine Robe,304,43,15,1,1,1
            3404, // 3,4940,1,9,Divine Robe,304,43,15,1,2,1
            770, // 3,5180,1,9,Adept's Robe,304,45,15,1,0,1
            2382, // 3,5180,1,9,Adept's Robe,304,45,15,1,1,1
            3405, // 3,5180,1,9,Adept's Robe,304,45,15,1,2,1
            772, // 3,6185,1,11,Delusions Robe,304,52,15,1,0,1
            2384, // 3,6185,1,11,Delusions Robe,304,52,15,1,1,1
            3407, // 3,6185,1,11,Delusions Robe,304,52,15,1,2,1
            773, // 3,8470,1,11,Dieudo Robe,304,55,15,1,0,1
            2385, // 3,8470,1,11,Dieudo Robe,304,55,15,1,1,1
            3408, // 3,8470,1,11,Dieudo Robe,304,55,15,1,2,1
            774, // 3,5925,1,10,Witchcraft Robe,304,49,15,1,0,1
            2386, // 3,5925,1,10,Witchcraft Robe,304,49,15,1,1,1
            3409, // 3,5925,1,10,Witchcraft Robe,304,49,15,1,2,1
            466, // 3,30,1,1,Bronze Bangles,306,3,15,0,0,1
            2078, // 3,30,1,1,Bronze Bangles,306,3,15,0,1,1
            3101, // 3,30,1,1,Bronze Bangles,306,3,15,0,2,1
            467, // 3,60,1,2,Tiger Bangles,306,6,15,0,0,1
            2079, // 3,60,1,2,Tiger Bangles,306,6,15,0,1,1
            3102, // 3,60,1,2,Tiger Bangles,306,6,15,0,2,1
            468, // 3,270,1,3,Magician's Bangles,306,13,15,0,0,1
            2080, // 3,270,1,3,Magician's Bangles,306,13,15,0,1,1
            3103, // 3,270,1,3,Magician's Bangles,306,13,15,0,2,1
            469, // 3,410,1,4,Aura Bangles,306,16,15,0,0,1
            2081, // 3,410,1,4,Aura Bangles,306,16,15,0,1,1
            3104, // 3,410,1,4,Aura Bangles,306,16,15,0,2,1
            812, // 3,775,1,5,Sorcery Bangles,306,22,15,0,0,1
            2424, // 3,775,1,5,Sorcery Bangles,306,22,15,0,1,1
            3447, // 3,775,1,5,Sorcery Bangles,306,22,15,0,2,1
            813, // 3,920,1,5,Charm Bangles,306,24,15,0,0,1
            2425, // 3,920,1,5,Charm Bangles,306,24,15,0,1,1
            3448, // 3,920,1,5,Charm Bangles,306,24,15,0,2,1
            814, // 3,2560,1,8,Wyrm's Grasp,306,40,15,0,0,1
            2426, // 3,2560,1,8,Wyrm's Grasp,306,40,15,0,1,1
            3449, // 3,2560,1,8,Wyrm's Grasp,306,40,15,0,2,1
            815, // 3,1080,1,6,Mystic Bangles,306,26,15,0,0,1
            2427, // 3,1080,1,6,Mystic Bangles,306,26,15,0,1,1
            3450, // 3,1080,1,6,Mystic Bangles,306,26,15,0,2,1
            816, // 3,1255,1,6,Compact Bangles,306,28,15,0,0,1
            2428, // 3,1255,1,6,Compact Bangles,306,28,15,0,1,1
            3451, // 3,1255,1,6,Compact Bangles,306,28,15,0,2,1
            817, // 3,1440,1,6,Bellwether Bangles,306,30,15,0,0,1
            2429, // 3,1440,1,6,Bellwether Bangles,306,30,15,0,1,1
            3452, // 3,1440,1,6,Bellwether Bangles,306,30,15,0,2,1
            818, // 3,1640,1,7,Iron Bangles,306,32,15,0,0,1
            2430, // 3,1640,1,7,Iron Bangles,306,32,15,0,1,1
            3453, // 3,1640,1,7,Iron Bangles,306,32,15,0,2,1
            819, // 3,1850,1,7,Roar Bangles,306,34,15,0,0,1
            2431, // 3,1850,1,7,Roar Bangles,306,34,15,0,1,1
            3454, // 3,1850,1,7,Roar Bangles,306,34,15,0,2,1
            820, // 3,2075,1,8,Wizard Bangles,306,36,15,0,0,1
            2432, // 3,2075,1,8,Wizard Bangles,306,36,15,0,1,1
            3455, // 3,2075,1,8,Wizard Bangles,306,36,15,0,2,1
            821, // 3,2310,1,8,Precious Bangles,306,38,15,0,0,1
            2433, // 3,2310,1,8,Precious Bangles,306,38,15,0,1,1
            3456, // 3,2310,1,8,Precious Bangles,306,38,15,0,2,1
            822, // 3,2690,1,9,Victory Bangles,306,41,15,0,0,1
            2434, // 3,2690,1,9,Victory Bangles,306,41,15,0,1,1
            3457, // 3,2690,1,9,Victory Bangles,306,41,15,0,2,1
            823, // 3,2820,1,9,Crunch Bangles,306,43,15,0,0,1
            2435, // 3,2820,1,9,Crunch Bangles,306,43,15,0,1,1
            3458, // 3,2820,1,9,Crunch Bangles,306,43,15,0,2,1
            824, // 3,2960,1,9,Adept Bangles,306,45,15,0,0,1
            2436, // 3,2960,1,9,Adept Bangles,306,45,15,0,1,1
            3459, // 3,2960,1,9,Adept Bangles,306,45,15,0,2,1
            826, // 3,3535,1,11,Aeon Bangles,306,52,15,0,0,1
            2438, // 3,3535,1,11,Aeon Bangles,306,52,15,0,1,1
            3461, // 3,3535,1,11,Aeon Bangles,306,52,15,0,2,1
            827, // 3,4840,1,11,Dieudo Arms,306,55,15,0,0,1
            2439, // 3,4840,1,11,Dieudo Arms,306,55,15,0,1,1
            3462, // 3,4840,1,11,Dieudo Arms,306,55,15,0,2,1
            828, // 3,3385,1,10,Hardest Bangles,306,49,15,0,0,1
            2440, // 3,3385,1,10,Hardest Bangles,306,49,15,0,1,1
            3463, // 3,3385,1,10,Hardest Bangles,306,49,15,0,2,1
            462, // 3,37,1,1,Leather Shoes,307,3,15,0,0,1
            2074, // 3,37,1,1,Leather Shoes,307,3,15,0,1,1
            3097, // 3,37,1,1,Leather Shoes,307,3,15,0,2,1
            463, // 3,75,1,2,Neophyte's Boots,307,6,15,0,0,1
            2075, // 3,75,1,2,Neophyte's Boots,307,6,15,0,1,1
            3098, // 3,75,1,2,Neophyte's Boots,307,6,15,0,2,1
            464, // 3,340,1,3,Grass-Green Shoes,307,13,15,0,0,1
            2076, // 3,340,1,3,Grass-Green Shoes,307,13,15,0,1,1
            3099, // 3,340,1,3,Grass-Green Shoes,307,13,15,0,2,1
            465, // 3,510,1,4,Sagacity Shoes,307,16,15,0,0,1
            2077, // 3,510,1,4,Sagacity Shoes,307,16,15,0,1,1
            3100, // 3,510,1,4,Sagacity Shoes,307,16,15,0,2,1
            785, // 3,970,1,5,Prayer Breeches,307,22,15,0,0,1
            2397, // 3,970,1,5,Prayer Breeches,307,22,15,0,1,1
            3420, // 3,970,1,5,Prayer Breeches,307,22,15,0,2,1
            786, // 3,1150,1,5,Prodigy's Shoes,307,24,15,0,0,1
            2398, // 3,1150,1,5,Prodigy's Shoes,307,24,15,0,1,1
            3421, // 3,1150,1,5,Prodigy's Shoes,307,24,15,0,2,1
            787, // 3,3200,1,8,Wyrm's Scheme,307,40,15,0,0,1
            2399, // 3,3200,1,8,Wyrm's Scheme,307,40,15,0,1,1
            3422, // 3,3200,1,8,Wyrm's Scheme,307,40,15,0,2,1
            788, // 3,1350,1,6,Pleats of Sorcery,307,26,15,0,0,1
            2400, // 3,1350,1,6,Pleats of Sorcery,307,26,15,0,1,1
            3423, // 3,1350,1,6,Pleats of Sorcery,307,26,15,0,2,1
            789, // 3,1570,1,6,Faithful Waistguard,307,28,15,0,0,1
            2401, // 3,1570,1,6,Faithful Waistguard,307,28,15,0,1,1
            3424, // 3,1570,1,6,Faithful Waistguard,307,28,15,0,2,1
            790, // 3,1800,1,6,Bellwether Greaves,307,30,15,0,0,1
            2402, // 3,1800,1,6,Bellwether Greaves,307,30,15,0,1,1
            3425, // 3,1800,1,6,Bellwether Greaves,307,30,15,0,2,1
            791, // 3,2050,1,7,Elder's Boots,307,32,15,0,0,1
            2403, // 3,2050,1,7,Elder's Boots,307,32,15,0,1,1
            3426, // 3,2050,1,7,Elder's Boots,307,32,15,0,2,1
            792, // 3,2310,1,7,Newcomer's Waistguard,307,34,15,0,0,1
            2404, // 3,2310,1,7,Newcomer's Waistguard,307,34,15,0,1,1
            3427, // 3,2310,1,7,Newcomer's Waistguard,307,34,15,0,2,1
            793, // 3,2590,1,8,Pleats of Twilight,307,36,15,0,0,1
            2405, // 3,2590,1,8,Pleats of Twilight,307,36,15,0,1,1
            3428, // 3,2590,1,8,Pleats of Twilight,307,36,15,0,2,1
            794, // 3,2890,1,8,Precious Boots,307,38,15,0,0,1
            2406, // 3,2890,1,8,Precious Boots,307,38,15,0,1,1
            3429, // 3,2890,1,8,Precious Boots,307,38,15,0,2,1
            795, // 3,3360,1,9,Paradise Waistcloth,307,41,15,0,0,1
            2407, // 3,3360,1,9,Paradise Waistcloth,307,41,15,0,1,1
            3430, // 3,3360,1,9,Paradise Waistcloth,307,41,15,0,2,1
            796, // 3,3530,1,9,Domination Boots,307,43,15,0,0,1
            2408, // 3,3530,1,9,Domination Boots,307,43,15,0,1,1
            3431, // 3,3530,1,9,Domination Boots,307,43,15,0,2,1
            797, // 3,3700,1,9,Demon's Greaves,307,45,15,0,0,1
            2409, // 3,3700,1,9,Demon's Greaves,307,45,15,0,1,1
            3432, // 3,3700,1,9,Demon's Greaves,307,45,15,0,2,1
            799, // 3,4420,1,11,Pleats of Revival,307,52,15,0,0,1
            2411, // 3,4420,1,11,Pleats of Revival,307,52,15,0,1,1
            3434, // 3,4420,1,11,Pleats of Revival,307,52,15,0,2,1
            800, // 3,6050,1,11,Gallant Boots,307,55,15,0,0,1
            2412, // 3,6050,1,11,Gallant Boots,307,55,15,0,1,1
            3435, // 3,6050,1,11,Gallant Boots,307,55,15,0,2,1
            801, // 3,4230,1,10,Equinox Waistguard,307,49,15,0,0,1
            2413, // 3,4230,1,10,Equinox Waistguard,307,49,15,0,1,1
            3436, // 3,4230,1,10,Equinox Waistguard,307,49,15,0,2,1
        };

        private static readonly List<uint> HighQualityMageGear = new List<uint>()
        {
            4148, // 3,30,1,1,Supply Vest,304,1,0,1,3,1
            5171, // 3,30,1,1,Supply Vest,304,1,0,1,4,1
            4151, // 3,30,1,1,Supply Bottoms,307,1,0,0,3,1
            5174, // 3,30,1,1,Supply Bottoms,307,1,0,0,4,1
            4112, // 3,30,1,1,Magician's Hat,303,3,15,0,3,1
            5135, // 3,30,1,1,Magician's Hat,303,3,15,0,4,1
            4113, // 3,60,1,2,Neophyte's Hood,303,6,15,0,3,1
            5136, // 3,60,1,2,Neophyte's Hood,303,6,15,0,4,1
            4114, // 3,270,1,3,Healer's Hood,303,13,15,0,3,1
            5137, // 3,270,1,3,Healer's Hood,303,13,15,0,4,1
            4115, // 3,410,1,4,Halo of Beginning,303,16,15,0,3,1
            5138, // 3,410,1,4,Halo of Beginning,303,16,15,0,4,1
            4389, // 3,775,1,5,Spike Head,303,22,15,0,3,1
            5412, // 3,775,1,5,Spike Head,303,22,15,0,4,1
            4390, // 3,920,1,5,Mentor's Cap,303,24,15,0,3,1
            5413, // 3,920,1,5,Mentor's Cap,303,24,15,0,4,1
            4391, // 3,2560,1,8,Wyrm's Will,303,40,15,0,3,1
            5414, // 3,2560,1,8,Wyrm's Will,303,40,15,0,4,1
            4392, // 3,1080,1,6,Saintly Hood,303,26,15,0,3,1
            5415, // 3,1080,1,6,Saintly Hood,303,26,15,0,4,1
            4393, // 3,1255,1,6,Prayer Hood,303,28,15,0,3,1
            5416, // 3,1255,1,6,Prayer Hood,303,28,15,0,4,1
            4394, // 3,1440,1,6,Bellwether Hood,303,30,15,0,3,1
            5417, // 3,1440,1,6,Bellwether Hood,303,30,15,0,4,1
            4395, // 3,1640,1,7,Atonement Cap,303,32,15,0,3,1
            5418, // 3,1640,1,7,Atonement Cap,303,32,15,0,4,1
            4396, // 3,1850,1,7,Archmage's Hat,303,34,15,0,3,1
            5419, // 3,1850,1,7,Archmage's Hat,303,34,15,0,4,1
            4397, // 3,2075,1,8,Crowned Hood,303,36,15,0,3,1
            5420, // 3,2075,1,8,Crowned Hood,303,36,15,0,4,1
            4398, // 3,2310,1,8,Circlet of Wisdom,303,38,15,0,3,1
            5421, // 3,2310,1,8,Circlet of Wisdom,303,38,15,0,4,1
            4399, // 3,2690,1,9,Halo of the New Moon,303,41,15,0,3,1
            5422, // 3,2690,1,9,Halo of the New Moon,303,41,15,0,4,1
            4400, // 3,2820,1,9,Cathedral Cap,303,43,15,0,3,1
            5423, // 3,2820,1,9,Cathedral Cap,303,43,15,0,4,1
            4401, // 3,2960,1,9,Adept's Hat,303,45,15,0,3,1
            5424, // 3,2960,1,9,Adept's Hat,303,45,15,0,4,1
            4403, // 3,3535,1,11,Circlet of Divine Sight,303,52,15,0,3,1
            5426, // 3,3535,1,11,Circlet of Divine Sight,303,52,15,0,4,1
            4404, // 3,4840,1,11,Peerless Shade,303,55,15,0,3,1
            5427, // 3,4840,1,11,Peerless Shade,303,55,15,0,4,1
            4405, // 3,3385,1,10,Diadem,303,49,15,0,3,1
            5428, // 3,3385,1,10,Diadem,303,49,15,0,4,1
            4116, // 3,52,1,1,Apprentice's Coat,304,3,15,1,3,1
            5139, // 3,52,1,1,Apprentice's Coat,304,3,15,1,4,1
            4117, // 3,105,1,2,Hallowed Robe,304,6,15,1,3,1
            5140, // 3,105,1,2,Hallowed Robe,304,6,15,1,4,1
            4118, // 3,475,1,3,Jongleur's Wrap,304,13,15,1,3,1
            5141, // 3,475,1,3,Jongleur's Wrap,304,13,15,1,4,1
            4119, // 3,715,1,4,Dalmatica,304,16,15,1,3,1
            5142, // 3,715,1,4,Dalmatica,304,16,15,1,4,1
            4132, // 3,52,1,1,Sage's Robe,304,3,17,1,3,1
            5155, // 3,52,1,1,Sage's Robe,304,3,17,1,4,1
            4416, // 3,1355,1,5,Shaman's Robe,304,22,15,1,3,1
            5439, // 3,1355,1,5,Shaman's Robe,304,22,15,1,4,1
            4417, // 3,1615,1,5,Ocean Robe,304,24,15,1,3,1
            5440, // 3,1615,1,5,Ocean Robe,304,24,15,1,4,1
            4418, // 3,4480,1,8,Wyrm's Idea,304,40,15,1,3,1
            5441, // 3,4480,1,8,Wyrm's Idea,304,40,15,1,4,1
            4419, // 3,1895,1,6,Sorcerer's Coat,304,26,15,1,3,1
            5442, // 3,1895,1,6,Sorcerer's Coat,304,26,15,1,4,1
            4420, // 3,2195,1,6,Combat Robe,304,28,15,1,3,1
            5443, // 3,2195,1,6,Combat Robe,304,28,15,1,4,1
            4421, // 3,2520,1,6,Bellwether Robe,304,30,15,1,3,1
            5444, // 3,2520,1,6,Bellwether Robe,304,30,15,1,4,1
            4422, // 3,2870,1,7,Elder's Robe,304,32,15,1,3,1
            5445, // 3,2870,1,7,Elder's Robe,304,32,15,1,4,1
            4423, // 3,3235,1,7,Senior's Coat,304,34,15,1,3,1
            5446, // 3,3235,1,7,Senior's Coat,304,34,15,1,4,1
            4424, // 3,3630,1,8,Wizard's Robe,304,36,15,1,3,1
            5447, // 3,3630,1,8,Wizard's Robe,304,36,15,1,4,1
            4425, // 3,4045,1,8,Precious Robe,304,38,15,1,3,1
            5448, // 3,4045,1,8,Precious Robe,304,38,15,1,4,1
            4426, // 3,4705,1,9,Tide Robe,304,41,15,1,3,1
            5449, // 3,4705,1,9,Tide Robe,304,41,15,1,4,1
            4427, // 3,4940,1,9,Divine Robe,304,43,15,1,3,1
            5450, // 3,4940,1,9,Divine Robe,304,43,15,1,4,1
            4428, // 3,5180,1,9,Adept's Robe,304,45,15,1,3,1
            5451, // 3,5180,1,9,Adept's Robe,304,45,15,1,4,1
            4430, // 3,6185,1,11,Delusions Robe,304,52,15,1,3,1
            5453, // 3,6185,1,11,Delusions Robe,304,52,15,1,4,1
            4431, // 3,8470,1,11,Dieudo Robe,304,55,15,1,3,1
            5454, // 3,8470,1,11,Dieudo Robe,304,55,15,1,4,1
            4432, // 3,5925,1,10,Witchcraft Robe,304,49,15,1,3,1
            5455, // 3,5925,1,10,Witchcraft Robe,304,49,15,1,4,1
            4124, // 3,30,1,1,Bronze Bangles,306,3,15,0,3,1
            5147, // 3,30,1,1,Bronze Bangles,306,3,15,0,4,1
            4125, // 3,60,1,2,Tiger Bangles,306,6,15,0,3,1
            5148, // 3,60,1,2,Tiger Bangles,306,6,15,0,4,1
            4126, // 3,270,1,3,Magician's Bangles,306,13,15,0,3,1
            5149, // 3,270,1,3,Magician's Bangles,306,13,15,0,4,1
            4127, // 3,410,1,4,Aura Bangles,306,16,15,0,3,1
            5150, // 3,410,1,4,Aura Bangles,306,16,15,0,4,1
            4470, // 3,775,1,5,Sorcery Bangles,306,22,15,0,3,1
            5493, // 3,775,1,5,Sorcery Bangles,306,22,15,0,4,1
            4471, // 3,920,1,5,Charm Bangles,306,24,15,0,3,1
            5494, // 3,920,1,5,Charm Bangles,306,24,15,0,4,1
            4472, // 3,2560,1,8,Wyrm's Grasp,306,40,15,0,3,1
            5495, // 3,2560,1,8,Wyrm's Grasp,306,40,15,0,4,1
            4473, // 3,1080,1,6,Mystic Bangles,306,26,15,0,3,1
            5496, // 3,1080,1,6,Mystic Bangles,306,26,15,0,4,1
            4474, // 3,1255,1,6,Compact Bangles,306,28,15,0,3,1
            5497, // 3,1255,1,6,Compact Bangles,306,28,15,0,4,1
            4475, // 3,1440,1,6,Bellwether Bangles,306,30,15,0,3,1
            5498, // 3,1440,1,6,Bellwether Bangles,306,30,15,0,4,1
            4476, // 3,1640,1,7,Iron Bangles,306,32,15,0,3,1
            5499, // 3,1640,1,7,Iron Bangles,306,32,15,0,4,1
            4477, // 3,1850,1,7,Roar Bangles,306,34,15,0,3,1
            5500, // 3,1850,1,7,Roar Bangles,306,34,15,0,4,1
            4478, // 3,2075,1,8,Wizard Bangles,306,36,15,0,3,1
            5501, // 3,2075,1,8,Wizard Bangles,306,36,15,0,4,1
            4479, // 3,2310,1,8,Precious Bangles,306,38,15,0,3,1
            5502, // 3,2310,1,8,Precious Bangles,306,38,15,0,4,1
            4480, // 3,2690,1,9,Victory Bangles,306,41,15,0,3,1
            5503, // 3,2690,1,9,Victory Bangles,306,41,15,0,4,1
            4481, // 3,2820,1,9,Crunch Bangles,306,43,15,0,3,1
            5504, // 3,2820,1,9,Crunch Bangles,306,43,15,0,4,1
            4482, // 3,2960,1,9,Adept Bangles,306,45,15,0,3,1
            5505, // 3,2960,1,9,Adept Bangles,306,45,15,0,4,1
            4484, // 3,3535,1,11,Aeon Bangles,306,52,15,0,3,1
            5507, // 3,3535,1,11,Aeon Bangles,306,52,15,0,4,1
            4485, // 3,4840,1,11,Dieudo Arms,306,55,15,0,3,1
            5508, // 3,4840,1,11,Dieudo Arms,306,55,15,0,4,1
            4486, // 3,3385,1,10,Hardest Bangles,306,49,15,0,3,1
            5509, // 3,3385,1,10,Hardest Bangles,306,49,15,0,4,1
            4120, // 3,37,1,1,Leather Shoes,307,3,15,0,3,1
            5143, // 3,37,1,1,Leather Shoes,307,3,15,0,4,1
            4121, // 3,75,1,2,Neophyte's Boots,307,6,15,0,3,1
            5144, // 3,75,1,2,Neophyte's Boots,307,6,15,0,4,1
            4122, // 3,340,1,3,Grass-Green Shoes,307,13,15,0,3,1
            5145, // 3,340,1,3,Grass-Green Shoes,307,13,15,0,4,1
            4123, // 3,510,1,4,Sagacity Shoes,307,16,15,0,3,1
            5146, // 3,510,1,4,Sagacity Shoes,307,16,15,0,4,1
            4443, // 3,970,1,5,Prayer Breeches,307,22,15,0,3,1
            5466, // 3,970,1,5,Prayer Breeches,307,22,15,0,4,1
            4444, // 3,1150,1,5,Prodigy's Shoes,307,24,15,0,3,1
            5467, // 3,1150,1,5,Prodigy's Shoes,307,24,15,0,4,1
            4445, // 3,3200,1,8,Wyrm's Scheme,307,40,15,0,3,1
            5468, // 3,3200,1,8,Wyrm's Scheme,307,40,15,0,4,1
            4446, // 3,1350,1,6,Pleats of Sorcery,307,26,15,0,3,1
            5469, // 3,1350,1,6,Pleats of Sorcery,307,26,15,0,4,1
            4447, // 3,1570,1,6,Faithful Waistguard,307,28,15,0,3,1
            5470, // 3,1570,1,6,Faithful Waistguard,307,28,15,0,4,1
            4448, // 3,1800,1,6,Bellwether Greaves,307,30,15,0,3,1
            5471, // 3,1800,1,6,Bellwether Greaves,307,30,15,0,4,1
            4449, // 3,2050,1,7,Elder's Boots,307,32,15,0,3,1
            5472, // 3,2050,1,7,Elder's Boots,307,32,15,0,4,1
            4450, // 3,2310,1,7,Newcomer's Waistguard,307,34,15,0,3,1
            5473, // 3,2310,1,7,Newcomer's Waistguard,307,34,15,0,4,1
            4451, // 3,2590,1,8,Pleats of Twilight,307,36,15,0,3,1
            5474, // 3,2590,1,8,Pleats of Twilight,307,36,15,0,4,1
            4452, // 3,2890,1,8,Precious Boots,307,38,15,0,3,1
            5475, // 3,2890,1,8,Precious Boots,307,38,15,0,4,1
            4453, // 3,3360,1,9,Paradise Waistcloth,307,41,15,0,3,1
            5476, // 3,3360,1,9,Paradise Waistcloth,307,41,15,0,4,1
            4454, // 3,3530,1,9,Domination Boots,307,43,15,0,3,1
            5477, // 3,3530,1,9,Domination Boots,307,43,15,0,4,1
            4455, // 3,3700,1,9,Demon's Greaves,307,45,15,0,3,1
            5478, // 3,3700,1,9,Demon's Greaves,307,45,15,0,4,1
            4457, // 3,4420,1,11,Pleats of Revival,307,52,15,0,3,1
            5480, // 3,4420,1,11,Pleats of Revival,307,52,15,0,4,1
            4458, // 3,6050,1,11,Gallant Boots,307,55,15,0,3,1
            5481, // 3,6050,1,11,Gallant Boots,307,55,15,0,4,1
            4459, // 3,4230,1,10,Equinox Waistguard,307,49,15,0,3,1
            5482, // 3,4230,1,10,Equinox Waistguard,307,49,15,0,4,1
        };

        private static readonly List<uint> LowQualityFighterWarriorArmor = new List<uint>()
        {
            490, // 3,0,1,1,Supply Vest,304,1,0,1,0,1
            2102, // 3,30,1,1,Supply Vest,304,1,0,1,1,1
            3125, // 3,30,1,1,Supply Vest,304,1,0,1,2,1
            493, // 3,0,1,1,Supply Bottoms,307,1,0,0,0,1
            2105, // 3,30,1,1,Supply Bottoms,307,1,0,0,1,1
            3128, // 3,30,1,1,Supply Bottoms,307,1,0,0,2,1
            422, // 3,30,1,1,Horned Helm,303,3,12,0,0,1
            2034, // 3,30,1,1,Horned Helm,303,3,12,0,1,1
            3057, // 3,30,1,1,Horned Helm,303,3,12,0,2,1
            423, // 3,80,1,2,Wolf Head,303,8,12,0,0,1
            2035, // 3,80,1,2,Wolf Head,303,8,12,0,1,1
            3058, // 3,80,1,2,Wolf Head,303,8,12,0,2,1
            424, // 3,60,1,2,Bronze Helm,303,6,12,0,0,1
            2036, // 3,60,1,2,Bronze Helm,303,6,12,0,1,1
            3059, // 3,60,1,2,Bronze Helm,303,6,12,0,2,1
            425, // 3,195,1,3,Bronze Sallet,303,11,12,0,0,1
            2037, // 3,195,1,3,Bronze Sallet,303,11,12,0,1,1
            3060, // 3,195,1,3,Bronze Sallet,303,11,12,0,2,1
            8115, // 3,410,1,4,Wolverine Head,303,16,12,0,0,1
            8116, // 3,410,1,4,Wolverine Head,303,16,12,0,1,1
            8117, // 3,410,1,4,Wolverine Head,303,16,12,0,2,1
            8120, // 3,520,1,4,Barrel Helm,303,18,12,0,0,1
            8121, // 3,520,1,4,Barrel Helm,303,18,12,0,1,1
            8122, // 3,520,1,4,Barrel Helm,303,18,12,0,2,1
            8125, // 3,3685,1,11,Kerberos Head,303,52,12,0,0,1
            8126, // 3,3685,1,11,Kerberos Head,303,52,12,0,1,1
            8127, // 3,3685,1,11,Kerberos Head,303,52,12,0,2,1
            521, // 3,775,1,5,Battle Helm,303,22,12,0,0,1
            2133, // 3,775,1,5,Battle Helm,303,22,12,0,1,1
            3156, // 3,775,1,5,Battle Helm,303,22,12,0,2,1
            522, // 3,920,1,5,Brute Armet,303,24,12,0,0,1
            2134, // 3,920,1,5,Brute Armet,303,24,12,0,1,1
            3157, // 3,920,1,5,Brute Armet,303,24,12,0,2,1
            523, // 3,2560,1,8,Dragon's Helm,303,40,12,0,0,1
            2135, // 3,2560,1,8,Dragon's Helm,303,40,12,0,1,1
            3158, // 3,2560,1,8,Dragon's Helm,303,40,12,0,2,1
            524, // 3,1080,1,6,Battle Sallet,303,26,12,0,0,1
            2136, // 3,1080,1,6,Battle Sallet,303,26,12,0,1,1
            3159, // 3,1080,1,6,Battle Sallet,303,26,12,0,2,1
            525, // 3,1255,1,6,Sturdy Helm,303,28,12,0,0,1
            2137, // 3,1255,1,6,Sturdy Helm,303,28,12,0,1,1
            3160, // 3,1255,1,6,Sturdy Helm,303,28,12,0,2,1
            526, // 3,1440,1,6,Serenity Helm,303,30,12,0,0,1
            2138, // 3,1440,1,6,Serenity Helm,303,30,12,0,1,1
            3161, // 3,1440,1,6,Serenity Helm,303,30,12,0,2,1
            527, // 3,1640,1,7,Fiend's Helmet,303,32,12,0,0,1
            2139, // 3,1640,1,7,Fiend's Helmet,303,32,12,0,1,1
            3162, // 3,1640,1,7,Fiend's Helmet,303,32,12,0,2,1
            528, // 3,1850,1,7,Raptor Helm,303,34,12,0,0,1
            2140, // 3,1850,1,7,Raptor Helm,303,34,12,0,1,1
            3163, // 3,1850,1,7,Raptor Helm,303,34,12,0,2,1
            529, // 3,2075,1,8,Lieutenant's Helm,303,36,12,0,0,1
            2141, // 3,2075,1,8,Lieutenant's Helm,303,36,12,0,1,1
            3164, // 3,2075,1,8,Lieutenant's Helm,303,36,12,0,2,1
            530, // 3,2310,1,8,Viking Helm,303,38,12,0,0,1
            2142, // 3,2310,1,8,Viking Helm,303,38,12,0,1,1
            3165, // 3,2310,1,8,Viking Helm,303,38,12,0,2,1
            531, // 3,2690,1,9,Gigant Head,303,41,12,0,0,1
            2143, // 3,2690,1,9,Gigant Head,303,41,12,0,1,1
            3166, // 3,2690,1,9,Gigant Head,303,41,12,0,2,1
            532, // 3,2960,1,9,Dusk Helm,303,45,12,0,0,1
            2144, // 3,2960,1,9,Dusk Helm,303,45,12,0,1,1
            3167, // 3,2960,1,9,Dusk Helm,303,45,12,0,2,1
            533, // 3,2820,1,9,Gryphus Helm,303,43,12,0,0,1
            2145, // 3,2820,1,9,Gryphus Helm,303,43,12,0,1,1
            3168, // 3,2820,1,9,Gryphus Helm,303,43,12,0,2,1
            535, // 3,3535,1,10,Silver Sallet,303,49,12,0,0,1
            2147, // 3,3535,1,10,Silver Sallet,303,49,12,0,1,1
            3170, // 3,3535,1,10,Silver Sallet,303,49,12,0,2,1
            536, // 3,4840,1,11,Galeam Gloria,303,55,12,0,0,1
            2148, // 3,4840,1,11,Galeam Gloria,303,55,12,0,1,1
            3171, // 3,4840,1,11,Galeam Gloria,303,55,12,0,2,1
            537, // 3,3385,1,10,Knight Head Helm,303,46,12,0,0,1
            2149, // 3,3385,1,10,Knight Head Helm,303,46,12,0,1,1
            3172, // 3,3385,1,10,Knight Head Helm,303,46,12,0,2,1
            426, // 3,52,1,1,Hide Armor,304,3,12,1,0,1
            2038, // 3,52,1,1,Hide Armor,304,3,12,1,1,1
            3061, // 3,52,1,1,Hide Armor,304,3,12,1,2,1
            427, // 3,105,1,2,Bronze Plate,304,6,12,1,0,1
            2039, // 3,105,1,2,Bronze Plate,304,6,12,1,1,1
            3062, // 3,105,1,2,Bronze Plate,304,6,12,1,2,1
            428, // 3,475,1,3,Dread Armor,304,13,12,1,0,1
            2040, // 3,475,1,3,Dread Armor,304,13,12,1,1,1
            3063, // 3,475,1,3,Dread Armor,304,13,12,1,2,1
            429, // 3,715,1,4,Gaudy Armor,304,16,12,1,0,1
            2041, // 3,715,1,4,Gaudy Armor,304,16,12,1,1,1
            3064, // 3,715,1,4,Gaudy Armor,304,16,12,1,2,1
            8223, // 3,340,1,3,Half Cuirass,304,11,12,1,0,1
            8224, // 3,340,1,3,Half Cuirass,304,11,12,1,1,1
            8225, // 3,340,1,3,Half Cuirass,304,11,12,1,2,1
            547, // 3,1355,1,5,Rock Armor,304,22,12,1,0,1
            2159, // 3,1355,1,5,Rock Armor,304,22,12,1,1,1
            3182, // 3,1355,1,5,Rock Armor,304,22,12,1,2,1
            548, // 3,1615,1,5,Vice Cuirass,304,24,12,1,0,1
            2160, // 3,1615,1,5,Vice Cuirass,304,24,12,1,1,1
            3183, // 3,1615,1,5,Vice Cuirass,304,24,12,1,2,1
            549, // 3,4480,1,8,Dragon Breastplate,304,40,12,1,0,1
            2161, // 3,4480,1,8,Dragon Breastplate,304,40,12,1,1,1
            3184, // 3,4480,1,8,Dragon Breastplate,304,40,12,1,2,1
            550, // 3,1895,1,6,Grand Surcoat,304,26,12,1,0,1
            2162, // 3,1895,1,6,Grand Surcoat,304,26,12,1,1,1
            3185, // 3,1895,1,6,Grand Surcoat,304,26,12,1,2,1
            551, // 3,2195,1,6,Sturdy Plate,304,28,12,1,0,1
            2163, // 3,2195,1,6,Sturdy Plate,304,28,12,1,1,1
            3186, // 3,2195,1,6,Sturdy Plate,304,28,12,1,2,1
            552, // 3,2520,1,6,Serenity Armor,304,30,12,1,0,1
            2164, // 3,2520,1,6,Serenity Armor,304,30,12,1,1,1
            3187, // 3,2520,1,6,Serenity Armor,304,30,12,1,2,1
            553, // 3,2870,1,7,Savage Armor,304,32,12,1,0,1
            2165, // 3,2870,1,7,Savage Armor,304,32,12,1,1,1
            3188, // 3,2870,1,7,Savage Armor,304,32,12,1,2,1
            554, // 3,3235,1,7,Foul Armor,304,34,12,1,0,1
            2166, // 3,3235,1,7,Foul Armor,304,34,12,1,1,1
            3189, // 3,3235,1,7,Foul Armor,304,34,12,1,2,1
            555, // 3,3630,1,8,Royal Surcoat,304,36,12,1,0,1
            2167, // 3,3630,1,8,Royal Surcoat,304,36,12,1,1,1
            3190, // 3,3630,1,8,Royal Surcoat,304,36,12,1,2,1
            556, // 3,4045,1,8,Cruel Armor,304,38,12,1,0,1
            2168, // 3,4045,1,8,Cruel Armor,304,38,12,1,1,1
            3191, // 3,4045,1,8,Cruel Armor,304,38,12,1,2,1
            557, // 3,4705,1,9,Guardsman's Armbelt,304,41,12,1,0,1
            2169, // 3,4705,1,9,Guardsman's Armbelt,304,41,12,1,1,1
            3192, // 3,4705,1,9,Guardsman's Armbelt,304,41,12,1,2,1
            558, // 3,4940,1,9,Gryphus Armor,304,43,12,1,0,1
            2170, // 3,4940,1,9,Gryphus Armor,304,43,12,1,1,1
            3193, // 3,4940,1,9,Gryphus Armor,304,43,12,1,2,1
            559, // 3,5180,1,9,Dawn Plate,304,45,12,1,0,1
            2171, // 3,5180,1,9,Dawn Plate,304,45,12,1,1,1
            3194, // 3,5180,1,9,Dawn Plate,304,45,12,1,2,1
            561, // 3,6185,1,10,Silver Cuirass,304,49,12,1,0,1
            2173, // 3,6185,1,10,Silver Cuirass,304,49,12,1,1,1
            3196, // 3,6185,1,10,Silver Cuirass,304,49,12,1,2,1
            562, // 3,8470,1,11,Arma Gloria,304,55,12,1,0,1
            2174, // 3,8470,1,11,Arma Gloria,304,55,12,1,1,1
            3197, // 3,8470,1,11,Arma Gloria,304,55,12,1,2,1
            563, // 3,6450,1,11,Shining Scale Coat,304,52,12,1,0,1
            2175, // 3,6450,1,11,Shining Scale Coat,304,52,12,1,1,1
            3198, // 3,6450,1,11,Shining Scale Coat,304,52,12,1,2,1
            434, // 3,30,1,1,Leather Gloves,306,3,12,0,0,1
            2046, // 3,30,1,1,Leather Gloves,306,3,12,0,1,1
            3069, // 3,30,1,1,Leather Gloves,306,3,12,0,2,1
            435, // 3,60,1,2,Bronze Guard,306,6,12,0,0,1
            2047, // 3,60,1,2,Bronze Guard,306,6,12,0,1,1
            3070, // 3,60,1,2,Bronze Guard,306,6,12,0,2,1
            436, // 3,270,1,3,Emblem Arms,306,13,12,0,0,1
            2048, // 3,270,1,3,Emblem Arms,306,13,12,0,1,1
            3071, // 3,270,1,3,Emblem Arms,306,13,12,0,2,1
            437, // 3,410,1,4,Gaudy Bracers,306,16,12,0,0,1
            2049, // 3,410,1,4,Gaudy Bracers,306,16,12,0,1,1
            3072, // 3,410,1,4,Gaudy Bracers,306,16,12,0,2,1
            599, // 3,775,1,5,Murg Armguards,306,22,12,0,0,1
            2211, // 3,775,1,5,Murg Armguards,306,22,12,0,1,1
            3234, // 3,775,1,5,Murg Armguards,306,22,12,0,2,1
            600, // 3,920,1,5,Wild Gauntlet,306,24,12,0,0,1
            2212, // 3,920,1,5,Wild Gauntlet,306,24,12,0,1,1
            3235, // 3,920,1,5,Wild Gauntlet,306,24,12,0,2,1
            601, // 3,2560,1,8,Dragoon Arms,306,40,12,0,0,1
            2213, // 3,2560,1,8,Dragoon Arms,306,40,12,0,1,1
            3236, // 3,2560,1,8,Dragoon Arms,306,40,12,0,2,1
            602, // 3,1080,1,6,Smoky Gloves,306,26,12,0,0,1
            2214, // 3,1080,1,6,Smoky Gloves,306,26,12,0,1,1
            3237, // 3,1080,1,6,Smoky Gloves,306,26,12,0,2,1
            603, // 3,1255,1,6,Sturdy Guard,306,28,12,0,0,1
            2215, // 3,1255,1,6,Sturdy Guard,306,28,12,0,1,1
            3238, // 3,1255,1,6,Sturdy Guard,306,28,12,0,2,1
            604, // 3,1440,1,6,Serenity Arms,306,30,12,0,0,1
            2216, // 3,1440,1,6,Serenity Arms,306,30,12,0,1,1
            3239, // 3,1440,1,6,Serenity Arms,306,30,12,0,2,1
            605, // 3,1640,1,7,Gooseneck Arm,306,32,12,0,0,1
            2217, // 3,1640,1,7,Gooseneck Arm,306,32,12,0,1,1
            3240, // 3,1640,1,7,Gooseneck Arm,306,32,12,0,2,1
            606, // 3,1850,1,7,Raptor Gauntlets,306,34,12,0,0,1
            2218, // 3,1850,1,7,Raptor Gauntlets,306,34,12,0,1,1
            3241, // 3,1850,1,7,Raptor Gauntlets,306,34,12,0,2,1
            607, // 3,2075,1,8,Sturdy Gauntlets,306,36,12,0,0,1
            2219, // 3,2075,1,8,Sturdy Gauntlets,306,36,12,0,1,1
            3242, // 3,2075,1,8,Sturdy Gauntlets,306,36,12,0,2,1
            608, // 3,2310,1,8,Cruel Arms,306,38,12,0,0,1
            2220, // 3,2310,1,8,Cruel Arms,306,38,12,0,1,1
            3243, // 3,2310,1,8,Cruel Arms,306,38,12,0,2,1
            609, // 3,2690,1,9,Brute Gauntlets,306,41,12,0,0,1
            2221, // 3,2690,1,9,Brute Gauntlets,306,41,12,0,1,1
            3244, // 3,2690,1,9,Brute Gauntlets,306,41,12,0,2,1
            610, // 3,2820,1,9,Gryphus Gauntlets,306,43,12,0,0,1
            2222, // 3,2820,1,9,Gryphus Gauntlets,306,43,12,0,1,1
            3245, // 3,2820,1,9,Gryphus Gauntlets,306,43,12,0,2,1
            611, // 3,2960,1,9,Twilight Bracers,306,45,12,0,0,1
            2223, // 3,2960,1,9,Twilight Bracers,306,45,12,0,1,1
            3246, // 3,2960,1,9,Twilight Bracers,306,45,12,0,2,1
            613, // 3,3535,1,10,Silver Gauntlets,306,49,12,0,0,1
            2225, // 3,3535,1,10,Silver Gauntlets,306,49,12,0,1,1
            3248, // 3,3535,1,10,Silver Gauntlets,306,49,12,0,2,1
            614, // 3,4840,1,11,Caestus Gloria,306,55,12,0,0,1
            2226, // 3,4840,1,11,Caestus Gloria,306,55,12,0,1,1
            3249, // 3,4840,1,11,Caestus Gloria,306,55,12,0,2,1
            615, // 3,3685,1,11,Metal Riff Arm,306,52,12,0,0,1
            2227, // 3,3685,1,11,Metal Riff Arm,306,52,12,0,1,1
            3250, // 3,3685,1,11,Metal Riff Arm,306,52,12,0,2,1
            430, // 3,37,1,1,Battle Greaves,307,3,12,0,0,1
            2042, // 3,37,1,1,Battle Greaves,307,3,12,0,1,1
            3065, // 3,37,1,1,Battle Greaves,307,3,12,0,2,1
            431, // 3,75,1,2,Bronze Shoes,307,6,12,0,0,1
            2043, // 3,75,1,2,Bronze Shoes,307,6,12,0,1,1
            3066, // 3,75,1,2,Bronze Shoes,307,6,12,0,2,1
            432, // 3,340,1,3,Copper Greaves,307,13,12,0,0,1
            2044, // 3,340,1,3,Copper Greaves,307,13,12,0,1,1
            3067, // 3,340,1,3,Copper Greaves,307,13,12,0,2,1
            433, // 3,510,1,4,Gaudy Greaves,307,16,12,0,0,1
            2045, // 3,510,1,4,Gaudy Greaves,307,16,12,0,1,1
            3068, // 3,510,1,4,Gaudy Greaves,307,16,12,0,2,1
            573, // 3,970,1,5,Red Greaves,307,22,12,0,0,1
            2185, // 3,970,1,5,Red Greaves,307,22,12,0,1,1
            3208, // 3,970,1,5,Red Greaves,307,22,12,0,2,1
            574, // 3,1150,1,5,Wild Sabatons,307,24,12,0,0,1
            2186, // 3,1150,1,5,Wild Sabatons,307,24,12,0,1,1
            3209, // 3,1150,1,5,Wild Sabatons,307,24,12,0,2,1
            575, // 3,3200,1,8,Dragoon Boots,307,40,12,0,0,1
            2187, // 3,3200,1,8,Dragoon Boots,307,40,12,0,1,1
            3210, // 3,3200,1,8,Dragoon Boots,307,40,12,0,2,1
            576, // 3,1350,1,6,Iron Greaves,307,26,12,0,0,1
            2188, // 3,1350,1,6,Iron Greaves,307,26,12,0,1,1
            3211, // 3,1350,1,6,Iron Greaves,307,26,12,0,2,1
            577, // 3,1570,1,6,Sturdy Cuisses,307,28,12,0,0,1
            2189, // 3,1570,1,6,Sturdy Cuisses,307,28,12,0,1,1
            3212, // 3,1570,1,6,Sturdy Cuisses,307,28,12,0,2,1
            578, // 3,1800,1,6,Serenity Legs,307,30,12,0,0,1
            2190, // 3,1800,1,6,Serenity Legs,307,30,12,0,1,1
            3213, // 3,1800,1,6,Serenity Legs,307,30,12,0,2,1
            579, // 3,2050,1,7,Savage Greaves,307,32,12,0,0,1
            2191, // 3,2050,1,7,Savage Greaves,307,32,12,0,1,1
            3214, // 3,2050,1,7,Savage Greaves,307,32,12,0,2,1
            580, // 3,2310,1,7,Foul Greaves,307,34,12,0,0,1
            2192, // 3,2310,1,7,Foul Greaves,307,34,12,0,1,1
            3215, // 3,2310,1,7,Foul Greaves,307,34,12,0,2,1
            581, // 3,2590,1,8,Howl Sabatons,307,36,12,0,0,1
            2193, // 3,2590,1,8,Howl Sabatons,307,36,12,0,1,1
            3216, // 3,2590,1,8,Howl Sabatons,307,36,12,0,2,1
            582, // 3,2890,1,8,Cruel Greaves,307,38,12,0,0,1
            2194, // 3,2890,1,8,Cruel Greaves,307,38,12,0,1,1
            3217, // 3,2890,1,8,Cruel Greaves,307,38,12,0,2,1
            583, // 3,3360,1,9,Guardsman's Greaves,307,41,12,0,0,1
            2195, // 3,3360,1,9,Guardsman's Greaves,307,41,12,0,1,1
            3218, // 3,3360,1,9,Guardsman's Greaves,307,41,12,0,2,1
            584, // 3,3530,1,9,Gryphus Greaves,307,43,12,0,0,1
            2196, // 3,3530,1,9,Gryphus Greaves,307,43,12,0,1,1
            3219, // 3,3530,1,9,Gryphus Greaves,307,43,12,0,2,1
            585, // 3,3700,1,9,Sunset Cuisses,307,45,12,0,0,1
            2197, // 3,3700,1,9,Sunset Cuisses,307,45,12,0,1,1
            3220, // 3,3700,1,9,Sunset Cuisses,307,45,12,0,2,1
            87, // 3,4420,1,10,Silver Sabatons,307,49,12,0,0,1
            2199, // 3,4420,1,10,Silver Sabatons,307,49,12,0,1,1
            3222, // 3,4420,1,10,Silver Sabatons,307,49,12,0,2,1
            588, // 3,6050,1,11,Foot Gloria,307,55,12,0,0,1
            2200, // 3,6050,1,11,Foot Gloria,307,55,12,0,1,1
            3223, // 3,6050,1,11,Foot Gloria,307,55,12,0,2,1
            589, // 3,4610,1,11,High Scale Whisker,307,52,12,0,0,1
            2201, // 3,4610,1,11,High Scale Whisker,307,52,12,0,1,1
            3224, // 3,4610,1,11,High Scale Whisker,307,52,12,0,2,1
        };

        private static readonly List<uint> HighQualityFighterWarriorArmor = new List<uint>()
        {
            4148, // 3,30,1,1,Supply Vest,304,1,0,1,3,1
            5171, // 3,30,1,1,Supply Vest,304,1,0,1,4,1
            4151, // 3,30,1,1,Supply Bottoms,307,1,0,0,3,1
            5174, // 3,30,1,1,Supply Bottoms,307,1,0,0,4,1
            4080, // 3,30,1,1,Horned Helm,303,3,12,0,3,1
            5103, // 3,30,1,1,Horned Helm,303,3,12,0,4,1
            4081, // 3,80,1,2,Wolf Head,303,8,12,0,3,1
            5104, // 3,80,1,2,Wolf Head,303,8,12,0,4,1
            4082, // 3,60,1,2,Bronze Helm,303,6,12,0,3,1
            5105, // 3,60,1,2,Bronze Helm,303,6,12,0,4,1
            4083, // 3,195,1,3,Bronze Sallet,303,11,12,0,3,1
            5106, // 3,195,1,3,Bronze Sallet,303,11,12,0,4,1
            8118, // 3,410,1,4,Wolverine Head,303,16,12,0,3,1
            8119, // 3,410,1,4,Wolverine Head,303,16,12,0,4,1
            8123, // 3,520,1,4,Barrel Helm,303,18,12,0,3,1
            8124, // 3,520,1,4,Barrel Helm,303,18,12,0,4,1
            8128, // 3,3685,1,11,Kerberos Head,303,52,12,0,3,1
            8129, // 3,3685,1,11,Kerberos Head,303,52,12,0,4,1
            4179, // 3,775,1,5,Battle Helm,303,22,12,0,3,1
            5202, // 3,775,1,5,Battle Helm,303,22,12,0,4,1
            4180, // 3,920,1,5,Brute Armet,303,24,12,0,3,1
            5203, // 3,920,1,5,Brute Armet,303,24,12,0,4,1
            4181, // 3,2560,1,8,Dragon's Helm,303,40,12,0,3,1
            5204, // 3,2560,1,8,Dragon's Helm,303,40,12,0,4,1
            4182, // 3,1080,1,6,Battle Sallet,303,26,12,0,3,1
            5205, // 3,1080,1,6,Battle Sallet,303,26,12,0,4,1
            4183, // 3,1255,1,6,Sturdy Helm,303,28,12,0,3,1
            5206, // 3,1255,1,6,Sturdy Helm,303,28,12,0,4,1
            4184, // 3,1440,1,6,Serenity Helm,303,30,12,0,3,1
            5207, // 3,1440,1,6,Serenity Helm,303,30,12,0,4,1
            4185, // 3,1640,1,7,Fiend's Helmet,303,32,12,0,3,1
            5208, // 3,1640,1,7,Fiend's Helmet,303,32,12,0,4,1
            4186, // 3,1850,1,7,Raptor Helm,303,34,12,0,3,1
            5209, // 3,1850,1,7,Raptor Helm,303,34,12,0,4,1
            4187, // 3,2075,1,8,Lieutenant's Helm,303,36,12,0,3,1
            5210, // 3,2075,1,8,Lieutenant's Helm,303,36,12,0,4,1
            4188, // 3,2310,1,8,Viking Helm,303,38,12,0,3,1
            5211, // 3,2310,1,8,Viking Helm,303,38,12,0,4,1
            4189, // 3,2690,1,9,Gigant Head,303,41,12,0,3,1
            5212, // 3,2690,1,9,Gigant Head,303,41,12,0,4,1
            4190, // 3,2960,1,9,Dusk Helm,303,45,12,0,3,1
            5213, // 3,2960,1,9,Dusk Helm,303,45,12,0,4,1
            4191, // 3,2820,1,9,Gryphus Helm,303,43,12,0,3,1
            5214, // 3,2820,1,9,Gryphus Helm,303,43,12,0,4,1
            4193, // 3,3535,1,10,Silver Sallet,303,49,12,0,3,1
            5216, // 3,3535,1,10,Silver Sallet,303,49,12,0,4,1
            4194, // 3,4840,1,11,Galeam Gloria,303,55,12,0,3,1
            5217, // 3,4840,1,11,Galeam Gloria,303,55,12,0,4,1
            4195, // 3,3385,1,10,Knight Head Helm,303,46,12,0,3,1
            5218, // 3,3385,1,10,Knight Head Helm,303,46,12,0,4,1
            4084, // 3,52,1,1,Hide Armor,304,3,12,1,3,1
            5107, // 3,52,1,1,Hide Armor,304,3,12,1,4,1
            4085, // 3,105,1,2,Bronze Plate,304,6,12,1,3,1
            5108, // 3,105,1,2,Bronze Plate,304,6,12,1,4,1
            4086, // 3,475,1,3,Dread Armor,304,13,12,1,3,1
            5109, // 3,475,1,3,Dread Armor,304,13,12,1,4,1
            4087, // 3,715,1,4,Gaudy Armor,304,16,12,1,3,1
            5110, // 3,715,1,4,Gaudy Armor,304,16,12,1,4,1
            8226, // 3,340,1,3,Half Cuirass,304,11,12,1,3,1
            8227, // 3,340,1,3,Half Cuirass,304,11,12,1,4,1
            4205, // 3,1355,1,5,Rock Armor,304,22,12,1,3,1
            5228, // 3,1355,1,5,Rock Armor,304,22,12,1,4,1
            4206, // 3,1615,1,5,Vice Cuirass,304,24,12,1,3,1
            5229, // 3,1615,1,5,Vice Cuirass,304,24,12,1,4,1
            4207, // 3,4480,1,8,Dragon Breastplate,304,40,12,1,3,1
            5230, // 3,4480,1,8,Dragon Breastplate,304,40,12,1,4,1
            4208, // 3,1895,1,6,Grand Surcoat,304,26,12,1,3,1
            5231, // 3,1895,1,6,Grand Surcoat,304,26,12,1,4,1
            4209, // 3,2195,1,6,Sturdy Plate,304,28,12,1,3,1
            5232, // 3,2195,1,6,Sturdy Plate,304,28,12,1,4,1
            4210, // 3,2520,1,6,Serenity Armor,304,30,12,1,3,1
            5233, // 3,2520,1,6,Serenity Armor,304,30,12,1,4,1
            4211, // 3,2870,1,7,Savage Armor,304,32,12,1,3,1
            5234, // 3,2870,1,7,Savage Armor,304,32,12,1,4,1
            4212, // 3,3235,1,7,Foul Armor,304,34,12,1,3,1
            5235, // 3,3235,1,7,Foul Armor,304,34,12,1,4,1
            4213, // 3,3630,1,8,Royal Surcoat,304,36,12,1,3,1
            5236, // 3,3630,1,8,Royal Surcoat,304,36,12,1,4,1
            4214, // 3,4045,1,8,Cruel Armor,304,38,12,1,3,1
            5237, // 3,4045,1,8,Cruel Armor,304,38,12,1,4,1
            4215, // 3,4705,1,9,Guardsman's Armbelt,304,41,12,1,3,1
            5238, // 3,4705,1,9,Guardsman's Armbelt,304,41,12,1,4,1
            4216, // 3,4940,1,9,Gryphus Armor,304,43,12,1,3,1
            5239, // 3,4940,1,9,Gryphus Armor,304,43,12,1,4,1
            4217, // 3,5180,1,9,Dawn Plate,304,45,12,1,3,1
            5240, // 3,5180,1,9,Dawn Plate,304,45,12,1,4,1
            4219, // 3,6185,1,10,Silver Cuirass,304,49,12,1,3,1
            5242, // 3,6185,1,10,Silver Cuirass,304,49,12,1,4,1
            4220, // 3,8470,1,11,Arma Gloria,304,55,12,1,3,1
            5243, // 3,8470,1,11,Arma Gloria,304,55,12,1,4,1
            4221, // 3,6450,1,11,Shining Scale Coat,304,52,12,1,3,1
            5244, // 3,6450,1,11,Shining Scale Coat,304,52,12,1,4,1
            4092, // 3,30,1,1,Leather Gloves,306,3,12,0,3,1
            5115, // 3,30,1,1,Leather Gloves,306,3,12,0,4,1
            4093, // 3,60,1,2,Bronze Guard,306,6,12,0,3,1
            5116, // 3,60,1,2,Bronze Guard,306,6,12,0,4,1
            4094, // 3,270,1,3,Emblem Arms,306,13,12,0,3,1
            5117, // 3,270,1,3,Emblem Arms,306,13,12,0,4,1
            4095, // 3,410,1,4,Gaudy Bracers,306,16,12,0,3,1
            5118, // 3,410,1,4,Gaudy Bracers,306,16,12,0,4,1
            4257, // 3,775,1,5,Murg Armguards,306,22,12,0,3,1
            5280, // 3,775,1,5,Murg Armguards,306,22,12,0,4,1
            4258, // 3,920,1,5,Wild Gauntlet,306,24,12,0,3,1
            5281, // 3,920,1,5,Wild Gauntlet,306,24,12,0,4,1
            4259, // 3,2560,1,8,Dragoon Arms,306,40,12,0,3,1
            5282, // 3,2560,1,8,Dragoon Arms,306,40,12,0,4,1
            4260, // 3,1080,1,6,Smoky Gloves,306,26,12,0,3,1
            5283, // 3,1080,1,6,Smoky Gloves,306,26,12,0,4,1
            4261, // 3,1255,1,6,Sturdy Guard,306,28,12,0,3,1
            5284, // 3,1255,1,6,Sturdy Guard,306,28,12,0,4,1
            4262, // 3,1440,1,6,Serenity Arms,306,30,12,0,3,1
            5285, // 3,1440,1,6,Serenity Arms,306,30,12,0,4,1
            4263, // 3,1640,1,7,Gooseneck Arm,306,32,12,0,3,1
            5286, // 3,1640,1,7,Gooseneck Arm,306,32,12,0,4,1
            4264, // 3,1850,1,7,Raptor Gauntlets,306,34,12,0,3,1
            5287, // 3,1850,1,7,Raptor Gauntlets,306,34,12,0,4,1
            4265, // 3,2075,1,8,Sturdy Gauntlets,306,36,12,0,3,1
            5288, // 3,2075,1,8,Sturdy Gauntlets,306,36,12,0,4,1
            4266, // 3,2310,1,8,Cruel Arms,306,38,12,0,3,1
            5289, // 3,2310,1,8,Cruel Arms,306,38,12,0,4,1
            4267, // 3,2690,1,9,Brute Gauntlets,306,41,12,0,3,1
            5290, // 3,2690,1,9,Brute Gauntlets,306,41,12,0,4,1
            4268, // 3,2820,1,9,Gryphus Gauntlets,306,43,12,0,3,1
            5291, // 3,2820,1,9,Gryphus Gauntlets,306,43,12,0,4,1
            4269, // 3,2960,1,9,Twilight Bracers,306,45,12,0,3,1
            5292, // 3,2960,1,9,Twilight Bracers,306,45,12,0,4,1
            4271, // 3,3535,1,10,Silver Gauntlets,306,49,12,0,3,1
            5294, // 3,3535,1,10,Silver Gauntlets,306,49,12,0,4,1
            4272, // 3,4840,1,11,Caestus Gloria,306,55,12,0,3,1
            5295, // 3,4840,1,11,Caestus Gloria,306,55,12,0,4,1
            4273, // 3,3685,1,11,Metal Riff Arm,306,52,12,0,3,1
            5296, // 3,3685,1,11,Metal Riff Arm,306,52,12,0,4,1
            4088, // 3,37,1,1,Battle Greaves,307,3,12,0,3,1
            5111, // 3,37,1,1,Battle Greaves,307,3,12,0,4,1
            4089, // 3,75,1,2,Bronze Shoes,307,6,12,0,3,1
            5112, // 3,75,1,2,Bronze Shoes,307,6,12,0,4,1
            4090, // 3,340,1,3,Copper Greaves,307,13,12,0,3,1
            5113, // 3,340,1,3,Copper Greaves,307,13,12,0,4,1
            4091, // 3,510,1,4,Gaudy Greaves,307,16,12,0,3,1
            5114, // 3,510,1,4,Gaudy Greaves,307,16,12,0,4,1
            4231, // 3,970,1,5,Red Greaves,307,22,12,0,3,1
            5254, // 3,970,1,5,Red Greaves,307,22,12,0,4,1
            4232, // 3,1150,1,5,Wild Sabatons,307,24,12,0,3,1
            5255, // 3,1150,1,5,Wild Sabatons,307,24,12,0,4,1
            4233, // 3,3200,1,8,Dragoon Boots,307,40,12,0,3,1
            5256, // 3,3200,1,8,Dragoon Boots,307,40,12,0,4,1
            4234, // 3,1350,1,6,Iron Greaves,307,26,12,0,3,1
            5257, // 3,1350,1,6,Iron Greaves,307,26,12,0,4,1
            4235, // 3,1570,1,6,Sturdy Cuisses,307,28,12,0,3,1
            5258, // 3,1570,1,6,Sturdy Cuisses,307,28,12,0,4,1
            4236, // 3,1800,1,6,Serenity Legs,307,30,12,0,3,1
            5259, // 3,1800,1,6,Serenity Legs,307,30,12,0,4,1
            4237, // 3,2050,1,7,Savage Greaves,307,32,12,0,3,1
            5260, // 3,2050,1,7,Savage Greaves,307,32,12,0,4,1
            4238, // 3,2310,1,7,Foul Greaves,307,34,12,0,3,1
            5261, // 3,2310,1,7,Foul Greaves,307,34,12,0,4,1
            4239, // 3,2590,1,8,Howl Sabatons,307,36,12,0,3,1
            5262, // 3,2590,1,8,Howl Sabatons,307,36,12,0,4,1
            4240, // 3,2890,1,8,Cruel Greaves,307,38,12,0,3,1
            5263, // 3,2890,1,8,Cruel Greaves,307,38,12,0,4,1
            4241, // 3,3360,1,9,Guardsman's Greaves,307,41,12,0,3,1
            5264, // 3,3360,1,9,Guardsman's Greaves,307,41,12,0,4,1
            4242, // 3,3530,1,9,Gryphus Greaves,307,43,12,0,3,1
            5265, // 3,3530,1,9,Gryphus Greaves,307,43,12,0,4,1
            4243, // 3,3700,1,9,Sunset Cuisses,307,45,12,0,3,1
            5266, // 3,3700,1,9,Sunset Cuisses,307,45,12,0,4,1
            4245, // 3,4420,1,10,Silver Sabatons,307,49,12,0,3,1
            5268, // 3,4420,1,10,Silver Sabatons,307,49,12,0,4,1
            4246, // 3,6050,1,11,Foot Gloria,307,55,12,0,3,1
            5269, // 3,6050,1,11,Foot Gloria,307,55,12,0,4,1
            4247, // 3,4610,1,11,High Scale Whisker,307,52,12,0,3,1
            5270, // 3,4610,1,11,High Scale Whisker,307,52,12,0,4,1
        };

        private static readonly List<uint> LowQualityTankArmor = new List<uint>()
        {
            490, // 3,0,1,1,Supply Vest,304,1,0,1,0,1
            2102, // 3,30,1,1,Supply Vest,304,1,0,1,1,1
            3125, // 3,30,1,1,Supply Vest,304,1,0,1,2,1
            493, // 3,0,1,1,Supply Bottoms,307,1,0,0,0,1
            2105, // 3,30,1,1,Supply Bottoms,307,1,0,0,1,1
            3128, // 3,30,1,1,Supply Bottoms,307,1,0,0,2,1
            470, // 3,30,1,1,Round Helm,303,3,17,0,0,1
            2082, // 3,30,1,1,Round Helm,303,3,17,0,1,1
            3105, // 3,30,1,1,Round Helm,303,3,17,0,2,1
            471, // 3,60,1,2,Garrison's Helmet,303,6,17,0,0,1
            2083, // 3,60,1,2,Garrison's Helmet,303,6,17,0,1,1
            3106, // 3,60,1,2,Garrison's Helmet,303,6,17,0,2,1
            472, // 3,270,1,3,Noble's Helm,303,13,17,0,0,1
            2084, // 3,270,1,3,Noble's Helm,303,13,17,0,1,1
            3107, // 3,270,1,3,Noble's Helm,303,13,17,0,2,1
            473, // 3,410,1,4,Sentry Helm,303,16,17,0,0,1
            2085, // 3,410,1,4,Sentry Helm,303,16,17,0,1,1
            3108, // 3,410,1,4,Sentry Helm,303,16,17,0,2,1
            839, // 3,775,1,5,Quest Helm,303,22,17,0,0,1
            2451, // 3,775,1,5,Quest Helm,303,22,17,0,1,1
            3474, // 3,775,1,5,Quest Helm,303,22,17,0,2,1
            840, // 3,920,1,5,Another Eyes,303,24,17,0,0,1
            2452, // 3,920,1,5,Another Eyes,303,24,17,0,1,1
            3475, // 3,920,1,5,Another Eyes,303,24,17,0,2,1
            841, // 3,2560,1,8,Ddraig Goch Frons,303,40,17,0,0,1
            2453, // 3,2560,1,8,Ddraig Goch Frons,303,40,17,0,1,1
            3476, // 3,2560,1,8,Ddraig Goch Frons,303,40,17,0,2,1
            842, // 3,1080,1,6,Sergeant's Helm,303,26,17,0,0,1
            2454, // 3,1080,1,6,Sergeant's Helm,303,26,17,0,1,1
            3477, // 3,1080,1,6,Sergeant's Helm,303,26,17,0,2,1
            843, // 3,1255,1,6,Showdown Helm,303,28,17,0,0,1
            2455, // 3,1255,1,6,Showdown Helm,303,28,17,0,1,1
            3478, // 3,1255,1,6,Showdown Helm,303,28,17,0,2,1
            844, // 3,1440,1,6,Guardian Helm,303,30,17,0,0,1
            2456, // 3,1440,1,6,Guardian Helm,303,30,17,0,1,1
            3479, // 3,1440,1,6,Guardian Helm,303,30,17,0,2,1
            845, // 3,1640,1,7,Advance Helm,303,32,17,0,0,1
            2457, // 3,1640,1,7,Advance Helm,303,32,17,0,1,1
            3480, // 3,1640,1,7,Advance Helm,303,32,17,0,2,1
            846, // 3,1850,1,7,Accomplished Helm,303,34,17,0,0,1
            2458, // 3,1850,1,7,Accomplished Helm,303,34,17,0,1,1
            3481, // 3,1850,1,7,Accomplished Helm,303,34,17,0,2,1
            847, // 3,2075,1,8,Grand Sage Cap,303,36,17,0,0,1
            2459, // 3,2075,1,8,Grand Sage Cap,303,36,17,0,1,1
            3482, // 3,2075,1,8,Grand Sage Cap,303,36,17,0,2,1
            848, // 3,2075,1,8,Principal Helm,303,38,17,0,0,1
            2460, // 3,2075,1,8,Principal Helm,303,38,17,0,1,1
            3483, // 3,2075,1,8,Principal Helm,303,38,17,0,2,1
            849, // 3,2690,1,9,General's Helm,303,41,17,0,0,1
            2461, // 3,2690,1,9,General's Helm,303,41,17,0,1,1
            3484, // 3,2690,1,9,General's Helm,303,41,17,0,2,1
            850, // 3,2820,1,9,Triumph Helm,303,43,17,0,0,1
            2462, // 3,2820,1,9,Triumph Helm,303,43,17,0,1,1
            3485, // 3,2820,1,9,Triumph Helm,303,43,17,0,2,1
            851, // 3,2960,1,9,Greater Eyes,303,45,17,0,0,1
            2463, // 3,2960,1,9,Greater Eyes,303,45,17,0,1,1
            3486, // 3,2960,1,9,Greater Eyes,303,45,17,0,2,1
            854, // 3,4840,1,11,Chevalier Helm,303,55,17,0,0,1
            2466, // 3,4840,1,11,Chevalier Helm,303,55,17,0,1,1
            3489, // 3,4840,1,11,Chevalier Helm,303,55,17,0,2,1
            855, // 3,3685,1,11,Helm of Concealment,303,55,17,0,0,1
            2467, // 3,3685,1,11,Helm of Concealment,303,55,17,0,1,1
            3490, // 3,3685,1,11,Helm of Concealment,303,55,17,0,2,1
            474, // 3,52,1,1,Sage's Robe,304,3,17,1,0,1
            2086, // 3,52,1,1,Sage's Robe,304,3,17,1,1,1
            3109, // 3,52,1,1,Sage's Robe,304,3,17,1,2,1
            475, // 3,105,1,2,Garrison's Jacket,304,6,17,1,0,1
            2087, // 3,105,1,2,Garrison's Jacket,304,6,17,1,1,1
            3110, // 3,105,1,2,Garrison's Jacket,304,6,17,1,2,1
            476, // 3,475,1,3,Noble Jacket,304,13,17,1,0,1
            2088, // 3,475,1,3,Noble Jacket,304,13,17,1,1,1
            3111, // 3,475,1,3,Noble Jacket,304,13,17,1,2,1
            477, // 3,715,1,4,Calm Robe,304,16,17,1,0,1
            2089, // 3,715,1,4,Calm Robe,304,16,17,1,1,1
            3112, // 3,715,1,4,Calm Robe,304,16,17,1,2,1
            865, // 3,1355,1,5,Majesty Drapes,304,22,17,1,0,1
            2477, // 3,1355,1,5,Majesty Drapes,304,22,17,1,1,1
            3500, // 3,1355,1,5,Majesty Drapes,304,22,17,1,2,1
            866, // 3,1615,1,5,Strong Jacket,304,24,17,1,0,1
            2478, // 3,1615,1,5,Strong Jacket,304,24,17,1,1,1
            3501, // 3,1615,1,5,Strong Jacket,304,24,17,1,2,1
            867, // 3,4480,1,8,Ddraig Goch Pectus,304,40,17,1,0,1
            2479, // 3,4480,1,8,Ddraig Goch Pectus,304,40,17,1,1,1
            3502, // 3,4480,1,8,Ddraig Goch Pectus,304,40,17,1,2,1
            868, // 3,1895,1,6,Valiant Coat,304,26,17,1,0,1
            2480, // 3,1895,1,6,Valiant Coat,304,26,17,1,1,1
            3503, // 3,1895,1,6,Valiant Coat,304,26,17,1,2,1
            869, // 3,2195,1,6,Showdown Jacket,304,28,17,1,0,1
            2481, // 3,2195,1,6,Showdown Jacket,304,28,17,1,1,1
            3504, // 3,2195,1,6,Showdown Jacket,304,28,17,1,2,1
            870, // 3,2520,1,6,Guardian Coat,304,30,17,1,0,1
            2482, // 3,2520,1,6,Guardian Coat,304,30,17,1,1,1
            3505, // 3,2520,1,6,Guardian Coat,304,30,17,1,2,1
            871, // 3,2870,1,7,Advance Coat,304,32,17,1,0,1
            2483, // 3,2870,1,7,Advance Coat,304,32,17,1,1,1
            3506, // 3,2870,1,7,Advance Coat,304,32,17,1,2,1
            872, // 3,3235,1,7,Resolver Clothes,304,34,17,1,0,1
            2484, // 3,3235,1,7,Resolver Clothes,304,34,17,1,1,1
            3507, // 3,3235,1,7,Resolver Clothes,304,34,17,1,2,1
            873, // 3,3630,1,8,Tracer's Jacket,304,36,17,1,0,1
            2485, // 3,3630,1,8,Tracer's Jacket,304,36,17,1,1,1
            3508, // 3,3630,1,8,Tracer's Jacket,304,36,17,1,2,1
            874, // 3,4045,1,8,Tactician's Coat,304,38,17,1,0,1
            2486, // 3,4045,1,8,Tactician's Coat,304,38,17,1,1,1
            3509, // 3,4045,1,8,Tactician's Coat,304,38,17,1,2,1
            875, // 3,4705,1,9,Trooper's Coat,304,41,17,1,0,1
            2487, // 3,4705,1,9,Trooper's Coat,304,41,17,1,1,1
            3510, // 3,4705,1,9,Trooper's Coat,304,41,17,1,2,1
            876, // 3,4940,1,9,Triumph Jacket,304,43,17,1,0,1
            2488, // 3,4940,1,9,Triumph Jacket,304,43,17,1,1,1
            3511, // 3,4940,1,9,Triumph Jacket,304,43,17,1,2,1
            877, // 3,5180,1,9,Enforcer's Coat,304,45,17,1,0,1
            2489, // 3,5180,1,9,Enforcer's Coat,304,45,17,1,1,1
            3512, // 3,5180,1,9,Enforcer's Coat,304,45,17,1,2,1
            482, // 3,30,1,1,Entry Gloves,306,3,17,0,0,1
            2094, // 3,30,1,1,Entry Gloves,306,3,17,0,1,1
            3117, // 3,30,1,1,Entry Gloves,306,3,17,0,2,1
            483, // 3,60,1,2,Hard Knuckles,306,6,17,0,0,1
            2095, // 3,60,1,2,Hard Knuckles,306,6,17,0,1,1
            3118, // 3,60,1,2,Hard Knuckles,306,6,17,0,2,1
            484, // 3,270,1,3,Noble Arm,306,13,17,0,0,1
            2096, // 3,270,1,3,Noble Arm,306,13,17,0,1,1
            3119, // 3,270,1,3,Noble Arm,306,13,17,0,2,1
            485, // 3,410,1,4,Powered Gloves,306,16,17,0,0,1
            2097, // 3,410,1,4,Powered Gloves,306,16,17,0,1,1
            3120, // 3,410,1,4,Powered Gloves,306,16,17,0,2,1
            917, // 3,775,1,5,Buster Knuckles,306,22,17,0,0,1
            2529, // 3,775,1,5,Buster Knuckles,306,22,17,0,1,1
            3552, // 3,775,1,5,Buster Knuckles,306,22,17,0,2,1
            918, // 3,920,1,5,Strong Arms,306,24,17,0,0,1
            2530, // 3,920,1,5,Strong Arms,306,24,17,0,1,1
            3553, // 3,920,1,5,Strong Arms,306,24,17,0,2,1
            919, // 3,2560,1,8,Ddraig Palmae,306,40,17,0,0,1
            2531, // 3,2560,1,8,Ddraig Palmae,306,40,17,0,1,1
            3554, // 3,2560,1,8,Ddraig Palmae,306,40,17,0,2,1
            920, // 3,1080,1,6,Deft Gloves,306,26,17,0,0,1
            2532, // 3,1080,1,6,Deft Gloves,306,26,17,0,1,1
            3555, // 3,1080,1,6,Deft Gloves,306,26,17,0,2,1
            921, // 3,1255,1,6,Battle Arms,306,28,17,0,0,1
            2533, // 3,1255,1,6,Battle Arms,306,28,17,0,1,1
            3556, // 3,1255,1,6,Battle Arms,306,28,17,0,2,1
            922, // 3,1440,1,6,Guardian Arms,306,30,17,0,0,1
            2534, // 3,1440,1,6,Guardian Arms,306,30,17,0,1,1
            3557, // 3,1440,1,6,Guardian Arms,306,30,17,0,2,1
            923, // 3,1640,1,7,Custodian Arms,306,32,17,0,0,1
            2535, // 3,1640,1,7,Custodian Arms,306,32,17,0,1,1
            3558, // 3,1640,1,7,Custodian Arms,306,32,17,0,2,1
            924, // 3,1850,1,7,Resolver Arms,306,34,17,0,0,1
            2536, // 3,1850,1,7,Resolver Arms,306,34,17,0,1,1
            3559, // 3,1850,1,7,Resolver Arms,306,34,17,0,2,1
            925, // 3,2075,1,8,Stalwart Arms,306,36,17,0,0,1
            2537, // 3,2075,1,8,Stalwart Arms,306,36,17,0,1,1
            3560, // 3,2075,1,8,Stalwart Arms,306,36,17,0,2,1
            926, // 3,2310,1,8,Principal Arms,306,38,17,0,0,1
            2538, // 3,2310,1,8,Principal Arms,306,38,17,0,1,1
            3561, // 3,2310,1,8,Principal Arms,306,38,17,0,2,1
            927, // 3,2690,1,9,Trooper's Gloves,306,41,17,0,0,1
            2539, // 3,2690,1,9,Trooper's Gloves,306,41,17,0,1,1
            3562, // 3,2690,1,9,Trooper's Gloves,306,41,17,0,2,1
            928, // 3,2820,1,9,Triumph Arms,306,43,17,0,0,1
            2540, // 3,2820,1,9,Triumph Arms,306,43,17,0,1,1
            3563, // 3,2820,1,9,Triumph Arms,306,43,17,0,2,1
            929, // 3,2960,1,9,Gruel Knuckles,306,45,17,0,0,1
            2541, // 3,2960,1,9,Gruel Knuckles,306,45,17,0,1,1
            3564, // 3,2960,1,9,Gruel Knuckles,306,45,17,0,2,1
            478, // 3,37,1,1,Entry Boots,307,3,17,0,0,1
            2090, // 3,37,1,1,Entry Boots,307,3,17,0,1,1
            3113, // 3,37,1,1,Entry Boots,307,3,17,0,2,1
            479, // 3,75,1,2,Garrison's Boots,307,6,17,0,0,1
            2091, // 3,75,1,2,Garrison's Boots,307,6,17,0,1,1
            3114, // 3,75,1,2,Garrison's Boots,307,6,17,0,2,1
            480, // 3,340,1,3,Noble Greaves,307,13,17,0,0,1
            2092, // 3,340,1,3,Noble Greaves,307,13,17,0,1,1
            3115, // 3,340,1,3,Noble Greaves,307,13,17,0,2,1
            481, // 3,510,1,4,Sentry Boots,307,16,17,0,0,1
            2093, // 3,510,1,4,Sentry Boots,307,16,17,0,1,1
            3116, // 3,510,1,4,Sentry Boots,307,16,17,0,2,1
            892, // 3,970,1,5,Valor Legs,307,22,17,0,0,1
            2504, // 3,970,1,5,Valor Legs,307,22,17,0,1,1
            3527, // 3,970,1,5,Valor Legs,307,22,17,0,2,1
            893, // 3,1150,1,5,Strong Legs,307,24,17,0,0,1
            2505, // 3,1150,1,5,Strong Legs,307,24,17,0,1,1
            3528, // 3,1150,1,5,Strong Legs,307,24,17,0,2,1
            894, // 3,3200,1,8,Ddraig Crura,307,40,17,0,0,1
            2506, // 3,3200,1,8,Ddraig Crura,307,40,17,0,1,1
            3529, // 3,3200,1,8,Ddraig Crura,307,40,17,0,2,1
            895, // 3,1350,1,6,Mudguards,307,26,17,0,0,1
            2507, // 3,1350,1,6,Mudguards,307,26,17,0,1,1
            3530, // 3,1350,1,6,Mudguards,307,26,17,0,2,1
            896, // 3,1570,1,6,Battle Legs,307,28,17,0,0,1
            2508, // 3,1570,1,6,Battle Legs,307,28,17,0,1,1
            3531, // 3,1570,1,6,Battle Legs,307,28,17,0,2,1
            897, // 3,1800,1,6,Guardian Legs,307,30,17,0,0,1
            2509, // 3,1800,1,6,Guardian Legs,307,30,17,0,1,1
            3532, // 3,1800,1,6,Guardian Legs,307,30,17,0,2,1
            898, // 3,2050,1,7,Custodian Boots,307,32,17,0,0,1
            2510, // 3,2050,1,7,Custodian Boots,307,32,17,0,1,1
            3533, // 3,2050,1,7,Custodian Boots,307,32,17,0,2,1
            899, // 3,2310,1,7,Resolver Legs,307,34,17,0,0,1
            2511, // 3,2310,1,7,Resolver Legs,307,34,17,0,1,1
            3534, // 3,2310,1,7,Resolver Legs,307,34,17,0,2,1
            900, // 3,2590,1,8,Hearty Boots,307,36,17,0,0,1
            2512, // 3,2590,1,8,Hearty Boots,307,36,17,0,1,1
            3535, // 3,2590,1,8,Hearty Boots,307,36,17,0,2,1
            901, // 3,2890,1,8,Principal Legs,307,38,17,0,0,1
            2513, // 3,2890,1,8,Principal Legs,307,38,17,0,1,1
            3536, // 3,2890,1,8,Principal Legs,307,38,17,0,2,1
            902, // 3,3360,1,9,Trooper's Legs,307,41,17,0,0,1
            2514, // 3,3360,1,9,Trooper's Legs,307,41,17,0,1,1
            3537, // 3,3360,1,9,Trooper's Legs,307,41,17,0,2,1
            903, // 3,3530,1,9,Triumph Greaves,307,43,17,0,0,1
            2515, // 3,3530,1,9,Triumph Greaves,307,43,17,0,1,1
            3538, // 3,3530,1,9,Triumph Greaves,307,43,17,0,2,1
            904, // 3,3700,1,9,Bounty Boots,307,45,17,0,0,1
            2516, // 3,3700,1,9,Bounty Boots,307,45,17,0,1,1
            3539, // 3,3700,1,9,Bounty Boots,307,45,17,0,2,1
        };

        private static readonly List<uint> HighQualityTankArmor = new List<uint>()
        {
            4148, // 3,30,1,1,Supply Vest,304,1,0,1,3,1
            5171, // 3,30,1,1,Supply Vest,304,1,0,1,4,1
            4151, // 3,30,1,1,Supply Bottoms,307,1,0,0,3,1
            5174, // 3,30,1,1,Supply Bottoms,307,1,0,0,4,1
            4128, // 3,30,1,1,Round Helm,303,3,17,0,3,1
            5151, // 3,30,1,1,Round Helm,303,3,17,0,4,1
            4129, // 3,60,1,2,Garrison's Helmet,303,6,17,0,3,1
            5152, // 3,60,1,2,Garrison's Helmet,303,6,17,0,4,1
            4130, // 3,270,1,3,Noble's Helm,303,13,17,0,3,1
            5153, // 3,270,1,3,Noble's Helm,303,13,17,0,4,1
            4131, // 3,410,1,4,Sentry Helm,303,16,17,0,3,1
            5154, // 3,410,1,4,Sentry Helm,303,16,17,0,4,1
            4497, // 3,775,1,5,Quest Helm,303,22,17,0,3,1
            5520, // 3,775,1,5,Quest Helm,303,22,17,0,4,1
            4498, // 3,920,1,5,Another Eyes,303,24,17,0,3,1
            5521, // 3,920,1,5,Another Eyes,303,24,17,0,4,1
            4499, // 3,2560,1,8,Ddraig Goch Frons,303,40,17,0,3,1
            5522, // 3,2560,1,8,Ddraig Goch Frons,303,40,17,0,4,1
            4500, // 3,1080,1,6,Sergeant's Helm,303,26,17,0,3,1
            5523, // 3,1080,1,6,Sergeant's Helm,303,26,17,0,4,1
            4501, // 3,1255,1,6,Showdown Helm,303,28,17,0,3,1
            5524, // 3,1255,1,6,Showdown Helm,303,28,17,0,4,1
            4502, // 3,1440,1,6,Guardian Helm,303,30,17,0,3,1
            5525, // 3,1440,1,6,Guardian Helm,303,30,17,0,4,1
            4503, // 3,1640,1,7,Advance Helm,303,32,17,0,3,1
            5526, // 3,1640,1,7,Advance Helm,303,32,17,0,4,1
            4504, // 3,1850,1,7,Accomplished Helm,303,34,17,0,3,1
            5527, // 3,1850,1,7,Accomplished Helm,303,34,17,0,4,1
            4505, // 3,2075,1,8,Grand Sage Cap,303,36,17,0,3,1
            5528, // 3,2075,1,8,Grand Sage Cap,303,36,17,0,4,1
            4506, // 3,2075,1,8,Principal Helm,303,38,17,0,3,1
            5529, // 3,2075,1,8,Principal Helm,303,38,17,0,4,1
            4507, // 3,2690,1,9,General's Helm,303,41,17,0,3,1
            5530, // 3,2690,1,9,General's Helm,303,41,17,0,4,1
            4508, // 3,2820,1,9,Triumph Helm,303,43,17,0,3,1
            5531, // 3,2820,1,9,Triumph Helm,303,43,17,0,4,1
            4509, // 3,2960,1,9,Greater Eyes,303,45,17,0,3,1
            5532, // 3,2960,1,9,Greater Eyes,303,45,17,0,4,1
            4512, // 3,4840,1,11,Chevalier Helm,303,55,17,0,3,1
            5535, // 3,4840,1,11,Chevalier Helm,303,55,17,0,4,1
            4513, // 3,3685,1,11,Helm of Concealment,303,55,17,0,3,1
            5536, // 3,3685,1,11,Helm of Concealment,303,55,17,0,4,1
            4132, // 3,52,1,1,Sage's Robe,304,3,17,1,3,1
            5155, // 3,52,1,1,Sage's Robe,304,3,17,1,4,1
            4133, // 3,105,1,2,Garrison's Jacket,304,6,17,1,3,1
            5156, // 3,105,1,2,Garrison's Jacket,304,6,17,1,4,1
            4134, // 3,475,1,3,Noble Jacket,304,13,17,1,3,1
            5157, // 3,475,1,3,Noble Jacket,304,13,17,1,4,1
            4135, // 3,715,1,4,Calm Robe,304,16,17,1,3,1
            5158, // 3,715,1,4,Calm Robe,304,16,17,1,4,1
            4523, // 3,1355,1,5,Majesty Drapes,304,22,17,1,3,1
            5546, // 3,1355,1,5,Majesty Drapes,304,22,17,1,4,1
            4524, // 3,1615,1,5,Strong Jacket,304,24,17,1,3,1
            5547, // 3,1615,1,5,Strong Jacket,304,24,17,1,4,1
            4525, // 3,4480,1,8,Ddraig Goch Pectus,304,40,17,1,3,1
            5548, // 3,4480,1,8,Ddraig Goch Pectus,304,40,17,1,4,1
            4526, // 3,1895,1,6,Valiant Coat,304,26,17,1,3,1
            5549, // 3,1895,1,6,Valiant Coat,304,26,17,1,4,1
            4527, // 3,2195,1,6,Showdown Jacket,304,28,17,1,3,1
            5550, // 3,2195,1,6,Showdown Jacket,304,28,17,1,4,1
            4528, // 3,2520,1,6,Guardian Coat,304,30,17,1,3,1
            5551, // 3,2520,1,6,Guardian Coat,304,30,17,1,4,1
            4529, // 3,2870,1,7,Advance Coat,304,32,17,1,3,1
            5552, // 3,2870,1,7,Advance Coat,304,32,17,1,4,1
            4530, // 3,3235,1,7,Resolver Clothes,304,34,17,1,3,1
            5553, // 3,3235,1,7,Resolver Clothes,304,34,17,1,4,1
            4531, // 3,3630,1,8,Tracer's Jacket,304,36,17,1,3,1
            5554, // 3,3630,1,8,Tracer's Jacket,304,36,17,1,4,1
            4532, // 3,4045,1,8,Tactician's Coat,304,38,17,1,3,1
            5555, // 3,4045,1,8,Tactician's Coat,304,38,17,1,4,1
            4533, // 3,4705,1,9,Trooper's Coat,304,41,17,1,3,1
            5556, // 3,4705,1,9,Trooper's Coat,304,41,17,1,4,1
            4534, // 3,4940,1,9,Triumph Jacket,304,43,17,1,3,1
            5557, // 3,4940,1,9,Triumph Jacket,304,43,17,1,4,1
            4535, // 3,5180,1,9,Enforcer's Coat,304,45,17,1,3,1
            5558, // 3,5180,1,9,Enforcer's Coat,304,45,17,1,4,1
            4140, // 3,30,1,1,Entry Gloves,306,3,17,0,3,1
            5163, // 3,30,1,1,Entry Gloves,306,3,17,0,4,1
            4141, // 3,60,1,2,Hard Knuckles,306,6,17,0,3,1
            5164, // 3,60,1,2,Hard Knuckles,306,6,17,0,4,1
            4142, // 3,270,1,3,Noble Arm,306,13,17,0,3,1
            5165, // 3,270,1,3,Noble Arm,306,13,17,0,4,1
            4143, // 3,410,1,4,Powered Gloves,306,16,17,0,3,1
            5166, // 3,410,1,4,Powered Gloves,306,16,17,0,4,1
            4575, // 3,775,1,5,Buster Knuckles,306,22,17,0,3,1
            5598, // 3,775,1,5,Buster Knuckles,306,22,17,0,4,1
            4576, // 3,920,1,5,Strong Arms,306,24,17,0,3,1
            5599, // 3,920,1,5,Strong Arms,306,24,17,0,4,1
            4577, // 3,2560,1,8,Ddraig Palmae,306,40,17,0,3,1
            5600, // 3,2560,1,8,Ddraig Palmae,306,40,17,0,4,1
            4578, // 3,1080,1,6,Deft Gloves,306,26,17,0,3,1
            5601, // 3,1080,1,6,Deft Gloves,306,26,17,0,4,1
            4579, // 3,1255,1,6,Battle Arms,306,28,17,0,3,1
            5602, // 3,1255,1,6,Battle Arms,306,28,17,0,4,1
            4580, // 3,1440,1,6,Guardian Arms,306,30,17,0,3,1
            5603, // 3,1440,1,6,Guardian Arms,306,30,17,0,4,1
            4581, // 3,1640,1,7,Custodian Arms,306,32,17,0,3,1
            5604, // 3,1640,1,7,Custodian Arms,306,32,17,0,4,1
            4582, // 3,1850,1,7,Resolver Arms,306,34,17,0,3,1
            5605, // 3,1850,1,7,Resolver Arms,306,34,17,0,4,1
            4583, // 3,2075,1,8,Stalwart Arms,306,36,17,0,3,1
            5606, // 3,2075,1,8,Stalwart Arms,306,36,17,0,4,1
            4584, // 3,2310,1,8,Principal Arms,306,38,17,0,3,1
            5607, // 3,2310,1,8,Principal Arms,306,38,17,0,4,1
            4585, // 3,2690,1,9,Trooper's Gloves,306,41,17,0,3,1
            5608, // 3,2690,1,9,Trooper's Gloves,306,41,17,0,4,1
            4586, // 3,2820,1,9,Triumph Arms,306,43,17,0,3,1
            5609, // 3,2820,1,9,Triumph Arms,306,43,17,0,4,1
            4587, // 3,2960,1,9,Gruel Knuckles,306,45,17,0,3,1
            5610, // 3,2960,1,9,Gruel Knuckles,306,45,17,0,4,1
            4136, // 3,37,1,1,Entry Boots,307,3,17,0,3,1
            5159, // 3,37,1,1,Entry Boots,307,3,17,0,4,1
            4137, // 3,75,1,2,Garrison's Boots,307,6,17,0,3,1
            5160, // 3,75,1,2,Garrison's Boots,307,6,17,0,4,1
            4138, // 3,340,1,3,Noble Greaves,307,13,17,0,3,1
            5161, // 3,340,1,3,Noble Greaves,307,13,17,0,4,1
            4139, // 3,510,1,4,Sentry Boots,307,16,17,0,3,1
            5162, // 3,510,1,4,Sentry Boots,307,16,17,0,4,1
            4550, // 3,970,1,5,Valor Legs,307,22,17,0,3,1
            5573, // 3,970,1,5,Valor Legs,307,22,17,0,4,1
            4551, // 3,1150,1,5,Strong Legs,307,24,17,0,3,1
            5574, // 3,1150,1,5,Strong Legs,307,24,17,0,4,1
            4552, // 3,3200,1,8,Ddraig Crura,307,40,17,0,3,1
            5575, // 3,3200,1,8,Ddraig Crura,307,40,17,0,4,1
            4553, // 3,1350,1,6,Mudguards,307,26,17,0,3,1
            5576, // 3,1350,1,6,Mudguards,307,26,17,0,4,1
            4554, // 3,1570,1,6,Battle Legs,307,28,17,0,3,1
            5577, // 3,1570,1,6,Battle Legs,307,28,17,0,4,1
            4555, // 3,1800,1,6,Guardian Legs,307,30,17,0,3,1
            5578, // 3,1800,1,6,Guardian Legs,307,30,17,0,4,1
            4556, // 3,2050,1,7,Custodian Boots,307,32,17,0,3,1
            5579, // 3,2050,1,7,Custodian Boots,307,32,17,0,4,1
            4557, // 3,2310,1,7,Resolver Legs,307,34,17,0,3,1
            5580, // 3,2310,1,7,Resolver Legs,307,34,17,0,4,1
            4558, // 3,2590,1,8,Hearty Boots,307,36,17,0,3,1
            5581, // 3,2590,1,8,Hearty Boots,307,36,17,0,4,1
            4559, // 3,2890,1,8,Principal Legs,307,38,17,0,3,1
            5582, // 3,2890,1,8,Principal Legs,307,38,17,0,4,1
            4560, // 3,3360,1,9,Trooper's Legs,307,41,17,0,3,1
            5583, // 3,3360,1,9,Trooper's Legs,307,41,17,0,4,1
            4561, // 3,3530,1,9,Triumph Greaves,307,43,17,0,3,1
            5584, // 3,3530,1,9,Triumph Greaves,307,43,17,0,4,1
            4562, // 3,3700,1,9,Bounty Boots,307,45,17,0,3,1
            5585, // 3,3700,1,9,Bounty Boots,307,45,17,0,4,1
        };

        private static readonly Dictionary<JobId, List<uint>> gLowQualityArmors = new Dictionary<JobId, List<uint>>()
        {
            [JobId.Fighter] = LowQualityFighterWarriorArmor,
            [JobId.Warrior] = LowQualityFighterWarriorArmor,
            [JobId.Seeker] = LowQualityHunterSeekerSpiritArmor,
            [JobId.Hunter] = LowQualityHunterSeekerSpiritArmor,
            [JobId.SpiritLancer] = LowQualityHunterSeekerSpiritArmor,
            [JobId.Priest] = LowQualityMageGear,
            [JobId.Sorcerer] = LowQualityMageGear,
            [JobId.ElementArcher] = LowQualityMageGear,
            [JobId.ShieldSage] = LowQualityTankArmor,
            [JobId.Alchemist] = LowQualityTankArmor,
            [JobId.HighScepter] = LowQualityTankArmor,
        };

        private static readonly Dictionary<JobId, List<uint>> gHighQualityArmors = new Dictionary<JobId, List<uint>>()
        {
            [JobId.Fighter] = HighQualityFighterWarriorArmor,
            [JobId.Warrior] = HighQualityFighterWarriorArmor,
            [JobId.Seeker] = HighQualityHunterSeekerSpiritArmor,
            [JobId.Hunter] = HighQualityHunterSeekerSpiritArmor,
            [JobId.SpiritLancer] = HighQualityHunterSeekerSpiritArmor,
            [JobId.Priest] = HighQualityMageGear,
            [JobId.Sorcerer] = HighQualityMageGear,
            [JobId.ElementArcher] = HighQualityMageGear,
            [JobId.ShieldSage] = HighQualityTankArmor,
            [JobId.Alchemist] = HighQualityTankArmor,
            [JobId.HighScepter] = HighQualityTankArmor,
        };
    }
}
