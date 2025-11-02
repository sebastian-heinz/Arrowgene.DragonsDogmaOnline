using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGpCourseGetAvailableListHandler : GameRequestPacketHandler<C2SGpGpCourseGetAvailableListReq, S2CGpGpCourseGetAvailableListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGpCourseGetAvailableListHandler));

        public GpGpCourseGetAvailableListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGpCourseGetAvailableListRes Handle(GameClient client, C2SGpGpCourseGetAvailableListReq request)
        {
            S2CGpGpCourseGetAvailableListRes res = new S2CGpGpCourseGetAvailableListRes();

            // foreach (var course in server.AssetRepository.GPCourseInfoAsset.ValidCourses)
            // {
            //
            // }

            // TODO: Send back real data based on JSON contents?
            // TODO: PCAP doesn't have sample packet contents to see what is in it.
            return res;
        }
    }
}
