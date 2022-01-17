using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailGetListFootHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(MailMailGetListFootHandler));


        public MailMailGetListFootHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_MAIL_MAIL_GET_LIST_FOOT_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_81);
        }
    }
}
