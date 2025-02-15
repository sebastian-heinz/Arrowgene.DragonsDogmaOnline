using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RankingBoardListHandler : GameRequestPacketHandler<C2SRankingBoardListReq, S2CRankingBoardListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RankingBoardListHandler));

        public RankingBoardListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRankingBoardListRes Handle(GameClient client, C2SRankingBoardListReq request)
        {
            S2CRankingBoardListRes res = new();

            var questList = Server.Database.SelectUsedRankingBoardQuests();

            //Calculate the previous Monday.
            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

            foreach (var questId in questList)
            {
                res.RankingBoardList.Add(new()
                {
                    BoardId = questId,
                    QuestId = questId,
                    State = RankingBoardState.ReportRanks,
                    RegisteredNum = 0,
                    IsWarMission = false, // TODO: Parse this from the QuestId
                    Begin = monday,
                    End = monday.AddDays(7),
                    Expire = DateTimeOffset.UtcNow,
                    Tallied = DateTimeOffset.UtcNow
                });
            }

            return res;
        }
    }
}
