#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopBuyShopGoodsHandler : GameRequestPacketHandler<C2SShopBuyShopGoodsReq, S2CShopBuyShopGoodsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopBuyShopGoodsHandler));
        
        public ShopBuyShopGoodsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CShopBuyShopGoodsRes Handle(GameClient client, C2SShopBuyShopGoodsReq packet)
        {
            S2CShopGetShopGoodsListRes shop = client.InstanceShopManager.GetAssets(client.Character.LastEnteredShopId);
            CDataGoodsParam good = shop.GoodsParamList.Where(good => good.Index == packet.GoodsIndex).Single();

            uint boughtAmount = packet.Num;
            uint totalPrice = good.Price * boughtAmount;
            if(packet.Price != totalPrice)
            {
                // Dude's hacking, boot them out
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SHOP_PRICE_NO_MATCH);
            }

            bool sendToItemBag;
            switch(packet.Destination) {
                case 19:
                    // If packet.Structure.Destination is 19: Send to corresponding item bag
                    sendToItemBag = true;
                    break;
                case 20:
                    // If packet.Structure.Destination is 20: Send to storage 
                    sendToItemBag = false;
                    break;
                default:
                    throw new Exception("Unexpected destination when buying goods: "+packet.Destination);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                // UPDATE INVENTORY
                List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(Server, client.Character, sendToItemBag, good.ItemId, boughtAmount, connectionIn: connection);

                boughtAmount = (uint)itemUpdateResults.Select(result => result.UpdateItemNum).Sum();
                if (boughtAmount == 0)
                {
                    // Send empty response, no items bought
                    client.Send(new S2CItemUpdateCharacterItemNtc()
                    {
                        UpdateType = ItemNoticeType.ShopGoods_buy
                    });
                }
                else
                {
                    // Substract stock, substract wallet points, and send response with the stock, wallet points and inventory
                    totalPrice = good.Price * boughtAmount;

                    // UPDATE SHOP
                    if (good.Stock != byte.MaxValue)
                    {
                        // If stock isn't infinite (255), substract bought quantity from it
                        good.Stock = (byte)Math.Max(0, good.Stock - boughtAmount);
                    }

                    // UPDATE CHARACTER WALLET
                    client.Send(new S2CItemUpdateCharacterItemNtc()
                    {
                        UpdateType = ItemNoticeType.ShopGoods_buy,
                        UpdateWalletList = new List<CDataUpdateWalletPoint>()
                    {
                        Server.WalletManager.RemoveFromWallet(client.Character, shop.WalletType, totalPrice, connection)
                    },
                        UpdateItemList = itemUpdateResults
                    });
                }
            });
            
            // Send packets
            return new S2CShopBuyShopGoodsRes()
            {
                PointType = shop.WalletType // ?
            };
        }
    }
}
