using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GpCourseGetInfoHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpCourseGetInfoHandler));


        public GpCourseGetInfoHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GP_COURSE_GET_INFO_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            client.Send(LoginDump.Dump_22);
        }
    }
}
