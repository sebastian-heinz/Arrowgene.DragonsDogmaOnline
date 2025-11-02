using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailGetListDataHandler : GameRequestPacketHandler<C2SMailMailGetListDataReq, S2CMailMailGetListDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailMailGetListDataHandler));

        public MailMailGetListDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailMailGetListDataRes Handle(GameClient client, C2SMailMailGetListDataReq request)
        {
            // client.Send(InGameDump.Dump_79);
            //var res = new S2CMailMailGetListDataRes.Serializer().Read(InGameDump.Dump_79.AsBuffer());

            S2CMailMailGetListDataRes res = new()
            {
                MailInfo = [.. Server.Database.SelectMailMessages(client.Character.CharacterId).Select(x => x.ToCDataMailInfo(0))]
            };

            return res;

        }
    }
}

