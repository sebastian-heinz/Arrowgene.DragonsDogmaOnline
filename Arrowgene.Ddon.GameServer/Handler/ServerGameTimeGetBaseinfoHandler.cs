using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            res.GameTimeBaseInfo.GameTimeOneDayMin = Server.Setting.GameLogicSetting.GameClockTimescale;
            res.WeatherLoop = Server.WeatherManager.WeatherLoopList;

            //TODO: Investigate these values. The moon cycles but predicting the current phase serverside is still unclear.
            res.MoonAgeLoopSec = Server.Setting.GameLogicSetting.GameClockTimescale * 60;
            res.MoonSchedule.Add(new CDataMoonSchedule()
            {
                BeginTimeSec = long.MinValue,
                EndTimeSec = long.MaxValue,
                MoonAge = 14
            });

            return res;
        }
    }
}
