using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGameTimeGetBaseinfoHandler : StructurePacketHandler<GameClient, C2SServerGameTimeGetBaseInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGameTimeGetBaseinfoHandler));

        private static readonly long ORIGINAL_REAL_TIME_SEC = 0x55DDD470; // Taken from the pcaps. A few days before DDOn release. Wednesday, 26 August 2015 15:00:00

        public ServerGameTimeGetBaseinfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SServerGameTimeGetBaseInfoReq> packet)
        {
            // Uncommenting the commented lines makes the game crash for some reason
            S2CServerGameTimeGetBaseInfoRes response = EntitySerializer.Get<S2CServerGameTimeGetBaseInfoRes>().Read(InGameDump.data_Dump_29);
            //response.GameTimeBaseInfo.GameTimeOneDayMin=0;
            response.GameTimeBaseInfo.GameTimeYearMonth = 0;
            response.GameTimeBaseInfo.GameTimeMonthDay = 0;
            //response.GameTimeBaseInfo.GameTimeDayHour=0;
            response.GameTimeBaseInfo.GameTimeWeekDay = 0;
            response.GameTimeBaseInfo.GameTimeMoonAge = 0;
            response.GameTimeBaseInfo.OriginalRealTimeSec = 0;
            response.GameTimeBaseInfo.OriginalGameTimeSec = this.calcGameTimeMSec(response.GameTimeBaseInfo.GameTimeOneDayMin, response.GameTimeBaseInfo.GameTimeDayHour) / 1000;
            response.GameTimeBaseInfo.OriginalWeek = 0;
            response.GameTimeBaseInfo.OriginalMoonAge = 0;
            response.WeatherLoop = new List<CDataWeatherLoop>();
            response.WeatherSchedule = new List<CDataWeatherSchedule>();
            response.MoonAgeLoopSec = 0;
            response.MoonSchedule = new List<CDataMoonSchedule>();
            client.Send(response);
        }

        // Adapted from the client's code
        private long calcGameTimeMSec(uint gameTimeOneDayMin, uint gameTimeDayHour)
        {
            return (1440 * (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1000 * (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ORIGINAL_REAL_TIME_SEC)) / gameTimeOneDayMin)
                % (3600000 * gameTimeDayHour);
        }
    }
}
