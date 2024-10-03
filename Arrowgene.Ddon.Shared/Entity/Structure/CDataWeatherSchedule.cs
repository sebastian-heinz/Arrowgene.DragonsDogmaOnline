using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWeatherSchedule
    {

        public CDataWeatherSchedule()
        {
            WeatherId=0;
            BeginTimeSec=0;
            EndTimeSec=0;
        }

        public Weather WeatherId { get; set; }
        public long BeginTimeSec { get; set; }
        public long EndTimeSec { get; set; }

        public class Serializer : EntitySerializer<CDataWeatherSchedule>
        {
            public override void Write(IBuffer buffer, CDataWeatherSchedule obj)
            {
                WriteByte(buffer, (byte)obj.WeatherId);
                WriteInt64(buffer, obj.BeginTimeSec);
                WriteInt64(buffer, obj.EndTimeSec);
            }

            public override CDataWeatherSchedule Read(IBuffer buffer)
            {
                CDataWeatherSchedule obj = new CDataWeatherSchedule();
                obj.WeatherId = (Weather)ReadByte(buffer);
                obj.BeginTimeSec = ReadInt64(buffer);
                obj.EndTimeSec = ReadInt64(buffer);
                return obj;
            }
        }
    }
}
