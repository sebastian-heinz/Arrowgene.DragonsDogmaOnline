using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetDropItemListHandler : GameRequestPacketHandler<C2SInstanceGetDropItemListReq, S2CInstanceGetDropItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetDropItemListHandler));

        public InstanceGetDropItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetDropItemListRes Handle(GameClient client, C2SInstanceGetDropItemListReq request)
        {
            var items = client.InstanceDropItemManager.Fetch(request.LayoutId, request.SetId);

            return new S2CInstanceGetDropItemListRes()
            {
                LayoutId = request.LayoutId,
                SetId = request.SetId,
                ItemList = items.Select((dropItem, index) => new CDataGatheringItemElement()
                {
                    SlotNo = (uint) index,
                    ItemId = dropItem.ItemId,
                    ItemNum = dropItem.ItemNum
                    // TODO: Other properties
                }).ToList()
            };
        }
    }
}
