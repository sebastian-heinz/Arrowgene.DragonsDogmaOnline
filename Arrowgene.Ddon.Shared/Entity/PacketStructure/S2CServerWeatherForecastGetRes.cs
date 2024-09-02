using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CServerWeatherForecastGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SERVER_WEATHER_FORECAST_GET_RES;

        public S2CServerWeatherForecastGetRes()
        {
            ForecastList = new List<CDataWeatherForecast> {};
        }

        public uint IntervalGameHour { get; set; }
        public uint GameDayToEarthMin { get; set; }
        public List<CDataWeatherForecast> ForecastList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CServerWeatherForecastGetRes>
        {
            public override void Write(IBuffer buffer, S2CServerWeatherForecastGetRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.IntervalGameHour);
                WriteUInt32(buffer, obj.GameDayToEarthMin);
                WriteEntityList<CDataWeatherForecast>(buffer, obj.ForecastList);
            }

            public override S2CServerWeatherForecastGetRes Read(IBuffer buffer)
            {
                S2CServerWeatherForecastGetRes obj = new S2CServerWeatherForecastGetRes();
                ReadServerResponse(buffer, obj);
                obj.IntervalGameHour = ReadUInt32(buffer);
                obj.GameDayToEarthMin = ReadUInt32(buffer);
                obj.ForecastList = ReadEntityList<CDataWeatherForecast>(buffer);

                return obj;
            }
        }
    }
}
