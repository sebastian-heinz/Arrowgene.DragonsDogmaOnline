using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetSetQuestInfoListHandler : GameRequestPacketHandler<C2SQuestGetSetQuestInfoListReq, S2CQuestGetSetQuestInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetSetQuestInfoListHandler));

        /// <summary>
        /// Mapping of the DistributeId sent by the client and the warp point ID used to warp from the Lestania News menu.
        /// </summary>
        private static readonly Dictionary<QuestAreaId, uint> WarpPointNewsMapping = new Dictionary<QuestAreaId, uint>()
        {
            { QuestAreaId.HidellPlains, 2 },
            { QuestAreaId.BreyaCoast, 3 },
            { QuestAreaId.MysreeForest, 5 },
            { QuestAreaId.VoldenMines, 7 },
            { QuestAreaId.DoweValley, 6 },
            { QuestAreaId.MysreeGrove, 9 },
            { QuestAreaId.DeenanWoods, 14 },
            { QuestAreaId.BetlandPlains, 15 },
            { QuestAreaId.NorthernBetlandPlains, 10 },
            { QuestAreaId.ZandoraWastelands, 11 },
            { QuestAreaId.EasternZandora, 12 },
            { QuestAreaId.MergodaRuins, 13 },
            { QuestAreaId.BloodbaneIsle, 16 },
            { QuestAreaId.ElanWaterGrove, 17 },
            { QuestAreaId.FaranaPlains, 17 },
            { QuestAreaId.MorrowForest, 19 },
            { QuestAreaId.KingalCanyon, 20 },
            { QuestAreaId.FeryanaWilderness, 71 },
            { QuestAreaId.MegadosysPlateau, 82 },
            { QuestAreaId.UrtecaMountains, 96 },
            { QuestAreaId.RathniteFoothills, 68 },
        };

        public QuestGetSetQuestInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetSetQuestInfoListRes Handle(GameClient client, C2SQuestGetSetQuestInfoListReq request)
        {
            var res = new S2CQuestGetSetQuestInfoListRes()
            {
                DistributeId = request.DistributeId
            };

            // The client will let you warp to points you haven't unlocked through Lestania News,
            // and there doesn't seem to be a flag we can return to prevent this.
            // In the mean time, if you don't have the "main" warp point for a region, no quests are presented.
            // TODO: Investigate how this information was actually presented.
            if (!client.Character.ReleasedWarpPoints
                .Where(x => x.WarpPointId == WarpPointNewsMapping[request.DistributeId])
                .Any())
            {
                return res;
            }

            var completedQuests = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.World);
            foreach (var questScheduleId in client.Party.QuestState.AreaQuests(request.DistributeId))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questInfo = quest.ToCDataSetQuestInfoList();
                questInfo.CompleteNum = (ushort)(client.Party.QuestState.IsCompletedWorldQuest(quest.QuestId) ? 1 : 0); // Completed in the current instance, hides rewards.
                questInfo.IsDiscovery = quest.IsDiscoverable || (completedQuests.Where(y => y.QuestId == quest.QuestId).FirstOrDefault()?.ClearCount ?? 0) > 0;
                res.SetQuestList.Add(questInfo);
            }

            return res;
        }
    }
}
