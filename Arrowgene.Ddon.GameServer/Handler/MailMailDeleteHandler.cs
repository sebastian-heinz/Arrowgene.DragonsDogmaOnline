using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailDeleteHandler : GameRequestPacketHandler<C2SMailMailDeleteReq, S2CMailMailDeleteRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailMailDeleteHandler));

        public MailMailDeleteHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailMailDeleteRes Handle(GameClient client, C2SMailMailDeleteReq request)
        {
            Server.Database.DeleteMailMessage(request.MailId);
            return new()
            {
                MailId = request.MailId,
            };
        }
    }
}
