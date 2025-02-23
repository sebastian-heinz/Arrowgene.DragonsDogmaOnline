using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetValidChatComGroupHandler : GameRequestPacketHandler<C2SGpGetValidChatComGroupReq, S2CGpGetValidChatComGroupRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetValidChatComGroupHandler));

        public GpGetValidChatComGroupHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGetValidChatComGroupRes Handle(GameClient client, C2SGpGetValidChatComGroupReq request)
        {
            S2CGpGetValidChatComGroupRes response = new S2CGpGetValidChatComGroupRes();
            response.ValidChatComGroups.Add(new CDataCommonU32(1));
            response.ValidChatComGroups.Add(new CDataCommonU32(2));
            return response;
        }
    }
}
