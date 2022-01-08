using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientX8Handler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX8Handler));


        public ClientX8Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X8;

        public override void Handle(Client client, Packet packet)
        {

        }
    }
}
