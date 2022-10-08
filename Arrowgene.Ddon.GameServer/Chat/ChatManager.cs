using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatManager));

        private readonly List<IChatHandler> _handler;
        private readonly GameRouter _router;
        private readonly DdonGameServer _server;

        public ChatManager(DdonGameServer server, GameRouter router)
        {
            _server = server;
            _router = router;
            _handler = new List<IChatHandler>();
        }

        public void AddHandler(IChatHandler handler)
        {
            _handler.Add(handler);
        }

        public void SendMessage(string message, string firstName, string lastName, LobbyChatMsgType type,
            params uint[] characterIds)
        {
            ChatResponse response = new ChatResponse();
            response.Deliver = true;
            response.Message = message;
            response.FirstName = firstName;
            response.LastName = lastName;
            response.CharacterId = 0;
            response.Type = type;
            response.Unk2 = 0;
            response.Unk3 = 0;
            response.Unk4 = 0;
            foreach (uint characterId in characterIds)
            {
                GameClient client = _server.ClientLookup.GetClientByCharacterId(characterId);
                if (client == null)
                {
                    continue;
                }

                response.Recipients.Add(client);
            }

            _router.Send(response);
        }

        public void SendMessage(string message, string firstName, string lastName, LobbyChatMsgType type,
            List<GameClient> recipients)
        {
            ChatResponse response = new ChatResponse();
            response.Deliver = true;
            response.Message = message;
            response.FirstName = firstName;
            response.LastName = lastName;
            response.CharacterId = 0;
            response.Type = type;
            response.Unk2 = 0;
            response.Unk3 = 0;
            response.Unk4 = 0;
            response.Recipients.AddRange(recipients);
            _router.Send(response);
        }

        public void Handle(GameClient client, ChatMessage message)
        {
            if (client == null)
            {
                Logger.Debug("Client is Null");
                return;
            }

            if (message == null)
            {
                Logger.Debug(client, "Chat Message is Null");
                return;
            }

            List<ChatResponse> responses = new List<ChatResponse>();
            foreach (IChatHandler handler in _handler)
            {
                handler.Handle(client, message, responses);
            }

            if (message.Deliver)
            {
                // deliver original chat message
                ChatResponse response = ChatResponse.FromMessage(client, message);
                Deliver(client, response);
            }

            foreach (ChatResponse response in responses)
            {
                // deliver additional messages generated form handler
                if (!response.Deliver)
                {
                    continue;
                }

                Deliver(client, response);
            }
        }

        private void Deliver(GameClient client, ChatResponse response)
        {
            switch (response.Type)
            {
                case LobbyChatMsgType.Say:
                case LobbyChatMsgType.Shout:
                    response.Recipients.AddRange(_server.Clients);
                    break;
                case LobbyChatMsgType.Party:
                    PartyGroup party = client.Party;
                    if (party != null)
                    {
                        response.Recipients.AddRange(party.Clients);
                    }
                    break;
                default:
                    response.Recipients.Add(client);
                    break;
            }

            _router.Send(response);
        }
    }
}
