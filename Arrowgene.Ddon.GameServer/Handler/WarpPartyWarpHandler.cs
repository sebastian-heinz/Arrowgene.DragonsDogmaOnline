using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpPartyWarpHandler : GameRequestPacketHandler<C2SWarpPartyWarpReq, S2CWarpPartyWarpRes>
    {
        public static readonly uint PARTY_WARP_SECONDS = 60;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpPartyWarpHandler));
        
        public WarpPartyWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpPartyWarpRes Handle(GameClient client, C2SWarpPartyWarpReq request)
        {
            S2CWarpPartyWarpRes res = new S2CWarpPartyWarpRes();

            // Check if its true that the party leader has teleported there
            // and that the request was sent before the time ran out
            var lastWarpDateTime = client.Party.Leader?.Client.LastWarpDateTime ?? DateTime.MinValue;
            TimeSpan warpTimeDifference = DateTime.UtcNow - lastWarpDateTime;
            if(warpTimeDifference > TimeSpan.FromSeconds(60)) {
                res.Result = 1; // I guess
            }

            return res;
        }
    }
}
