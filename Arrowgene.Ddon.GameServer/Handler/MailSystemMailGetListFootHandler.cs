using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailGetListFootHandler : GameRequestPacketHandler<C2SMailSystemMailGetListFootReq, S2CMailSystemMailGetListFootRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailSystemMailGetListFootHandler));

        public MailSystemMailGetListFootHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailSystemMailGetListFootRes Handle(GameClient client, C2SMailSystemMailGetListFootReq request)
        {
            // client.Send(InGameDump.Dump_87);
            return new S2CMailSystemMailGetListFootRes();
        }
    }
}
