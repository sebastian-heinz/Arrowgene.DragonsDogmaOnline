using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : StructurePacketHandler<GameClient, C2SQuestQuestProgressReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestProgressHandler));

        private readonly ExpManager _ExpManager;

        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
            _ExpManager = server.ExpManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestProgressReq> packet)
        {
            QuestState questState = QuestState.InProgress;
            S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
            res.QuestScheduleId = packet.Structure.QuestScheduleId;
            res.QuestProgressResult = 0;


            Logger.Debug($"KeyId={packet.Structure.KeyId} ProgressCharacterId={packet.Structure.ProgressCharacterId}, QuestScheduleId={packet.Structure.QuestScheduleId}, ProcessNo={packet.Structure.ProcessNo}\n");

            QuestId questId = (QuestId) packet.Structure.QuestScheduleId;
            if (!client.Character.Quests.ContainsKey(questId))
            {
                List<CDataQuestCommand> ResultCommandList = new List<CDataQuestCommand>();
                ResultCommandList.Add(new CDataQuestCommand()
                {
                    Command = (ushort)QuestCommandCheckType.IsEndTimer,
                    Param01 = 0x173
                });

                res.QuestScheduleId = 0x32f00;
                res.QuestProcessState.Add(new CDataQuestProcessState()
                {
                    ProcessNo = 0x1b,
                    SequenceNo = 0x1,
                    BlockNo = 0x2,
                    ResultCommandList = ResultCommandList
                });
            }
            else
            {
                var quest = client.Character.Quests[questId];
                res.QuestProcessState = quest.StateMachineExecute(packet.Structure.KeyId, packet.Structure.QuestScheduleId, packet.Structure.ProcessNo, out questState);

                if (questState == QuestState.Cleared)
                {
                    var rewards = quest.CreateRewardsPacket();
                    client.Send(rewards);

                    // Delete the quest
                    client.Character.Quests.Remove(questId);
                }
            }

            S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = res.QuestScheduleId,
                QuestProcessStateList = res.QuestProcessState,
            };
            client.Party.SendToAllExcept(ntc, client);

            client.Send(res);

#if false
            if (questState == QuestState.Cleared)
            {
                // This seems set to some reset packet or something interesting
                var complete = new S2CQuestCompleteNtc() { QuestScheduleId = packet.Structure.QuestScheduleId };
                client.Party.SendToAll(complete);
            }
#endif

#if false
            if (process == QuestProcess.ProcessEnd)
            {
            }

            client.Send(res);

            // TODO: These can be saved in some dictionary
            // TODO: Extra params for which character it is for
            // TODO: Make some interface they adhere to
            // TODO: Other quests happen here as well, this is the main quest handler
            // -- Missions
            // -- World Quests
            // -- Personal Requests
            // -- Spirit Dragon
            QuestProcess process = QuestProcess.ExecuteCommand;
            if (packet.Structure.QuestScheduleId == 287350)
            {
                Logger.Debug("============ Main Story Quest activated ============");
                Logger.Debug($"ProcessNo = {packet.Structure.ProcessNo}");
                res.QuestProcessState = Mq000002_TheSlumberingGod.StateMachineExecute(packet.Structure.KeyId, packet.Structure.QuestScheduleId, packet.Structure.ProcessNo);
            }
            else if (packet.Structure.QuestScheduleId == 20005010)
            {
                Logger.Debug("============ World Quest activated ============");
                Logger.Debug($"ProcessNo = {packet.Structure.ProcessNo}");
                res.QuestProcessState = WorldQuests.LestaniaCyclops.StateMachineExecute(client, packet.Structure.KeyId, packet.Structure.QuestScheduleId, packet.Structure.ProcessNo, out process, _ExpManager);
            }
            else
            {
                List<CDataQuestCommand> ResultCommandList = new List<CDataQuestCommand>();
                ResultCommandList.Add(new CDataQuestCommand()
                {
                    Command = (ushort)QuestCommandCheckType.IsEndTimer,
                    Param01 = 0x173
                });

                res.QuestScheduleId = 0x32f00;
                res.QuestProcessState.Add(new CDataQuestProcessState()
                {
                    ProcessNo = 0x1b,
                    SequenceNo = 0x1,
                    BlockNo = 0x2,
                    ResultCommandList = ResultCommandList
                });
            }
#endif
        }
    }
}
