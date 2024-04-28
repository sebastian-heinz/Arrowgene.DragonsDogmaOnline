#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ShopBuyShopGoodsHandler : GameStructurePacketHandler<C2SShopBuyShopGoodsReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ShopBuyShopGoodsHandler));
        
        private readonly ItemManager _itemManager;

        public ShopBuyShopGoodsHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SShopBuyShopGoodsReq> packet)
        {
            S2CShopGetShopGoodsListRes shop = client.InstanceShopManager.GetAssets(client.Character.LastEnteredShopId);
            CDataGoodsParam good = shop.GoodsParamList.Where(good => good.Index == packet.Structure.GoodsIndex).Single();

            uint boughAmount = packet.Structure.Num;
            uint totalPrice = good.Price * boughAmount;
            if(packet.Structure.Price != totalPrice)
            {
                // Dude's hacking, boot them out
                client.Send(new S2CShopBuyShopGoodsRes()
                {
                    Error = 1
                });
                return;
            }

            bool sendToItemBag;
            switch(packet.Structure.Destination) {
                case 19:
                    // If packet.Structure.Destination is 19: Send to corresponding item bag
                    sendToItemBag = true;
                    break;
                case 20:
                    // If packet.Structure.Destination is 20: Send to storage 
                    sendToItemBag = false;
                    break;
                default:
                    throw new Exception("Unexpected destination when buying goods: "+packet.Structure.Destination);
            }

            // UPDATE INVENTORY
            CDataItemUpdateResult? itemUpdateResult = _itemManager.AddItem(Server, client.Character, sendToItemBag, good.ItemId, boughAmount);

            if(itemUpdateResult.UpdateItemNum == 0)
            {
                // Send empty response, no items bought
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0x10a
                });
            }
            else
            {
                // Substract stock, substract wallet points, and send response with the stock, wallet points and inventory
                boughAmount = (uint) itemUpdateResult.UpdateItemNum;
                totalPrice = good.Price * boughAmount;

                // UPDATE SHOP
                if(good.Stock != byte.MaxValue)
                {
                    // If stock isn't infinite (255), substract bought quantity from it
                    good.Stock = (byte) Math.Max(0, good.Stock - boughAmount); 
                }

                // UPDATE CHARACTER WALLET
                CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == shop.WalletType).Single();
                wallet.Value = (uint) Math.Max(0, (int)wallet.Value - (int)totalPrice);
                Database.UpdateWalletPoint(client.Character.CharacterId, wallet);

                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0x10a,
                    UpdateWalletList = new List<CDataUpdateWalletPoint>()
                    {
                        new CDataUpdateWalletPoint()
                        {
                            Type = shop.WalletType,
                            AddPoint = (int) -totalPrice,
                            Value = wallet.Value
                        }
                    },
                    UpdateItemList = new List<CDataItemUpdateResult>()
                    {
                        itemUpdateResult
                    }
                });
            }

            // Send packets
            client.Send(new S2CShopBuyShopGoodsRes()
            {
                PointType = shop.WalletType // ?
            });
        }
    }
}