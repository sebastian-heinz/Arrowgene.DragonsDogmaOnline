using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Model;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientX4Handler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX4Handler));


        public ClientX4Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X4_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(Dump.LoginDump.Dump_22);
        }

    }
}
