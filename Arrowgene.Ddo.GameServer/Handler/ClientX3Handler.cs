using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Model;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientX3Handler : PacketHandler
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(ClientX3Handler));


        public ClientX3Handler(DdoGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X3_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(Dump.LoginDump.Dump_20);
        }

    }
}
