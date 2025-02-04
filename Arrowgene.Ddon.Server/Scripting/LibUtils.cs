using System;

namespace Arrowgene.Ddon.Server.Scripting
{
    public class LibUtils
    {
        public static DateTime EventTime(int month, int day)
        {
            return new DateTime(DateTime.Now.Year, month, day);
        }

        public static DateTime EventTime(string date)
        {
            var parts = date.Split("/");
            return LibUtils.EventTime(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public static (DateTime StartDate, DateTime EndDate) EventTimespan(string start, string end)
        {
            return (LibUtils.EventTime(start), LibUtils.EventTime(end));
        }

        public static bool WithinTimespan(DateTime startDate, DateTime endDate)
        {
            var now = DateTime.Now;
            return (now.Month >= startDate.Month && now.Month <= endDate.Month) &&
                   (now.Day >= startDate.Day && now.Day <= endDate.Day);
        }

        public static bool WithinTimespan((DateTime StartDate, DateTime EndDate) timespan)
        {
            return LibUtils.WithinTimespan(timespan.StartDate, timespan.EndDate);
        }
    }
}
