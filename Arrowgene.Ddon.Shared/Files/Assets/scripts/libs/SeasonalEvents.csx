/**
 * @brief Library file which has common functionality for Seasonal Events which can be loaded using "#load"
 */
public class SeasonalEvents
{
    public static bool CheckConfigSettings(DdonGameServer server, string enableKey, string yearKey, uint year, string periodKey)
    {
        if (!server.GameLogicSettings.Get<bool>("SeasonalEvents", enableKey))
        {
            return false;
        }

        if (server.GameLogicSettings.Get<uint>("SeasonalEvents", yearKey) != year)
        {
            return false;
        }

        var timespan = server.GameLogicSettings.Get<(DateTime, DateTime)>("SeasonalEvents", periodKey);
        return LibUtils.WithinTimespan(timespan);
    }
}
