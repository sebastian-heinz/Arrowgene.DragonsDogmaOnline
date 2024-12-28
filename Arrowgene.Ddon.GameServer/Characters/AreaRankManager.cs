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
            List<CDataRewardItemInfo> list = new();

            var areaSupplies = Server.AssetRepository.AreaRankSupplyAsset.Where(x => x.AreaId == areaId);
            if (!areaSupplies.Any())
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE, $"No valid asset for {areaId} found in AreaRankSupply asset.");
            }

            var rankSupplies = areaSupplies.Where(x => x.MinRank <= rank).OrderBy(x => x.MinRank).LastOrDefault();
            if (rankSupplies is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE, $"No valid asset for {areaId}, rank {rank} found in AreaRankSupply asset.");
            }

            var pointSupplies = rankSupplies.SupplyItemInfoList.Where(x => x.MinAreaPoint <= points).OrderBy(x => x.MinAreaPoint).LastOrDefault();
            if (pointSupplies is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE, $"No valid asset for {areaId}, rank {rank}, {points} points found in AreaRankSupply asset.");
            }
            return pointSupplies.SupplyItemList.Select((x, i) => new CDataRewardItemInfo()
            {
                Index = (uint)i,
                ItemId = x.ItemId,
                Num = (byte)x.ItemNum
            }).ToList();
        }

        public uint MaxRank(QuestAreaId areaId)
        {
            return Server.AssetRepository.AreaRankRequirementAsset.Where(x => x.AreaId == areaId).Select(x => x.Rank).Max();
        }

        public uint GetMaxPoints(QuestAreaId areaId, uint rank)
        {
            var requirements = Server.AssetRepository.AreaRankRequirementAsset.Where(x => x.AreaId == areaId).ToList();

            if (rank >= requirements.Count())
            {
                return 0;
            }
            
            var nextRank = requirements.Find(x => x.Rank == rank+1);
            return nextRank.MinPoint;
        }

        public bool CanRankUp(GameClient client, QuestAreaId areaId)
        {
            AreaRank clientRank = client.Character.AreaRanks.Find(x => x.AreaId == areaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            Dictionary<QuestId, CompletedQuest> completedQuests = client.Character.CompletedQuests;
            List<AreaRankRequirement> requirements = Server.AssetRepository.AreaRankRequirementAsset.Where(x => x.AreaId == areaId).ToList();

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
            return Server.AssetRepository.AreaRankRequirementAsset
                .Where(x =>
                    x.AreaId == areaId
                    && (x.AreaTrial > 0 || x.ExternalQuest > 0)
                )
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
            AreaRank clientRank = client.Character.AreaRanks.Find(x => x.AreaId == areaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            if (clientRank is null || clientRank.Rank == 0) {
                return queue;
            }

            List<AreaRankRequirement> requirements = Server.AssetRepository.AreaRankRequirementAsset.Where(x => x.AreaId == areaId).ToList();
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
    }
}
