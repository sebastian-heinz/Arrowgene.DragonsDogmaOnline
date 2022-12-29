using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemHandler : StructurePacketHandler<GameClient, C2SInstanceGetGatheringItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemHandler));


        public InstanceGetGatheringItemHandler(DdonGameServer server) : base(server)
        {
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
            for (int r = 0; r < req.Structure.GatheringItemGetRequestList.Count; r++)
            {
                CDataGatheringItemGetRequest itemRequest = req.Structure.GatheringItemGetRequestList[r];
                CDataGatheringItemListUnk2 item = InstanceGetGatheringItemListHandler.ITEMS.Where(item => item.SlotNo == itemRequest.SlotNo).Single();
                CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                ntcData0.ItemList.ItemUId = EquipItem.GenerateEquipItemUId();
                ntcData0.ItemList.ItemId = item.ItemId;
                ntcData0.ItemList.ItemNum = itemRequest.Num; // Take only the requested amount, not item.ItemId which would be all available
                ntcData0.ItemList.Unk3 = 0;
                ntcData0.ItemList.StorageType = 1;
                ntcData0.ItemList.SlotNo = (ushort) item.SlotNo;
                ntcData0.ItemList.Unk6 = 0;
                ntcData0.ItemList.Unk7 = 0;
                ntcData0.ItemList.Bind = false;
                ntcData0.ItemList.Unk9 = 0;
                ntcData0.ItemList.Unk10 = 0;
                ntcData0.ItemList.Unk11 = 0;
                ntcData0.ItemList.WeaponCrestDataList = new List<CDataWeaponCrestData>();
                ntcData0.ItemList.ArmorCrestDataList = new List<CDataArmorCrestData>();
                ntcData0.ItemList.EquipElementParamList = new List<CDataEquipElementParam>();
                ntcData0.UpdateItemNum = 0;
                ntc.UpdateItemList.Add(ntcData0);

                CDataUpdateWalletPoint ntcData1 = new CDataUpdateWalletPoint();
                ntc.UpdateWallet.Add(ntcData1);
            }

            client.Send(ntc);
        }
    }
}
