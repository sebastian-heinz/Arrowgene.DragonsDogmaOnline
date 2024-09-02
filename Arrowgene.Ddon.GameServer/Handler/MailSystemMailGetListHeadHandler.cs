using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailGetListHeadHandler : GameRequestPacketHandler<C2SMailSystemMailGetListHeadReq, S2CMailSystemMailGetListHeadRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailSystemMailGetListHeadHandler));

        public MailSystemMailGetListHeadHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailSystemMailGetListHeadRes Handle(GameClient client, C2SMailSystemMailGetListHeadReq request)
        {
            var pcap = new S2CMailSystemMailGetListHeadRes.Serializer().Read(InGameDump.Dump_83.AsBuffer());

            var messages = Server.Database.SelectSystemMailMessages(client.Character.CharacterId);
            return new S2CMailSystemMailGetListHeadRes()
            {
                Num = (uint) messages.Count,
            };
        }
    }
}
