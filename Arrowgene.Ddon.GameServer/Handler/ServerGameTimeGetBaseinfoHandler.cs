using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGameTimeGetBaseinfoHandler : StructurePacketHandler<GameClient, C2SServerGameTimeGetBaseInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGameTimeGetBaseinfoHandler));

        public ServerGameTimeGetBaseinfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SServerGameTimeGetBaseInfoReq> packet)
        {
            client.Send(new S2CServerGameTimeGetBaseInfoRes());
        }

        // Adapted from the client's code
        private long calcGameTimeMSec(DateTimeOffset realTime, long originalRealTimeSec, uint gameTimeOneDayMin, uint gameTimeDayHour)
        {
            return (1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - originalRealTimeSec)) / gameTimeOneDayMin)
            % (3600000 * gameTimeDayHour);
        }
    }
}
