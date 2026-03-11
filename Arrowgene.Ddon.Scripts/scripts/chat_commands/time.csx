using System.Text;
using static Arrowgene.Ddon.GameServer.WeatherManager;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName            => "time";
    public override string HelpText               => "/time` - Print details about the time/weather/moon";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        StringBuilder sb = new StringBuilder();

        long OriginalRealTimeSec = WeatherManager.OriginalRealTimeSec;

        DateTimeOffset now = DateTimeOffset.UtcNow;
        ulong secondsElapsed = (ulong)(now.ToUnixTimeSeconds() - OriginalRealTimeSec);
        ulong remainingSeconds = secondsElapsed % server.WeatherManager.WeatherLoopTotalLength;

        int weatherIndex = 0;
        foreach (var weatherLoop in server.WeatherManager.WeatherLoopList)
        {
            if (remainingSeconds <= weatherLoop.TimeSec)
            {
                break;
            }
            weatherIndex++;
            remainingSeconds -= weatherLoop.TimeSec;
        }

        uint currentMoon = server.WeatherManager.GetMoonPhase();

        sb.Append($"Weather:{server.WeatherManager.GetWeather()} ({weatherIndex}) ");
        sb.Append($"{remainingSeconds}/{server.WeatherManager.WeatherLoopList[weatherIndex].TimeSec} seconds; ");
        sb.Append($"Moon: {MoonPhaseName(currentMoon)} ({currentMoon}/{WeatherManager.GameTimeMoonAges})");

        responses.Add(ChatResponse.ServerChat(client, sb.ToString()));
    }

    private static string MoonPhaseName(uint age)
    {
        if (age == 0)
        {
            return "New Moon";
        }
        else if (age >= 1 && age <= 6)
        {
            return "Waxing Crescent";
        }
        else if (age == 7)
        {
            return "First Quarter";
        }
        else if (age >= 8 && age <= 14)
        {
            return "Waxing Gibbous";
        }
        else if (age == 15)
        {
            return "Full Moon";
        }
        else if (age >= 16 && age <= 21)
        {
            return "Waning Gibbous";
        }
        else if (age == 22)
        {
            return "Last Quarter";
        }
        else if (age >= 23 && age <= 29)
        {
            return "Waning Crescent";
        }
        else
        {
            return "Unknown Phase";
        }
    }

}

return new ChatCommand();
