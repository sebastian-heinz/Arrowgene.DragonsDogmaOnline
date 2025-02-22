using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetDropItemHandler : GameRequestPacketQueueHandler<C2SInstanceGetDropItemReq, S2CInstanceGetDropItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetDropItemHandler));
        
        public InstanceGetDropItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SInstanceGetDropItemReq request)
        {
            var items = client.InstanceDropItemManager.Fetch(request.LayoutId, request.SetId);

            PacketQueue queue = new();

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Drop
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in request.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem dropItem = items[(int)gatheringItemRequest.SlotNo];
                    queue.AddRange(Server.ItemManager.GatherItem(client, ntc, dropItem, gatheringItemRequest.Num, connection));
                }
            });
            client.Enqueue(ntc, queue);

            client.Enqueue(new S2CInstanceGetDropItemRes() 
            {
                LayoutId = request.LayoutId,
                SetId = request.SetId,
                GatheringItemGetRequestList = request.GatheringItemGetRequestList
            }, queue);
            return queue;
        }
    }
}
