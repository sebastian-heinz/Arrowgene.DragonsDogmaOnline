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
                Recipients = { client }
            };
        }

        public static ChatResponse ServerMessage(GameClient client, string message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                Message = message,
                Type = LobbyChatMsgType.System,
                Recipients = { client }
            };
        }

        public static ChatResponse ServerChat(GameClient client, string message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                Message = message,
                Type = LobbyChatMsgType.ManagementGuideC,
                Recipients = { client }
            };
        }

        public static ChatResponse FromMessage(GameClient client, ChatMessage message)
        {
            return new ChatResponse()
            {
                HandleId = message.HandleId,
                Deliver = true,
                Message = message.Message,
                FirstName = client.Character.FirstName,
                LastName = client.Character.LastName,
                CharacterId = client.Character.CharacterId,
                Type = message.Type,
                MessageFlavor = message.MessageFlavor,
                PhrasesCategory = message.PhrasesCategory,
                PhrasesIndex = message.PhrasesIndex,
                Recipients = { client }
            };
        }

        public ChatResponse()
        {
            HandleId = 0;
            Recipients = new List<GameClient>();
            Deliver = true;
            Type = LobbyChatMsgType.Say;
            MessageFlavor = 0;
            PhrasesCategory = 0;
            PhrasesIndex = 0;
            CharacterId = 0;
            Message = "";
            FirstName = "";
            LastName = "";
            ClanName = "";
        }

        public uint HandleId { get; set; }
        public List<GameClient> Recipients { get; }
        public bool Deliver { get; set; }
        public LobbyChatMsgType Type { get; set; }
        public byte MessageFlavor { get; set; }
        public uint PhrasesCategory { get; set; }
        public uint PhrasesIndex { get; set; }
        public string Message { get; set; }
        public uint CharacterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClanName { get; set; }
    }
}
