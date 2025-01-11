using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetDropItemHandler : GameRequestPacketHandler<C2SInstanceGetDropItemReq, S2CInstanceGetDropItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetDropItemHandler));
        
        public InstanceGetDropItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetDropItemRes Handle(GameClient client, C2SInstanceGetDropItemReq request)
        {
            // This call is for when an item is claimed from a bag. It needs the drops rolled from the enemy to keep track of the items left.

            List<InstancedGatheringItem> items = new List<InstancedGatheringItem>();

            if (client.InstanceQuestDropManager.IsQuestDrop(request.LayoutId, request.SetId))
            {
                items.AddRange(client.InstanceQuestDropManager.FetchEnemyLoot());
            } else
            {
                items.AddRange(client.InstanceDropItemManager.GetAssets(request.LayoutId, (int)request.SetId));
            }

            // Special Event Items
            items.AddRange(client.InstanceEventDropItemManager.FetchEventItems(client, request.LayoutId, request.SetId));

            // Add Epitaph Items
            items.AddRange(client.InstanceEpiDropItemManager.FetchItems(client, request.LayoutId, request.SetId));

            S2CInstanceGetDropItemRes res = new()
            {
                LayoutId = request.LayoutId,
                SetId = request.SetId,
                GatheringItemGetRequestList = request.GatheringItemGetRequestList
            };

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Drop
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in request.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem dropItem = items[(int)gatheringItemRequest.SlotNo];
                    Server.ItemManager.GatherItem(client.Character, ntc, dropItem, gatheringItemRequest.Num, connection);
                }
            });
            
            client.Send(ntc);
            return res;
        }
    }
}
