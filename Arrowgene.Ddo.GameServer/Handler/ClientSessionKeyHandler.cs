using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientSessionKeyHandler : PacketHandler
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(ClientSessionKeyHandler));


        public ClientSessionKeyHandler(DdoGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_SESSION_KEY_REQ;

        public override void Handle(Client client, Packet packet)
        {
            IBuffer recv = packet.AsBuffer();
            recv.ReadByte();
            string sessionKey = recv.ReadCString();
            
            Logger.Debug(client, $"Received SessionKey: {sessionKey}");

            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteInt32(0); //n_result
            buffer.WriteUInt16(0);
            buffer.WriteUInt16(0);
            buffer.WriteCString("F3829860AD5A421A87BD34C289147CB36");
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0x28);
            buffer.WriteByte(0xFE);
            buffer.WriteByte(0x7C);
            buffer.WriteInt32(0);
            client.Send(new Packet(PacketId.L2C_SESSION_KEY_RES, buffer.GetAllBytes()));
        }
    }
}
