using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyChatMsgHandler : StructurePacketHandler<GameClient, C2SLobbyChatMsgReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyChatMsgHandler));

        private ChatManager _chatManager;

        public LobbyLobbyChatMsgHandler(DdonGameServer server) : base(server)
        {
            _chatManager = server.ChatManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyChatMsgReq> request)
        {
            ChatMessage message = new ChatMessage(
                request.Structure.Type,
                // Unk1?
                request.Structure.MessageFlavor,
                request.Structure.PhrasesCategory,
                request.Structure.PhrasesIndex,
                request.Structure.Message
            );
            _chatManager.Handle(client, message);
            client.Send(new S2CLobbyChatMsgRes());
        }
    }
}
