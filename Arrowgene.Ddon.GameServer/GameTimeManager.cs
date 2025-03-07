using System;

namespace Arrowgene.Ddon.GameServer
{
    public class GameTimeManager
    {
        public const long OriginalRealTimeSec = 0x55DDD470; // Taken from the pcaps. A few days before DDOn release
        public const long OriginalGameTimeSec = 0x22C2ED000; // Taken from the pcaps.
        public const uint GameTimeDayHour = 24;
        private readonly uint GameClockTimescale;

        public const long DayTimeStart = 21600000; // 06:00 in milliseconds
        public const long NightTimeStart = 64800000; // 18:00 in milliseconds

        public static readonly (long Start, long End) DayTime = (DayTimeStart, NightTimeStart);
        public static readonly (long Start, long End) NightTime = (NightTimeStart, DayTimeStart);

        private DdonGameServer Server { get; set; }
        public GameTimeManager(DdonGameServer server)
        {
            Server = server;
            GameClockTimescale = server.GameSettings.GameServerSettings.GameClockTimescale;
        }

        public long GameTime(DateTimeOffset realTime)
        {
            return 1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - OriginalRealTimeSec)) / GameClockTimescale % (3600000 * GameTimeDayHour);
        }

        public long GameTimeNow()
        {
            return GameTime(DateTimeOffset.UtcNow);
        }

        public bool IsDayTime()
        {
            var now = GameTimeNow();
            return (now >= DayTimeStart) && (now <= NightTimeStart);
        }

        public bool IsNightTime()
        {
            var now = GameTimeNow();
            // Need to account for time wrapping from 1800 around
            // at midnight and increasing again until the morning
            return (now >= NightTimeStart) || (now < DayTimeStart);
        }

        /// <summary>
        /// Comverts a time value of HH:MM into milliseconds.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long ConvertTimeToMilliseconds(string time)
        {
            // Split the start time at the colon to get hours and minutes
            string[] startTimeComponents = time.Split(':');
            int startHours = int.Parse(startTimeComponents[0]);
            int startMinutes = int.Parse(startTimeComponents[1]);

            // Convert hours and minutes into milliseconds
            return (startHours * 3600000) + (startMinutes * 60000);
        }
    }
}
