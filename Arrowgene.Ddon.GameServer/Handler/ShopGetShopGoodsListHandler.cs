using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopGetShopGoodsListHandler : GameStructurePacketHandler<C2SShopGetShopGoodsListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopGetShopGoodsListHandler));
        
        public ShopGetShopGoodsListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SShopGetShopGoodsListReq> packet)
        {
            S2CShopGetShopGoodsListRes res = new S2CShopGetShopGoodsListRes();
            res.Unk0 = packet.Structure.ShopId;
            res.WalletType = WalletType.Gold;
            res.GoodsParamList = new List<CDataGoodsParam>()
            {
                new CDataGoodsParam()
                {
                    Index = 0,
                    ItemId = 68,
                    Stock = byte.MaxValue,
                    Price = 1000
                },
                new CDataGoodsParam()
                {
                    Index = 1,
                    ItemId = 74,
                    Stock = byte.MaxValue,
                    Price = 1000
                },
            };
            client.Send(res);
        }
    }
}