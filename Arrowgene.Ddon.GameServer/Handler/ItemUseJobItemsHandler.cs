#nullable enable
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemUseJobItemsHandler : GameRequestPacketHandler<C2SItemUseJobItemsReq, S2CItemUseJobItemsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemUseJobItemsHandler));
        
        private readonly ItemManager _itemManager;

        public ItemUseJobItemsHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override S2CItemUseJobItemsRes Handle(GameClient client, C2SItemUseJobItemsReq request)
        {
            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.UseJobItem
            };

            foreach (CDataItemUIDList itemUIdListElement in request.ItemUIdList)
            {
                var update = _itemManager.ConsumeItemByUId(Server, client.Character, StorageType.ItemBagJob, itemUIdListElement.ItemUID, itemUIdListElement.Num);
                if (update != null)
                {
                    ntc.UpdateItemList.Add(update);
                }
                else
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NUM_SHORT);
                }
            }

            client.Send(ntc);
            return new S2CItemUseJobItemsRes();
        }
    }
}
