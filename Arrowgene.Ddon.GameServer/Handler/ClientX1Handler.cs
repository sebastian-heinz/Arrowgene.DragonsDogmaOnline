using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientX1Handler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX1Handler));


        public ClientX1Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X1_REQ;

        public override void Handle(Client client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);

            // buffer.WriteByte(0x1F); // str len ??
            //  buffer.WriteString("F3829860AD5A4");  // but it is shorter?
            buffer.WriteByte((byte) client.State.SessionKey.Length);
            buffer.WriteString(client.State.SessionKey);
            
            client.Send(new Packet(PacketId.X1_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
