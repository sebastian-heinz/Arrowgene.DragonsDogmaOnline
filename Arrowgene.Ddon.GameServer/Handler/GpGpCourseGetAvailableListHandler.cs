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
    public class GpGpCourseGetAvailableListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGpCourseGetAvailableListHandler));

        private AssetRepository _AssetRepo;

        public GpGpCourseGetAvailableListHandler(DdonGameServer server) : base(server)
        {
            _AssetRepo = server.AssetRepository;
        }

        public override PacketId Id => PacketId.C2S_GP_GP_COURSE_GET_AVAILABLE_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CGpGpCourseGetAvailableListRes Response = new S2CGpGpCourseGetAvailableListRes();

            // foreach (var Course in _AssetRepo.GPCourseInfoAsset.ValidCourses)
            // {
            //
            // }

            // TODO: Send back real data based on JSON contents?
            // TODO: PCAP doesn't have sample packet contents to see what is in it.
            client.Send(Response);
        }
    }
}
