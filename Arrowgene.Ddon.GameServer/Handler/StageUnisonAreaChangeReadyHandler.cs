using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageUnisonAreaChangeReadyHandler : GameRequestPacketHandler<C2SStageUnisonAreaChangeReadyReq, S2CStageUnisonAreaChangeReadyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageUnisonAreaChangeReadyHandler));

        public StageUnisonAreaChangeReadyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageUnisonAreaChangeReadyRes Handle(GameClient client, C2SStageUnisonAreaChangeReadyReq request)
        {
            var dungeonId = Server.BonusDungeonManager.GetPartyDungeonId(client.Party);
            
            Server.BonusDungeonManager.MarkReady(client.Party, client.Character, dungeonId);
            if (Server.BonusDungeonManager.PartyIsReady(client.Party))
            {
                Server.BonusDungeonManager.StartDungeon(client.Party);
            }

            return new S2CStageUnisonAreaChangeReadyRes();
        }
    }
}
