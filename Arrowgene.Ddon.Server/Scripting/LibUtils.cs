using System;

namespace Arrowgene.Ddon.Server.Scripting
{
    public class LibUtils
    {
        public static DateTime EventTime(int month, int day)
        {
            if (month == 2 && day == 29)
            {
                if (!DateTime.IsLeapYear(DateTime.Today.Year))
                {
                    // Move the date back by 1 since this year is not a leap year
                    day = 28;
                }
            }

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
            return (now >= startDate && now <= endDate);
        }

        public static bool WithinTimespan((DateTime StartDate, DateTime EndDate) timespan)
        {
            return LibUtils.WithinTimespan(timespan.StartDate, timespan.EndDate);
        }
    }
}
