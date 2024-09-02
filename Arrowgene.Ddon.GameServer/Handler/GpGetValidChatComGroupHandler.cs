using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            S2CGpGetValidChatComGroupRes response = new S2CGpGetValidChatComGroupRes();
            response.ValidChatComGroups.Add(new CDataCommonU32(1));
            response.ValidChatComGroups.Add(new CDataCommonU32(2));
            client.Send(response);
        }
    }
}