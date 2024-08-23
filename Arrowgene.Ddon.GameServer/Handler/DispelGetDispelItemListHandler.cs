using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DispelGetDispelItemListHandler : GameRequestPacketHandler<C2SDispelGetDispelItemListReq, S2CDispelGetDispelItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelGetDispelItemListHandler));

        public DispelGetDispelItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CDispelGetDispelItemListRes Handle(GameClient client, C2SDispelGetDispelItemListReq request)
        {
            S2CDispelGetDispelItemListRes res = new S2CDispelGetDispelItemListRes();

            var shopCategories = Server.AssetRepository.SpecialShopAsset.ShopCategories;
            if (shopCategories.ContainsKey(request.Category))
            {
                var shopCategory = shopCategories[request.Category];
                uint sortId = 0;
                foreach (var item in shopCategory.Items)
                {
                    var itemResult = item.AsCDataDispelBaseItem(client.Character.Job);
                    itemResult.SortId = sortId++;
                    res.DispelBaseItemList.Add(itemResult);
                }
            }

            return res;
        }
    }
}

