using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemListHandler : StructurePacketHandler<GameClient, C2SInstanceGetGatheringItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemListHandler));

        private readonly GatheringItemManager _gatheringItemManager;

        public InstanceGetGatheringItemListHandler(DdonGameServer server) : base(server)
        {
            this._gatheringItemManager = server.GatheringItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemListReq> req)
        {
            S2CInstanceGetGatheringItemListRes res = new S2CInstanceGetGatheringItemListRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemUId = "PROBANDO"; // TODO: Find in item bag the used gathering item
            res.IsGatheringItemBreak = false; // TODO: Random, and update broken item by sending S2CItemUpdateCharacterItemNtc
            res.Unk0 = false;
            res.Unk1 = new List<CDataGatheringItemListUnk1>();
            res.Unk2 = client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, req.Structure.PosId)
                .Select((asset, index) => new CDataGatheringItemListUnk2()
                {
                    SlotNo = (uint) index,
                    ItemId = asset.ItemId,
                    ItemNum = asset.ItemNum,
                    Unk3 = asset.Unk3,
                    Unk4 = asset.Unk4
                })
                .ToList();
            client.Send(res);
        }
    }
}
