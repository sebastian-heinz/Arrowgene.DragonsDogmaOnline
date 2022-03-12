using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetWarpPointListHandler : StructurePacketHandler<GameClient, C2SWarpGetWarpPointListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));


        public WarpGetWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpGetWarpPointListReq> packet)
        {
            S2CWarpGetWarpPointListRes res = EntitySerializer.Get<S2CWarpGetWarpPointListRes>().Read(GameFull.data_Dump_140);
            //S2CWarpGetWarpPointListRes res = new S2CWarpGetWarpPointListRes();
            //res.WarpPointList.Add(new CDataWarpPoint(0x01, 0)); // White Dragon Temple, 0 RP
            //res.WarpPointList.Add(new CDataWarpPoint(0x02, 100)); // Tel, 100 RP
            //res.WarpPointList.Add(new CDataWarpPoint(0x03, 42069)); // Rotes, 42069 RP
            client.Send(res);
        }
    }
}
