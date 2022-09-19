using System;
using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using static Arrowgene.Ddon.Shared.Util;

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

        public event EventHandler<ChatMessageLogEntry> ChatMessageEvent;

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

            ChatMessageLogEntry logEntry = new ChatMessageLogEntry(client.Character, message);
            _chatMessageLog.Add(logEntry);

            // Event will be null if there are no subscribers
            EventHandler<ChatMessageLogEntry> raiseEvent = ChatMessageEvent;
            if (raiseEvent != null)
                raiseEvent(this, logEntry);

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

            client.Send(new S2CLobbyChatMsgRes());
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
                    response.Recipients.AddRange(client.Party.Members
                        .Where(x => x is GameClient)
                        .Select(x => x as GameClient));
                    break;
                default:
                    response.Recipients.Add(client);
                    break;
            }

            _router.Send(response);
        }

        
        public class ChatMessageLogEntry : EventArgs
        {
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
