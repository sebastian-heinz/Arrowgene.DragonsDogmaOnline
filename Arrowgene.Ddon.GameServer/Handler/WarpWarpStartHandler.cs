using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpWarpStartHandler : GameStructurePacketHandler<C2SWarpWarpStartNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpWarpStartHandler));
        
        public WarpWarpStartHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpWarpStartNtc> packet)
        {
            if(client.LastWarpPointId != 0 && (client.Party?.GetPlayerPartyMember(client)?.IsLeader ?? false))
            {
                S2CWarpLeaderWarpNtc ntc = new S2CWarpLeaderWarpNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    DestPointId = client.LastWarpPointId,
                    RestSecond = WarpPartyWarpHandler.PARTY_WARP_SECONDS
                };
                client.Party.SendToAllExcept(ntc, client);
            }
        }
    }
}
