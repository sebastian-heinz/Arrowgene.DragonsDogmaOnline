using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetSetQuestListHandler : GameRequestPacketHandler<C2SQuestGetSetQuestListReq, S2CQuestGetSetQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));

        public QuestGetSetQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetSetQuestListRes Handle(GameClient client, C2SQuestGetSetQuestListReq request)
        {
            // client.Send(GameFull.Dump_132);

            client.Character.AreaId = request.DistributeId;

            S2CQuestGetSetQuestListRes res = new S2CQuestGetSetQuestListRes()
            {
                DistributeId = request.DistributeId
            };

            // Remove all world quests which have no progress made
            client.Party.QuestState.RemoveInactiveWorldQuests();

            if (QuestManager.HasWorldQuestAreaReleased(client.Character, request.DistributeId))
            {
                /**
                 * World quests get added here instead of QuestGetWorldManageQuestListHandler because
                 * "World Manage Quests" are different from "World Quests". World manage quests appear
                 * to control the state of the game world (doors, paths, gates, etc.). World quests
                 * are random fetch, deliver and kill type quests.
                 */

                // Populate state for all quests currently in progress by the player
                foreach (var questScheduleId in client.Party.QuestState.GetActiveQuestScheduleIds())
                {
                    Quest quest = client.Party.QuestState.GetQuest(questScheduleId);
                    if (quest is null || !QuestManager.IsWorldQuest(quest.QuestId))
                    {
                        continue;
                    }

                    CompletedQuest questStats = client.Party.Leader?.Client.Character.CompletedQuests.GetValueOrDefault(quest.QuestId);
                    QuestState questState = client.Party.QuestState.GetQuestState(quest);
                    res.SetQuestList.Add(quest.ToCDataSetQuestList(questState?.Step ?? 0, questStats?.ClearCount ?? 0));
                }

                foreach (var questScheduleId in client.Party.QuestState.AreaQuests(request.DistributeId))
                {
                    Quest quest = QuestManager.GetQuestByScheduleId(questScheduleId);

                    if (quest is null
                        || client.Party.QuestState.IsQuestActive(questScheduleId)
                        || client.Party.QuestState.IsCompletedWorldQuest(questScheduleId))
                    {
                        // Skip quests already populated or completed
                        continue;
                    }

                    CompletedQuest questStats = client.Party.Leader?.Client.Character.CompletedQuests.GetValueOrDefault(quest.QuestId);
                    res.SetQuestList.Add(quest.ToCDataSetQuestList(0, questStats?.ClearCount ?? 0));
                    client.Party.QuestState.AddNewQuest(quest, 0);
                }
            }

            // Add Debug Quest
            var debugQuest = QuestManager.GetQuestByScheduleId(70000001);
            res.SetQuestList.Add(new CDataSetQuestList()
            {
                Detail = new CDataSetQuestDetail()
                {
                    IsDiscovery = false,
                    ClearCount = 0
                },
                Param = debugQuest.ToCDataQuestList(0),
            });

            S2CQuestGetSetQuestListNtc ntc = new S2CQuestGetSetQuestListNtc()
            {
                DistributeId = request.DistributeId,
                SelectCharacterId = client.Party.Leader.Client.Character.CharacterId,
                SetQuestList = res.SetQuestList
            };

            client.Party.SendToAll(ntc);

            return res;
        }
    }
}
