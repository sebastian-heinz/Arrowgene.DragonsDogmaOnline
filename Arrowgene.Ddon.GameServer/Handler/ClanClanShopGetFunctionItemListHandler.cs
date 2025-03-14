using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanShopGetFunctionItemListHandler : GameRequestPacketHandler<C2SClanClanShopGetFunctionItemListReq, S2CClanClanShopGetFunctionItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMemberListHandler));

        public ClanClanShopGetFunctionItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanShopGetFunctionItemListRes Handle(GameClient client, C2SClanClanShopGetFunctionItemListReq request)
        {
            // TODO: Figure out what the contents of CDataClanShopFunctionInfo actually do.

            S2CClanClanShopGetFunctionItemListRes res = new();
            var clan = Server.ClanManager.GetClan(client.Character.ClanId);

            var purchases = Server.Database.SelectClanShopPurchases(client.Character.ClanId);

            res.ClanPoint = clan.ClanServerParam.MoneyClanPoint;
            res.ShopInfo.ClanPoint = res.ClanPoint;
            res.ShopInfo.CurrentFunctionList = purchases.Select(x => Server.AssetRepository.ClanShopAsset[x])
                .Where(x => x.Type == ClanShopLineupType.BaseFunction || x.Type == ClanShopLineupType.BaseFurniture)
                .Select(x => new CDataCommonU32(x.LineupId))
                .ToList();

            res.FunctionItemList = Server.AssetRepository.ClanShopAsset.Values
                .Where(x => x.Type == ClanShopLineupType.BaseFunction || x.Type == ClanShopLineupType.BaseFurniture)
                .Select(x => x.ToCDataClanShopFunctionItem())
                .ToList();

            return res;
        }
    }
}
