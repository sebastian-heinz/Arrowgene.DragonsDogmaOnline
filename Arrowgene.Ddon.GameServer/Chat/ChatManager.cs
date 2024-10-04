using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            response.MessageFlavor = 0;
            response.PhrasesCategory = 0;
            response.PhrasesIndex = 0;
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
            ChatResponse response = new ChatResponse
            {
                HandleId = 0,
                Deliver = true,
                Message = message,
                FirstName = firstName,
                LastName = lastName,
                CharacterId = 0,
                Type = type,
                MessageFlavor = 0,
                PhrasesCategory = 0,
                PhrasesIndex = 0
            };
            response.Recipients.AddRange(recipients);
            _router.Send(response);
        }
        
        // TODO: add support for sending tell messages across worlds - requires some form of access to available worlds and their associated clients
        public void SendTellMessage(uint handleId, CDataCommunityCharacterBaseInfo senderCharacterInfo, CDataCommunityCharacterBaseInfo receiverCharacterInfo, C2SChatSendTellMsgReq request, GameClient sender, GameClient receiver)
        {
            ChatResponse senderChatResponse = GetTellChatResponse(handleId, receiverCharacterInfo, request);
            senderChatResponse.Recipients.Add(sender);
            ChatResponse receiverChatResponse = GetTellChatResponse(handleId, senderCharacterInfo, request);
            receiverChatResponse.Recipients.Add(receiver);

            _router.Send(senderChatResponse);
            _router.Send(receiverChatResponse);
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
                    response.Recipients.AddRange(_server.ClientLookup.GetAll());
                    break;
                case LobbyChatMsgType.Party:
                    PartyGroup party = client.Party;
                    if (party != null)
                    {
                        response.Recipients.AddRange(party.Clients);
                    }
                    break;
                case LobbyChatMsgType.Clan:
                    if (client.Character.ClanId == 0)
                    {
                        response.Recipients.Add(client);
                        break;
                    }

                    response.Recipients.AddRange(_server.ClientLookup.GetAll().Where(
                        x => x.Character != null 
                        && client.Character != null
                        && x.Character.ClanId == client.Character.ClanId)
                    );
                    break;
                default:
                    response.Recipients.Add(client);
                    break;
            }

            _router.Send(response);
        }
        
        public static S2CLobbyChatMsgNotice GetTellMsgNtc(uint handleId, CDataCommunityCharacterBaseInfo characterInfo, C2SChatSendTellMsgReq request)
        {
            return new S2CLobbyChatMsgNotice
            {
                Type = LobbyChatMsgType.Tell,
                HandleId = handleId,
                CharacterBaseInfo = characterInfo,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex,
                Message = request.Message
            };
        }
        
        public static ChatResponse GetTellChatResponse(uint handleId, CDataCommunityCharacterBaseInfo characterInfo, C2SChatSendTellMsgReq request)
        {
            return new ChatResponse
            {
                HandleId = handleId,
                Deliver = false,
                FirstName = characterInfo.CharacterName.FirstName,
                LastName = characterInfo.CharacterName.LastName,
                ClanName = characterInfo.ClanName,
                CharacterId = characterInfo.CharacterId,
                Type = LobbyChatMsgType.Tell,
                Message = request.Message,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex
            };
        }
    }
}
