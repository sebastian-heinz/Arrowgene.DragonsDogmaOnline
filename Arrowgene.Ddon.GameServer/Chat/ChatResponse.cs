using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatResponse
    {
        public static ChatResponse CommandError(GameClient client, string message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                Message = message,
                Type = LobbyChatMsgType.ManagementAlertC,
                Recipients = {client}
            };
        }

        public static ChatResponse ServerMessage(GameClient client, string message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                Message = message,
                Type = LobbyChatMsgType.System,
                Recipients = {client}
            };
        }

        public static ChatResponse FromMessage(GameClient client, ChatMessage message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                Message = message.Message,
                FirstName = client.Character.FirstName,
                LastName = client.Character.LastName,
                CharacterId = client.Character.Id,
                Type = message.Type,
                Unk3 = message.Unk3,
                Unk4 = message.Unk4,
                Unk5 = message.Unk5,
                Recipients = {client}
            };
        }

        public ChatResponse()
        {
            Recipients = new List<GameClient>();
            Deliver = true;
            Type = LobbyChatMsgType.Say;
            Unk3 = 0;
            Unk4 = 0;
            Unk5 = 0;
            CharacterId = 0;
            Message = "";
            FirstName = "";
            LastName = "";
            ClanName = "";
        }

        public List<GameClient> Recipients { get; }
        public bool Deliver { get; set; }
        public LobbyChatMsgType Type { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public string Message { get; set; }
        public uint CharacterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClanName { get; set; }
    }
}
