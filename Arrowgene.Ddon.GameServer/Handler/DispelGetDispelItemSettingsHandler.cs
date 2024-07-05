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
    public class DispelGetDispelItemSettingsHandler : GameRequestPacketHandler<C2SDispelGetDispelItemSettingsReq, S2CDispelGetDispelItemSettingsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelGetDispelItemSettingsHandler));

        // AKA "Rare Item Apprasial"
        public DispelGetDispelItemSettingsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CDispelGetDispelItemSettingsRes Handle(GameClient client, C2SDispelGetDispelItemSettingsReq request)
        {
            var res = new S2CDispelGetDispelItemSettingsRes();

            var specialShops = Server.AssetRepository.SpecialShopAsset.SpecialShops;
            if (specialShops.ContainsKey(request.ShopType))
            {
                foreach (var category in specialShops[request.ShopType])
                {
                    res.CategoryList.Add(new CDataDispelItemCategoryInfo()
                    {
                        Category = category.Id,
                        CategoryName = category.Label
                    });
                }
            }

            return res;
        }
    }
}

