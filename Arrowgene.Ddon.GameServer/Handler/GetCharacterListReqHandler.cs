using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Model;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientX5Handler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX5Handler));


        public ClientX5Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(Dump.LoginDump.Dump_24);
        }

    }
}
