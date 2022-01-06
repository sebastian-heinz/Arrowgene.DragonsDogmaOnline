using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientX1Handler : PacketHandler
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(ClientX1Handler));


        public ClientX1Handler(DdoGameServer server) : base(server)
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
