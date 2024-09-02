using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatMessage
    {
        public ChatMessage()
        {
        }

        public uint HandleId { get; set; }
        public LobbyChatMsgType Type { get; set; }
        public byte MessageFlavor { get; set; }
        public uint PhrasesCategory { get; set; }
        public uint PhrasesIndex { get; set; }
        public string Message { get; set; }
        public bool Deliver { get; set; }
    }
}
