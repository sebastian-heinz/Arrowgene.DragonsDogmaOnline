using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Arrowgene.Ddon.Bot;
using Arrowgene.Ddon.WebServer;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class BotCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(BotCommand));

        public string Key => "bot";

        public string Description => $"Usage: `bot [login|act]{Environment.NewLine}" +
                                     $"Web Auth: 'bot login ip:port --web-url \"ip:port/api/account\" --web-user \"username\" --web-pass \"password\"{Environment.NewLine}`'" +
                                     $"Token Auth: 'bot login ip:port --token \"abcdefgh\"'{Environment.NewLine}" +
                                     $"Bot Action: 'bot act x{Environment.NewLine}" +
                                     $"Bot Action: 'bot dc";

        private DdonBot _bot;


        public CommandResultType Run(CommandParameter parameter)
        {
            if (parameter.Arguments.Count < 1)
            {
                Logger.Error("to few arguments.");
                return CommandResultType.Exit;
            }

            string op = parameter.Arguments[0];
            if (op == "dc")
            {
                DisconnectBot();
                return CommandResultType.Completed;
            }

            if (op == "act")
            {
                return BotAction(parameter);
            }

            if (op == "login")
            {
                return BotLogin(parameter);
            }

            Logger.Error("invalid operation");
            return CommandResultType.Exit;
        }

        private CommandResultType BotLogin(CommandParameter parameter)
        {
            if (parameter.Arguments.Count < 2)
            {
                Logger.Error("BotLogin - to few arguments.");
                return CommandResultType.Exit;
            }

            string loginEndpointStr = parameter.Arguments[1];
            if (!IPEndPoint.TryParse(loginEndpointStr, out IPEndPoint loginEndpoint))
            {
                Logger.Error("invalid ip:port");
                return CommandResultType.Exit;
            }

            if (parameter.SwitchMap.TryGetValue("--web-url", out string webUrl))
            {
                if (!parameter.SwitchMap.TryGetValue("--web-user", out string webUser))
                {
                    Logger.Error("missing `--web-user`");
                    return CommandResultType.Exit;
                }

                if (!parameter.SwitchMap.TryGetValue("--web-pass", out string webPass))
                {
                    Logger.Error("missing `--web-pass`");
                    return CommandResultType.Exit;
                }

                return WebAuth(loginEndpoint, webUrl, webUser, webPass);
            }

            if (parameter.SwitchMap.TryGetValue("--token", out string token))
            {
                return TokenAuth(loginEndpoint, token);
            }

            Logger.Error("specify either `--web-url` or `--token` authentication");
            return CommandResultType.Exit;
        }

        private CommandResultType WebAuth(IPEndPoint loginEndpoint, string webUrl, string username, string password)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Arrowgene.Ddon.Bot");

            AccountRoute.AccountRequest accReq = new AccountRoute.AccountRequest();
            accReq.Action = "login";
            accReq.Account = username;
            accReq.Password = password;

            string accReqJson = JsonSerializer.Serialize(accReq);
            HttpRequestMessage webReq = new HttpRequestMessage(HttpMethod.Post, webUrl)
            {
                Content = new StringContent(accReqJson, Encoding.UTF8, "application/json"),
            };
            HttpResponseMessage webRsp = client.Send(webReq);
            if (!webRsp.IsSuccessStatusCode)
            {
                Logger.Error($"Web Auth failed: {webRsp.StatusCode}");
                return CommandResultType.Exit;
            }

            string accRspJson = webRsp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            AccountRoute.AccountResponse accRsp = JsonSerializer.Deserialize<AccountRoute.AccountResponse>(accRspJson);

            if (!string.IsNullOrEmpty(accRsp.Error))
            {
                Logger.Error($"Web Auth failed: {accRsp.Error}");
                return CommandResultType.Exit;
            }

            return TokenAuth(loginEndpoint, accRsp.Token);
        }

        private CommandResultType TokenAuth(IPEndPoint loginEndpoint, string token)
        {
            if (_bot == null)
            {
                _bot = new DdonBot();
            }
            _bot.Connect(loginEndpoint);
            _bot.Execute($"login|{token}");
            return CommandResultType.Continue;
        }

        private CommandResultType BotAction(CommandParameter parameter)
        {
            if (parameter.Arguments.Count < 2)
            {
                Logger.Error("BotAction - to few arguments.");
                return CommandResultType.Exit;
            }

            string action = parameter.Arguments[1];
            _bot.Execute(action);
            return CommandResultType.Continue;
        }

        private void DisconnectBot()
        {
            if (_bot != null)
            {
                _bot.Disconnect();
            }
        }

        public void Shutdown()
        {
            DisconnectBot();
        }
    }
}
