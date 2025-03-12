using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyLeaveHandler : GameRequestPacketHandler<C2SPartyPartyLeaveReq, S2CPartyPartyLeaveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyLeaveHandler));

        public PartyPartyLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyLeaveRes Handle(GameClient client, C2SPartyPartyLeaveReq request)
        {
            PartyGroup party = client.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "Could not leave party, does not exist");

            if (client.Character.PartnerPawnId != 0)
            {
                Server.PartnerPawnManager.HandleLeaveFromParty(client);
            }

            party.Leave(client);
            Logger.Info(client, $"Left PartyId:{party.Id}");

            if (party.ContentId != 0)
            {
                var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);
                if (!data.IsInRecreate)
                {
                    Server.BoardManager.RemoveCharacterFromGroup(client.Character);
                    Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Online);

                    if (BoardManager.BoardIdIsExm(party.ContentId))
                    {
                        Server.PartyQuestContentManager.RemovePartyMember(party.Id, client.Character);
                    }
                }
                else
                {
                    Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.EntryBoard);
                }
            }
            else
            {
                Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Online);
            }

            if (party.MemberCount() == 1 && party.Leader != null && !BoardManager.BoardIdIsExm(party.ContentId))
            {
                Server.CharacterManager.UpdateOnlineStatus(party.Leader.Client, party.Leader.Client.Character, OnlineStatus.Online);
            }

            S2CPartyPartyLeaveNtc partyLeaveNtc = new S2CPartyPartyLeaveNtc();
            partyLeaveNtc.CharacterId = client.Character.CharacterId;
            party.SendToAll(partyLeaveNtc);

            return new();
        }
    }
}
