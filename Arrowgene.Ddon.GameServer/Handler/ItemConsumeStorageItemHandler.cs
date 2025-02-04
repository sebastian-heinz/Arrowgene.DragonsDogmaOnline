using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemConsumeStorageItemHandler : GameRequestPacketHandler<C2SItemConsumeStorageItemReq, S2CItemConsumeStorageItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemConsumeStorageItemHandler));
        
        public ItemConsumeStorageItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemConsumeStorageItemRes Handle(GameClient client, C2SItemConsumeStorageItemReq request)
        {
            S2CItemConsumeStorageItemRes res = new S2CItemConsumeStorageItemRes();

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.ConsumeBag
            };

            Server.Database.ExecuteInTransaction(connection => 
            {
                foreach (CDataStorageItemUIDList consumeItem in request.ConsumeItemList)
                {
                    CDataItemUpdateResult itemUpdate;
                    if (consumeItem.SlotNo == 0)
                    {
                        itemUpdate = Server.ItemManager.ConsumeItemByUId(Server, client.Character, consumeItem.StorageType, consumeItem.ItemUId, consumeItem.Num, connection)
                            ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND, 
                            $"Cannot find item in {consumeItem.StorageType}, UID {consumeItem.ItemUId}");
                    }
                    else
                    {
                        itemUpdate = Server.ItemManager.ConsumeItemInSlot(Server, client.Character, consumeItem.StorageType, consumeItem.SlotNo, consumeItem.Num, connection)
                            ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND, 
                            $"Cannot find item in {consumeItem.StorageType}, slot {consumeItem.SlotNo}");
                    }

                    ntc.UpdateItemList.Add(itemUpdate);
                }
            });
                
            client.Send(ntc);
            return res;
        }
    }
}
