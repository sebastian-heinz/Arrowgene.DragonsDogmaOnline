using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailGetListHeadHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(MailMailGetListHeadHandler));


        public MailMailGetListHeadHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_MAIL_MAIL_GET_LIST_HEAD_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_77);
        }
    }
}
