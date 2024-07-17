using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailDeleteHandler : GameRequestPacketHandler<C2SMailSystemMailDeleteReq, S2CMailSystemMailDeleteRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailSystemMailDeleteHandler));

        public MailSystemMailDeleteHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailSystemMailDeleteRes Handle(GameClient client, C2SMailSystemMailDeleteReq request)
        {
            Server.Database.DeleteSystemMailMessage(request.MessageId);
            return new S2CMailSystemMailDeleteRes()
            {
                MessageId = request.MessageId,
            };
        }
    }
}
