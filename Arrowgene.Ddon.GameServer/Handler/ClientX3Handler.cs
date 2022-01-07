using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Model;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientX3Handler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX3Handler));


        public ClientX3Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X3_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(Dump.LoginDump.Dump_20);
        }

    }
}
