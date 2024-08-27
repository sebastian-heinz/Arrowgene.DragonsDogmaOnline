using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyChatMsgHandler : GameRequestPacketHandler<C2SLobbyChatMsgReq, S2CLobbyChatMsgRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyChatMsgHandler));


        public LobbyLobbyChatMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CLobbyChatMsgRes Handle(GameClient client, C2SLobbyChatMsgReq request)
        {
            ChatMessage message = new ChatMessage
            {
                HandleId = 0,
                Type = request.Type,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex,
                Message = request.Message,
                Deliver = true
            };
            Server.ChatManager.Handle(client, message);
            return new S2CLobbyChatMsgRes();
        }
    }
}
