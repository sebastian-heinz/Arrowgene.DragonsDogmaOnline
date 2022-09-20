using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using static Arrowgene.Ddon.Shared.Util;
using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatManager));

        public IEnumerable<ChatMessageLogEntry> ChatMessageLog { get => _chatMessageLog; }

        private readonly List<IChatHandler> _handler;
        private readonly GameRouter _router;
        private readonly DdonGameServer _server;
        private readonly RollingList<ChatMessageLogEntry> _chatMessageLog;

        public ChatManager(DdonGameServer server, GameRouter router)
        {
            _server = server;
            _router = router;
            _handler = new List<IChatHandler>();
            _chatMessageLog = new RollingList<ChatMessageLogEntry>(100); // TODO: Move to server config
        }

        public void AddHandler(IChatHandler handler)
        {
            _handler.Add(handler);
        }

        public void Handle(IPartyMember messageSender, ChatMessage message)
        {
            if (messageSender == null)
            {
                Logger.Debug("Client is Null");
                return;
            }

            if (message == null)
            {
                if(messageSender is GameClient)
                {
                    Logger.Debug(messageSender as GameClient, "Chat Message is Null");
                }
                else
                {
                    Logger.Debug("Chat Message is Null");
                }
                return;
            }

            ChatMessageLogEntry logEntry = new ChatMessageLogEntry(messageSender.Character, message);
            _chatMessageLog.Add(logEntry);

            List<ChatResponse> responses = new List<ChatResponse>();
            foreach (IChatHandler handler in _handler)
            {
                handler.Handle(messageSender, message, responses);
            }

            if (message.Deliver)
            {
                // deliver original chat message
                ChatResponse response = ChatResponse.FromMessage(messageSender, message);
                Deliver(messageSender, response);
            }

            foreach (ChatResponse response in responses)
            {
                // deliver additional messages generated form handler
                if (!response.Deliver)
                {
                    continue;
                }

                Deliver(messageSender, response);
            }
        }

        private void Deliver(IPartyMember messageSender, ChatResponse response)
        {
            switch (response.Type)
            {
                case LobbyChatMsgType.Say:
                case LobbyChatMsgType.Shout:
                    response.Recipients.AddRange(_server.Clients);
                    break;
                case LobbyChatMsgType.Party:
                    if(messageSender.Party != null)
                        response.Recipients.AddRange(messageSender.Party.Members);
                    break;
                default:
                    response.Recipients.Add(messageSender);
                    break;
            }

            _router.Send(response);
        }

        
        public class ChatMessageLogEntry
        {
            // For the JSON deserializer
            public ChatMessageLogEntry()
            {
            }

            public ChatMessageLogEntry(Character character, ChatMessage chatMessage)
            {
                DateTime = DateTime.Now;
                FirstName = character.FirstName;
                LastName = character.LastName;
                CharacterId = character.Id;
                ChatMessage = chatMessage;
            }

            public DateTime DateTime { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public uint CharacterId { get; set; }            
            public ChatMessage ChatMessage { get; set; }
        }
    }
}
