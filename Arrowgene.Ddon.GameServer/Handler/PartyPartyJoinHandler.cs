using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyJoinHandler : GameRequestPacketHandler<C2SPartyPartyJoinReq, S2CPartyPartyJoinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyJoinHandler));

        public PartyPartyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyJoinRes Handle(GameClient client, C2SPartyPartyJoinReq request)
        {
            S2CPartyPartyJoinRes res = new S2CPartyPartyJoinRes();

            PartyGroup party = Server.PartyManager.GetParty(request.PartyId) 
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, 
                "failed to find party (Server.PartyManager.GetParty() == null)");

            PlayerPartyMember join = party.Join(client);
            
            var partyLeader = party.Leader?.Client.Character
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_LEADER_ABSENCE);

            res.ContentNumber = party.ContentId;
            if (BoardManager.BoardIdIsExm(party.ContentId))
            {
                Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Contents);
            }
            else
            {
                Server.CharacterManager.UpdateOnlineStatus(party.Leader.Client, partyLeader, OnlineStatus.PtLeader);
                if (partyLeader.CharacterId != client.Character.CharacterId)
                {
                    Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.PtMember);
                }
            }

            var progress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.All);
            foreach (var questProgress in progress)
            {
                var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                if (quest != null && quest.IsPersonal)
                { 
                    join.QuestState.AddNewQuest(questProgress.QuestScheduleId, questProgress.Step);
                }
            }

            S2CPartyPartyJoinNtc ntc = new S2CPartyPartyJoinNtc();
            ntc.HostCharacterId = party.Host.Client.Character.CharacterId;
            ntc.LeaderCharacterId = partyLeader.CharacterId;
            foreach (PartyMember member in party.Members)
            {
                ntc.PartyMembers.Add(member.GetCDataPartyMember());
            }

            party.SendToAll(ntc);

            S2CContextGetPartyPlayerContextNtc newMemberContext = join.GetPartyContext();
            if (partyLeader.CommonId != client.Character.CommonId)
            {
                // Update player position when joining from a different stage
                client.Character.StageNo = partyLeader.StageNo;
                client.Character.Stage = partyLeader.Stage;
                newMemberContext.Context.Base.StageNo = (int) partyLeader.StageNo;
            }

            if (party.Clients.Count > 0)
            {
                // Send existing party player context NTCs to the new member
                foreach (PartyMember member in party.Members)
                {
                    if (member.MemberIndex == join.MemberIndex)
                    {
                        // Skip ourselves
                        continue;
                    }

                    if (member is PlayerPartyMember playerMember)
                    {
                        client.Send(playerMember.GetPartyContext());
                    }
                    else if (member is PawnPartyMember pawnPartyMember)
                    {
                        client.Send(pawnPartyMember.GetPartyContext());
                    }
                }

                // Send new members to all existing party members
                // client.Party.SendToAllExcept(newMemberContext, client);
                client.Party.SendToAll(newMemberContext);
            }

            res.PartyId = party.Id;

            Logger.Info(client, $"joined PartyId:{party.Id}");

            return res;
        }
    }
}
