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
            var dungeonId = Server.DungeonManager.GetPartyContentId(client.Party);
            
            Server.DungeonManager.MarkReady(client.Party, client.Character, dungeonId);
            if (Server.DungeonManager.PartyIsReady(client.Party))
            {
                Server.DungeonManager.StartActivity(client.Party, DungeonManager.StartDungeon);
            }

            return new S2CStageUnisonAreaChangeReadyRes();
        }
    }
}
