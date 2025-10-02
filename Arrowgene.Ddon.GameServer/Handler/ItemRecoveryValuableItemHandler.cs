using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class ItemRecoveryValuableItemHandler : GameRequestPacketHandler<C2SItemRecoveryValuableItemReq, S2CItemRecoveryValuableItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetValuableItemListHandler));


        public ItemRecoveryValuableItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemRecoveryValuableItemRes Handle(GameClient client, C2SItemRecoveryValuableItemReq request)
        {
            PacketQueue queue = new();

            StorageType targetStorageType = StorageType.ItemPost;
            ClientItemInfo itemInfo = Server.AssetRepository.ClientItemInfos[request.ItemId];

            if (request.DestinationStorage == StorageType.ReceiveInItemBagCraft)
            {
                targetStorageType = itemInfo.StorageType;
            }
            else if (request.DestinationStorage == StorageType.ReceiveInStorageCraft)
            {
                if (client.Character.Storage.GetStorage(StorageType.StorageBoxNormal).EmptySlots() > 0)
                {
                    targetStorageType = StorageType.StorageBoxNormal;
                }
                else if (client.Character.Storage.GetStorage(StorageType.StorageBoxExpansion).EmptySlots() > 0)
                {
                    targetStorageType = StorageType.StorageBoxExpansion;
                }
                else
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
                }
            }
            else if (request.DestinationStorage == StorageType.StorageChestDrawer1)
            {
                if (client.Character.Storage.GetStorage(StorageType.StorageChestDrawer1).EmptySlots() > 0)
                {
                    targetStorageType = StorageType.StorageChestDrawer1;
                }
                else if (client.Character.Storage.GetStorage(StorageType.StorageChestDrawer2).EmptySlots() > 0)
                {
                    targetStorageType = StorageType.StorageChestDrawer2;
                }
                else if (client.Character.Storage.GetStorage(StorageType.StorageChestDrawer3).EmptySlots() > 0)
                {
                    targetStorageType = StorageType.StorageChestDrawer3;
                }
                else
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
                }
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE);
            }

            S2CItemUpdateCharacterItemNtc updateNtc = new()
            {
                UpdateType = ItemNoticeType.GetDispelItem // This probably uses one of the unknown updatetypes.
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                updateNtc.UpdateItemList.AddRange(Server.ItemManager.AddItem(Server, client.Character, targetStorageType, (uint)request.ItemId, 1, connectionIn: connection));

                foreach (var priceElement in request.Price)
                {
                    updateNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(client.Character, priceElement.Type, priceElement.Value, connection));
                }
            });

            // Recovering an emblem stone
            if (itemInfo.SubCategory == ItemSubCategory.EmblemStone)
            {
                Server.JobEmblemManager.AddNewEmblemItem(client.Character, updateNtc.UpdateItemList.FirstOrDefault()?.ItemList.ItemUId);
            }

            client.Send(updateNtc);

            return new();
        }
    }
}
