using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailGetListHeadHandler : GameRequestPacketHandler<C2SMailMailGetListHeadReq, S2CMailMailGetListHeadRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailMailGetListHeadHandler));

        public MailMailGetListHeadHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailMailGetListHeadRes Handle(GameClient client, C2SMailMailGetListHeadReq request)
        {
            // client.Send(InGameDump.Dump_77);
            // TODO: Num = 1 is the single message from the static packet currently used in the other
            // TODO: parts of this mail handler.
            return new S2CMailMailGetListHeadRes() { Num = 1 };
        }
    }
}
