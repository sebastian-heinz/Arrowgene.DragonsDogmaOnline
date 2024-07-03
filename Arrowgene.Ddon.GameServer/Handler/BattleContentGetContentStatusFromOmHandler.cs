using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
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
            Random random = new Random();

            var mazeAssets = Server.AssetRepository.BitterblackMazeAsset;

            var stageId = request.StageLayoutId.AsStageId();
            if (!mazeAssets.Stages.ContainsKey(stageId))
            {
                return new S2CBattleContentGetContentStatusFromOmRes()
                {
                    Error = 1
                };
            }

            var mazeConfig = mazeAssets.Stages[stageId];
            var stageIndex = random.Next(mazeConfig.Destinations.Count);
            // TODO: Save in database?

            return new S2CBattleContentGetContentStatusFromOmRes()
            {
                Unk0 = 2, // Should this be a key id (?) mazeConfig.Destinations[stageIndex]
                Unk1 = (uint)(stageId.Id == 602 ? 0 : 1), // Connected to existing progress or not
                Unk2 = 1,
                Unk3 = 1,
                Unk4 = 1,
            };

            // 0, 0, 0, 0 says can't do content
            // 1, 1, 1, 1 bring up chaarcter slect board
            // 1, 1, 0, 0 introduces phase change req packet???
            // 1, 1, 1, 0
            // 0, 0, 1, 0 asks if player wants to procede
        }
    }
}
