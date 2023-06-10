#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemHandler : StructurePacketHandler<GameClient, C2SInstanceGetGatheringItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemHandler));

        private static readonly Dictionary<uint, (WalletType Type, uint Quantity)> ItemIdWalletTypeAndQuantity = new Dictionary<uint, (WalletType Type, uint Amount)>() { 
            {7789, (WalletType.Gold, 1)},
            {7790, (WalletType.Gold, 10)},
            {7791, (WalletType.Gold, 100)},
            {7792, (WalletType.RiftPoints,1)},
            {7793, (WalletType.RiftPoints,10)},
            {7794, (WalletType.RiftPoints,100)},
            {7795, (WalletType.BloodOrbs,1)}, // Doesn't show up 
            {7796, (WalletType.BloodOrbs,10)}, // Doesn't show up
            {7797, (WalletType.BloodOrbs,100)}, // Doesn't show up
            {18742, (WalletType.HighOrbs,1)},
            {18743, (WalletType.HighOrbs,10)},
            {18744, (WalletType.HighOrbs,100)},
            {18828,(WalletType.Gold,7500)},
            {18829,(WalletType.RiftPoints,1250)},
            {18830,(WalletType.BloodOrbs,750)},
            {19508,(WalletType.Gold,1000)},
            {19509,(WalletType.Gold,10000)},
            {19510,(WalletType.RiftPoints,1000)},
            {19511,(WalletType.BloodOrbs,1000)}
            // TODO: Find all items that add wallet points
        };

        // [[item]]
        // id = 16822 (Adds 100 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'
        // [[item]]
        // id = 16831 (Adds 10000 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'
        // [[item]]
        // id = 18831 (Adds 63000 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'

        // [[item]]
        // id = 18832 (Adds 18 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25651 (Adds 1 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25652 (Adds 10 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25653 (Adds 100 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'

        private readonly ItemManager _itemManager;

        public InstanceGetGatheringItemHandler(DdonGameServer server) : base(server)
        {
            this._itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemReq> req)
        {
            S2CInstanceGetGatheringItemRes res = new S2CInstanceGetGatheringItemRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemGetRequestList = req.Structure.GatheringItemGetRequestList;
            client.Send(res);

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 1;
            foreach (CDataGatheringItemGetRequest gatheringItemRequest in req.Structure.GatheringItemGetRequestList)
            {
                GatheringItem gatheredItem = client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, req.Structure.PosId)[(int) gatheringItemRequest.SlotNo];
                if(ItemIdWalletTypeAndQuantity.ContainsKey(gatheredItem.ItemId)) {
                    var walletTypeAndQuantity = ItemIdWalletTypeAndQuantity[gatheredItem.ItemId];

                    uint pickedGatherItems = gatheringItemRequest.Num;
                    uint totalQuantityToAdd = walletTypeAndQuantity.Quantity * gatheredItem.ItemNum;
                    
                    CDataWalletPoint characterWalletPoint = client.Character.WalletPointList.Where(wp => wp.Type == walletTypeAndQuantity.Type).First();
                    characterWalletPoint.Value += totalQuantityToAdd; // TODO: Cap to maximum for that wallet
                    Database.UpdateWalletPoint(client.Character.CharacterId, characterWalletPoint);

                    CDataUpdateWalletPoint walletUpdate = new CDataUpdateWalletPoint();
                    walletUpdate.Type = walletTypeAndQuantity.Type;
                    walletUpdate.AddPoint = (int) totalQuantityToAdd;
                    walletUpdate.Value = characterWalletPoint.Value;
                    ntc.UpdateWalletList.Add(walletUpdate);
                    
                    gatheredItem.ItemNum -= pickedGatherItems;
                } else {
                    CDataItemUpdateResult? result = this._itemManager.AddItem(Server, client.Character, true, gatheredItem.ItemId, gatheringItemRequest.Num);
                    ntc.UpdateItemList.Add(result);
                    gatheredItem.ItemNum -= (uint) result.UpdateItemNum;
                }

            }

            client.Send(ntc);
        }
    }
}
