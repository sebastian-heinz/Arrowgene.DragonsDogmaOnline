using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientPingHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientPingHandler));


        public ClientPingHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_PING_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            client.PingTime = DateTime.UtcNow;
            ServerRes res = new ServerRes(PacketId.L2C_PING_RES);
            client.Send(res);
        }
    }
}
