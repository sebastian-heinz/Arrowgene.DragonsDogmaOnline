using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyBreakupHandler : GameStructurePacketHandler<C2SPartyPartyBreakupReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyChangeLeaderHandler));

        public PartyPartyBreakupHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyBreakupReq> packet)
        {
            S2CPartyPartyBreakupRes res = new S2CPartyPartyBreakupRes();

            PartyGroup party = client.Party;
            if (party == null)
            {
                Logger.Error(client, "Could not breakup party, does not exist");
                // todo return error
                return;
            }

            ErrorRes<List<PartyMember>> members = party.Breakup(client);
            if (members.HasError)
            {
                Logger.Info(client, $"error during breakup");
                res.Error = (uint)members.ErrorCode;
                client.Send(res);
                return;
            }

            S2CPartyPartyBreakupNtc ntc = new S2CPartyPartyBreakupNtc();
            foreach (PartyMember member in members.Value)
            {
                if (member is PlayerPartyMember player)
                {
                    player.Client.Send(ntc);
                }
            }

            client.Send(res);
            Logger.Info(client, $"breakup PartyId:{party.Id}");
        }
    }
}
