using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;
using System.Text;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLoginHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientLoginHandler));


        public ClientLoginHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_LOGIN_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer recv = packet.AsBuffer();
            ushort len = recv.ReadUInt16(Endianness.Big);
            string onetimeToken = recv.ReadString(len);

            /*
            PLATFORM_TYPE_NONE = 0x0,
            PLATFORM_TYPE_PC = 0x1,
            PLATFORM_TYPE_PS3 = 0x2,
            PLATFORM_TYPE_PS4 = 0x3,
            */
            byte inPlatform = recv.ReadByte();

            Logger.Debug(client, $"Received OnetimeToken: {onetimeToken}");

            client.OnetimeToken = onetimeToken;

            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteInt32(0); //n_result

            // Onetime Token (MtString)
            byte[] utf8 = Encoding.UTF8.GetBytes(client.OnetimeToken);
            buffer.WriteUInt16((ushort)client.OnetimeToken.Length, Endianness.Big);
            buffer.WriteBytes(utf8);

            client.Send(new Packet(PacketId.L2C_LOGIN_RES, buffer.GetAllBytes()));
        }
    }
}
