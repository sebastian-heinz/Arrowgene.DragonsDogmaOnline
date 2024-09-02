using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            S2CQuestJoinLobbyQuestInfoNtc pcap = EntitySerializer.Get<S2CQuestJoinLobbyQuestInfoNtc>().Read(InGameDump.data_Dump_20B);
            S2CQuestJoinLobbyQuestInfoNtc ntc = new S2CQuestJoinLobbyQuestInfoNtc();

            ntc.TutorialQuestIdList = new List<CDataQuestId>
            {
                new CDataQuestId
                {
                    QuestId = 60000012
                },
                new CDataQuestId
                {
                    QuestId = 60200006
                },
                new CDataQuestId
                {
                    QuestId = 60070002
                },
                new CDataQuestId
                {
                    QuestId = 60000004
                },
                new CDataQuestId
                {
                    QuestId = 60030002
                },
                new CDataQuestId
                {
                    QuestId = 60215005
                },
                new CDataQuestId
                {
                    QuestId = 60000013
                },
                new CDataQuestId
                {
                    QuestId = 60201001
                },
                new CDataQuestId
                {
                    QuestId = 60000014
                },
                new CDataQuestId
                {
                    QuestId = 60206001
                },
                new CDataQuestId
                {
                    QuestId = 60000016
                },
                new CDataQuestId
                {
                    QuestId = 60000006
                },
                new CDataQuestId
                {
                    QuestId = 60200203
                },
                new CDataQuestId
                {
                    QuestId = 60202000
                },
                new CDataQuestId
                {
                    QuestId = 60040002
                },
                new CDataQuestId
                {
                    QuestId = 60300407
                },
                new CDataQuestId
                {
                    QuestId = 60000001
                },
                new CDataQuestId
                {
                    QuestId = 60110002
                },
                new CDataQuestId
                {
                    QuestId = 60100001
                },
                new CDataQuestId
                {
                    QuestId = 60000003
                },
                new CDataQuestId
                {
                    QuestId = 60050002
                },
                new CDataQuestId
                {
                    QuestId = 60000008
                },
                new CDataQuestId
                {
                    QuestId = 60200002
                },
                new CDataQuestId
                {
                    QuestId = 60000009
                },
                new CDataQuestId
                {
                    QuestId = 60000017
                },
                new CDataQuestId
                {
                    QuestId = 60208000
                },
                new CDataQuestId
                {
                    QuestId = 60000018
                },
                new CDataQuestId
                {
                    QuestId = 60300003
                },
                new CDataQuestId
                {
                    QuestId = 60000053
                },
                new CDataQuestId
                {
                    QuestId = 60213000
                },
                new CDataQuestId
                {
                    QuestId = 60000055
                },
                new CDataQuestId
                {
                    QuestId = 60000056
                },
                new CDataQuestId
                {
                    QuestId = 60040001
                },
                new CDataQuestId
                {
                    QuestId = 60300413
                },
                new CDataQuestId
                {
                    QuestId = 60030001
                },
                new CDataQuestId
                {
                    QuestId = 60010001
                },
                new CDataQuestId
                {
                    QuestId = 60120001
                },
                new CDataQuestId
                {
                    QuestId = 60010002
                },
                new CDataQuestId
                {
                    QuestId = 60020002
                },
                new CDataQuestId
                {
                    QuestId = 60000100
                },
                new CDataQuestId
                {
                    QuestId = 60050001
                },
                new CDataQuestId
                {
                    QuestId = 60060001
                },
                new CDataQuestId
                {
                    QuestId = 60000015
                },
                new CDataQuestId
                {
                    QuestId = 60300020
                },
                new CDataQuestId
                {
                    QuestId = 60060002
                },
                new CDataQuestId
                {
                    QuestId = 60070001
                },
                new CDataQuestId
                {
                    QuestId = 60204000
                },
                new CDataQuestId
                {
                    QuestId = 60080001
                },
                new CDataQuestId
                {
                    QuestId = 60100002
                },
                new CDataQuestId
                {
                    QuestId = 60080002
                },
                new CDataQuestId
                {
                    QuestId = 60090001
                },
                new CDataQuestId
                {
                    QuestId = 60200000
                },
                new CDataQuestId
                {
                    QuestId = 60020001
                },
                new CDataQuestId
                {
                    QuestId = 60020003
                },
                new CDataQuestId
                {
                    QuestId = 60000002
                },
                new CDataQuestId
                {
                    QuestId = 60110001
                },
                new CDataQuestId
                {
                    QuestId = 60090002
                },
                new CDataQuestId
                {
                    QuestId = 60120002
                },
                new CDataQuestId
                {
                    QuestId = 60000101
                },
                new CDataQuestId
                {
                    QuestId = 60000020
                },
                new CDataQuestId
                {
                    QuestId = 60202001
                },
                new CDataQuestId
                {
                    QuestId = 60200127
                },
                new CDataQuestId
                {
                    QuestId = 60200001
                },
                new CDataQuestId
                {
                    QuestId = 60201000
                },
                new CDataQuestId
                {
                    QuestId = 60200112
                },
                new CDataQuestId
                {
                    QuestId = 60200120
                },
                new CDataQuestId
                {
                    QuestId = 60200124
                },
                new CDataQuestId
                {
                    QuestId = 60200007
                },
                new CDataQuestId
                {
                    QuestId = 60200012
                },
                new CDataQuestId
                {
                    QuestId = 60206000
                },
                new CDataQuestId
                {
                    QuestId = 60208001
                },
                new CDataQuestId
                {
                    QuestId = 60213001
                },
                new CDataQuestId
                {
                    QuestId = 60200003
                },
                new CDataQuestId
                {
                    QuestId = 60203000
                },
                new CDataQuestId
                {
                    QuestId = 60203001
                },
                new CDataQuestId
                {
                    QuestId = 60200113
                },
                new CDataQuestId
                {
                    QuestId = 60200121
                },
                new CDataQuestId
                {
                    QuestId = 60200125
                },
                new CDataQuestId
                {
                    QuestId = 60200008
                },
                new CDataQuestId
                {
                    QuestId = 60200009
                },
                new CDataQuestId
                {
                    QuestId = 60205000
                },
                new CDataQuestId
                {
                    QuestId = 60207000
                },
                new CDataQuestId
                {
                    QuestId = 60209000
                },
                new CDataQuestId
                {
                    QuestId = 60211000
                },
                new CDataQuestId
                {
                    QuestId = 60210000
                },
                new CDataQuestId
                {
                    QuestId = 60212000
                },
                new CDataQuestId
                {
                    QuestId = 60207001
                },
                new CDataQuestId
                {
                    QuestId = 60209001
                },
                new CDataQuestId
                {
                    QuestId = 60211001
                },
                new CDataQuestId
                {
                    QuestId = 60210001
                },
                new CDataQuestId
                {
                    QuestId = 60212001
                },
                new CDataQuestId
                {
                    QuestId = 60213002
                },
                new CDataQuestId
                {
                    QuestId = 60200011
                },
                new CDataQuestId
                {
                    QuestId = 60200014
                },
                new CDataQuestId
                {
                    QuestId = 60200010
                },
                new CDataQuestId
                {
                    QuestId = 60200005
                },
                new CDataQuestId
                {
                    QuestId = 60319000
                },
                new CDataQuestId
                {
                    QuestId = 60200114
                },
                new CDataQuestId
                {
                    QuestId = 60200122
                },
                new CDataQuestId
                {
                    QuestId = 60200126
                },
                new CDataQuestId
                {
                    QuestId = 60215000
                },
                new CDataQuestId
                {
                    QuestId = 60215001
                },
                new CDataQuestId
                {
                    QuestId = 60215002
                },
                new CDataQuestId
                {
                    QuestId = 60214000
                },
                new CDataQuestId
                {
                    QuestId = 60216000
                },
                new CDataQuestId
                {
                    QuestId = 60216001
                },
                new CDataQuestId
                {
                    QuestId = 60216002
                },
                new CDataQuestId
                {
                    QuestId = 60217000
                },
                new CDataQuestId
                {
                    QuestId = 60215006
                },
                new CDataQuestId
                {
                    QuestId = 60217001
                },
                new CDataQuestId
                {
                    QuestId = 60217002
                },
                new CDataQuestId
                {
                    QuestId = 60200115
                },
                new CDataQuestId
                {
                    QuestId = 60200123
                },
                new CDataQuestId
                {
                    QuestId = 60215003
                },
                new CDataQuestId
                {
                    QuestId = 60320011
                },
                new CDataQuestId
                {
                    QuestId = 60215004
                },
                new CDataQuestId
                {
                    QuestId = 60217003
                },
                new CDataQuestId
                {
                    QuestId = 60217004
                },
                new CDataQuestId
                {
                    QuestId = 61000004
                },
                new CDataQuestId
                {
                    QuestId = 60200004
                },
                new CDataQuestId
                {
                    QuestId = 60200200
                },
                new CDataQuestId
                {
                    QuestId = 60200201
                },
                new CDataQuestId
                {
                    QuestId = 60200202
                },
                new CDataQuestId
                {
                    QuestId = 60318000
                },
                new CDataQuestId
                {
                    QuestId = 60318001
                },
                new CDataQuestId
                {
                    QuestId = 60318002
                },
                new CDataQuestId
                {
                    QuestId = 60318004
                },
                new CDataQuestId
                {
                    QuestId = 60300405
                },
                new CDataQuestId
                {
                    QuestId = 60300000
                },
                new CDataQuestId
                {
                    QuestId = 60300001
                },
                new CDataQuestId
                {
                    QuestId = 60300010
                },
                new CDataQuestId
                {
                    QuestId = 60300100
                },
                new CDataQuestId
                {
                    QuestId = 60300104
                },
                new CDataQuestId
                {
                    QuestId = 60300002
                },
                new CDataQuestId
                {
                    QuestId = 60300004
                },
                new CDataQuestId
                {
                    QuestId = 60300021
                },
                new CDataQuestId
                {
                    QuestId = 60319010
                },
                new CDataQuestId
                {
                    QuestId = 60319011
                },
                new CDataQuestId
                {
                    QuestId = 60300011
                },
                new CDataQuestId
                {
                    QuestId = 60300022
                },
                new CDataQuestId
                {
                    QuestId = 60300044
                },
                new CDataQuestId
                {
                    QuestId = 60300047
                },
                new CDataQuestId
                {
                    QuestId = 60320000
                },
                new CDataQuestId
                {
                    QuestId = 60320010
                },
                new CDataQuestId
                {
                    QuestId = 60300023
                },
                new CDataQuestId
                {
                    QuestId = 60350002
                },
                new CDataQuestId
                {
                    QuestId = 61000000
                },
                new CDataQuestId
                {
                    QuestId = 61000001
                },
                new CDataQuestId
                {
                    QuestId = 61000002
                }
            };

            ntc.WorldManageQuestOrderList = pcap.WorldManageQuestOrderList; // Recover paths + change vocation

            ntc.QuestDefine = pcap.QuestDefine; // Recover quest log data to be able to accept quests

            // pcap.MainQuestIdList; (this will add back all missing functionality which depends on complete MSQ)
            var completedMsq = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.Main);
            foreach (var msq in completedMsq)
            {
                ntc.MainQuestIdList.Add(new CDataQuestId() { QuestId = (uint)msq.QuestId });
            }

            var completedTutorials = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.Tutorial);
            foreach (var tut in completedTutorials)
            {
                ntc.TutorialQuestIdList.Add(new CDataQuestId() { QuestId = (uint) tut.QuestId});
            }

            var tutorialQuestInProgress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.Tutorial);
            foreach (var questProgress in tutorialQuestInProgress)
            {
                var quest = QuestManager.GetQuest(questProgress.QuestId);
                var tutorialQuest = quest.ToCDataTutorialQuestOrderList(questProgress.Step);
                ntc.TutorialQuestOrderList.Add(tutorialQuest);
            }

            if (client.Party != null)
            {
                var priorityQuests = Server.Database.GetPriorityQuests(client.Party.Leader.Client.Character.CommonId);
                foreach (var questId in priorityQuests)
                {
                    var quest = client.Party.QuestState.GetQuest(questId);
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
