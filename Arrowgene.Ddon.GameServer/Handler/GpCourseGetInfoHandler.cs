using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpCourseGetInfoHandler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GpCourseGetInfoHandler));


        public GpCourseGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GP_COURSE_GET_INFO_REQ;

        public override void Handle(Client client, Packet packet)
        {
            client.Send(LoginDump.Dump_22);
        }
    }
}
