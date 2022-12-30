using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;


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
            S2CInstanceGetGatheringItemRes res = new S2CInstanceGetGatheringItemRes(req.Structure);
            client.Send(res);

            for (uint r = 0; r < req.Structure.Length; r++)
            {
                uint length = req.Structure.Length;
                uint n = 0;
                if (length == 12) { n = r; }
                if (length == 1) { n = req.Structure.ItemNo; }
                CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                ntcData0.ItemList.Unk2 = 10;
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
                CDataUpdateWalletPoint ntcData1 = new CDataUpdateWalletPoint();

                //Temp
                ushort i;
                if (n == 0) { i = 1; ntcData0.ItemList.Unk0 = "12345678"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 13807; }
                if (n == 1) { i = 2; ntcData0.ItemList.Unk0 = "12345679"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 11407; }
                if (n == 2) { i = 3; ntcData0.ItemList.Unk0 = "12345680"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9378; }
                if (n == 3) { i = 4; ntcData0.ItemList.Unk0 = "12345681"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 13801; }
                if (n == 4) { i = 5; ntcData0.ItemList.Unk0 = "12345682"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 55; }
                if (n == 5) { i = 6; ntcData0.ItemList.Unk0 = "12345683"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9387; }
                if (n == 6) { i = 7; ntcData0.ItemList.Unk0 = "12345684"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9389; }
                if (n == 7) { i = 8; ntcData0.ItemList.Unk0 = "12345685"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9429; }
                if (n == 8) { i = 9; ntcData0.ItemList.Unk0 = "12345686"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 47; }
                if (n == 9) { i = 10; ntcData0.ItemList.Unk0 = "12345687"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9404; }
                if (n == 10) { i = 11; ntcData0.ItemList.Unk0 = "12345688"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9405; }
                if (n == 11) { i = 12; ntcData0.ItemList.Unk0 = "12345689"; ntcData0.ItemList.Unk5 = i; ntcData0.ItemList.Unk1 = 9406; }

                S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
                ntc.UpdateType = 1;
                ntc.ItemUpdateResultList.Add(ntcData0);
                ntc.UpdateWalletPointList.Add(ntcData1);
                client.Send(ntc);

                if (req.Structure.Length == 1)
                {
                    break;
                }

            }

        }

    }
}
