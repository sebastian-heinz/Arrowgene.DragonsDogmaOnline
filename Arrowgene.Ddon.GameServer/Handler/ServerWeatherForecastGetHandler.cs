using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerWeatherForecastGetHandler : GameRequestPacketHandler<C2SServerWeatherForecastGetReq, S2CServerWeatherForecastGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerWeatherForecastGetHandler));

        public ServerWeatherForecastGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerWeatherForecastGetRes Handle(GameClient client, C2SServerWeatherForecastGetReq request)
        {
            var res = new S2CServerWeatherForecastGetRes()
            {
                IntervalGameHour = WeatherManager.ForecastIntervalGameHour,
                GameDayToEarthMin = Server.GameSettings.GameServerSettings.GameClockTimescale,
                ForecastList = Server.WeatherManager.GetForecast()
            };

            return res;
        }
    }
}
