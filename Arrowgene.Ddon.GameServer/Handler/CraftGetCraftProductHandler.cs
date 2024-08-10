using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProductHandler : GameStructurePacketHandler<C2SCraftGetCraftProductReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProductHandler));

        public CraftGetCraftProductHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftGetCraftProductReq> packet)
        {
            // TODO: Implement once we get the UI to actually show up..

            C2SCraftGetCraftProductRes craftGetCraftProductRes = new C2SCraftGetCraftProductRes();

            client.Send(craftGetCraftProductRes);
        }
    }
}
