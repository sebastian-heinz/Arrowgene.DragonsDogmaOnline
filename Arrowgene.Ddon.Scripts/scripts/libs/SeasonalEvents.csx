/**
 * @brief Library file which has common functionality for Seasonal Events which can be loaded using "#load"
 */
public class SeasonalEvents
{
    public static bool CheckConfigSettings(string enableKey, string yearKey, uint year, string periodKey)
    {
        if (!LibDdon.GetSetting<bool>("SeasonalEventSettings", enableKey))
        {
            return false;
        }

        if (LibDdon.GetSetting<uint>("SeasonalEventSettings", yearKey) != year)
        {
            return false;
        }

        var timespan = LibDdon.GetSetting<(DateTime, DateTime)>("SeasonalEventSettings", periodKey);
        return LibUtils.WithinTimespan(timespan);
    }
}
