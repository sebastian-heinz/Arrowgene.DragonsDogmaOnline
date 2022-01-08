using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientSessionKeyHandler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientSessionKeyHandler));


        public ClientSessionKeyHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_GAME_SESSION_KEY_REQ;

        public override void Handle(Client client, Packet packet)
        {
            IBuffer recv = packet.AsBuffer();
            ushort len = recv.ReadUInt16(Endianness.Big);
            string sessionKey = recv.ReadString(len);
            byte unknown = recv.ReadByte();

            Logger.Debug(client, $"Received SessionKey: {sessionKey}");

            client.State.SessionKey = sessionKey;

            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteInt32(0); //n_result
            buffer.WriteUInt16(0);
            buffer.WriteUInt16(0);

            //  buffer.WriteByte(0x1F); //str len ??
            //   buffer.WriteString("F3829860AD5A421A87BD34C289147CB36"); // but it is longer..?
            buffer.WriteByte((byte) client.State.SessionKey.Length);
            buffer.WriteString(client.State.SessionKey);

            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0x28);
            buffer.WriteByte(0xFE);
            buffer.WriteByte(0x7C);
            buffer.WriteInt32(0);
            client.Send(new Packet(PacketId.L2C_GET_GAME_SESSION_KEY_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
