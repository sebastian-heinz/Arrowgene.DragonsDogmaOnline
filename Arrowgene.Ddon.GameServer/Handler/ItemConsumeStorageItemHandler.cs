using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemConsumeStorageItemHandler : GameStructurePacketHandler<C2SItemConsumeStorageItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemConsumeStorageItemHandler));
        
        private readonly ItemManager _itemManager;

        public ItemConsumeStorageItemHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemConsumeStorageItemReq> req)
        {
            client.Send(new S2CItemConsumeStorageItemRes());

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 4;
            foreach (CDataStorageItemUIDList consumeItem in req.Structure.ConsumeItemList)
            {
                ntc.UpdateItemList.Add(_itemManager.ConsumeItemInSlot(Server, client.Character, consumeItem.StorageType, consumeItem.SlotNo, consumeItem.Num));
            }
            client.Send(ntc);
        }
    }
}