using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetValidChatComGroupHandler : StructurePacketHandler<GameClient, C2SGpGetValidChatComGroupReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetValidChatComGroupHandler));

        public GpGetValidChatComGroupHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SGpGetValidChatComGroupReq> packet)
        {
            client.Send(new S2CGpGetValidChatComGroupRes());
        }
    }
}