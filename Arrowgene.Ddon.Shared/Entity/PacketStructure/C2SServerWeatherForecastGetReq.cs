using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SServerWeatherForecastGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SERVER_WEATHER_FORECAST_GET_REQ;

        public C2SServerWeatherForecastGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SServerWeatherForecastGetReq>
        {
            public override void Write(IBuffer buffer, C2SServerWeatherForecastGetReq obj)
            {
            }

            public override C2SServerWeatherForecastGetReq Read(IBuffer buffer)
            {
                C2SServerWeatherForecastGetReq obj = new C2SServerWeatherForecastGetReq();
                return obj;
            }
        }
    }
}
