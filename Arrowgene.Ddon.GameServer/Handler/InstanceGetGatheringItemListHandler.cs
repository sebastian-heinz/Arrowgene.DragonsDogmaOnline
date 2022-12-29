using System.Collections.Generic;
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

        // TODO: Move somewhere else
        public static readonly List<CDataGatheringItemListUnk2> ITEMS = new List<CDataGatheringItemListUnk2>() {
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x00, 
                    ItemId = 0x35EF, 
                    ItemNum = 0x0A,
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x01, 
                    ItemId = 0x2C8F, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x02, 
                    ItemId = 0x24A2, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x03, 
                    ItemId = 0x35E9, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x04, 
                    ItemId = 0x037, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false,  
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x05, 
                    ItemId = 0x24AB, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x06, 
                    ItemId = 0x24AD, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x07, 
                    ItemId = 0x24D5, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x08, 
                    ItemId = 0x02F, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x09, 
                    ItemId = 0x24BC, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x0A, 
                    ItemId = 0x24BD, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false, 
                },
                new CDataGatheringItemListUnk2() {
                    SlotNo = 0x0B, 
                    ItemId = 0x24BE, 
                    ItemNum = 0x0A, 
                    Unk3 = 0x00, 
                    Unk4 = false
                }
            };

        public InstanceGetGatheringItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemListReq> req)
        {
            S2CInstanceGetGatheringItemListRes res = new S2CInstanceGetGatheringItemListRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemUId = EquipItem.GenerateEquipItemUId();
            res.IsGatheringItemBreak = false; // TODO: Random, and update broken item by sending S2CItemUpdateCharacterItemNtc
            res.Unk0 = false;
            res.Unk1 = new List<CDataGatheringItemListUnk1>();
            res.Unk2 = ITEMS;
            client.Send(res);
        }
    }
}
