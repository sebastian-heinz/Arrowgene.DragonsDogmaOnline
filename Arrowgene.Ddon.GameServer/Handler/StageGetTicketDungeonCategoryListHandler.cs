using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetTicketDungeonCategoryListHandler : GameRequestPacketHandler<C2SStageGetTicketDungeonCategoryListReq, S2CStageGetTicketDungeonCategoryListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageGetTicketDungeonCategoryListHandler));

        public StageGetTicketDungeonCategoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageGetTicketDungeonCategoryListRes Handle(GameClient client, C2SStageGetTicketDungeonCategoryListReq request)
        {
            var result = new S2CStageGetTicketDungeonCategoryListRes();

            foreach (var category in Server.AssetRepository.BonusDungeonAsset.DungeonCategories.Values)
            {
                result.CategoryList.Add(new CDataStageTicketDungeonCategory()
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                });
            }

            return result;
        }
    }
}
