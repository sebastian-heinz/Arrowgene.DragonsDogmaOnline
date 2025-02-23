/**
 * @brief Library file which has common functionality for Seasonal Events which can be loaded using "#load"
 */
public class SeasonalEvents
{
    public static bool CheckConfigSettings(string enableKey, string yearKey, uint year, string periodKey)
    {
        if (!LibDdon.GetSetting<bool>("SeasonalEvents", enableKey))
        {
            return false;
        }

        if (LibDdon.GetSetting<uint>("SeasonalEvents", yearKey) != year)
        {
            return false;
        }

        var timespan = LibDdon.GetSetting<(DateTime, DateTime)>("SeasonalEvents", periodKey);
        return LibUtils.WithinTimespan(timespan);
    }
}
