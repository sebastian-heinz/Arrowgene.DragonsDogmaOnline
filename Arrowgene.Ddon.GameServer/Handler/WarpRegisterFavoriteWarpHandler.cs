using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpRegisterFavoriteWarpHandler : StructurePacketHandler<GameClient, C2SWarpRegisterFavoriteWarpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpRegisterFavoriteWarpHandler));

        public WarpRegisterFavoriteWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpRegisterFavoriteWarpReq> request)
        {
            // TODO: Figure out what they do
            S2CWarpRegisterFavoriteWarpRes response = new S2CWarpRegisterFavoriteWarpRes();
            response.SlotNo = 0;
            response.WarpPointId = 0;

            client.Send(response);
        }
    }
}