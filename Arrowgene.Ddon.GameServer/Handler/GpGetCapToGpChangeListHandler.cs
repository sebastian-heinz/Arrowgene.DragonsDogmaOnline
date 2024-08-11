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
            // TODO: implement S2C_GP_GET_CAP_TO_GP_CHANGE_LIST_RES
            S2CGpGetCapToGpChangeListRes res = new S2CGpGetCapToGpChangeListRes();

            res.List.Add(new CDataCAPtoGPChangeElement
            {
                ID = 1,
                CAP = 1,
                GP = 1,
                Comment = "Test",
                BackIconID = 0,
                FrameIconID = 0
            });
            
            return res;
        }
    }
}
