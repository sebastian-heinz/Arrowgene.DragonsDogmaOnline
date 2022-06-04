using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanGetFurnitureHandler : StructurePacketHandler<GameClient, C2SClanGetFurnitureReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanGetFurnitureHandler));

        public ClanGetFurnitureHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SClanGetFurnitureReq> req)
        {
            S2CClanGetFurnitureRes res = new S2CClanGetFurnitureRes();
            client.Send(res);
        }
    }
}
