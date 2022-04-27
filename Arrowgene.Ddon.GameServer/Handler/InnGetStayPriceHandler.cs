using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnGetStayPriceHandler : StructurePacketHandler<GameClient, C2SInnGetStayPriceReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnGetStayPriceHandler));

        public InnGetStayPriceHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnGetStayPriceReq> req)
        {
            S2CInnGetStayPriceRes res = new S2CInnGetStayPriceRes();
            res.ReqData = req.Structure;
            client.Send(res);
        }
    }
}
