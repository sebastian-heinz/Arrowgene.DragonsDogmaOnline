using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemListHandler : StructurePacketHandler<GameClient, C2SInstanceGetGatheringItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemListHandler));

        public InstanceGetGatheringItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemListReq> req)
        {
            List<InstancedGatheringItem> gatheringItems = client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, req.Structure.PosId);

            S2CInstanceGetGatheringItemListRes res = new S2CInstanceGetGatheringItemListRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemUId = "PROBANDO"; // TODO: Find out somehow what gathering item was used and send it back
            res.IsGatheringItemBreak = false; // TODO: False by default. True if lockpick?, or random if other gathering item. Update broken item by sending S2CItemUpdateCharacterItemNtc
            res.Unk0 = false;
            res.Unk1 = new List<CDataGatheringItemListUnk1>();
            res.ItemList = gatheringItems
                .Select((asset, index) => new CDataGatheringItemElement()
                {
                    SlotNo = (uint) index,
                    ItemId = asset.ItemId,
                    ItemNum = asset.ItemNum,
                    Quality = asset.Quality,
                    IsHidden = asset.IsHidden
                })
                .ToList();
            client.Send(res);
        }
    }
}
