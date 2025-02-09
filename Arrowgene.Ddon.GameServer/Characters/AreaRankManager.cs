using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class AreaRankManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanManager));

        private readonly DdonGameServer Server;

        public AreaRankManager(DdonGameServer server)
        {
            Server = server;
        }

        public List<CDataRewardItemInfo> GetSupplyRewardList(QuestAreaId areaId, uint rank, uint points)
        {

            if (!IsValidAreaId(areaId))
            {
                return new();
            }
            List<CDataRewardItemInfo> list = new();

            var areaSupplies = Server.AssetRepository.AreaRankSupplyAsset.GetValueOrDefault(areaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            if (!areaSupplies.Any())
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE, $"No valid asset for {areaId} found in AreaRankSupply asset.");
            }

            var rankSupplies = areaSupplies.Where(x => x.MinRank <= rank).OrderBy(x => x.MinRank).LastOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE, $"No valid asset for {areaId}, rank {rank} found in AreaRankSupply asset.");

            var pointSupplies = rankSupplies.SupplyItemInfoList.Where(x => x.MinAreaPoint <= points).OrderBy(x => x.MinAreaPoint).LastOrDefault() 
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE, $"No valid asset for {areaId}, rank {rank}, {points} points found in AreaRankSupply asset.");
            
            return pointSupplies.SupplyItemList.Select((x, i) => new CDataRewardItemInfo()
            {
                Index = (uint)i,
                ItemId = x.ItemId,
                Num = (byte)x.ItemNum
            }).ToList();
        }

        public uint MaxRank(QuestAreaId areaId)
        {
            return (uint)Server.AssetRepository.AreaRankRequirementAsset[areaId].Count;
        }

        public uint GetMaxPoints(QuestAreaId areaId, uint rank)
        {
            var requirements = Server.AssetRepository.AreaRankRequirementAsset[areaId];

            if (rank >= requirements.Count())
            {
                return 0;
            }
            
            var nextRank = requirements.Find(x => x.Rank == rank+1);
            return nextRank.MinPoint;
        }

        public bool CanRankUp(GameClient client, QuestAreaId areaId)
        {
            if (!IsValidAreaId(areaId))
            {
                return false;
            }

            AreaRank clientRank = client.Character.AreaRanks.GetValueOrDefault(areaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            Dictionary<QuestId, CompletedQuest> completedQuests = client.Character.CompletedQuests;
            List<AreaRankRequirement> requirements = Server.AssetRepository.AreaRankRequirementAsset[areaId];

            if (clientRank.Rank >= requirements.Count())
            {
                return false;
            }

            var nextRank = requirements.Find(x => x.Rank == clientRank.Rank + 1);
            if (nextRank.ExternalQuest > 0 && !completedQuests.ContainsKey((QuestId)nextRank.ExternalQuest))
            {
                return false;
            }
            if (nextRank.AreaTrial > 0 && !completedQuests.ContainsKey((QuestId)nextRank.AreaTrial))
            {
                return false;
            }
            if (nextRank.MinPoint > 0 && clientRank.Point < nextRank.MinPoint)
            {
                return false;
            }

            return true;
        }

        public List<CDataAreaRankUpQuestInfo> RankUpQuestInfo(QuestAreaId areaId)
        {
            return Server.AssetRepository.AreaRankRequirementAsset[areaId]
                .Where(x => x.AreaTrial > 0 || x.ExternalQuest > 0)
                .Select(x => new CDataAreaRankUpQuestInfo()
                {
                    Rank = x.Rank - 1,
                    QuestId = x.AreaTrial > 0 ? x.AreaTrial : x.ExternalQuest
                })
                .ToList();
        }

        public PacketQueue AddAreaPoint(GameClient client, QuestAreaId areaId, uint point, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new PacketQueue();

            if (!IsValidAreaId(areaId))
            {
                return queue;
            }

            AreaRank clientRank = client.Character.AreaRanks.GetValueOrDefault(areaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            if (clientRank is null || clientRank.Rank == 0) {
                return queue;
            }

            List<AreaRankRequirement> requirements = Server.AssetRepository.AreaRankRequirementAsset[areaId];
            var nextRank = requirements.Find(x => x.Rank == clientRank.Rank + 1);

            uint bonusPoint = 0; // TODO: Settings multiplier.

            clientRank.Point += point;
            clientRank.WeekPoint += point;
            bool canRankUp = clientRank.Rank < MaxRank(areaId) && nextRank.MinPoint > 0 && clientRank.Point >= nextRank.MinPoint;

            client.Enqueue(new S2CAreaPointUpNtc()
            {
                AreaId = areaId,
                AddPoint = point, // + bonusPoint???
                AddPointByCharge = bonusPoint,
                TotalPoint = clientRank.Point,
                WeekPoint = clientRank.WeekPoint,
                CanRankUp = canRankUp,
            }, queue);

            if (canRankUp)
            {
                client.Enqueue(new S2CAreaRankUpReadyNtc()
                {
                    AreaRankList = new() { new() { AreaId = areaId, Rank = nextRank.Rank } }
                }, queue);
            }

            Server.Database.UpdateAreaRank(client.Character.CharacterId, clientRank, connectionIn);

            return queue;
        }

        public uint GetEffectiveRank(Character character, QuestAreaId areaId)
        {
            AreaRank rank = character.AreaRanks.GetValueOrDefault(areaId) 
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);

            uint effectiveRank = rank.Rank;
            if (!Server.GameLogicSettings.EnableAreaRankSpotLocks)
            {
                effectiveRank = MaxRank(areaId);
            }
            return effectiveRank;
        }

        public uint GetAreaPointReward(Quest quest)
        {
            uint amount;
            QuestAreaId areaId = quest.QuestAreaId;

            if (!IsValidAreaId(areaId))
            {
                return 0;
            }
            else if (areaId >= QuestAreaId.BloodbaneIsle)
            {
                amount = 500;
            }
            else
            {
                uint tier = quest.BaseLevel / 5;
                amount = 5 * tier * tier + 5 * tier + 30;
            }

            if (QuestManager.IsBoardQuest(quest))
            {
                amount /= 2;
            }

            return amount;
        }

        public static bool IsValidAreaId(QuestAreaId areaId)
        {
            return areaId >= QuestAreaId.HidellPlains && areaId <= QuestAreaId.UrtecaMountains;
        }

        public PacketQueue NotifyAreaRankUpOnQuestComplete(GameClient client, Quest quest)
        {
            PacketQueue queue = new();
            S2CAreaRankUpReadyNtc ntc = new();

            if (quest.QuestType == QuestType.Main)
            {
                foreach ((var area, var rank) in client.Character.AreaRanks)
                {
                    List<AreaRankRequirement> requirements = Server.AssetRepository.AreaRankRequirementAsset[area];

                    if (rank.Rank >= requirements.Count())
                    {
                        continue;
                    }

                    var nextRank = requirements.Find(x => x.Rank == rank.Rank + 1);
                    if (nextRank.ExternalQuest == (uint)quest.QuestId)
                    {
                        ntc.AreaRankList.Add(new() { AreaId = area, Rank = nextRank.Rank });
                    }
                }
            }
            else if (quest.QuestType == QuestType.Tutorial)
            {
                var area = quest.QuestAreaId;
                var rank = client.Character.AreaRanks[area];
                List<AreaRankRequirement> requirements = Server.AssetRepository.AreaRankRequirementAsset[area];

                if (rank.Rank < requirements.Count)
                {
                    var nextRank = requirements.Find(x => x.Rank == rank.Rank + 1);
                    if (nextRank.AreaTrial == (uint)quest.QuestId)
                    {
                        ntc.AreaRankList.Add(new() { AreaId = area, Rank = nextRank.Rank });
                    }
                }
            }

            if (ntc.AreaRankList.Any())
            {
                client.Enqueue(ntc, queue);
            }

            return queue;
        }

        public bool CheckSpot(GameClient client, AreaRankSpotInfo spot)
        {
            AreaRank rank = client.Character.AreaRanks.GetValueOrDefault(spot.AreaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);

            if (!Server.GameLogicSettings.EnableAreaRankSpotLocks)
            {
                return true;
            }

            if (rank.Rank < spot.UnlockRank)
            {
                return false;
            }

            var completedQuests = client.Character.CompletedQuests;
            if (spot.UnlockQuest != 0 && !completedQuests.ContainsKey((QuestId)spot.UnlockQuest))
            {
                return false;
            }

            return true;
        }
    }
}
