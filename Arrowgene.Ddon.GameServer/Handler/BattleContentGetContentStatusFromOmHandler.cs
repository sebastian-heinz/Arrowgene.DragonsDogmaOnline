using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using YamlDotNet.Core.Tokens;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentGetContentStatusFromOmHandler : GameRequestPacketHandler<C2SBattleContentGetContentStatusFromOmReq, S2CBattleContentGetContentStatusFromOmRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentGetContentStatusFromOmHandler));

        public BattleContentGetContentStatusFromOmHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentGetContentStatusFromOmRes Handle(GameClient client, C2SBattleContentGetContentStatusFromOmReq request)
        {
            var progress = client.Character.BbmProgress;
            var stageId = request.StageLayoutId.AsStageLayoutId();
            var mazeAssets = Server.AssetRepository.BitterblackMazeAsset;

            if (!StageManager.IsBitterBlackMazeStageId(stageId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_INVALID_STAGE_ID, $"Unexpected StageId for BBM {stageId}");
            }

            if (!mazeAssets.Stages.ContainsKey(stageId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_INVALID_STAGE_ID, $"Unexpected StageId for BBM {stageId}");
            }

            bool contentInProgress = progress.StartTime > 0;
            if (contentInProgress && stageId.Id == 602)
            {
                // Player has some progress and embarking from the cove
                BattleContentMode contentMode = (stageId.GroupId == 26) ? BattleContentMode.Rotunda : BattleContentMode.Abyss;
                if (contentMode != progress.ContentMode || progress.ContentId == 0)
                {
                    // User has progress in a different battle content mode or completed their run, so reject
                    return new S2CBattleContentGetContentStatusFromOmRes() { GameMode = GameMode.BitterblackMaze };
                }

                var match = mazeAssets.Stages.FirstOrDefault(x => (x.Value.ContentMode == contentMode) && (x.Value.Tier == progress.Tier));
                stageId = match.Key;
            }

            var destinations = mazeAssets.Stages[stageId].Destinations;
            client.Character.NextBBMStageId = destinations[Random.Shared.Next(destinations.Count)];
            progress.ContentId = mazeAssets.Stages[stageId].ContentId;
            progress.Tier = mazeAssets.Stages[stageId].Tier;
            progress.ContentMode = mazeAssets.Stages[stageId].ContentMode;

            return new S2CBattleContentGetContentStatusFromOmRes()
            {
                GameMode = GameMode.BitterblackMaze,
                NotifyJobLock = contentInProgress ? 1U : 0U,
                Unk2 = 2,
                Unk3 = 3,
                Unk4 = 4,
            };

            // 0, 0, 0, 0 says can't do content
            // 1, 1, 1, 1 bring up chaarcter slect board
            // 1, 1, 0, 0 introduces phase change req packet???
            // 1, 1, 1, 0
            // 0, 0, 1, 0 asks if player wants to procede
        }
    }
}
