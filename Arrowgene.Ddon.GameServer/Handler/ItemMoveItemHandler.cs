using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
                ntc.UpdateItemList.AddRange(Server.ItemManager.MoveItem(Server, client.Character, itemFromTo.SrcStorageType, itemFromTo.ItemUId, itemFromTo.Num, itemFromTo.DstStorageType, itemFromTo.SlotNo));
            }
            client.Send(ntc);
            
            client.Send(new S2CItemMoveItemRes());
        }

        // Taken from sItemManager::moveItemsFunc (0xB9F867 in the PC Dump)
        // TODO: Cleanup
        private ushort DetermineUpdateType(byte sourceGameStorageType)
        {
            switch ( sourceGameStorageType )
            {
                case 1:
                    return 49;
                case 7:
                    return 22;
                case 19:
                    return 8;
                case 20:
                    return 9;
                default:
                    return 0;
            }
        }
    }
}