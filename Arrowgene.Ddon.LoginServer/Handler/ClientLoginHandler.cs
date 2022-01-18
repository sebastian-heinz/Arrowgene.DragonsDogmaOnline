using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Text;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLoginHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientLoginHandler));


        public ClientLoginHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_LOGIN_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            // Read C2L_LOGIN_REQ packet;
            IBuffer recv = packet.AsBuffer();
            string onetimeToken = recv.ReadMtString();
            if (!recv.ReadEnumByte(out PlatformType platformType))
            {
                platformType = PlatformType.None;
                Logger.Error(client, "Failed to read PlatformType");
            }

            Logger.Debug(client, $"Received OnetimeToken: {onetimeToken} for platform: {platformType}");

            client.OnetimeToken = onetimeToken;
            
            
           
            

            // Write L2C_LOGIN_RES packet.
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteInt32(0); //n_result
            buffer.WriteMtString(client.OnetimeToken);
            client.Send(new Packet(PacketId.L2C_LOGIN_RES, buffer.GetAllBytes()));
        }
    }
}
