using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailGetListDataHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(MailSystemMailGetListDataHandler));


        public MailSystemMailGetListDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_LIST_DATA_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_85);
        }
    }
}
