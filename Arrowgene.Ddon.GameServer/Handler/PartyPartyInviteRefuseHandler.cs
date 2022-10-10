using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteRefuseHandler : GameStructurePacketHandler<C2SPartyPartyInviteRefuseReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyChangeLeaderHandler));

        public PartyPartyInviteRefuseHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInviteRefuseReq> packet)
        {
            S2CPartyPartyInviteRefuseRes res = new S2CPartyPartyInviteRefuseRes();

            PartyInvitation invitation = Server.PartyManager.GetPartyInvitation(client);
            if (invitation == null)
            {
                Logger.Error(client, "failed to find invitation");
                res.Error = (uint)ErrorCode.ERROR_CODE_PARTY_NOT_INVITED;
                client.Send(res);
                return;
            }

            GameClient host = invitation.Host;
            if (host == null)
            {
                Logger.Error(client, "invitation has no host");
                res.Error = (uint)ErrorCode.ERROR_CODE_PARTY_NOT_INVITED;
                client.Send(res);
                return;
            }

            PartyGroup party = invitation.Party;
            if (party == null)
            {
                Logger.Error(client, "failed to find party");
                res.Error = (uint)ErrorCode.ERROR_CODE_PARTY_NOT_INVITED;
                client.Send(res);
                return;
            }

            ErrorRes<PartyInvitation> refuse = party.RefuseInvite(client);
            if (refuse.HasError)
            {
                res.Error = (uint)refuse.ErrorCode;
                client.Send(res);
                return;
            }

            client.Send(res);
        }
    }
}
