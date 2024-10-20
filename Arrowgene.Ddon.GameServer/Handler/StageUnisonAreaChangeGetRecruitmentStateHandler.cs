using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageUnisonAreaChangeGetRecruitmentStateHandler : GameRequestPacketHandler<C2SStageUnisonAreaChangeGetRecruitmentStateReq, S2CStageUnisonAreaChangeGetRecruitmentStateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageUnisonAreaChangeGetRecruitmentStateHandler));

        public StageUnisonAreaChangeGetRecruitmentStateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageUnisonAreaChangeGetRecruitmentStateRes Handle(GameClient client, C2SStageUnisonAreaChangeGetRecruitmentStateReq request)
        {
            var result = new S2CStageUnisonAreaChangeGetRecruitmentStateRes();

            uint dungeonId = Server.BonusDungeonManager.GetPartyDungeonId(client.Party);
            if (dungeonId != 0)
            {
                var dungeonInfo = Server.AssetRepository.BonusDungeonAsset.DungeonInfo[dungeonId];
                result.Unk0 = dungeonInfo.DungeonId;
                result.StageId = dungeonInfo.StageId;
                result.StartPos = dungeonInfo.StartingPos;
                result.Unk4 = true;
                result.EntryCostList = dungeonInfo.EntryCostList;
            }

            return result;
        }
    }
}
