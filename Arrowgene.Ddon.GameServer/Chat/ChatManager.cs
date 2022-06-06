using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
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

            client.Send(new S2CLobbyChatMsgRes());
        }

        private void Deliver(GameClient client, ChatResponse response)
        {
            switch (response.Type)
            {
                case LobbyChatMsgType.Say:
                    response.Recipients.AddRange(_server.Clients);
                    break;
                default:
                    response.Recipients.Add(client);
                    break;
            }

            _router.Send(response);
        }
    }
}
