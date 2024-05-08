using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemConsumeStorageItemHandler : GameStructurePacketHandler<C2SItemConsumeStorageItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemConsumeStorageItemHandler));
        
        public ItemConsumeStorageItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemConsumeStorageItemReq> req)
        {
            S2CItemConsumeStorageItemRes res = new S2CItemConsumeStorageItemRes();
            try
            {
                S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
                ntc.UpdateType = 4;
                foreach (CDataStorageItemUIDList consumeItem in req.Structure.ConsumeItemList)
                {
                    CDataItemUpdateResult itemUpdate;
                    if(consumeItem.SlotNo == 0)
                    {
                        itemUpdate = Server.ItemManager.ConsumeItemByUId(Server, client.Character, consumeItem.StorageType, consumeItem.ItemUId, consumeItem.Num);
                    }
                    else
                    {
                        itemUpdate = Server.ItemManager.ConsumeItemInSlot(Server, client.Character, consumeItem.StorageType, consumeItem.SlotNo, consumeItem.Num);
                    }
                    ntc.UpdateItemList.Add(itemUpdate);
                }
                client.Send(ntc);
            }
            catch(Exception _)
            {
                res.Error = (uint)ErrorCode.ERROR_CODE_INSTANCE_AREA_GATHERING_ITEM_ERROR;
            }
            client.Send(res);
        }
    }
}