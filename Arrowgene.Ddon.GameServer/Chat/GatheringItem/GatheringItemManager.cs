using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Chat.GatheringItem
{
    public class GatheringItemManager
    {
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

        public List<CDataGatheringItemListUnk2> GetItems(CDataStageLayoutId stageLayoutId, uint posId)
        {
            // TODO: Proper implementation
            return ITEMS;
        }
    }
}