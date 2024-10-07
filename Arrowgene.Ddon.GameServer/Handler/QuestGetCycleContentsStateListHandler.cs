using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetCycleContentsStateListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetCycleContentsStateListHandler));


        public QuestGetCycleContentsStateListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
#if false
            /*
             * @note If something goes wrong, we can always change this preprocessor directive to
             * true and get back the original functionality before we started to play with this function.
             */
            EntitySerializer<S2CQuestJoinLobbyQuestInfoNtc> serializer = EntitySerializer.Get<S2CQuestJoinLobbyQuestInfoNtc>();
            S2CQuestJoinLobbyQuestInfoNtc pcap = serializer.Read(InGameDump.data_Dump_20A);
            client.Send(pcap);
#else
            S2CQuestJoinLobbyQuestInfoNtc pcap = EntitySerializer.Get<S2CQuestJoinLobbyQuestInfoNtc>().Read(InGameDump.data_Dump_20A);
            S2CQuestJoinLobbyQuestInfoNtc ntc = new S2CQuestJoinLobbyQuestInfoNtc();

            ntc.WorldManageQuestOrderList = pcap.WorldManageQuestOrderList; // Recover paths + change vocation

            ntc.QuestDefine = pcap.QuestDefine; // Recover quest log data to be able to accept quests

            // pcap.MainQuestIdList; (this will add back all missing functionality which depends on complete MSQ)
            var completedMsq = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.Main);
            foreach (var msq in completedMsq)
            {
                ntc.MainQuestIdList.Add(new CDataQuestId() { QuestId = (uint) msq.QuestId });
            }

            var completedTutorials = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.Tutorial);
            foreach (var tut in completedTutorials)
            {
                ntc.TutorialQuestIdList.Add(new CDataQuestId() { QuestId = (uint) tut.QuestId});
            }

            var tutorialQuestInProgress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.Tutorial);
            foreach (var questProgress in tutorialQuestInProgress)
            {
                if (!QuestManager.IsQuestEnabled(questProgress.QuestScheduleId))
                {
                    continue;
                }

                var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                var tutorialQuest = quest.ToCDataTutorialQuestOrderList(questProgress.Step);
                ntc.TutorialQuestOrderList.Add(tutorialQuest);
            }

            var wildHuntsInProgress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.WildHunt);
            foreach (var questProgress in wildHuntsInProgress)
            {
                if (!QuestManager.IsQuestEnabled(questProgress.QuestScheduleId))
                {
                    continue;
                }

                var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                var mobHuntQuest = quest.ToCDataMobHuntQuestOrderList(questProgress.Step);
                ntc.MobHuntQuestOrderList.Add(mobHuntQuest);
            }

            if (client.Party != null)
            {
                var priorityQuests = Server.Database.GetPriorityQuestScheduleIds(client.Party.Leader.Client.Character.CommonId);
                foreach (var questId in priorityQuests)
                {
                    if (!QuestManager.IsQuestEnabled(questId))
                    {
                        continue;
                    }

                    var quest = QuestManager.GetQuestByScheduleId(questId);
                    ntc.PriorityQuestList.Add(new CDataPriorityQuest()
                    {
                        QuestId = (uint)quest.QuestId,
                        QuestScheduleId = (uint)quest.QuestScheduleId
                    });
                }
            }

            client.Send(ntc);
#endif
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big);
            buffer.WriteInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            client.Send(new Packet(PacketId.S2C_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_RES, buffer.GetAllBytes()));

            // client.Send(InGameDump.Dump_24);
        }
    }
}
