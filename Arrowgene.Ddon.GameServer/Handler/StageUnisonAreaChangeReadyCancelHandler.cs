using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageUnisonAreaChangeReadyCancelHandler : GameRequestPacketHandler<C2SStageUnisonAreaChangeReadyCancelReq, S2CStageUnisonAreaChangeReadyCancelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageUnisonAreaChangeReadyCancelHandler));

        public StageUnisonAreaChangeReadyCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageUnisonAreaChangeReadyCancelRes Handle(GameClient client, C2SStageUnisonAreaChangeReadyCancelReq request)
        {
            Server.DungeonManager.EndPartyReadyCheck(client.Party);

            client.Party.SendToAll(new S2CStageUnisonAreaReadyCancelNtc());
            return new S2CStageUnisonAreaChangeReadyCancelRes();
        }
    }
}
