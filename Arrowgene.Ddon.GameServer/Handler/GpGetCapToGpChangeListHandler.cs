#nullable enable
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetCapToGpChangeListHandler : GameRequestPacketHandler<C2SGpGetCapToGpChangeListReq, S2CGpGetCapToGpChangeListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetCapToGpChangeListHandler));

        public GpGetCapToGpChangeListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGetCapToGpChangeListRes Handle(GameClient client, C2SGpGetCapToGpChangeListReq request)
        {
            S2CGpGetCapToGpChangeListRes res = new S2CGpGetCapToGpChangeListRes();

            // TODO: introduce some form of asset to track all potential variants of GG
            res.List.AddRange(new List<CDataCAPtoGPChangeElement>
            {
                new()
                {
                    ID = 1,
                    CAP = 0,
                    GP = 1,
                    Comment = "ID1 0CAP for 1GG",
                    BackIconID = 0,
                    FrameIconID = 0
                },
                new()
                {
                    ID = 2,
                    CAP = 0,
                    GP = 10,
                    Comment = "ID2 0CAP for 10GG",
                    BackIconID = 0,
                    FrameIconID = 1
                }
            });

            return res;
        }
    }
}
