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
    public class ItemSellItemHandler : GameStructurePacketHandler<C2SItemSellItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSellItemHandler));
        
        private readonly ItemManager _itemManager;
        private readonly WalletManager _walletManager;

        public ItemSellItemHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemSellItemReq> req)
        {
            client.Send(new S2CItemSellItemRes());

            uint totalAmountToAdd = 0;

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.ShopItemSell // No named enum for this?
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataStorageItemUIDList consumeItem in req.Structure.ConsumeItemList)
                {
                    var ntcData = _itemManager.ConsumeItemByUId(Server, client.Character, consumeItem.StorageType, consumeItem.ItemUId, consumeItem.Num, connection);
                    ntc.UpdateItemList.Add(ntcData);

                    uint goldValue = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, ntcData.ItemList.ItemId).Price;
                    uint amountToAdd = goldValue * consumeItem.Num;
                    totalAmountToAdd += amountToAdd;
                }
                CDataUpdateWalletPoint walletUpdate = Server.WalletManager.AddToWallet(client.Character, WalletType.Gold, totalAmountToAdd, connection);
                ntc.UpdateWalletList.Add(walletUpdate);
            });
            
            client.Send(ntc);
        }
    }
}
