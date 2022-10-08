using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
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
                client.Send(res);
                return;
            }

            GameClient host = invitation.Host;
            if (host == null)
            {
                Logger.Error(client, "host does not exist");
                client.Send(res);
                return;
            }

   

            PartyGroup party = invitation.Party;
            if (party == null)
            {
                Logger.Error(client, "failed to find invited party");
                client.Send(res);
                return;
            }

            party.RefuseInvite(client);
            
            // TODO update others about refuse

            client.Send(res);
            Logger.Info(client, $"refuse invite for PartyId:{invitation.Party.Id}");
        }
    }
}
