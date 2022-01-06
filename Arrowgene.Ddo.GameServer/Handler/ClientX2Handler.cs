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

        public override PacketId Id => PacketId.X2_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(StaticResponse.Character);
        //    client.Send(StaticResponse.Websites);
          //  client.Send(StaticResponse.P);
       //     client.Send(StaticResponse.Account);
        }

    }
}
