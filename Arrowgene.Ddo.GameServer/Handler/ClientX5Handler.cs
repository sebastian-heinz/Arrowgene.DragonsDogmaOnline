using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Model;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientX5Handler : PacketHandler
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(ClientX5Handler));


        public ClientX5Handler(DdoGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X5_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(Dump.LoginDump.Dump_24);
        }

    }
}
