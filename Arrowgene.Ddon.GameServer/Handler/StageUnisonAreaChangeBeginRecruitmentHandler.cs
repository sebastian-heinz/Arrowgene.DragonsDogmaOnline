using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageUnisonAreaChangeBeginRecruitmentHandler : GameRequestPacketHandler<C2SStageUnisonAreaChangeBeginRecruitmentReq, S2CStageUnisonAreaChangeBeginRecruitmentRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageUnisonAreaChangeBeginRecruitmentHandler));

        public StageUnisonAreaChangeBeginRecruitmentHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageUnisonAreaChangeBeginRecruitmentRes Handle(GameClient client, C2SStageUnisonAreaChangeBeginRecruitmentReq request)
        {
            Server.BonusDungeonManager.MarkReady(client.Party, client.Character, request.Unk0);
            if (Server.BonusDungeonManager.PartyIsReady(client.Party))
            {
                Server.BonusDungeonManager.StartDungeon(client.Party);
            }

            return new S2CStageUnisonAreaChangeBeginRecruitmentRes();
        }
    }
}
