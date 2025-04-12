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
        sb.Append($"Moon: {currentMoon}/{WeatherManager.GameTimeMoonAges}");

        responses.Add(ChatResponse.ServerChat(client, sb.ToString()));
    }
}

return new ChatCommand();
