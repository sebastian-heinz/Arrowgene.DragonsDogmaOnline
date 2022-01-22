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
    public class LobbyLobbyChatMsgHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyChatMsgHandler));


        public LobbyLobbyChatMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_LOBBY_LOBBY_CHAT_MSG_REQ;

        public override void Handle(GameClient requestingClient, Packet request)
        {
            // Read request
            C2SLobbyChatMsgReq req = EntitySerializer.Get<C2SLobbyChatMsgReq>().Read(request.AsBuffer());
            Logger.Debug(requestingClient, $"{req.type}, {req.unk2}, {req.unk3}, {req.unk4}, {req.unk5}: {req.strMessage}"); // Log chat message

            // Write response
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            S2CLobbyChatMsgRes res = new S2CLobbyChatMsgRes();
            EntitySerializer.Get<S2CLobbyChatMsgRes>().Write(resBuffer, res);
            Packet response = new Packet(PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_RES, resBuffer.GetAllBytes());
            requestingClient.Send(response);

            // Notify all players
            foreach (GameClient client in Server.Clients)
            {
                IBuffer ntcBuffer = new StreamBuffer();
                S2CLobbyChatMsgNotice ntc = new S2CLobbyChatMsgNotice();
                ntc.unk0 = (byte) req.type;
                ntc.strMessage = req.strMessage;
                ntc.characterBaseInfo.strFirstName = "FirstName";
                ntc.characterBaseInfo.strLastName = "LastName";
                ntc.characterBaseInfo.strClanName = "ClanName";
                EntitySerializer.Get<S2CLobbyChatMsgNotice>().Write(ntcBuffer, ntc);
                Packet notice = new Packet(PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_NTC, ntcBuffer.GetAllBytes());
                client.Send(notice);
            }
        }
    }
}
