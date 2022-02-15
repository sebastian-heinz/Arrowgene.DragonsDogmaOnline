using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatMessage
    {
        public ChatMessage(LobbyChatMsgType messageType, string message)
        {
            Type = messageType;
            Message = message;
            Deliver = true;
        }

        public LobbyChatMsgType Type { get; }
        public string Message { get; }
        public bool Deliver { get; set; }
    }
}
