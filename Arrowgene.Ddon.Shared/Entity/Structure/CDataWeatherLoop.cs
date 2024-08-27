using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWeatherLoop
    {
        public CDataWeatherLoop(byte weatherId, uint timeSec)
        {
            WeatherId= (Weather)weatherId;
            TimeSec=timeSec;
        }

        public CDataWeatherLoop()
        {
            WeatherId=0;
            TimeSec=0;
        }

        public Weather WeatherId { get; set; }
        public uint TimeSec { get; set; }

        public class Serializer : EntitySerializer<CDataWeatherLoop>
        {
            public override void Write(IBuffer buffer, CDataWeatherLoop obj)
            {
                WriteByte(buffer, (byte)obj.WeatherId);
                WriteUInt32(buffer, obj.TimeSec);
            }

            public override CDataWeatherLoop Read(IBuffer buffer)
            {
                CDataWeatherLoop obj = new CDataWeatherLoop();
                obj.WeatherId = (Weather)ReadByte(buffer);
                obj.TimeSec = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
