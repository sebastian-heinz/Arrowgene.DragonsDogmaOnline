using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWeatherSchedule
    {

        public CDataWeatherSchedule(byte weatherId, long beginTimeSec, long endTimeSec)
        {
            WeatherId=weatherId;
            BeginTimeSec=beginTimeSec;
            EndTimeSec=endTimeSec;
        }

        public CDataWeatherSchedule()
        {
            WeatherId=0;
            BeginTimeSec=0;
            EndTimeSec=0;
        }

        public byte WeatherId { get; set; }
        public long BeginTimeSec { get; set; }
        public long EndTimeSec { get; set; }

        public class Serializer : EntitySerializer<CDataWeatherSchedule>
        {
            public override void Write(IBuffer buffer, CDataWeatherSchedule obj)
            {
                WriteByte(buffer, obj.WeatherId);
                WriteInt64(buffer, obj.BeginTimeSec);
                WriteInt64(buffer, obj.EndTimeSec);
            }

            public override CDataWeatherSchedule Read(IBuffer buffer)
            {
                CDataWeatherSchedule obj = new CDataWeatherSchedule();
                obj.WeatherId = ReadByte(buffer);
                obj.BeginTimeSec = ReadInt64(buffer);
                obj.EndTimeSec = ReadInt64(buffer);
                return obj;
            }
        }
    }
}