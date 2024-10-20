using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetSetQuestListHandler : GameStructurePacketHandler<C2SQuestGetSetQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));

        public QuestGetSetQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetSetQuestListReq> packet)
        {
            // client.Send(GameFull.Dump_132);

            S2CQuestGetSetQuestListRes res = new S2CQuestGetSetQuestListRes();

            // Remove all world quests which have no progress made
            client.Party.QuestState.RemoveInactiveWorldQuests();

            /**
             * World quests get added here instead of QuestGetWorldManageQuestListHandler because
             * "World Manage Quests" are different from "World Quests". World manage quests appear
             * to control the state of the game world (doors, paths, gates, etc.). World quests
             * are random fetch, deliver and kill type quests.
             */

            // Populate state for all quests currently in progress by the player
            foreach (var questScheduleId in client.Party.QuestState.GetActiveQuestScheduleIds())
            {
                var quest = client.Party.QuestState.GetQuest(questScheduleId);
                if (!QuestManager.IsWorldQuest(quest.QuestId))
                {
                    continue;
                }

                var questStats = client.Party.Leader.Client.Character.CompletedQuests.GetValueOrDefault(quest.QuestId);
                var questState = client.Party.QuestState.GetQuestState(quest);

                res.SetQuestList.Add(new CDataSetQuestList()
                {
                    Detail = new CDataSetQuestDetail()
                    {
                        IsDiscovery = (questStats == null) ? quest.IsDiscoverable : true,
                        ClearCount = (questStats == null) ? 0 : questStats.ClearCount
                    },
                    Param = quest.ToCDataQuestList(questState.Step),
                });
            }

            foreach (var questScheduleId in client.Party.QuestState.AreaQuests(packet.Structure.DistributeId))
            {
                if (client.Party.QuestState.IsQuestActive(questScheduleId) || client.Party.QuestState.IsCompletedWorldQuest(questScheduleId))
                {
                    // Skip quests already populated or completed
                    continue;
                }

                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questStats = client.Party.Leader.Client.Character.CompletedQuests.GetValueOrDefault(quest.QuestId);
                res.SetQuestList.Add(new CDataSetQuestList()
                {
                    Detail = new CDataSetQuestDetail()
                    {
                        IsDiscovery = (questStats == null) ? quest.IsDiscoverable : true,
                        ClearCount = (questStats == null) ? 0 : questStats.ClearCount
                    },
                    Param = quest.ToCDataQuestList(0),
                });
                client.Party.QuestState.AddNewQuest(quest, 0);
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
                SelectCharacterId = client.Party.Leader.Client.Character.CharacterId,
                SetQuestList = res.SetQuestList
            };

            client.Party.SendToAll(ntc);

            client.Send(res);
        }
    }
}
