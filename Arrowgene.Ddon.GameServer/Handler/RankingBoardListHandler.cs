using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Linq;

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

            var questList = Server.Database.SelectUsedRankingBoardQuests()
                .Select(x => QuestManager.GetQuestByScheduleId(x));

            //Calculate the previous Monday.
            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

            foreach (var quest in questList)
            {
                res.RankingBoardList.Add(new()
                {
                    BoardId = quest.QuestScheduleId,
                    QuestId = quest.QuestId,
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
