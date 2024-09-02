using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Chat.Command;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Chat.Log
{
    public class ChatLogHandler : IChatHandler
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatCommandHandler));

        private readonly RollingList<ChatMessageLogEntry> _chatMessageLog;
        
        public ChatLogHandler()
        {
            _chatMessageLog = new RollingList<ChatMessageLogEntry>(100); // TODO: Move to server config
        }
        
        public IEnumerable<ChatMessageLogEntry> ChatMessageLog
        {
            get => _chatMessageLog;
        }
        
        public void Handle(GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            Logger.Info(client, message.Message);
            
            ChatMessageLogEntry logEntry = new ChatMessageLogEntry(client.Character, message);
            _chatMessageLog.Add(logEntry);
        }

        public void AddEntry(uint characterId, string firstName, string lastName, ChatMessage message)
        {
            Logger.Info("Chat message: "+message.Message);
            
            ChatMessageLogEntry logEntry = new ChatMessageLogEntry(characterId, firstName, lastName, message);
            _chatMessageLog.Add(logEntry);
        }
    }
}
