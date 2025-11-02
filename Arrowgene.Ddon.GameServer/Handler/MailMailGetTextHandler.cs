using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailGetTextHandler : GameRequestPacketHandler<C2SMailMailGetTextReq, S2CMailMailGetTextRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailMailGetTextHandler));

        public MailMailGetTextHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailMailGetTextRes Handle(GameClient client, C2SMailMailGetTextReq request)
        {
            MailMessage message = new();

            Server.Database.ExecuteInTransaction(connection =>
            {
                message = Server.Database.SelectMailMessage(request.MailId, connection);
                Server.Database.UpdateMailMessageState(request.MailId, MailState.Opened, connection);
            });

            var result = new S2CMailMailGetTextRes()
            {
                MailId = request.MailId,
                MailText = message.Body
            };

            return result;
        }
    }
}
