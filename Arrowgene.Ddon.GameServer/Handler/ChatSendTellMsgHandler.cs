using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ChatSendTellMsgHandler : GameRequestPacketHandler<C2SChatSendTellMsgReq, S2CChatSendTellMsgRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatSendTellMsgHandler));

        public ChatSendTellMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CChatSendTellMsgRes Handle(GameClient senderClient, C2SChatSendTellMsgReq request)
        {
            GameClient receiverClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterInfo.CharacterId);
            if (receiverClient == null)
            {
                Server.ChatManager.SendTellMessageForeign(senderClient, request);
            }
            else
            {
                Server.ChatManager.SendTellMessage(senderClient, receiverClient, request);
            }

            return new S2CChatSendTellMsgRes
            {
                CharacterBaseInfo = request.CharacterInfo
            };
        }
    }
}
