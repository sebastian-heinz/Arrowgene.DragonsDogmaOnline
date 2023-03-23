using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
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
        private readonly ShopManager _shopManager;

        public ShopBuyShopGoodsHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
            _shopManager = server.ShopManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SShopBuyShopGoodsReq> packet)
        {
            S2CShopGetShopGoodsListRes shop = _shopManager.GetAssets(client.Character.LastEnteredShopId);
            CDataGoodsParam good = shop.GoodsParamList.Where(good => good.Index == packet.Structure.GoodsIndex).Single();

            // TODO: Send to according item bag (consumable/material/equipment...), 
            // figuring it out by its item ID, if packet.Structure.Destination is 19
            // or send to storage if Destination is 20. Figure out other values, if any
            StorageType destinationStorageType = StorageType.StorageBoxNormal;

            // TODO: Cap to destination storage type max items
            uint boughAmount = packet.Structure.Num;

            uint totalPrice = good.Price * boughAmount;

            if(packet.Structure.Price != totalPrice)
            {
                // Dude's hacking, boot them out
                client.Send(new S2CShopBuyShopGoodsRes()
                {
                    Error = 1
                });
            }
            else
            {
                // UPDATE SHOP
                if(good.Stock != byte.MaxValue)
                {
                    // If stock isn't infinite (255), substract bought quantity from it
                    good.Stock = (byte) Math.Max(0, good.Stock - packet.Structure.Num); 
                }

                // UPDATE CHARACTER WALLET
                CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == shop.WalletType).Single();
                wallet.Value = Math.Max(0, wallet.Value - totalPrice);
                Database.UpdateWalletPoint(client.Character.Id, wallet);

                // UPDATE INVENTORY
                CDataItemUpdateResult itemUpdateResult = _itemManager.AddItem(Server.Database, client.Character, destinationStorageType, good.ItemId, boughAmount);

                // Send packets
                client.Send(new S2CShopBuyShopGoodsRes()
                {
                    PointType = shop.WalletType // ?
                });

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
        }
    }
}