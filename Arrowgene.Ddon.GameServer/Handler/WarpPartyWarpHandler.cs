using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpPartyWarpHandler : GameStructurePacketHandler<C2SWarpPartyWarpReq>
    {
        public static readonly uint PARTY_WARP_SECONDS = 60;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpPartyWarpHandler));
        
        public WarpPartyWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpPartyWarpReq> packet)
        {
            S2CWarpPartyWarpRes res = new S2CWarpPartyWarpRes();

            // Check if its true that the party leader has teleported there
            // and that the request was sent before the time ran out
            TimeSpan warpTimeDifference = DateTime.Now - client.Party.Leader.Client.LastWarpDateTime;
            if(warpTimeDifference > TimeSpan.FromSeconds(60)) {
                res.Result = 1; // I guess
            }

            client.Send(res);
        }
    }
}