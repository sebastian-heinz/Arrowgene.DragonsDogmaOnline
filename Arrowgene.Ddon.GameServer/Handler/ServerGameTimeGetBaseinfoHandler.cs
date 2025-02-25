using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGameTimeGetBaseinfoHandler : GameRequestPacketHandler<C2SServerGameTimeGetBaseInfoReq, S2CServerGameTimeGetBaseInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGameTimeGetBaseinfoHandler));

        public ServerGameTimeGetBaseinfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerGameTimeGetBaseInfoRes Handle(GameClient client, C2SServerGameTimeGetBaseInfoReq request)
        {
            var res = new S2CServerGameTimeGetBaseInfoRes();
            res.GameTimeBaseInfo.OriginalGameTimeSec = WeatherManager.OriginalGameTimeSec;
            res.GameTimeBaseInfo.OriginalRealTimeSec = WeatherManager.OriginalRealTimeSec;
            res.GameTimeBaseInfo.GameTimeOneDayMin = Server.GameSettings.GameServerSettings.GameClockTimescale;
            res.WeatherLoop = Server.WeatherManager.WeatherLoopList;
            res.MoonAgeLoopSec = WeatherManager.MoonAgeLoopSec;

            return res;
        }
    }
}
