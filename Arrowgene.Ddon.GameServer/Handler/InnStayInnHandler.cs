using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayInnHandler : StructurePacketHandler<GameClient, C2SInnStayInnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnStayInnHandler));

        public InnStayInnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnStayInnReq> req)
        {
            S2CInnStayInnRes res = new S2CInnStayInnRes();
            res.ReqData = req.Structure;
            res.Point = 10000000;
            client.Send(res);
        }
    }
}
