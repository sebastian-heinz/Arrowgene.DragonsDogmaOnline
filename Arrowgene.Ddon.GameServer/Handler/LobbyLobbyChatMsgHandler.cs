using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

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
                request.Structure.StrMessage
            );
            _chatManager.Handle(client, message);
            
            
         //   Logger.Debug(requestingClient, $"{request.Structure.Type}, {request.Structure.Unk2}, {request.Structure.Unk3}, {request.Structure.Unk4}, {request.Structure.Unk5}: {request.Structure.StrMessage}"); // Log chat message

         //  S2CLobbyChatMsgRes response = new S2CLobbyChatMsgRes();
         //  requestingClient.Send(response);

         //  // Notify all players
         //  S2CLobbyChatMsgNotice notice = new S2CLobbyChatMsgNotice();
         //  notice.Unk0 = (byte) request.Structure.Type;
         //  notice.StrMessage = request.Structure.StrMessage;
         //  notice.CharacterBaseInfo.strFirstName = requestingClient.Character.FirstName;
         //  notice.CharacterBaseInfo.strLastName = requestingClient.Character.LastName;
         //  notice.CharacterBaseInfo.strClanName = "ClanName";
         //  foreach (GameClient client in Server.Clients)
         //  {
         //      client.Send(notice);
         //  }
        }
    }
}
