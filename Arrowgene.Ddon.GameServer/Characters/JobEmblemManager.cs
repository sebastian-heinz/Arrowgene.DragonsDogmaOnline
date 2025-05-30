using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobEmblemManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemManager));

        private DdonGameServer Server { get; }
        public JobEmblemManager(DdonGameServer server)
        {
            Server = server;
        }

        public Dictionary<JobId, JobEmblem> InitializeEmblemData(Character character, DbConnection connectionIn)
        {
            var results = new Dictionary<JobId, JobEmblem>();

            foreach (var jobId in Enum.GetValues(typeof(JobId)).Cast<JobId>())
            {
                if (jobId == JobId.None)
                {
                    continue;
                }

                results[jobId] = Server.Database.GetJobEmblemData(character.CharacterId, jobId, connectionIn);
                if (results[jobId] == null)
                {
                    results[jobId] = new()
                    {
                        JobId = jobId,
                        EmblemLevel = 1,
                        EmblemPointsUsed = 0,
                    };

                    foreach (var equipStat in Enum.GetValues(typeof(EquipStatId)).Cast<EquipStatId>())
                    {
                        if (equipStat == EquipStatId.EmblemLevel)
                        {
                            continue;
                        }
                        results[jobId].StatLevels[equipStat] = 0;
                    }
                }

                foreach (var (storageType, emblemItemData) in character.Storage.FindItemsByIdInStorage(ItemManager.EquipmentStorages, jobId.VocationEmblemItemId()))
                {
                    results[jobId].UIDs.Add(emblemItemData.Item.UId);
                    emblemItemData.Item.EquipStatParamList = GetEquipStatParamList(results[jobId]);
                }
            }

            return results;
        }

        public void AddNewEmblemItem(Character character, string emblemUid)
        {
            var (storageType, emblemItemData) = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, emblemUid);
            var (_, emblemItem, _) = emblemItemData;

            var jobId = GetJobIdForEmblem((ItemId) emblemItem.ItemId);
            if (jobId == JobId.None)
            {
                Logger.Error($"Unable to determine job for emblem item {emblemItem.ItemId}");
                return;
            }

            character.JobEmblems[jobId].UIDs.Add(emblemUid);
            emblemItem.EquipStatParamList = GetEquipStatParamList(character.JobEmblems[jobId]);
        }

        private byte GetInheritanceBasePercentageChance(Item item)
        {
            var count = item.EquipElementParamList.Where(x => x.CrestId != 0).Sum(x => 1);
            if (count >= Server.GameSettings.EmblemSettings.InheritanceUnlockLevels.Count)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR, $"No information for inheritance count {count}");
            }
            return Server.GameSettings.EmblemSettings.InheritanceUnlockLevels[count].BaseChance;
        }

        private byte GetInheritanceAdditionalPercentageChance(List<CDataItemAmount> payments)
        {
            byte additionalChance = 0;
            foreach (var payment in payments)
            {
                var match = Server.GameSettings.EmblemSettings.InheritanceChanceIncreaseItems.Where(x => x.ItemId == payment.ItemId).FirstOrDefault();
                if (match.ItemId == 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND, $"Unable to find configuration information for {payment.ItemId}");
                }

                if (payment.Num % match.AmountConsumed != 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR, "The amount of crest inhertience chance items is an incorrect amount");
                }

                additionalChance = (byte)(match.PercentIncrease * (payment.Num / match.AmountConsumed));
            }
            return Math.Min(additionalChance, Server.GameSettings.EmblemSettings.MaxInheritanceChanceIncrease);
        }

        public float GetInheritanceChance(Item item, List<CDataItemAmount> payments)
        {
            var chance = Math.Min(GetInheritanceBasePercentageChance(item) + GetInheritanceAdditionalPercentageChance(payments), 100);
            return (float)chance / 100.0f;
        }

        public ushort MaxEmblemPointsForLevel(JobEmblem jobEmblem)
        {
            return (ushort)Server.GameSettings.EmblemSettings.LevelingData.Where(x => x.Level <= jobEmblem.EmblemLevel).Sum(x => x.EP);
        }

        public ushort GetAvailableEmblemPoints(JobEmblem jobEmblem)
        {
            return (ushort)(MaxEmblemPointsForLevel(jobEmblem) - jobEmblem.EmblemPointsUsed);
        }

        public ushort GetMaxTotalEmblemPoints()
        {
            return (ushort) Server.GameSettings.EmblemSettings.LevelingData.Where(x => x.Level <= Server.GameSettings.EmblemSettings.MaxEmblemLevel).Sum(x => x.EP);
        }

        public uint LevelUpPPCost(byte emblemLevel)
        {
            var levelingData = Server.GameSettings.EmblemSettings.LevelingData.Where(x => x.Level == emblemLevel).FirstOrDefault();
            if (levelingData.Level == 0)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SERVER_CONFIG_ERROR, $"No job emblem level up data for level {emblemLevel}");
            }
            return levelingData.PPCost;
        }

        public List<CDataEquipStatParam> GetEquipStatParamList(JobEmblem jobEmblem)
        {
            return jobEmblem.StatLevels.Select(x => new CDataEquipStatParam()
            {
                EffectID = (byte)x.Key,
                EffectValue = Server.ScriptManager.JobEmblemStatModule.GetStatAmount(x.Key, x.Value)
            }).ToList();
        }

        public ushort CalculatedUsedPoints(JobEmblem emblemData)
        {
            ushort pointsUsed = 0;
            foreach (var (stateId, level) in emblemData.StatLevels)
            {
                pointsUsed += (ushort) Server.GameSettings.EmblemSettings.StatUpgradeCost.Where(x => x.Level <= level).Sum(x => x.EPAmount);
            }

            return pointsUsed;
        }

        public List<CDataEquipStatParam> GetEmblemStatsForCurrentJob(Character character, JobId jobId)
        {
            if (jobId == JobId.None)
            {
                Logger.Error($"The character {character.CharacterId} attempted to calculate emblem stats for JobId.None");
                return new();
            }

            var emblemData = character.JobEmblems[jobId];
            if (emblemData.UIDs.Count == 0)
            {
                return new();
            }

            return GetEquipStatParamList(emblemData);
        }

        public List<CDataEquipStatParam> GetEmblemStatsForCurrentJob(Character character)
        {
            return GetEmblemStatsForCurrentJob(character, character.ActiveCharacterJobData.Job);
        }

        public bool IsEmblemItem(ItemId itemId)
        {
            return Enum.GetValues(typeof(JobId)).Cast<JobId>().Where(x => x != JobId.None).Any(x => x.VocationEmblemItemId() == itemId);
        }

        public JobId GetJobIdForEmblem(ItemId itemId)
        {
            return Enum.GetValues(typeof(JobId)).Cast<JobId>().Where(x => x != JobId.None).Where(x => x.VocationEmblemItemId() == itemId).FirstOrDefault();
        }
    }
}
