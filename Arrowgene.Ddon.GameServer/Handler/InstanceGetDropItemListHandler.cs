using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
            List<InstancedGatheringItem> items = client.InstanceDropItemManager.GetAssets(packet.Structure.LayoutId, packet.Structure.SetId);
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