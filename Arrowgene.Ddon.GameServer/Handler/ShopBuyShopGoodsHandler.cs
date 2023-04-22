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

            StorageType destinationStorageType;
            switch(packet.Structure.Destination) {
                case 19:
                    // If packet.Structure.Destination is 19: Send to corresponding item bag
                    destinationStorageType = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, good.ItemId).StorageType;
                    break;
                case 20:
                    // If packet.Structure.Destination is 20: Send to storage 
                    // TODO: Check if storage is full
                    destinationStorageType = StorageType.StorageBoxNormal;
                    break;
                default:
                    throw new Exception("Unexpected destination when buying goods: "+packet.Structure.Destination);
            }

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