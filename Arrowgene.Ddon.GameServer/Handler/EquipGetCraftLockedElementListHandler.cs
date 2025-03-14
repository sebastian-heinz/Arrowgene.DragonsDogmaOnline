using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipGetCraftLockedElementListHandler : GameRequestPacketHandler<C2SEquipGetCraftLockedElementListReq, S2CEquipGetCraftLockedElementListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipGetCraftLockedElementListHandler));

        public EquipGetCraftLockedElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipGetCraftLockedElementListRes Handle(GameClient client, C2SEquipGetCraftLockedElementListReq request)
        {
            S2CEquipGetCraftLockedElementListRes res = new S2CEquipGetCraftLockedElementListRes();
            
            //TODO: Add list of items whose quality is locked, i.e. crests can not be destroyed and quality can not be upgraded

            return res;
        }
    }
}
