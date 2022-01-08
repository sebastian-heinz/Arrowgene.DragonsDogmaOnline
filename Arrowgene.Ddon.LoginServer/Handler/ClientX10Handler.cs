using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientX10Handler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX10Handler));


        public ClientX10Handler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X10_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            client.Send(LoginDump.Dump_36);
        }
    }
}
