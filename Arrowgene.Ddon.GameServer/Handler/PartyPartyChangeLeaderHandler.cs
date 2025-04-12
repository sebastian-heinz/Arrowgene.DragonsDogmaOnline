using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "Could not leave party, does not exist");

            var previousLeader = client.Party.Leader;

            party.ChangeLeader(client, newLeaderCharacterId);

            S2CPartyPartyChangeLeaderNtc ntc = new S2CPartyPartyChangeLeaderNtc();
            ntc.CharacterId = newLeaderCharacterId;
            party.SendToAll(ntc);


            PlayerPartyMember currentLeader = party.Leader;
            if (previousLeader != null)
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
                Server.CharacterManager.UpdateOnlineStatus(currentLeader.Client, currentLeader.Client.Character, OnlineStatus.Online);
            }
            else
            {
                Server.CharacterManager.UpdateOnlineStatus(currentLeader.Client, currentLeader.Client.Character, OnlineStatus.PtLeader);
            }

            return res;
        }
    }
}
