using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailGetListFootHandler : GameRequestPacketHandler<C2SMailMailGetListFootReq, S2CMailMailGetListFootRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailMailGetListFootHandler));

        public MailMailGetListFootHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailMailGetListFootRes Handle(GameClient client, C2SMailMailGetListFootReq request)
        {
            // client.Send(InGameDump.Dump_81);
            return new S2CMailMailGetListFootRes();
        }
    }
}
