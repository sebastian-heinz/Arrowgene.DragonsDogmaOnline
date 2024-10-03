using Arrowgene.Ddon.Database.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class TimeCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "time";
        public override string HelpText => "usage: `/time` - Print details about the time/weather/moon";

        private readonly DdonGameServer _server;

        public TimeCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            StringBuilder sb = new StringBuilder();

            long OriginalRealTimeSec = WeatherManager.OriginalRealTimeSec;

            DateTimeOffset now = DateTimeOffset.UtcNow;
            ulong secondsElapsed = (ulong)(now.ToUnixTimeSeconds() - OriginalRealTimeSec);
            ulong remainingSeconds = secondsElapsed % _server.WeatherManager.WeatherLoopTotalLength;

            int weatherIndex = 0;
            foreach (var weatherLoop in _server.WeatherManager.WeatherLoopList)
            {
                if (remainingSeconds <= weatherLoop.TimeSec)
                {
                    break;
                }
                weatherIndex++;
                remainingSeconds -= weatherLoop.TimeSec;
            }

            sb.Append($"Weather:{_server.WeatherManager.GetWeather()} ({weatherIndex}) ");
            sb.Append($"{remainingSeconds}/{_server.WeatherManager.WeatherLoopList[weatherIndex].TimeSec} seconds");

            ChatResponse response = new ChatResponse();
            response.Message = sb.ToString();
            responses.Add(response);
        }
    }
}
