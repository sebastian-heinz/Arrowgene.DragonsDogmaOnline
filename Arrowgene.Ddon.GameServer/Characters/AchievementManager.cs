using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class AchievementManager
    {
        private readonly DdonGameServer Server;

        public AchievementManager(DdonGameServer server)
        {
            Server = server;

            if (!EnemyIdParamLookup.Any())
            {
                foreach ((var type, var set) in KillEnemyTypeMap)
                {
                    foreach (var id in set)
                    {
                        if (!EnemyIdParamLookup.ContainsKey(id))
                        {
                            EnemyIdParamLookup[id] = new();
                        }
                        EnemyIdParamLookup[id].Add(type);
                    }
                }
            }
        }

        #region HandleAchievements
        public PacketQueue HandleAppraisal(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.Appraisal, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });
            
            return queue;
        }

        public PacketQueue HandleChangeColor(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.ChangeColor, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleClearBBM(GameClient client, bool isAbyss, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key;
            if (isAbyss)
            {
                key = (AchievementType.ClearBBM, (uint) AchievementBBMParam.Abyss);
            }
            else
            {
                key = (AchievementType.ClearBBM, (uint)AchievementBBMParam.Normal);
            }
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleClearQuest(GameClient client, Quest quest, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.ClearQuestType, (uint) GetClearQuestTypeSubtype(quest));

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                if (key.Item2 > 0)
                {
                    uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
                    progress++;
                    client.Character.AchievementProgress[key] = progress;
                    Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);

                    queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
                }

                uint clearCount = client.Character.CompletedQuests.GetValueOrDefault(quest.QuestId)?.ClearCount ?? 0;
                queue.AddRange(CheckGainAchievement(client, AchievementType.ClearQuest, (uint)quest.QuestId, clearCount, connection));
            });

            return queue;
        }

        public PacketQueue HandleCollect(GameClient client, AchievementCollectParam collectType, DbConnection? connectionIn = null)
        {
            // TODO: Need the information about which gathering points are which.
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.CollectType, (uint)collectType);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleCraft(GameClient client, ClientItemInfo item, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            AchievementCraftTypeParam param = GetCraftTypeSubtype(item);
            (AchievementType, uint) key = (AchievementType.CraftType, (uint)param);
            (AchievementType, uint) uniqueKey = (AchievementType.CraftTypeUnique, (uint)param);

            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            if (!client.Character.AchievementUniqueCrafts.ContainsKey(param))
            {
                client.Character.AchievementUniqueCrafts[param] = new();
            }
            client.Character.AchievementUniqueCrafts[param].Add((ItemId)item.ItemId);

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));

                if (Server.Database.InsertAchievementUniqueCraft(client.Character.CharacterId, param, (ItemId)item.ItemId, connection))
                {
                    uint uniqueProgress = (uint)client.Character.AchievementUniqueCrafts[param].Count;
                    queue.AddRange(CheckGainAchievement(client, uniqueKey.Item1, uniqueKey.Item2, uniqueProgress, connection));
                }
            });

            return queue;
        }

        public PacketQueue HandleEmblemStone(GameClient client, uint level, DbConnection? connectionIn = null)
        {
            // TODO: Implement.
            PacketQueue queue = new();
            return queue;
        }

        public PacketQueue HandleEnhanceItem(GameClient client, ClientItemInfo item, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.EnhanceItem, (uint) item.Quality);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleHirePawn(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.HirePawn, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleKillEnemy(GameClient client, Enemy enemy, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            // Handle total kills
            (AchievementType, uint) key = (AchievementType.KillTotalEnemy, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));

                // Handle specific kill groups
                foreach (var group in EnemyIdParamLookup.GetValueOrDefault((EnemyId)enemy.EnemyId, new()))
                {
                    (AchievementType, uint) specificKey = (AchievementType.KillEnemyType, (uint)group);
                    uint specificProgress = client.Character.AchievementProgress.GetValueOrDefault(specificKey);
                    specificProgress++;
                    client.Character.AchievementProgress[specificKey] = specificProgress;
                    Server.Database.UpsertAchievementProgress(client.Character.CharacterId, specificKey.Item1, specificKey.Item2, specificProgress, connection);

                    queue.AddRange(CheckGainAchievement(client, specificKey.Item1, specificKey.Item2, specificProgress, connection));
                }
            });

            return queue;
        }

        public PacketQueue HandleLearnAugments(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.LearnAugments, 0);
            uint progress = (uint) client.Character.LearnedAbilities.Select(x => (int) x.AbilityLv).Sum();

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleLearnSkills(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.LearnAugments, 0);
            uint progress = (uint)client.Character.LearnedCustomSkills.Select(x => (int)x.SkillLv).Sum();

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleMainLevel(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            // Handle single jobs
            (AchievementType, uint) key = (AchievementType.MainLevel, (uint)client.Character.Job);
            uint progress = client.Character.ActiveCharacterJobData.Lv;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));

                // Handle job groups
                foreach ((var groupType, var group) in LevelGroupMap.Where(x => x.Value.Contains(client.Character.Job)))
                {
                    uint lowestLevel = (uint)group.Select(job => client.Character.CharacterJobDataList.Where(x => x.Job == job).Select(x => (int)x.Lv).FirstOrDefault()).Min();
                    (AchievementType, uint) groupKey = (AchievementType.MainLevelGroup, (uint)groupType);

                    queue.AddRange(CheckGainAchievement(client, groupKey.Item1, groupKey.Item2, lowestLevel, connection));
                }
            });

            return queue;
        }

        public PacketQueue HandleMandragoraSpecies(GameClient client, DbConnection? connectionIn = null)
        {
            // TODO: Implement.
            PacketQueue queue = new();
            return queue;
        }

        public PacketQueue HandleMountCrest(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.MountCrest, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleOrbDevote(GameClient client, OrbGainParamType type, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.OrbDevote, (uint) type);
            var progress = (uint)OrbUnlockManager.CountParamType(client.Character).GetValueOrDefault(type);

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandlePawnAffection(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            if (!client.Character.Pawns.Any())
            {
                return queue;
            }

            uint maxAffection = client.Character.Pawns.Select(x => x.PartnerPawnData?.CalculateLikability() ?? 0).DefaultIfEmpty().Max();

            queue.AddRange(CheckGainAchievement(client, AchievementType.PawnAffection, 0, maxAffection, connectionIn));

            return queue;
        }

        public PacketQueue HandlePawnCrafting(GameClient client, Pawn pawn, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.PawnCrafting, 0);
            uint progress = pawn.CraftData.CraftRank;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandlePawnCraftingExam(GameClient client, Pawn pawn, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.PawnCraftingExam, 0);
            uint progress = pawn.CraftData.CraftRankLimit;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandlePawnExpedition(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.PawnExpedition, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandlePawnLevel(GameClient client, Pawn pawn, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            (AchievementType, uint) key = (AchievementType.PawnLevel, (uint)pawn.Job);
            uint progress = pawn.ActiveCharacterJobData.Lv;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandlePawnTraining(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.PawnTraining, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }

        public PacketQueue HandleSparkleCollect(GameClient client, QuestAreaId area, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.SparkleCollect, (uint) area);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;
            

            (AchievementType, uint) allKey = (AchievementType.SparkleCollect, 0);
            uint allProgress = client.Character.AchievementProgress.GetValueOrDefault(allKey);
            allProgress++;
            client.Character.AchievementProgress[allKey] = allProgress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));

                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, allKey.Item1, allKey.Item2, allProgress, connection);
                queue.AddRange(CheckGainAchievement(client, allKey.Item1, allKey.Item2, allProgress, connection));
            });

            return queue;
        }

        public PacketQueue HandleTakePhoto(GameClient client, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            (AchievementType, uint) key = (AchievementType.TakePhoto, 0);
            uint progress = client.Character.AchievementProgress.GetValueOrDefault(key);
            progress++;
            client.Character.AchievementProgress[key] = progress;

            Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                Server.Database.UpsertAchievementProgress(client.Character.CharacterId, key.Item1, key.Item2, progress, connection);
                queue.AddRange(CheckGainAchievement(client, key.Item1, key.Item2, progress, connection));
            });

            return queue;
        }
        #endregion

        private PacketQueue CheckGainAchievement(GameClient client, AchievementType type, uint param, uint count, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            foreach (var asset in Server.AssetRepository.AchievementAsset.GetValueOrDefault((type, param)) ?? new())
            {
                if (count >= asset.Count && !client.Character.AchievementStatus.ContainsKey(asset.Id))
                {
                    client.Character.AchievementStatus.Add(asset.Id, DateTimeOffset.UtcNow);
                    queue.Enqueue(client, new S2CAchievementCompleteNtc()
                    {
                        AchievementIdList = new() { new(asset.Id) }
                    });
                    Server.Database.InsertAchievementStatus(client.Character.CharacterId, asset, asset.RewardId > 0, connectionIn);
                }
            }

            return queue;
        }

        public List<uint> CheckProgress(GameClient client, IEnumerable<AchievementAsset> assets, DbConnection? connectionIn = null)
        { 
            var progress = new List<uint>();

            var orbProgress = OrbUnlockManager.CountParamType(client.Character);

            foreach (var asset in assets)
            {
                // Shortcut math for achievements we know we've completed.
                if (client.Character.AchievementStatus.ContainsKey(asset.Id))
                {
                    progress.Add(uint.MaxValue);
                    continue;
                }

                // TODO: ClearSubstory, EmblemStone, EpitaphRoad, MandragoraSpecies
                switch (asset.Type)
                {
                    case AchievementType.Appraisal:
                    case AchievementType.ChangeColor:
                    case AchievementType.ClearBBM:
                    case AchievementType.ClearQuestType:
                    case AchievementType.CollectType:
                    case AchievementType.CraftType:
                    case AchievementType.EnhanceItem:
                    case AchievementType.HirePawn:
                    case AchievementType.KillEnemyType:
                    case AchievementType.KillTotalEnemy:
                    case AchievementType.MountCrest:
                    case AchievementType.PawnExpedition:
                    case AchievementType.PawnTraining:
                    case AchievementType.SparkleCollect:
                    case AchievementType.TakePhoto:
                        progress.Add(client.Character.AchievementProgress.GetValueOrDefault((asset.Type, asset.Param)));
                        break;
                    case AchievementType.ClearQuest:
                        progress.Add(client.Character.CompletedQuests.GetValueOrDefault((QuestId)asset.Param)?.ClearCount ?? 0);
                        break;
                    case AchievementType.CraftTypeUnique:
                        progress.Add((uint)(client.Character.AchievementUniqueCrafts.GetValueOrDefault((AchievementCraftTypeParam)asset.Param) ?? new()).Count);
                        break;
                    case AchievementType.LearnAugments:
                        progress.Add((uint)client.Character.LearnedAbilities.Select(x => (int)x.AbilityLv).Sum());
                        break;
                    case AchievementType.LearnSkills:
                        progress.Add((uint)client.Character.LearnedCustomSkills.Select(x => (int)x.SkillLv).Sum());
                        break;
                    case AchievementType.MainLevel:
                        progress.Add(client.Character.CharacterJobDataList.Where(x => x.Job == (JobId)asset.Param).FirstOrDefault()?.Lv ?? 0);
                        break;
                    case AchievementType.MainLevelGroup:
                        {
                            HashSet<JobId> group = LevelGroupMap.GetValueOrDefault((AchievementLevelGroupParam)asset.Param);
                            int lowestLevel = group.Select(job => client.Character.CharacterJobDataList.Where(x => x.Job == job).Select(x => (int)x.Lv).FirstOrDefault()).Min();
                            uint result = lowestLevel >= asset.Count ? 1u : 0u;
                            progress.Add(result);
                            break;
                        }
                    case AchievementType.PawnAffection:
                        uint pawnAffection = client.Character.Pawns.Select(x => x.PartnerPawnData?.CalculateLikability() ?? 0).DefaultIfEmpty().Max();
                        progress.Add(pawnAffection >= asset.Count ? 1u : 0u);
                        break;
                    case AchievementType.PawnLevel:
                        progress.Add(client.Character.Pawns
                            .Select(x => x.CharacterJobDataList.Where(x => x.Job == (JobId)asset.Param).FirstOrDefault()?.Lv ?? 0)
                            .DefaultIfEmpty()
                            .Max());
                        break;
                    case AchievementType.PawnCrafting:
                        progress.Add(client.Character.Pawns
                            .Select(x => x.CraftData.CraftRank)
                            .DefaultIfEmpty()
                            .Max());
                        break;
                    case AchievementType.PawnCraftingExam:
                        {
                            var highestCrafting = client.Character.Pawns
                            .Select(x => x.CraftData.CraftRankLimit)
                            .DefaultIfEmpty()
                            .Max();
                            var result = highestCrafting > asset.Count ? 1u : 0u;
                            progress.Add(result);
                            break;
                        }
                    case AchievementType.OrbDevote:
                        {
                            progress.Add((uint)orbProgress.GetValueOrDefault((OrbGainParamType)asset.Param));
                            break;
                        }
                    default:
                        progress.Add(0);
                        break;
                }
            }
            return progress;
        }

        public PacketQueue CalculateProgress(GameClient client)
        {
            PacketQueue queue = new();
            var achievements = Server.AssetRepository.AchievementAsset.SelectMany(x => x.Value).ToList();
            var progress = CheckProgress(client, achievements).ToList();

            var zippedAchievements = Enumerable.Range(0, achievements.Count).Select(i => (
                achievements[i],
                progress[i],
                client.Character.AchievementStatus.GetValueOrDefault(achievements[i].Id)
            ));

            List<AchievementAsset> missedAchievements = new();

            List<CDataAchievementProgress> res = new();
            foreach(var z in zippedAchievements)
            {
                var cdata = new CDataAchievementProgress()
                {
                    AchieveIdentifier = new()
                    {
                        UId = z.Item1.Id,
                        Index = z.Item1.SortId
                    },
                    CurrentNum = z.Item2
                };

                if (client.Character.AchievementStatus.TryGetValue(z.Item1.Id, out var dateAchieved))
                {
                    cdata.CompleteDate = dateAchieved;
                }
                else if (CheckLateAchievement(z))
                {
                    // Check if you missed the DB write here because migrating this is nigh-impossible, so do it as people check.
                    cdata.CompleteDate = DateTimeOffset.UtcNow;
                    missedAchievements.Add(z.Item1);
                    client.Character.AchievementStatus[z.Item1.Id] = DateTimeOffset.UtcNow;
                }
                else
                {
                    cdata.CompleteDate = DateTimeOffset.MaxValue;
                }

                res.Add(cdata);
            }

            client.Enqueue(new S2CAchievementGetProgressListRes() { AchievementProgressList = res }, queue);

            if (missedAchievements.Any())
            {
                Server.Database.ExecuteInTransaction(connection =>
                {
                    foreach (var achievement in missedAchievements)
                    {
                        Server.Database.InsertAchievementStatus(client.Character.CharacterId, achievement, achievement.RewardId > 0, connection);
                        
                        // This can hardcore spam packets just to fill out some log info; probably not necessary.
                        //client.Enqueue(new S2CAchievementCompleteNtc() { AchievementIdList = new() { new(achievement.Id) } }, queue);
                    }
                });
            }

            return queue;
        }

        private static bool CheckLateAchievement((AchievementAsset, uint, DateTimeOffset) zippedAchievement)
        {
            switch (zippedAchievement.Item1.Type)
            {
                case AchievementType.MainLevelGroup:
                case AchievementType.PawnAffection:
                case AchievementType.PawnCraftingExam:
                    return zippedAchievement.Item2 > 0;
                default:
                    return zippedAchievement.Item2 >= zippedAchievement.Item1.Count;
            }
        }

        public List<CDataHistoryElement> GetArisenAchievementHistory(GameClient client)
        {
            return client.Character.AchievementStatus.Select(x => new CDataHistoryElement()
            {
                Type = 1,
                TargetID = x.Key,
                DateTime = x.Value,
            }).ToList();
        }

        public List<CDataAchieveCategoryStatus> GetCategoryStatus(GameClient client)
        {
            List<CDataAchieveCategoryStatus> categoryStatus = new();
            var achievements = Server.AssetRepository.AchievementAsset
                .SelectMany(x => x.Value)
                .Select(x => (x.Category, x.Id))
                .GroupBy(x => x.Category, x => x.Id);

            var achievementStatus = client.Character.AchievementStatus.Keys.ToHashSet();
            foreach (var category in achievements)
            {
                var categoryAchievements = category.ToHashSet();
                var finishedAchievements = categoryAchievements.Intersect(achievementStatus).ToHashSet();

                categoryStatus.Add(new()
                {
                    CategoryID = (byte)category.Key,
                    AchieveNum = (ushort)finishedAchievements.Count,
                    TargetNum = (ushort)categoryAchievements.Count,
                });
            }

            return categoryStatus;
        }

        public List<CDataAchieveRewardCommon> GetRewards(GameClient client)
        {
            List<CDataAchieveRewardCommon> rewardList = new();

            var bgRewards = client.Character.UnlockableItems
                .Where(x => x.Category == UnlockableItemCategory.ArisenCardBackground)
                .Select(x => x.Id);

            var itemRewards = client.Character.UnlockableItems
                .Where(x => x.Category == UnlockableItemCategory.FurnitureItem || x.Category == UnlockableItemCategory.CraftingRecipe)
                .Select(x => x.Id);

            rewardList.AddRange(Server.AssetRepository.AchievementBackgroundAsset.UnlockableBackgrounds
                .Where(x => x.Required <= client.Character.AchievementStatus.Count && !bgRewards.Contains(x.Id))
                .Select(x => new CDataAchieveRewardCommon()
                {
                    Type = 1,
                    RewardId = x.Id
                }));
            rewardList.AddRange(Server.AssetRepository.AchievementAsset.SelectMany(x => x.Value)
                .Where(x => x.RewardId > 0 && client.Character.AchievementStatus.ContainsKey(x.Id) && !itemRewards.Contains(x.RewardId))
                .Select(x => new CDataAchieveRewardCommon()
                {
                    Type = 2,
                    RewardId = x.Id,
                }));

            return rewardList;
        }

        private static AchievementCraftTypeParam GetCraftTypeSubtype(ClientItemInfo item)
        {
            if (item.RecipeCategory == RecipeCategory.Furniture)
            {
                return AchievementCraftTypeParam.Furniture;
            }
            else if (item.EquipSlot == EquipSlot.WepMain || item.EquipSlot == EquipSlot.WepSub)
            {
                return AchievementCraftTypeParam.Weapon;
            }
            else if (item.EquipSlot is not null)
            {
                return AchievementCraftTypeParam.Armor;
            }
            else
            {
                return AchievementCraftTypeParam.Other;
            }
        }

        private static AchievementQuestTypeParam GetClearQuestTypeSubtype(Quest quest)
        {
            switch (quest.QuestType)
            {
                case QuestType.World:
                    return AchievementQuestTypeParam.World;
                case QuestType.ExtremeMission:
                    return AchievementQuestTypeParam.Extreme;
                case QuestType.WildHunt:
                    return AchievementQuestTypeParam.WildHunt;
                case QuestType.Board:
                    if (quest.EnemyHunts.Any())
                    {
                        return AchievementQuestTypeParam.Bounty;
                    }
                    else if (quest.DeliveryItems.Any())
                    {
                        return AchievementQuestTypeParam.Delivery;
                    }
                    return AchievementQuestTypeParam.None;
                default:
                    return AchievementQuestTypeParam.None;
            }
        }

        // Inverted in the constructor.
        private static readonly Dictionary<EnemyId, HashSet<AchievementEnemyParam>> EnemyIdParamLookup = new();
        private static readonly Dictionary<AchievementEnemyParam, HashSet<EnemyId>> KillEnemyTypeMap = new()
        {
            {AchievementEnemyParam.AlchemizedGoblin, new() { EnemyId.AlchemizedGoblin, EnemyId.AlchemizedGoblinFighter } },
            {AchievementEnemyParam.AlchemizedGriffin, new() { EnemyId.AlchemizedGriffin} },
            {AchievementEnemyParam.AlchemizedHarpy, new() { EnemyId.AlchemizedHarpy } },
            {AchievementEnemyParam.AlchemizedSkeleton, new() { EnemyId.AlchemizedSkeleton} },
            {AchievementEnemyParam.AlchemizedWolf, new() { EnemyId.AlchemizedWolf} },
            {AchievementEnemyParam.AlchemyEye, new() { EnemyId.AlchemyEye } },
            {AchievementEnemyParam.AlteredZuhl, new() { EnemyId.AlteredZuhl} },
            {AchievementEnemyParam.Angules, new() { EnemyId.Angules0, EnemyId.Angules1 } },
            {AchievementEnemyParam.Ape, new() { EnemyId.BruteApe, EnemyId.DreadApe } },
            {AchievementEnemyParam.Banded, new() { EnemyId.BandedDefender, EnemyId.BandedFighter, EnemyId.BandedHealer, 
                EnemyId.BandedHunter, EnemyId.BandedMage, EnemyId.BandedSeeker, EnemyId.BandedWarrior } },
            {AchievementEnemyParam.BeastMaster, new() { EnemyId.BeastMaster0, EnemyId.BeastMaster1 } },
            {AchievementEnemyParam.Behemoth, new() { EnemyId.Behemoth0, EnemyId.Behemoth1 } },
            {AchievementEnemyParam.Bifrest, new() { EnemyId.Bifrest0, EnemyId.Bifrest1, EnemyId.Bifrest2 } },
            {AchievementEnemyParam.BlackGriffin, new() { EnemyId.BlackGriffin0, EnemyId.BlackGriffin1} },
            {AchievementEnemyParam.BlackKnight, new() { EnemyId.BlackKnight, EnemyId.BlackKnightDarkOnly, EnemyId.BlackKnightFanOfSwords, EnemyId.BlackKnightHolyIce0, EnemyId.BlackKnightHolyIce1 } },
            {AchievementEnemyParam.BlueNewt, new() { EnemyId.BlueNewt, EnemyId.LargeNewt } },
            {AchievementEnemyParam.BoltGrimwarg, new() { EnemyId.BoltGrimwarg } },
            {AchievementEnemyParam.Catoblepas, new() { EnemyId.Catoblepas } },
            {AchievementEnemyParam.Chicken, new() {EnemyId.ChickenBrown, EnemyId.ChickenWhite } },
            {AchievementEnemyParam.Chimera, new() { EnemyId.Chimera0, EnemyId.Chimera1 } },
            {AchievementEnemyParam.Cockatrice, new() { EnemyId.Cockatrice } },
            {AchievementEnemyParam.Colossus, new() { EnemyId.Colossus0, EnemyId.Colossus1, EnemyId.Colossus2 } },
            {AchievementEnemyParam.Corpse, new() { EnemyId.BoltCorpsePunisher, EnemyId.BoltCorpseTorturer, EnemyId.DarkCorpsePunisher, EnemyId.DarkCorpseTorturer,
                EnemyId.FlameCorpsePunisher, EnemyId.FlameCorpseTorturer, EnemyId.FrostCorpsePunisher, EnemyId.FrostCorpseTorturer, 
                EnemyId.LuxCorpsePunisher, EnemyId.LuxCorpseTorturer} },
            {AchievementEnemyParam.CursedDragon, new() { EnemyId.CursedDragon } },
            {AchievementEnemyParam.Cyclops, new() { EnemyId.Cyclops0, EnemyId.Cyclops1, EnemyId.Cyclops2, EnemyId.Cyclops3, EnemyId.Cyclops4, EnemyId.Cyclops5, 
                EnemyId.CyclopsClub, EnemyId.CyclopsGiant, EnemyId.ArmoredCyclops, EnemyId.ArmoredCyclopsClub } },
            {AchievementEnemyParam.Death, new() { EnemyId.Death } },
            {AchievementEnemyParam.DeathKnight, new() { EnemyId.DeathKnight} },
            {AchievementEnemyParam.Deer, new() { EnemyId.Doe, EnemyId.Buck } },
            {AchievementEnemyParam.Drake, new() { EnemyId.Drake0, EnemyId.Drake1 } },
            {AchievementEnemyParam.ElderDragon, new() { EnemyId.ElderDragon0, EnemyId.ElderDragon1, EnemyId.ElderDragonEpitathRoadTrial} },
            {AchievementEnemyParam.Eliminator, new() { EnemyId.Eliminator, EnemyId.EliminatorSlay } },
            {AchievementEnemyParam.EmpressGhost, new() { EnemyId.EmpressGhost } },
            {AchievementEnemyParam.Ent, new() { EnemyId.Ent } },
            {AchievementEnemyParam.EvilDragon, new() { EnemyId.TheEvilDragon0, EnemyId.TheEvilDragon1 } },
            {AchievementEnemyParam.EvilEye, new() { EnemyId.EvilEye0, EnemyId.EvilEye1 } },
            {AchievementEnemyParam.Footbiter, new() { EnemyId.FootBiter } },
            {AchievementEnemyParam.ForestGoblin, new() { EnemyId.ForestGoblin, EnemyId.ForestGoblinFighter, EnemyId.SlingForestGoblin } },
            {AchievementEnemyParam.Frog, new() {EnemyId.FrogBlue, EnemyId.FrogGold,} },
            {AchievementEnemyParam.FrostMachina, new() { EnemyId.FrostMachina, EnemyId.FrostMachinaTrinity } },
            {AchievementEnemyParam.Gargoyle, new() { EnemyId.Gargoyle } },
            {AchievementEnemyParam.GeoGolem, new() { EnemyId.GeoGolem } },
            {AchievementEnemyParam.Ghost, new() { EnemyId.Ghost, EnemyId.GrudgeGhost, EnemyId.RageGhost, EnemyId.MiseryGhost } },
            {AchievementEnemyParam.GhostMail, new() { EnemyId.GhostMail } },
            {AchievementEnemyParam.Ghoul, new() { EnemyId.Ghoul0, EnemyId.Ghoul1 } },
            {AchievementEnemyParam.GiantWarrior, new() { EnemyId.DamnedGolem, EnemyId.Goliath } },
            {AchievementEnemyParam.GigantMachina, new() { EnemyId.GigantMachina, EnemyId.GigantMachinaTrinity } },
            {AchievementEnemyParam.Goat, new() { EnemyId.Goat} },
            {AchievementEnemyParam.Goblin, new() { EnemyId.Goblin, EnemyId.GoblinAidShaman, EnemyId.GoblinBomber, EnemyId.GoblinFighter0, 
                EnemyId.GoblinHorn, EnemyId.GoblinLeader, EnemyId.GoblinShaman, EnemyId.SlingGoblinRock, EnemyId.SlingGoblinTorch} },
            {AchievementEnemyParam.Golem, new() { EnemyId.Golem } },
            {AchievementEnemyParam.Gorechimera, new() { EnemyId.Gorechimera0, EnemyId.Gorechimera1 } },
            {AchievementEnemyParam.Gorecyclops, new() { EnemyId.Gorecyclops0, EnemyId.Gorecyclops1, EnemyId.Gorecyclops2} },
            {AchievementEnemyParam.GrandEnt, new() { EnemyId.GrandEnt0, EnemyId.GrandEnt1, EnemyId.GrandEnt2 } },
            {AchievementEnemyParam.GreenGuardian, new() { EnemyId.GreenGuardian } },
            {AchievementEnemyParam.Griffin, new() { EnemyId.Griffin0, EnemyId.Griffin1 } },
            {AchievementEnemyParam.Grigori, new() { EnemyId.Grigori, EnemyId.BeardedGrigori, EnemyId.ShadowGrigori} },
            {AchievementEnemyParam.Grimwarg, new() { EnemyId.Grimwarg } },
            {AchievementEnemyParam.GroundInsect, new() { EnemyId.ArmoredInsectBlue, EnemyId.ArmoredInsectRed } },
            {AchievementEnemyParam.Harpy, new() { EnemyId.Harpy, EnemyId.SnowHarpy, EnemyId.Harpy} },
            {AchievementEnemyParam.Hobgoblin, new() { EnemyId.Hobgoblin, EnemyId.HobgoblinFighter, EnemyId.HobgoblinLeader,
                EnemyId.SlingHobgoblinOilFlask, EnemyId.SlingHobgoblinTorch} },
            {AchievementEnemyParam.Ifrit, new() { EnemyId.Ifrit2ndForm } },
            {AchievementEnemyParam.InfectedDirewolf, new() { EnemyId.InfectedDirewolf } },
            {AchievementEnemyParam.InfectedGriffin, new() { EnemyId.InfectedGriffin} },
            {AchievementEnemyParam.InfectedHobgoblin, new() { EnemyId.InfectedHobgoblin, EnemyId.InfectedHobgoblinFighter, EnemyId.InfectedSlingHobgoblin } },
            {AchievementEnemyParam.InfectedOrc, new() { EnemyId.InfectedOrcAimer, EnemyId.InfectedOrcBanger, EnemyId.InfectedOrcSoldier } },
            {AchievementEnemyParam.InfectedSnowHarpy, new() { EnemyId.InfectedSnowHarpy } },
            {AchievementEnemyParam.JewelEye, new() { EnemyId.CrystalEye, EnemyId.EmeraldEye, EnemyId.RubyEye, EnemyId.LapisEye } },
            {AchievementEnemyParam.KillerBee, new() { EnemyId.KillerBee} },
            {AchievementEnemyParam.Lindwurm, new() { EnemyId.Lindwurm0, EnemyId.Lindwurm1} },
            {AchievementEnemyParam.LivingArmor, new() { EnemyId.LivingArmor } },
            {AchievementEnemyParam.LostPawn, new() { EnemyId.PawnDefender, EnemyId.PawnElementArcher, EnemyId.PawnFighter, EnemyId.PawnHealer, 
                EnemyId.PawnHunter, EnemyId.PawnMage, EnemyId.PawnSeeker, EnemyId.PawnWarrior} },
            {AchievementEnemyParam.Mandragora, new() { EnemyId.Mandragora, EnemyId.MandragoraFlower} },
            {AchievementEnemyParam.Maneater, new() { EnemyId.Maneater, EnemyId.ManeaterClosed } },
            {AchievementEnemyParam.Manticore, new() { EnemyId.Manticore0, EnemyId.Manticore1 } },
            {AchievementEnemyParam.Medusa, new() { EnemyId.Medusa } },
            {AchievementEnemyParam.Mergan, new() { EnemyId.MerganDefender, EnemyId.MerganElementArcher, EnemyId.MerganFighter, EnemyId.MerganHealer,
                EnemyId.MerganHunter, EnemyId.MerganMage, EnemyId.MerganSeeker, EnemyId.MerganWarrior } },
            {AchievementEnemyParam.MistDrake, new() { EnemyId.MistDrake} },
            {AchievementEnemyParam.MistMan, new() { EnemyId.MistFighter, EnemyId.MistHunter, EnemyId.MistPriest, 
                EnemyId.MistSorcerer, EnemyId.MistWarrior } },
            {AchievementEnemyParam.MistWyrm, new() { EnemyId.MistWyrm} },
            {AchievementEnemyParam.Moth, new() { EnemyId.MothCyanSleep, EnemyId.MothMagentaPoison} },
            {AchievementEnemyParam.Mudmen, new() { EnemyId.Mudman, EnemyId.Sludgeman} },
            {AchievementEnemyParam.NecroMaster, new() { EnemyId.NecroMaster} },
            {AchievementEnemyParam.Nightmare, new() { EnemyId.Nightmare } },
            {AchievementEnemyParam.Ogre, new() { EnemyId.Ogre} },
            {AchievementEnemyParam.Orc, new() { EnemyId.OrcAimer, EnemyId.OrcBanger, EnemyId.OrcBattler, EnemyId.OrcBringer, 
                EnemyId.OrcTrooper, EnemyId.OrcSoldier0 } },
            {AchievementEnemyParam.OrcLeader, new() { EnemyId.CaptainOrc0, EnemyId.GeneralOrc } },
            {AchievementEnemyParam.Ox, new() { EnemyId.Ox } },
            {AchievementEnemyParam.PhantasmicGreatDragon, new() { EnemyId.WhiteDragon } }, // ???
            {AchievementEnemyParam.Phindymian, new() { EnemyId.PhindymianDefender,EnemyId.PhindymianElementArcher,EnemyId.PhindymianFighter,EnemyId.PhindymianHunter,
                EnemyId.PhindymianPriest,EnemyId.PhindymianSeeker,EnemyId.PhindymianSorcerer,EnemyId.PhindymianWarrior, } },
            {AchievementEnemyParam.PhyndymianEnt, new() { EnemyId.PhindymianEnt0, EnemyId.PhindymianEnt1 } },
            {AchievementEnemyParam.Pig, new() { EnemyId.Pig, EnemyId.PigDirty, EnemyId.Piglet, EnemyId.PigletDirty } },
            {AchievementEnemyParam.Pixie, new() { EnemyId.Pixie, EnemyId.PixieBiff, EnemyId.PixieDin, EnemyId.PixieJabber,
                EnemyId.PixieKing, EnemyId.PixiePow, EnemyId.HighPixieBiff, EnemyId.HighPixiePow, EnemyId.HighPixieZolda} },
            {AchievementEnemyParam.Rabbit, new() { EnemyId.Rabbit} },
            {AchievementEnemyParam.Rat, new() { EnemyId.GiantRat} },
            {AchievementEnemyParam.Redcap, new() { EnemyId.Redcap, EnemyId.RedcapFighter, EnemyId.SlingRedcap } },
            {AchievementEnemyParam.RockSaurian, new() { EnemyId.RockSaurian, EnemyId.RockSaurianSpinel } },
            {AchievementEnemyParam.Rogue, new() { EnemyId.RogueDefender, EnemyId.RogueFighter, EnemyId.RogueHealer, EnemyId.RogueHunter,
                EnemyId.RogueMage, EnemyId.RogueSeeker, EnemyId.RogueWarrior, } },
            {AchievementEnemyParam.Saurian, new() { EnemyId.Saurian, EnemyId.GiantSaurian} },
            {AchievementEnemyParam.Scarlet, new() { EnemyId.ScarletDefender, EnemyId.ScarletFighter, EnemyId.ScarletHealer, EnemyId.ScarletHunter,
                EnemyId.ScarletMage, EnemyId.ScarletSeeker, EnemyId.ScarletWarrior} },
            {AchievementEnemyParam.Scourge, new() { EnemyId.Scourge0, EnemyId.Scourge1} },
            {AchievementEnemyParam.SeverelyInfectedDemon, new() { EnemyId.SeverelyInfectedDemon } },
            {AchievementEnemyParam.SeverelyInfectedGriffin, new() { EnemyId.SeverelyInfectedGriffin} },
            {AchievementEnemyParam.ShadowChimera, new() { EnemyId.ShadowChimera} },
            {AchievementEnemyParam.ShadowGoblin, new() { EnemyId.ShadowGoblin, EnemyId.ShadowGoblinFighter, EnemyId.ShadowGoblinLeader, EnemyId.ShadowSlingGoblin } },
            {AchievementEnemyParam.ShadowHarpy, new() { EnemyId.ShadowHarpy} },
            {AchievementEnemyParam.ShadowMaster, new() { EnemyId.ShadowMasterGiant} }, // ???
            {AchievementEnemyParam.ShadowWolf, new() { EnemyId.ShadowWolf} },
            {AchievementEnemyParam.SilverRoar, new() { EnemyId.SilverRoar} },
            {AchievementEnemyParam.Siren, new() { EnemyId.Siren } },
            {AchievementEnemyParam.Skeleton, new() { EnemyId.Skeleton, EnemyId.SkeletonKnight, EnemyId.SkeletonMage0, EnemyId.SkeletonMage1,
                EnemyId.SkeletonMageDifferentAi, EnemyId.SkeletonSorcerer0, EnemyId.SkeletonSorcerer1, EnemyId.SkeletonSorcerer2, EnemyId.SkeletonSorcerer3,
                EnemyId.SkeletonWarrior, EnemyId.BoltSkeleton, EnemyId.DarkSkeleton, EnemyId.FlameSkeleton, EnemyId.FrostSkeleton, EnemyId.LuxSkeleton} },
            {AchievementEnemyParam.SkeletonBrute, new() { EnemyId.SkeletonBrute, EnemyId.BoltSkeletonBrute, EnemyId.DarkSkeletonBrute,
                EnemyId.FlameSkeletonBrute, EnemyId.FrostSkeletonBrute, EnemyId.LuxSkeletonBrute} },
            {AchievementEnemyParam.SkullLord, new() { EnemyId.SkullLord } },
            {AchievementEnemyParam.Slime, new() { EnemyId.Slime, EnemyId.DeepSlime, EnemyId.DimSlime, EnemyId.ElectricSlime,
                EnemyId.FreezingSlime, EnemyId.PhotonSlime, EnemyId.PyroSlime, EnemyId.Ooze, EnemyId.GluttonOoze} },
            {AchievementEnemyParam.Sphinx, new() { EnemyId.Sphinx0, EnemyId.Sphinx1 } },
            {AchievementEnemyParam.Spider, new() { EnemyId.Spider } },
            {AchievementEnemyParam.Spine, new() { EnemyId.Spineback0, EnemyId.Spineback1, EnemyId.Spineback2, EnemyId.LittleSpine} },
            {AchievementEnemyParam.SpiritDragonWilmia, new() { EnemyId.SpiritDragonWillmia0, EnemyId.SpiritDragonWillmia1, EnemyId.SpiritDragonWillmiaActive } },
            {AchievementEnemyParam.Strix, new() { EnemyId.Strix} },
            {AchievementEnemyParam.Stymphalides, new() { EnemyId.Stymphalides} },
            {AchievementEnemyParam.SulfurSaurian, new() { EnemyId.SulfurSaurian, EnemyId.GiantSulfurSaurian } },
            {AchievementEnemyParam.Tarasque, new() { EnemyId.Tarasque} },
            {AchievementEnemyParam.Troll, new() { EnemyId.Troll, EnemyId.MoleTroll0, EnemyId.MoleTroll1} },
            {AchievementEnemyParam.Undead, new() { EnemyId.UndeadFemale, EnemyId.UndeadMale, EnemyId.StoutUndead, EnemyId.SwordUndead, EnemyId.WarriorUndead} },
            {AchievementEnemyParam.Ushumgal, new() { EnemyId.Ushumgal} },
            {AchievementEnemyParam.Warg, new() { EnemyId.Warg } },
            {AchievementEnemyParam.WarMaster, new() { EnemyId.WarMaster0, EnemyId.WarMaster1} },
            {AchievementEnemyParam.WhiteChimera, new() { EnemyId.WhiteChimera0, EnemyId.WhiteChimera1 } },
            {AchievementEnemyParam.WhiteGriffin, new() { EnemyId.WhiteGriffin0, EnemyId.WhiteGriffin1} },
            {AchievementEnemyParam.Wight, new() { EnemyId.Wight0, EnemyId.Wight1} },
            {AchievementEnemyParam.WildBoar, new() {EnemyId.Boar, EnemyId.WildBoarBrown, EnemyId.WildBoarStripes} },
            {AchievementEnemyParam.Witch, new() { EnemyId.Witch } },
            {AchievementEnemyParam.Wolf, new() { EnemyId.Wolf, EnemyId.Direwolf} },
            {AchievementEnemyParam.Wyrm, new() { EnemyId.Wyrm0, EnemyId.Wyrm1} },
            {AchievementEnemyParam.Zuhl, new() { EnemyId.Zuhl0, EnemyId.Zuhl1} },
        };

        private static readonly Dictionary<AchievementLevelGroupParam, HashSet<JobId>> LevelGroupMap = new()
        {
            {AchievementLevelGroupParam.GroupFirst9, new() {
                JobId.Fighter, JobId.Seeker, JobId.Hunter,
                JobId.Priest,JobId.ShieldSage,JobId.Sorcerer,
                JobId.Warrior,JobId.ElementArcher,JobId.Alchemist  } },
            {AchievementLevelGroupParam.GroupFirst10, new() {
                JobId.Fighter, JobId.Seeker, JobId.Hunter,
                JobId.Priest,JobId.ShieldSage,JobId.Sorcerer,
                JobId.Warrior,JobId.ElementArcher,JobId.Alchemist,
                JobId.SpiritLancer} },
            {AchievementLevelGroupParam.GroupAll, new() { 
                JobId.Fighter, JobId.Seeker, JobId.Hunter,
                JobId.Priest,JobId.ShieldSage,JobId.Sorcerer,
                JobId.Warrior,JobId.ElementArcher,JobId.Alchemist,
                JobId.SpiritLancer, JobId.HighScepter} },
            {AchievementLevelGroupParam.GroupBlue, new() { JobId.ShieldSage, JobId.Alchemist } },
            {AchievementLevelGroupParam.GroupGreen, new() { JobId.Priest, JobId.ElementArcher } },
            {AchievementLevelGroupParam.GroupArcher, new() { JobId.Hunter, JobId.ElementArcher } },
            {AchievementLevelGroupParam.GroupMelee, new() { JobId.Fighter, JobId.Seeker, JobId.Warrior } },
            {AchievementLevelGroupParam.GroupMage, new() { JobId.Priest, JobId.Sorcerer, JobId.ShieldSage } },
        };
    }
}
