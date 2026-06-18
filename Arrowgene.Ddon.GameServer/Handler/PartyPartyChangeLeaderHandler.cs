using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyChangeLeaderHandler : GameRequestPacketHandler<C2SPartyPartyChangeLeaderReq, S2CPartyPartyChangeLeaderRes>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyChangeLeaderHandler));

        public PartyPartyChangeLeaderHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyChangeLeaderRes Handle(GameClient client, C2SPartyPartyChangeLeaderReq request)
        {
            S2CPartyPartyChangeLeaderRes res = new S2CPartyPartyChangeLeaderRes();
            uint newLeaderCharacterId = request.CharacterId;

            PartyGroup party = client.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "Could not change leader, party does not exist");

            var previousLeader = party.Leader;

            PlayerPartyMember newLeader = party.ChangeLeader(client, newLeaderCharacterId);

            // If leader didn't actually change (request ignored), do nothing
            if (newLeader == null || previousLeader == newLeader)
            {
                return res;
            }

            S2CPartyPartyChangeLeaderNtc ntc = new S2CPartyPartyChangeLeaderNtc
            {
                CharacterId = newLeader.Client.Character.CharacterId
            };

            party.SendToAll(ntc);

            PlayerPartyMember currentLeader = party.Leader;

            if (previousLeader?.Client != null)
            {
                Server.CharacterManager.UpdateOnlineStatus(previousLeader.Client, previousLeader.Client.Character, OnlineStatus.PtMember);
                Logger.Info(client, $"Party leader changed from {previousLeader.Client.Character.CharacterId} to {currentLeader.Client.Character.CharacterId} for PartyId:{party.Id}");
            }
            else
            {
                Logger.Info(client, $"The character {currentLeader.Client.Character.CharacterId} has been promoted to leader for PartyId:{party.Id}");
            }

            if (party.MemberCount() == 1)
            {
                Server.CharacterManager.UpdateOnlineStatus(currentLeader.Client, currentLeader.Client.Character, CharacterManager.GetDefaultOnlineStatus(currentLeader.Client.Character));
            }
            else
            {
                Server.CharacterManager.UpdateOnlineStatus(currentLeader.Client, currentLeader.Client.Character, OnlineStatus.PtLeader);
            }

            // Reload the new leader's quest state into shared state so priority quests resolve correctly.
            // Leave() is not called on a manual leader change, so shared state still holds the old leader's quests.
            try
            {
                foreach (var questScheduleId in party.QuestState.GetActiveQuestScheduleIds().ToList())
                {
                    party.QuestState.RemoveQuest(questScheduleId);
                }

                var progress = Server.Database.GetQuestProgressByType(currentLeader.Client.Character.CommonId, QuestType.All);
                var deliveryProgressByQuest = Server.Database.GetAllQuestDeliveryProgress(currentLeader.Client.Character.CommonId)
                    .GroupBy(x => x.QuestScheduleId)
                    .ToDictionary(g => g.Key, g => g.ToList());
                foreach (var questProgress in progress)
                {
                    var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                    if (quest is null) continue;
                    QuestStateManager questStateManager = QuestManager.GetQuestStateManager(currentLeader.Client, quest);
                    questStateManager.AddNewQuest(questProgress.QuestScheduleId, questProgress.Step);

                    if (deliveryProgressByQuest.TryGetValue(questProgress.QuestScheduleId, out var deliveryItems))
                    {
                        foreach (var dp in deliveryItems)
                            questStateManager.RestoreDeliveryProgress(questProgress.QuestScheduleId, (ItemId)dp.ItemId, dp.AmountDelivered);
                    }
                }

                party.SendToAll(new S2CQuestGetMainQuestNtc());

                if (currentLeader.Client?.Character?.AreaId != QuestAreaId.None)
                {
                    var questListNtc = QuestGetSetQuestListHandler.BuildQuestListNtc(currentLeader.Client, currentLeader.Client.Character.AreaId, mutating: false);
                    party.SendToAll(questListNtc);
                }

                Logger.Info(client, $"[ChangeLeader] Reloaded quest state for new leader {currentLeader.Client.Character.CharacterId}");
            }
            catch (System.Exception ex)
            {
                Logger.Error(client, $"[ChangeLeader] Failed to reload quest state for new leader: {ex.Message}");
            }

            return res;
        }
    }
}
