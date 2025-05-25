using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteCancelHandler : GameRequestPacketHandler<C2SPartyPartyInviteCancelReq, S2CPartyPartyInviteCancelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyChangeLeaderHandler));

        public PartyPartyInviteCancelHandler(DdonGameServer server) : base(server)
        {
        }

        // TODO: Figure out what the heck is going on here.
        public override S2CPartyPartyInviteCancelRes Handle(GameClient client, C2SPartyPartyInviteCancelReq request)
        {
            Logger.Info(client, $"C2SPartyPartyInviteCancelReq -> ServerId:{request.ServerId} PartyId:{request.PartyId}");
            S2CPartyPartyInviteCancelRes res = new S2CPartyPartyInviteCancelRes();
            
            PartyGroup party = client.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_INVITED, "failed to find party");

            //Server.PartyManager.GetPartyInvitation()

            S2CPartyPartyInviteCancelNtc ntc = new S2CPartyPartyInviteCancelNtc()
            {
                ErrorCode = ErrorCode.ERROR_CODE_PARTY_INVITE_HOST_CANCEL
            };
            party.SendToAll(ntc);

            Server.PartyManager.CancelPartyInvitation(party);

            Logger.Info(client, $"cancel invite for PartyId:{party.Id}");
            return res;
        }

    }
}
