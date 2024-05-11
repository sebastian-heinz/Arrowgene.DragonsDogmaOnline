using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.Chat.Log;

namespace Arrowgene.Ddon.Rpc.Command
{
    public class ChatPostCommand : IRpcCommand
    {
        private static readonly string DEFAULT_FIRST_NAME = "DDOn";
        private static readonly string DEFAULT_LAST_NAME = "Tools";

        public string Name => "ChatPostCommand";

        public ChatPostCommand(ChatMessageLogEntry entry)
        {
            _entry = entry;
        }

        private readonly ChatMessageLogEntry _entry;

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            gameServer.ChatManager.SendMessage(
                _entry.ChatMessage.Message, 
                _entry.FirstName, 
                _entry.LastName, 
                _entry.ChatMessage.Type, 
                gameServer.ClientLookup.GetAll()
            );
            gameServer.ChatLogHandler.AddEntry(0, _entry.FirstName, _entry.LastName, _entry.ChatMessage);
            return new RpcCommandResult(this, true);
        }
        
    }
}
