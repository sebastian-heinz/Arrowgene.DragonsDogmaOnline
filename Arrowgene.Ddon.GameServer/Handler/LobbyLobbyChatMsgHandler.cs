using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyChatMsgHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(LobbyLobbyChatMsgHandler));


        public LobbyLobbyChatMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_LOBBY_LOBBY_CHAT_MSG_REQ;

        public override void Handle(GameClient requestingClient, Packet request)
        {
            // Read request
            CDataLobbyChatMsgReq req = EntitySerializer.Get<CDataLobbyChatMsgReq>().Read(request.AsBuffer());
            Logger.Debug(requestingClient, $"{req.type}, {req.unk2}, {req.unk3}, {req.unk4}, {req.unk5}: {req.strMessage}"); // Log chat message

            // Write response
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            CDataLobbyChatMsgRes res = new CDataLobbyChatMsgRes();
            EntitySerializer.Get<CDataLobbyChatMsgRes>().Write(resBuffer, res);
            Packet response = new Packet(PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_RES, resBuffer.GetAllBytes());
            requestingClient.Send(response);

            // Notify all players
            foreach(GameClient client in Server.Clients)
            {
                IBuffer ntcBuffer = new StreamBuffer();
                CDataLobbyChatMsgNotice ntc = new CDataLobbyChatMsgNotice();
                ntc.unk0 = (byte) req.type;
                ntc.strMessage = req.strMessage;
                ntc.characterBaseInfo.strFirstName = "FirstName";
                ntc.characterBaseInfo.strLastName = "LastName";
                ntc.characterBaseInfo.strClanName = "ClanName";
                EntitySerializer.Get<CDataLobbyChatMsgNotice>().Write(ntcBuffer, ntc);
                Packet notice = new Packet(PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_NTC, ntcBuffer.GetAllBytes());
                client.Send(notice);
            }
        }
    }

}
