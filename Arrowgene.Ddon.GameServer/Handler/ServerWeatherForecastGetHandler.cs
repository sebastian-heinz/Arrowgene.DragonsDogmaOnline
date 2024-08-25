using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerWeatherForecastGetHandler : GameRequestPacketHandler<C2SServerWeatherForecastGetReq, S2CServerWeatherForecastGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerWeatherForecastGetHandler));

        /// <summary>
        /// Number of game hours between forecast times. 
        /// The first forecast is always at the top of the next hour, then the next three are separated by IntervalGameHour.
        /// </summary>
        public static readonly uint IntervalGameHour = 3;

        public ServerWeatherForecastGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerWeatherForecastGetRes Handle(GameClient client, C2SServerWeatherForecastGetReq request)
        {
            var res = new S2CServerWeatherForecastGetRes()
            {
                IntervalGameHour = IntervalGameHour,
                GameDayToEarthMin = Server.Setting.GameLogicSetting.GameClockTimescale,
                ForecastList = Server.WeatherManager.GetForecast()
            };

            return res;
        }
    }
}
