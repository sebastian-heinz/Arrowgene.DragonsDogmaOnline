using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardPartyRecruitCategoryListHandler : GameRequestPacketHandler<C2SEntryBoardPartyRecruitCategoryListReq, S2CEntryBoardPartyRecruitCategoryListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardPartyRecruitCategoryListHandler));

        public EntryBoardPartyRecruitCategoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardPartyRecruitCategoryListRes Handle(GameClient client, C2SEntryBoardPartyRecruitCategoryListReq request)
        {
            var res = new S2CEntryBoardPartyRecruitCategoryListRes();
            foreach (var (categoryId, data) in Server.AssetRepository.RecruitmentBoardCategoryAsset.RecruitmentBoardCategories)
            {
                res.Unk1List.Add(new CDataEntryRecruitCategoryData()
                {
                    CategoryId = data.CategoryId,
                    CategoryName = data.CategoryName,
                    NumGroups = (uint)Server.BoardManager.GetGroupsForBoardId(BoardManager.BoardIdFromRecruitmentCategory(data.CategoryId)).Where(x => x.ContentStatus == ContentStatus.Recruiting).ToList().Count
                });
            }
            return res;
        }
    }
}
