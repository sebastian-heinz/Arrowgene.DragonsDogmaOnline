using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetRealTimeHandler : StructurePacketHandler<GameClient, C2SServerGetRealTimeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetRealTimeHandler));


        public ServerGetRealTimeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SServerGetRealTimeReq> packet)
        {
            client.Send(new S2CServerGetRealTimeRes(DateTimeOffset.UtcNow));
        }
    }
}
