using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWeatherForecast
    {
        public CDataWeatherForecast()
        {
        }

        //This data structure expects it as a uint, but others seemingly expect a byte?
        public Weather Weather { get; set; } 

        public class Serializer : EntitySerializer<CDataWeatherForecast>
        {
            public override void Write(IBuffer buffer, CDataWeatherForecast obj)
            {
                WriteUInt32(buffer, (uint)obj.Weather);
            }

            public override CDataWeatherForecast Read(IBuffer buffer)
            {
                CDataWeatherForecast obj = new CDataWeatherForecast();
                obj.Weather = (Weather)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
