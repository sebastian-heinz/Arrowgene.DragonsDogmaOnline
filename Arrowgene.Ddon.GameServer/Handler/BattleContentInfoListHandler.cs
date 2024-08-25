using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentInfoListHandler : GameRequestPacketHandler<C2SBattleContentInfoListReq, S2CBattleContentInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentInfoListHandler));

        public BattleContentInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentInfoListRes Handle(GameClient client, C2SBattleContentInfoListReq request)
        {
            // client.Send(InGameDump.Dump_93);
            client.Character.BbmProgress = Server.Database.SelectBBMProgress(client.Character.CharacterId);

            S2CBattleContentInfoListRes result = new S2CBattleContentInfoListRes();

            var contentStatus = BitterblackMazeManager.GetUpdatedContentStatus(Server, client.Character);
            var contentInfo = new CDataBattleContentInfo()
            {
                GameMode = GameMode.BitterblackMaze,
                ContentName = "Bitterblack Maze",
                ItemTakeawayList = Server.AssetRepository.BitterblackMazeAsset.ItemTakeawayList,
                RareItemAppraisalList = Server.AssetRepository.BitterblackMazeAsset.RareItemAppraisalList,
                BattleContentStageList = Server.AssetRepository.BitterblackMazeAsset.Stages.Select(x => new CDataBattleContentStage()
                {
                    Id = x.Value.ContentId,
                    Mode = x.Value.ContentMode,
                    StageName = x.Value.ContentName
                }).ToList(),
                BattleContentStageProgressionList = Server.AssetRepository.BitterblackMazeAsset.StageProgressionList
            };

            result.BattleContentInfoList.Add(contentInfo);
            result.BattleContentStatusList.Add(contentStatus);

            return result;
        }
    }
}
