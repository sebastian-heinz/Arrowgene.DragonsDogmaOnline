using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemMoveItemHandler : GameStructurePacketHandler<C2SItemMoveItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetStorageItemListHandler));

        public ItemMoveItemHandler(DdonGameServer server) : base(server)
        {
        }
        public override void Handle(GameClient client, StructurePacket<C2SItemMoveItemReq> packet)
        {
            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = DetermineUpdateType(packet.Structure.SourceGameStorageType);
            foreach (CDataMoveItemUIDFromTo itemFromTo in packet.Structure.ItemUIDList)
            {
                Logger.Info($"Moving {itemFromTo.ItemUId} from {itemFromTo.SrcStorageType} to {itemFromTo.DstStorageType}");

                ntc.UpdateItemList.AddRange(
                    Server.ItemManager.MoveItem(
                        Server,
                        client.Character,
                        client.Character.Storage.GetStorage(itemFromTo.SrcStorageType),
                        itemFromTo.ItemUId,
                        itemFromTo.Num,
                        client.Character.Storage.GetStorage(itemFromTo.DstStorageType),
                        itemFromTo.SlotNo
                        )
                    );
            }

            Logger.Info($"Sending ItemUpdateCharacterItemNtc {ntc.UpdateType}.");
            client.Send(ntc);

            client.Send(new S2CItemMoveItemRes());
        }

        // Taken from sItemManager::moveItemsFunc (0xB9F867 in the PC Dump)
        // TODO: Cleanup
        private ItemNoticeType DetermineUpdateType(byte sourceGameStorageType)
        {
            Logger.Info($"Source storage type {sourceGameStorageType}");
            switch ( sourceGameStorageType )
            {
                case 1:
                    return ItemNoticeType.TemporaryItems;
                case 7:
                    return ItemNoticeType.ExStorageItems;
                case 8:
                case 9:
                case 10:
                    return ItemNoticeType.BaggageItems; //Found by binary search, may not be the "correct" one, but it does work.
                case 19:
                    return ItemNoticeType.StoreStorage_items;
                case 20:
                    return ItemNoticeType.LoadStorage_items;
                default:
                    return ItemNoticeType.Default;
            }
        }
    }
}
