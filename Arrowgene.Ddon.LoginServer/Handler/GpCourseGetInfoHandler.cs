using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GpCourseGetInfoHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GpCourseGetInfoHandler));


        public GpCourseGetInfoHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GP_COURSE_GET_INFO_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            client.Send(LoginDump.Dump_22);
        }
    }
}
