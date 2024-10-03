using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetDropItemListHandler : GameStructurePacketHandler<C2SInstanceGetDropItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetDropItemListHandler));

        public InstanceGetDropItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetDropItemListReq> packet)
        {
            // This call is for when a bag is opened. Get the correct drops stored from the kill handler.
            List<InstancedGatheringItem> items = new List<InstancedGatheringItem>();

            if(client.InstanceQuestDropManager.IsQuestDrop(packet.Structure.LayoutId, packet.Structure.SetId))
            {
                items.AddRange(client.InstanceQuestDropManager.FetchEnemyLoot());
            } else
            {
                items.AddRange(client.InstanceDropItemManager.GetAssets(packet.Structure.LayoutId, (int)packet.Structure.SetId));
            }

            // Special Event Items
            items.AddRange(client.InstanceEventDropItemManager.FetchEventItems(client, packet.Structure.LayoutId, packet.Structure.SetId));

            client.Send(new S2CInstanceGetDropItemListRes()
            {
                LayoutId = packet.Structure.LayoutId,
                SetId = packet.Structure.SetId,
                ItemList = items.Select((dropItem, index) => new CDataGatheringItemElement()
                {
                    SlotNo = (uint) index,
                    ItemId = dropItem.ItemId,
                    ItemNum = dropItem.ItemNum
                    // TODO: Other properties
                }).ToList()
            });
        }
    }
}
