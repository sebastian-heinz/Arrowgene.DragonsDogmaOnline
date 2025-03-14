using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetRealTimeHandler : GameRequestPacketHandler<C2SServerGetRealTimeReq, S2CServerGetRealTimeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetRealTimeHandler));


        public ServerGetRealTimeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerGetRealTimeRes Handle(GameClient client, C2SServerGetRealTimeReq request)
        {
            return new(DateTimeOffset.UtcNow);
        }
    }
}
