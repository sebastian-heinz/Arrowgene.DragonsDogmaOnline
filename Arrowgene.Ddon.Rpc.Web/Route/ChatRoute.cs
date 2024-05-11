using System;
using System.Threading.Tasks;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using System.Text.Json;
using Arrowgene.Ddon.GameServer.Chat.Log;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    public class ChatRoute : RpcWebRoute
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ChatRoute));

        public override string Route => "/rpc/chat";

        public ChatRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override async Task<WebResponse> Get(WebRequest request)
        {
            WebResponse response = new WebResponse();
            ChatCommand chat;
            if(request.QueryParameter.ContainsKey("since"))
            {
                string dateString = request.QueryParameter.Get("since");
                try {
                    chat = new ChatCommand(ParseSinceDate(dateString));
                }
                catch(FormatException e)
                {
                    Logger.Error($"Invalid date format: {dateString}");
                    response.StatusCode = 400;
                    await response.WriteAsync("Invalid date format. Dates can be in UTC in a format like this: 2022-09-19T22:04:04.9860144Z");
                    return response;
                }
            }
            else
            {
                chat = new ChatCommand();
            }
            
            RpcCommandResult result = Executer.Execute(chat);
            if (!result.Success)
            {
                response.StatusCode = 500;
                await response.WriteAsync("Error");
                return response;
            }
            response.StatusCode = 200;
            await response.WriteJsonAsync(chat.ChatMessageLog);
            return response;
        }

        public override async Task<WebResponse> Post(WebRequest request)
        {
            try
            {
                ChatMessageLogEntry entry = JsonSerializer.Deserialize<ChatMessageLogEntry>(request.Body);
                ChatPostCommand chat = new ChatPostCommand(entry);
                RpcCommandResult result = Executer.Execute(chat);
                if(!result.Success)
                {
                    WebResponse response = new WebResponse();
                    response.StatusCode = 500;
                    await response.WriteAsync("Error");
                    return response;
                }
                else
                {
                    WebResponse response = new WebResponse();
                    response.StatusCode = 201;
                    return response;
                }
            }
            catch(JsonException e)
            {
                WebResponse response = new WebResponse();
                Logger.Error($"Invalid request body");
                response.StatusCode = 400;
                await response.WriteAsync("Invalid request body");
                return response;
            }
        }

        private static long ParseSinceDate(string dateString)
        {
            if(DateTime.TryParse(dateString, out DateTime date))
            {
                return ((DateTimeOffset) DateTime.SpecifyKind(date, DateTimeKind.Utc)).ToUnixTimeMilliseconds();
            }

            if(long.TryParse(dateString, out long unixTimeMillis))
            {
                return unixTimeMillis;
            }

            throw new FormatException();
        }
    }
}
