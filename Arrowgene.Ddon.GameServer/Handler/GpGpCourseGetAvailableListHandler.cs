using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGpCourseGetAvailableListHandler : GameRequestPacketHandler<C2SGpCourseGetAvailableListReq, S2CGpGpCourseGetAvailableListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGpCourseGetAvailableListHandler));

        private AssetRepository _AssetRepo;

        public GpGpCourseGetAvailableListHandler(DdonGameServer server) : base(server)
        {
            _AssetRepo = server.AssetRepository;
        }

        public override S2CGpGpCourseGetAvailableListRes Handle(GameClient client, C2SGpCourseGetAvailableListReq request)
        {
            S2CGpGpCourseGetAvailableListRes res = new S2CGpGpCourseGetAvailableListRes();

            string urlBase = Server.GameSettings.GameServerSettings.UrlDomain;
            DateTimeOffset offset = DateTimeOffset.UtcNow;
            res.Items = new List<CDataGPCourseAvailable>
            {
                new CDataGPCourseAvailable
                {
                    ID = 1,
                    Name = "Adventure Passport (available)",
                    UseLimitTime = offset.AddMonths(12),
                    CourseID = 1,
                    LineupID = 1,
                    ImageAddr = $"{urlBase}/shop/img/payment/icon_course1.png",
                }
            };

            return res;
        }
    }
}
