using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanShopBuyFunctionItemHandler : GameRequestPacketHandler<C2SClanClanShopBuyFunctionItemReq, S2CClanClanShopBuyFunctionItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMemberListHandler));

        public ClanClanShopBuyFunctionItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanShopBuyFunctionItemRes Handle(GameClient client, C2SClanClanShopBuyFunctionItemReq request)
        {
            // TODO: Buying furniture seemingly doesn't have a string associated with it, so it displays an empty text box???
            // TODO: Clan History

            S2CClanClanShopBuyFunctionItemRes res = new();

            if (!Server.AssetRepository.ClanShopAsset.TryGetValue(request.LineupId, out var asset))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CLAN_SHOP_INVALID_LINEUP_ID);
            }

            var clan = Server.ClanManager.GetClan(client.Character.ClanId);
            lock(clan)
            {
                if (clan.ClanServerParam.MoneyClanPoint < asset.RequireClanPoint)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_CLAN_SHOP_COST_LACK);
                }
                clan.ClanServerParam.MoneyClanPoint -= asset.RequireClanPoint;
            }
            res.ClanPoint = clan.ClanServerParam.MoneyClanPoint;

            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.InsertClanShopPurchase(client.Character.ClanId, request.LineupId, connection);
                res.LineupFunctionList = Server.Database.SelectClanShopPurchases(client.Character.ClanId, connection)
                    .Select(x => new CDataCommonU32(x))
                    .ToList();
                Server.Database.UpdateClan(clan, connection);
            });

            var ntc = new S2CClanClanShopBuyItemNtc()
            {
                FunctionList = new()
                {
                    asset.ToCDataClanShopFunctionItem()
                },
                ClanPoint = clan.ClanServerParam.MoneyClanPoint
            };
            GameStructure.CDataCharacterName(ntc.BuyerName, client.Character);
            Server.ClanManager.SendToClan(client.Character.ClanId, ntc);
            Server.RpcManager.AnnounceClanPacket(client.Character.ClanId, ntc);

            return res;
        }
    }
}
