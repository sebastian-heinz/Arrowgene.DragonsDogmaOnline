using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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
                IntervalGameHour = 6, // From pcap data
                GameDayToEarthMin = 90 // From pcap data
            };

            res.ForecastList.Add(new CDataWeatherForecast() { Weather = Weather.Fine });
            res.ForecastList.Add(new CDataWeatherForecast() { Weather = Weather.Cloudy });
            res.ForecastList.Add(new CDataWeatherForecast() { Weather = Weather.Rainy });
            res.ForecastList.Add(new CDataWeatherForecast() { Weather = Weather.Fine });

            return res;
        }
    }
}
