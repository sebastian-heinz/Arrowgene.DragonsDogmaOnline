using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetDropItemHandler : GameStructurePacketHandler<C2SInstanceGetDropItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetDropItemHandler));
        
        public InstanceGetDropItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetDropItemReq> packet)
        {
            // This call is for when an item is claimed from a bag. It needs the drops rolled from the enemy to keep track of the items left.

            List<InstancedGatheringItem> items;

            if (client.InstanceQuestDropManager.IsQuestDrop(packet.Structure.LayoutId, packet.Structure.SetId))
            {
                items = client.InstanceQuestDropManager.FetchEnemyLoot();
            } else
            {
                items = client.InstanceDropItemManager.GetAssets(packet.Structure.LayoutId, (int)packet.Structure.SetId);
            }

            S2CInstanceGetDropItemRes res = new()
            {
                LayoutId = packet.Structure.LayoutId,
                SetId = packet.Structure.SetId,
                GatheringItemGetRequestList = packet.Structure.GatheringItemGetRequestList
            };

            client.Send(res);

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Drop
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in packet.Structure.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem dropItem = items[(int)gatheringItemRequest.SlotNo];
                    Server.ItemManager.GatherItem(Server, client.Character, ntc, dropItem, gatheringItemRequest.Num, connection);
                }
            });
            
            client.Send(ntc);
        }
    }
}
