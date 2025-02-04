using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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
            var items = client.InstanceDropItemManager.Fetch(request.LayoutId, request.SetId);

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

            return new()
            {
                LayoutId = request.LayoutId,
                SetId = request.SetId,
                GatheringItemGetRequestList = request.GatheringItemGetRequestList
            };
        }
    }
}
