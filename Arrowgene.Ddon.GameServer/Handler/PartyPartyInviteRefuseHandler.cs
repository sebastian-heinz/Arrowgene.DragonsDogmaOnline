using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteRefuseHandler : GameRequestPacketHandler<C2SPartyPartyInviteRefuseReq, S2CPartyPartyInviteRefuseRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteRefuseHandler));

        public PartyPartyInviteRefuseHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyInviteRefuseRes Handle(GameClient client, C2SPartyPartyInviteRefuseReq request)
        {
            S2CPartyPartyInviteRefuseRes res = new S2CPartyPartyInviteRefuseRes();

            PartyInvitation invitation = Server.PartyManager.GetPartyInvitation(client)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_INVITED, "failed to find invitation");

            GameClient host = invitation.Host
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_INVITED, "invitation has no host");
            
            PartyGroup party = invitation.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_INVITED, "failed to find party");

            PartyInvitation refuse = party.RefuseInvite(client);
            return res;
        }
    }
}
