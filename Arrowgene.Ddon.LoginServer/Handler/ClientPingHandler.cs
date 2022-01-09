using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientPingHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientPingHandler));


        public ClientPingHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_PING_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);

            // Alignment to the next 16-byte boundary.
            // 9 byte header + 4 byte + 4 byte = 17 bytes of real data,
            // append 15 bytes of junk data to get 32 bytes total.
            //
            // TODO: Do this automatically on all packets in the transport layer,
            // this probably shouldn't be done manually in the handlers.
            buffer.WriteBytes(new byte[15]); 

            client.Send(new Packet(PacketId.L2C_PING_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
