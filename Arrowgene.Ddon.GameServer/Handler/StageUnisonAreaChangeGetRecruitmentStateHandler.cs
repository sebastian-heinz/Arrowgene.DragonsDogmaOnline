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

            uint contentId = Server.DungeonManager.GetPartyContentId(client.Party);
            if (Server.EpitaphRoadManager.IsEpitaphId(contentId))
            {
                if (Server.EpitaphRoadManager.IsSectionUnlocked(client.Character, contentId))
                {
                    var sectionInfo = Server.EpitaphRoadManager.GetSectionById(contentId);
                    result.ContentId = contentId;
                    result.StageId = sectionInfo.StageId;
                    result.StartPos = sectionInfo.StartingPos;
                    result.Unk4 = true;
                }
            }
            else if (contentId != 0)
            {
                var dungeonInfo = Server.AssetRepository.BonusDungeonAsset.DungeonInfo[contentId];
                result.ContentId = dungeonInfo.DungeonId;
                result.StageId = dungeonInfo.StageId;
                result.StartPos = dungeonInfo.StartingPos;
                result.Unk4 = true;
                result.EntryCostList = dungeonInfo.EntryCostList;
            }

            return result;
        }
    }
}
