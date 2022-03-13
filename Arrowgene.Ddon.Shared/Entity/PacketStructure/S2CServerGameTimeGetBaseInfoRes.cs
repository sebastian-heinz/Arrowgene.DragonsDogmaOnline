using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CServerGameTimeGetBaseInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SERVER_GAME_TIME_GET_BASEINFO_RES;

        public S2CServerGameTimeGetBaseInfoRes()
        {
            GameTimeBaseInfo = new CDataGameTimeBaseInfo();
            WeatherLoop = new List<CDataWeatherLoop>();
            WeatherSchedule = new List<CDataWeatherSchedule>();
            MoonAgeLoopSec = 0;
            MoonSchedule = new List<CDataMoonSchedule>();
        }

        public CDataGameTimeBaseInfo GameTimeBaseInfo { get; set; }
        public List<CDataWeatherLoop> WeatherLoop { get; set; }
        public List<CDataWeatherSchedule> WeatherSchedule { get; set; }
        public uint MoonAgeLoopSec { get; set; }
        public List<CDataMoonSchedule> MoonSchedule { get; set; }

        public class Serializer : PacketEntitySerializer<S2CServerGameTimeGetBaseInfoRes>
        {
            public override void Write(IBuffer buffer, S2CServerGameTimeGetBaseInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataGameTimeBaseInfo>(buffer, obj.GameTimeBaseInfo);
                WriteEntityList<CDataWeatherLoop>(buffer, obj.WeatherLoop);
                WriteEntityList<CDataWeatherSchedule>(buffer, obj.WeatherSchedule);
                WriteUInt32(buffer, obj.MoonAgeLoopSec);
                WriteEntityList<CDataMoonSchedule>(buffer, obj.MoonSchedule);
            }

            public override S2CServerGameTimeGetBaseInfoRes Read(IBuffer buffer)
            {
                S2CServerGameTimeGetBaseInfoRes obj = new S2CServerGameTimeGetBaseInfoRes();
                ReadServerResponse(buffer, obj);
                obj.GameTimeBaseInfo = ReadEntity<CDataGameTimeBaseInfo>(buffer);
                obj.WeatherLoop = ReadEntityList<CDataWeatherLoop>(buffer);
                obj.WeatherSchedule = ReadEntityList<CDataWeatherSchedule>(buffer);
                obj.MoonAgeLoopSec = ReadUInt32(buffer);
                obj.MoonSchedule = ReadEntityList<CDataMoonSchedule>(buffer);
                return obj;
            }
        }
    }
}