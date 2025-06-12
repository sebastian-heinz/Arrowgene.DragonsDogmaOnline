using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.GameServer.Quests.LightQuests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : GameRequestPacketQueueHandler<C2SStageAreaChangeReq, S2CStageAreaChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageAreaChangeHandler));

        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SStageAreaChangeReq packet)
        {
            PacketQueue queue = new();
            S2CStageAreaChangeRes res = new S2CStageAreaChangeRes();
            res.StageNo = StageManager.ConvertIdToStageNo(packet.StageId);
            res.IsBase = StageManager.IsSafeArea(packet.StageId); // This is set true for audience chamber and WDT for example
            
            if (res.StageNo == Stage.ArisensRoom.StageNo)
            {
                // Re-enables Arisen's Room dialogue for pawns.
                // These StageFeatures control chatter and other pawn behaviors.
                res.StageFeatureList.Add(new(9001));
            }

            // Order is notices sent manually, then the response, then other queued notices for Epitaph Road stuff.

            uint previousStageId = client.Character.Stage.Id;

            ContextManager.DelegateAllMasters(client);

            client.Character.StageNo = res.StageNo;
            client.Character.Stage = new StageLayoutId(packet.StageId, 0, 0);

            // For shared spaces, deal with all the context updating required for characters to be visible.
            // Must be done after Character.StageNo is set because of how the context is structured.
            Server.HubManager.UpdateLobbyContextOnStageChange(client, previousStageId, packet.StageId);

            foreach (var pawn in client.Character.Pawns)
            {
                pawn.StageNo = res.StageNo;
            }

            if (res.IsBase)
            {
                client.Character.LastSafeStageId = packet.StageId;

                bool shouldReset = true;
                // Check to see if all player members are in a safe area.
                foreach (var member in client.Party.Clients)
                {
                    // TODO: Is it safe to iterate over player party members this way?
                    // TODO: Can this logic be made part of the party object instead?
                    shouldReset &= StageManager.IsSafeArea(member.Character.Stage);
                    if (!shouldReset)
                    {
                        // No need to loop over rest of party members
                        break;
                    }
                }

                if (shouldReset)
                {
                    if (client.Party.MemberCount() > 1)
                    {
                        Server.ChatManager.BroadcastMessageToParty(client.Party, LobbyChatMsgType.ManagementGuideC, "The entire party has returned to a safe area.");
                    }

                    Server.EpitaphRoadManager.ResetInstance(client.Party);
                    client.Party.ResetInstance();
                    client.Party.SendToAll(new S2CInstanceAreaResetNtc());

                    Server.Database.ExecuteInTransaction(connection =>
                    {
                        foreach (var member in client.Party.Clients)
                        {
                            var activeScheduleIds = member.QuestState?.GetActiveQuestScheduleIds().Where(x => LightQuestManager.IsLightQuestScheduleId(x)) ?? [];
                            var droppedQuests = Server.LightQuestManager.GetDecayedQuests(activeScheduleIds);
                            foreach (var droppedQuest in droppedQuests)
                            {
                                Logger.Debug($"Quest {droppedQuest} has decayed and will be cancelled.");

                                var quest = QuestManager.GetQuestByScheduleId(droppedQuest);
                                if (quest is not null)
                                {
                                    // TODO: Is this the right notice?
                                    client.Enqueue(new S2CQuestQuestCancelSilentNtc()
                                    {
                                        QuestScheduleId = droppedQuest
                                    }, queue);
                                }
                                member.QuestState.CancelQuest(droppedQuest);
                                Server.Database.RemoveQuestProgress(member.Character.CommonId, droppedQuest, quest.QuestType, connection);
                            }

                            if (droppedQuests.Any())
                            {
                                member.Enqueue(new S2CLobbyChatMsgNotice()
                                {
                                    Type = LobbyChatMsgType.ManagementAlertC,
                                    Message = "A quest has been canceled because the\ndelivery time period has ended."
                                }, queue);

                                if (member.Party.IsSolo || member.Party.Leader?.Client == member)
                                {
                                    queue.AddRange(member.Party.QuestState.UpdatePriorityQuestList(member.Party.Leader.Client, connection));
                                }
                            }
                        }
                    });
                }
            }

            client.Enqueue(res, queue);

            // Tutorial quests which are not in the accepted state will continue to progress until they
            // become accepted. This might happen when using rules to prevent the quest from showing
            // until some requirements are met. Remove them when the player switches areas so when they
            // reenter the area, it start's over.
            QuestManager.PurgeUnstartedTutorialQuests(client);

            if (client.Party.ExmInProgress && BoardManager.BoardIdIsExm(client.Party.ContentId))
            {
                var quest = QuestManager.GetQuestByBoardId(client.Party.ContentId);
                if (quest != null)
                {
                    quest.HandleAreaChange(client, client.Character.Stage);
                }
            }

            if (client.IsPartyLeader())
            {
                Server.PartnerPawnManager.HandleStageAreaChange(client);
            }

            queue.AddRange(Server.JobMasterManager.HandleAreaChange(client));

            Server.EpitaphRoadManager.AreaChange(client, packet.StageId, queue);
            return queue;
        }
    }
}
