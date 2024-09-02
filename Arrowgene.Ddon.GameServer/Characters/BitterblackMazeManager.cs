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

            var rewards = server.Database.SelectBBMRewards(character.CharacterId);

            var availableRewards = new List<CDataBattleContentAvailableRewards>();
            var trackedRewards = server.Database.SelectBBMContentTreasure(character.CharacterId);
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
                    ContentName = (match.ContentMode == BattleContentMode.Rotunda) ? "Bitterblack Maze Rotunda" : "Bitterblack Maze Abyss",
                    ClearTime = (ulong) (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (long) progress.StartTime)
                };
                client.Send(clearNtc);

                progress.ContentId = 0;
                progress.Tier = 0;
            }
            server.Database.UpdateBBMProgress(character.CharacterId, progress);

            var rewards = server.Database.SelectBBMRewards(character.CharacterId);
            // TODO: handle BattleContentRewardBonus.Up (some sort of reward bonus)
            // TODO: Is there a reason we wouldn't get a reward here?
            rewards.GoldMarks += 1;
            rewards.SilverMarks += 5;
            rewards.RedMarks += 15;
            server.Database.UpdateBBMRewards(character.CharacterId, rewards);

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

            var assets = server.AssetRepository.BitterblackMazeAsset;

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
                var treasure = server.Database.SelectBBMContentTreasure(character.CharacterId).Where(x => x.ContentId == character.BbmProgress.ContentId).ToList();
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
                            ItemId = itemId,
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

        private static (uint Min, uint Max) DetermineItemTier(DdonGameServer server, ChestType chestType, StageId stageId)
        {
            var lootRange = server.AssetRepository.BitterblackMazeAsset.LootRanges[stageId.Id];
            switch (chestType)
            {
                case ChestType.Orange:
                case ChestType.Purple:
                    return lootRange.SealedRange;
                default:
                    return lootRange.NormalRange;
            }
        }

        private static uint SelectGear(DdonGameServer server, List<uint> items, ChestType chestType, StageId stageId)
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

        private static uint RollRareArmor(StageId stageId)
        {
            uint itemId = 0;
            var dropTable = StageManager.IsBitterBlackMazeBossStageId(stageId) ? gRareRotundaDrops : gRareAbyssNormalDrops;
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

        private static readonly List<uint> gRareRotundaDrops = new List<uint>()
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
    }
}
