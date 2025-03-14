using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class AreaPointResetTask : WeeklyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaPointResetTask));

        public AreaPointResetTask(DayOfWeek day, uint hour, uint minute) : base(TaskType.AreaPointReset, day, hour, minute)
        {

        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing weekly area point reset");

            server.RpcManager.AnnounceAll("internal/packet", RpcInternalCommand.AreaRankResetStart, null);

            List<CDataRewardItemInfo> rewards = new();
            server.Database.ExecuteInTransaction(connection =>
            {
                server.Database.DeleteAreaRankSupply(connection);
                server.Database.ResetAreaRankPoint(connection);

                var playerRanks = server.Database.SelectAllAreaRank(connection);
                foreach ((uint characterId, AreaRank rank) in playerRanks)
                {
                    var rewards = server.AreaRankManager.GetSupplyRewardList(rank.AreaId, rank.Rank, rank.LastWeekPoint);
                    foreach (var reward in rewards)
                    {
                        server.Database.InsertAreaRankSupply(characterId, rank.AreaId, reward.Index, reward.ItemId, reward.Num, connection);
                    }
                }
            });

            server.RpcManager.AnnounceAll("internal/packet", RpcInternalCommand.AreaRankResetEnd, null);
        }
    }
}
