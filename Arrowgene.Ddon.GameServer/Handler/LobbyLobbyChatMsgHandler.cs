using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyChatMsgHandler : StructurePacketHandler<GameClient, C2SLobbyChatMsgReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyChatMsgHandler));

        public LobbyLobbyChatMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient requestingClient, StructurePacket<C2SLobbyChatMsgReq> request)
        {
            Logger.Debug(requestingClient, $"{request.Structure.Type}, {request.Structure.Unk2}, {request.Structure.Unk3}, {request.Structure.Unk4}, {request.Structure.Unk5}: {request.Structure.StrMessage}"); // Log chat message

            S2CLobbyChatMsgRes response = new S2CLobbyChatMsgRes();
            requestingClient.Send(response);

            // Notify all players
            foreach (GameClient client in Server.Clients)
            {
                IBuffer ntcBuffer = new StreamBuffer();
                S2CLobbyChatMsgNotice notice = new S2CLobbyChatMsgNotice();
                notice.Unk0 = (byte) request.Structure.Type;
                notice.StrMessage = request.Structure.StrMessage;
                notice.CharacterBaseInfo.strFirstName = "FirstName";
                notice.CharacterBaseInfo.strLastName = "LastName";
                notice.CharacterBaseInfo.strClanName = "ClanName";
                client.Send(notice);
            }
        }
    }
}
