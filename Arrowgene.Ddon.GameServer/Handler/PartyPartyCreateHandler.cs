using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyCreateHandler : GameRequestPacketHandler<C2SPartyPartyCreateReq, S2CPartyPartyCreateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));

        public PartyPartyCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyCreateRes Handle(GameClient client, C2SPartyPartyCreateReq request)
        {
            S2CPartyPartyCreateRes res = new S2CPartyPartyCreateRes();

            PartyGroup party = Server.PartyManager.NewParty()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_CREATE_ERROR, "can not create party (Server.PartyManager.NewParty() == null)");

            party.AddHost(client);

            PlayerPartyMember join = party.Join(client);
            
            var progress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.All);
            foreach (var questProgress in progress)
            {
                var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                if (quest is null)
                {
                    continue;
                }

                Logger.Info($"QuestScheduleId={quest.QuestScheduleId}");

                QuestStateManager questStateManager = QuestManager.GetQuestStateManager(client, quest);
                questStateManager.AddNewQuest(questProgress.QuestScheduleId, questProgress.Step);
            }

            // Add quest for debug command
            party.QuestState.AddNewQuest(QuestManager.GetQuestByScheduleId(70000001));

            S2CPartyPartyJoinNtc ntc = new S2CPartyPartyJoinNtc();
            ntc.HostCharacterId = client.Character.CharacterId;
            ntc.LeaderCharacterId = client.Character.CharacterId;
            ntc.PartyMembers.Add(join.GetCDataPartyMember());
            client.Send(ntc);

            res.PartyId = party.Id;

            Logger.Info(client, $"created party with PartyId:{party.Id}");

            return res;
        }
    }
}
