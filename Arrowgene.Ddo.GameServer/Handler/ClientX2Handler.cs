using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Model;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientX2Handler : PacketHandler
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(ClientX2Handler));


        public ClientX2Handler(DdoGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X2_REQ_CHAR;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(Dump.LoginDump.Dump_8);
            client.Send(Dump.LoginDump.Dump_9);
            client.Send(Dump.LoginDump.Dump_10);
            client.Send(Dump.LoginDump.Dump_11);
            client.Send(Dump.LoginDump.Dump_12);
            client.Send(Dump.LoginDump.Dump_13);
            client.Send(Dump.LoginDump.Dump_14);
            client.Send(Dump.LoginDump.Dump_15);
            client.Send(Dump.LoginDump.Dump_16);
            client.Send(Dump.LoginDump.Dump_17);
            
            client.Send(Dump.LoginDump.Dump_18);
        }

    }
}
