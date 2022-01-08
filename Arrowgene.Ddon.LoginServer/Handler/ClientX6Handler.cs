using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientX6Handler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX6Handler));


        public ClientX6Handler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X60;

        public override void Handle(LoginClient client, Packet packet)
        {
            client.Send(LoginDump.Dump_28);
            client.Send(LoginDump.Dump_31);

            string ip = "127.0.0.1";
            IBuffer buffer = new StreamBuffer(LoginDump.data_Dump_32);
            buffer.SetPositionStart();
            buffer.Position = 48;
            buffer.WriteUInt16((ushort) ip.Length, Endianness.Big);
            buffer.WriteString(ip);
            buffer.WriteUInt16(52000, Endianness.Big);
            buffer.WriteBytes(new byte[] {0x0, 0x1, 0x46, 0xE9, 0x1D});
            byte[] data = buffer.GetBytes(0, buffer.Position);
            client.Send(new Packet(PacketId.X8, data, PacketSource.Server));
        }
    }
}
