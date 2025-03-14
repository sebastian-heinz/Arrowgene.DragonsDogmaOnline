using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanShopGetBuffItemListHandler : GameRequestPacketHandler<C2SClanClanShopGetBuffItemListReq, S2CClanClanShopGetBuffItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMemberListHandler));

        public ClanClanShopGetBuffItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanShopGetBuffItemListRes Handle(GameClient client, C2SClanClanShopGetBuffItemListReq request)
        {
            S2CClanClanShopGetBuffItemListRes res = new();
            var clan = Server.ClanManager.GetClan(client.Character.ClanId);
            var purchases = Server.Database.SelectClanShopPurchases(client.Character.ClanId);

            res.ClanPoint = clan.ClanServerParam.MoneyClanPoint;
            res.ShopInfo.ClanPoint = res.ClanPoint;
            res.ShopInfo.CurrentFunctionList = purchases.Select(x => Server.AssetRepository.ClanShopAsset[x])
                .Where(x => x.Type == ClanShopLineupType.BaseFunction)
                .Select(x => new CDataCommonU32(x.LineupId))
                .ToList();
            // TODO: Populate res.ShopInfo.CurrentBuffList from the database.

            res.BuffItemList = Server.AssetRepository.ClanShopAsset.Values
                .Where(x => x.Type == ClanShopLineupType.PawnExpeditionSupport)
                .Select(x => x.ToCDataClanShopBuffItem())
                .ToList();
            
            return res;
        }
    }
}
