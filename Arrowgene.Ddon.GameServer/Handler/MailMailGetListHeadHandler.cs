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
            var messages = Server.Database.SelectMailMessages(client.Character.CharacterId);

            return new S2CMailMailGetListHeadRes() 
            { 
                Num = (uint)messages.Count 
            };
        }
    }
}
