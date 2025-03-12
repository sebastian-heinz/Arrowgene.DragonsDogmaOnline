using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyBreakupHandler : GameRequestPacketHandler<C2SPartyPartyBreakupReq, S2CPartyPartyBreakupRes>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyChangeLeaderHandler));

        public PartyPartyBreakupHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartyPartyBreakupRes Handle(GameClient client, C2SPartyPartyBreakupReq request)
        {
            S2CPartyPartyBreakupRes res = new S2CPartyPartyBreakupRes();

            PartyGroup party = client.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "Could not breakup party, does not exist");

            List<PartyMember> members = party.Breakup(client);

            S2CPartyPartyBreakupNtc ntc = new S2CPartyPartyBreakupNtc();
            foreach (PartyMember member in members)
            {
                if (member is PlayerPartyMember player)
                {
                    Server.PartnerPawnManager.HandleLeaveFromParty(player.Client);
                    player.Client.Send(ntc);
                }
            }

            Logger.Info(client, $"breakup PartyId:{party.Id}");
            return res;
        }
    }
}
