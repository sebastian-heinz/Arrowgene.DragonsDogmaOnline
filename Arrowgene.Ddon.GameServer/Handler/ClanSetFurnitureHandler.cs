using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanSetFurnitureHandler : StructurePacketHandler<GameClient, C2SClanSetFurnitureReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanSetFurnitureHandler));

        public ClanSetFurnitureHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SClanSetFurnitureReq> req)
        {
            S2CClanSetFurnitureRes res = new S2CClanSetFurnitureRes();
            res.FurnitureUpdate = req.Structure;
            client.Send(res);
        }
    }
}
