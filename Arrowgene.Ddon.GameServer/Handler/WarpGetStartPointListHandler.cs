using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetStartPointListHandler : StructurePacketHandler<GameClient, C2SWarpGetStartPointListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetStartPointListHandler));


        public WarpGetStartPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpGetStartPointListReq> packet)
        {
            S2CWarpGetStartPointListRes res = new S2CWarpGetStartPointListRes();
            res.WarpPointIdList.Add(new CDataCommonU32(0x01)); // White Dragon Temple
            res.WarpPointIdList.Add(new CDataCommonU32(0x57)); // Megadosis
            client.Send(res);
        }
    }
}
