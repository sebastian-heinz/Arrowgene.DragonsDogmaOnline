using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatMessage
    {
        public ChatMessage()
        {
        }

        public ChatMessage(LobbyChatMsgType messageType, byte unk2, uint unk3, uint unk4, string message)
        {
            Type = messageType;
            Unk2 = unk2;
            Unk3 = unk3;
            Unk4 = unk4;
            Message = message;
            Deliver = true;
        }

        public LobbyChatMsgType Type { get; }
        public byte Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public string Message { get; set; }
        public bool Deliver { get; set; }
    }
}
