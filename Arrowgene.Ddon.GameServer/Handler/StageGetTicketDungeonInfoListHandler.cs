using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetTicketDungeonInfoListHandler : GameRequestPacketHandler<C2SStageGetTicketDungeonInfoListReq, S2CStageGetTicketDungeonInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageGetTicketDungeonInfoListHandler));

        public StageGetTicketDungeonInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageGetTicketDungeonInfoListRes Handle(GameClient client, C2SStageGetTicketDungeonInfoListReq request)
        {
            var result = new S2CStageGetTicketDungeonInfoListRes();

            foreach (var dungeon in Server.AssetRepository.BonusDungeonAsset.DungeonCategories[request.CategoryId].DungeonInformation.Values)
            {
                result.CategoryInfoList.Add(new CDataStageTicketDungeonCategoryInfo()
                {
                    DungeonId = dungeon.DungeonId,
                    DungeonName = dungeon.EventName,
                    EntryFeeList = dungeon.EntryCostList,
                    Unk0 = dungeon.DungeonId,
                });
            }

            return result;
        }
    }
}
