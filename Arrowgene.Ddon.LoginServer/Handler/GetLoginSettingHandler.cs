using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetLoginSettingHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetLoginSettingHandler));


        public GetLoginSettingHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_LOGIN_SETTING_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            client.Send(LoginDump.Dump_20);
        }
    }
}
