using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatMessage
    {
        public ChatMessage(LobbyChatMsgType messageType, uint unk3, uint unk4, uint unk5, string message)
        {
            Type = messageType;
            Unk3 = unk3;
            Unk4 = unk4;
            Unk5 = unk5;
            Message = message;
            Deliver = true;
        }

        public LobbyChatMsgType Type { get; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public string Message { get; }
        public bool Deliver { get; set; }
    }
}
