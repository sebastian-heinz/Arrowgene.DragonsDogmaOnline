using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Chat.Command;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Chat.Log
{
    public class ChatLogHandler : IChatHandler
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatCommandHandler));

        public void Handle(GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            Logger.Info(client, message.Message);
        }
    }
}
