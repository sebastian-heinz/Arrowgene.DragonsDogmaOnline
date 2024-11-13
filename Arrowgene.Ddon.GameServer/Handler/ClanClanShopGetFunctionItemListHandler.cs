using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class ClanClanShopGetFunctionItemListHandler : GameRequestPacketHandler<C2SClanClanShopGetFunctionItemListReq, S2CClanClanShopGetFunctionItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMemberListHandler));

        public ClanClanShopGetFunctionItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanShopGetFunctionItemListRes Handle(GameClient client, C2SClanClanShopGetFunctionItemListReq request)
        {
            S2CClanClanShopGetFunctionItemListRes res = new();
            var clan = Server.ClanManager.GetClan(client.Character.ClanId);

            res.ClanPoint = clan.ClanServerParam.MoneyClanPoint;
            res.ShopInfo.ClanPoint = res.ClanPoint;
            res.FunctionItemList = Server.AssetRepository.ClanShopAsset.Values
                .Where(x => x.Type == ClanShopLineupType.BaseFunction || x.Type == ClanShopLineupType.BaseFurniture)
                .Select(x => x.ToCDataClanShopFunctionItem())
                .ToList();

            return res;
        }
    }
}
