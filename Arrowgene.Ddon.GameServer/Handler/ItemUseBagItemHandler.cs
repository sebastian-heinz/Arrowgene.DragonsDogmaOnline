using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemUseBagItemHandler : StructurePacketHandler<GameClient, C2SItemUseBagItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemUseBagItemHandler));


        public ItemUseBagItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemUseBagItemReq> req)
        {
            S2CItemUseBagItemRes res = new S2CItemUseBagItemRes(req.Structure);
            client.Send(res);


            CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
            ntcData0.ItemList.ItemUId = req.Structure.ItemUID;
            ntcData0.ItemList.ItemNum = 9;
            ntcData0.ItemList.Unk3 = 0;
            ntcData0.ItemList.StorageType = 1;
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

            if (req.Structure.ItemUID == "12345678") { ntcData0.ItemList.ItemId = 13807; ntcData0.ItemList.SlotNo = 1; }
            if (req.Structure.ItemUID == "12345679") { ntcData0.ItemList.ItemId = 11407; ntcData0.ItemList.SlotNo = 2; }
            if (req.Structure.ItemUID == "12345680") { ntcData0.ItemList.ItemId = 9378; ntcData0.ItemList.SlotNo = 3; }
            if (req.Structure.ItemUID == "12345681") { ntcData0.ItemList.ItemId = 13801; ntcData0.ItemList.SlotNo = 4; }
            if (req.Structure.ItemUID == "12345682") { ntcData0.ItemList.ItemId = 55; ntcData0.ItemList.SlotNo = 5; }
            if (req.Structure.ItemUID == "12345683") { ntcData0.ItemList.ItemId = 9387; ntcData0.ItemList.SlotNo = 6; }
            if (req.Structure.ItemUID == "12345684") { ntcData0.ItemList.ItemId = 9389; ntcData0.ItemList.SlotNo = 7; }
            if (req.Structure.ItemUID == "12345685") { ntcData0.ItemList.ItemId = 9429; ntcData0.ItemList.SlotNo = 8; }
            if (req.Structure.ItemUID == "12345686") { ntcData0.ItemList.ItemId = 47; ntcData0.ItemList.SlotNo = 9; }
            if (req.Structure.ItemUID == "12345687") { ntcData0.ItemList.ItemId = 9404; ntcData0.ItemList.SlotNo = 10; }
            if (req.Structure.ItemUID == "12345688") { ntcData0.ItemList.ItemId = 9405; ntcData0.ItemList.SlotNo = 11; }
            if (req.Structure.ItemUID == "12345689") { ntcData0.ItemList.ItemId = 9406; ntcData0.ItemList.SlotNo = 12; }
            CDataUpdateWalletPoint ntcData1 = new CDataUpdateWalletPoint();

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 3;
            ntc.UpdateItemList.Add(ntcData0);
            ntc.UpdateWallet.Add(ntcData1);
            client.Send(ntc);

            if (req.Structure.ItemUID == "12345682")
            { client.Send(SelectedDump.lantern2_27_16); }



        }
    }
}
