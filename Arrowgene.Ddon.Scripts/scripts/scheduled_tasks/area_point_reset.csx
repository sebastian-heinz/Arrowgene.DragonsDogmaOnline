using System.Collections.Generic;

public class AreaPointResetTask : WeeklyTask
{
    public AreaPointResetTask(DayOfWeek day, uint hour, uint minute)
        : base(TaskType.AreaPointReset, day, hour, minute) { }

    public override void RunTask(DdonGameServer server)
    {
        server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.AreaRankResetStart, null);

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

            foreach (var character in server.ClientLookup.GetAllCharacter())
            {
                character.AreaSupply = server.Database.SelectAreaRankSupply(character.CharacterId, connection);
            }
        });

        server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.AreaRankResetEnd, null);
    }
}

return new AreaPointResetTask(DayOfWeek.Monday, 5, 0);
