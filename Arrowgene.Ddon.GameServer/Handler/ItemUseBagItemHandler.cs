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
            ntcData0.ItemList.Unk0 = req.Structure.ItemUID;
            ntcData0.ItemList.Unk2 = 9;
            ntcData0.ItemList.Unk3 = 0;
            ntcData0.ItemList.Unk4 = 1;
            ntcData0.ItemList.Unk6 = 0;
            ntcData0.ItemList.Unk7 = 0;
            ntcData0.ItemList.Unk8 = false;
            ntcData0.ItemList.Unk9 = 0;
            ntcData0.ItemList.Unk10 = 0;
            ntcData0.ItemList.Unk11 = 0;
            ntcData0.ItemList.Unk12 = new List<CDataWeaponCrestData>();
            ntcData0.ItemList.Unk13 = new List<CDataArmorCrestData>();
            ntcData0.ItemList.Unk14 = new List<CDataEquipElementParam>();
            ntcData0.UpdateItemNum = 0;

            if (req.Structure.ItemUID == "12345678") { ntcData0.ItemList.Unk1 = 13807; ntcData0.ItemList.Unk5 = 1; }
            if (req.Structure.ItemUID == "12345679") { ntcData0.ItemList.Unk1 = 11407; ntcData0.ItemList.Unk5 = 2; }
            if (req.Structure.ItemUID == "12345680") { ntcData0.ItemList.Unk1 = 9378; ntcData0.ItemList.Unk5 = 3; }
            if (req.Structure.ItemUID == "12345681") { ntcData0.ItemList.Unk1 = 13801; ntcData0.ItemList.Unk5 = 4; }
            if (req.Structure.ItemUID == "12345682") { ntcData0.ItemList.Unk1 = 55; ntcData0.ItemList.Unk5 = 5; }
            if (req.Structure.ItemUID == "12345683") { ntcData0.ItemList.Unk1 = 9387; ntcData0.ItemList.Unk5 = 6; }
            if (req.Structure.ItemUID == "12345684") { ntcData0.ItemList.Unk1 = 9389; ntcData0.ItemList.Unk5 = 7; }
            if (req.Structure.ItemUID == "12345685") { ntcData0.ItemList.Unk1 = 9429; ntcData0.ItemList.Unk5 = 8; }
            if (req.Structure.ItemUID == "12345686") { ntcData0.ItemList.Unk1 = 47; ntcData0.ItemList.Unk5 = 9; }
            if (req.Structure.ItemUID == "12345687") { ntcData0.ItemList.Unk1 = 9404; ntcData0.ItemList.Unk5 = 10; }
            if (req.Structure.ItemUID == "12345688") { ntcData0.ItemList.Unk1 = 9405; ntcData0.ItemList.Unk5 = 11; }
            if (req.Structure.ItemUID == "12345689") { ntcData0.ItemList.Unk1 = 9406; ntcData0.ItemList.Unk5 = 12; }
            CDataUpdateWalletPoint ntcData1 = new CDataUpdateWalletPoint();

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.Unk0 = 3;
            ntc.ItemUpdateResultList.Add(ntcData0);
            ntc.UpdateWalletPointList.Add(ntcData1);
            client.Send(ntc);

            if (req.Structure.ItemUID == "12345682")
            { client.Send(SelectedDump.lantern2_27_16); }



        }
    }
}
