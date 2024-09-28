/**
 * @brief Library file which has common functionality for Seasonal Events which can be loaded using "#load"
 */
public class SeasonalEvents
{
    public static bool CheckConfigSettings(DdonGameServer server, string enableKey, string yearKey, uint year, string periodKey)
    {
        if (!server.GameSettings.Get<bool>("SeasonalEventSettings", enableKey))
        {
            return false;
        }

        if (server.GameSettings.Get<uint>("SeasonalEventSettings", yearKey) != year)
        {
            return false;
        }

        var timespan = server.GameSettings.Get<(DateTime, DateTime)>("SeasonalEventSettings", periodKey);
        return LibUtils.WithinTimespan(timespan);
    }
}
