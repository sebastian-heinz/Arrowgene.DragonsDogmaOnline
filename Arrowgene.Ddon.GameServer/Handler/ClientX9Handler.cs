using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientX9Handler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX9Handler));


        public ClientX9Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X9_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(LoginDump.Dump_34);
        }
    }
}
