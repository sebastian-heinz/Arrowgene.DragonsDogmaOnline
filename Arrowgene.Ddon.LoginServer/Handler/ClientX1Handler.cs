using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientX1Handler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX1Handler));


        public ClientX1Handler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X1_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteBytes(new byte[15]);
            client.Send(new Packet(PacketId.X1_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
