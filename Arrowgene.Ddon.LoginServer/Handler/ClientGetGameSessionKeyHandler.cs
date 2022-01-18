using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientGetGameSessionKeyHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientGetGameSessionKeyHandler));


        public ClientGetGameSessionKeyHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_GAME_SESSION_KEY_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            // Request packet C2L_GET_GAME_SESSION_KEY_REQ has no data aside from header,
            // the rest is just padding/alignment to 16-byte boundary.


            int characterId = 0;

            GameToken token = GameToken.GenerateGameToken(client.Account.Id, characterId);
            if (!Database.SetToken(token))
            {
                Logger.Error(client, "Failed to store GameToken");
                // TODO err response
            }

            Logger.Info(client, $"Created GameToken:{token.Token}");

            // Write L2C_GET_GAME_SESSION_KEY_RES packet.
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteMtString(token.Token);
            buffer.WriteUInt16(0, Endianness.Big);
            client.Send(new Packet(PacketId.L2C_GET_GAME_SESSION_KEY_RES, buffer.GetAllBytes()));
        }
    }
}
