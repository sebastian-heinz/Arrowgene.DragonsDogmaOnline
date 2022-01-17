using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientGetGameSessionKeyHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientGetGameSessionKeyHandler));


        public ClientGetGameSessionKeyHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_GAME_SESSION_KEY_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            // Request packet C2L_GET_GAME_SESSION_KEY_REQ has no data aside from header,
            // the rest is just padding/alignment to 16-byte boundary.

            // Write L2C_GET_GAME_SESSION_KEY_RES packet.
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteMtString("F108F0F7B3034D56BE72506AC12CE94");
            buffer.WriteUInt16(0, Endianness.Big);
            client.Send(new Packet(PacketId.L2C_GET_GAME_SESSION_KEY_RES, buffer.GetAllBytes()));
        }
    }
}
