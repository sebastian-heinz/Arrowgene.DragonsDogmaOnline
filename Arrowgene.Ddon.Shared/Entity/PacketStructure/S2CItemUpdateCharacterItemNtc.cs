using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemUpdateCharacterItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ITEM_UPDATE_CHARACTER_ITEM_NTC;

        public S2CItemUpdateCharacterItemNtc()
        {
            UpdateType=0;
            UpdateItemList=new List<CDataItemUpdateResult>();
            UpdateWalletList=new List<CDataUpdateWalletPoint>();
        }

        // 1: S2C_INSTANCE_GET_GATHERING_ITEM_RES
        // 2: S2C_INSTANCE_GET_DROP_ITEM_RES
        // 3: S2C_ITEM_USE_BAG_ITEM_RES
        // 4: S2C_ITEM_CONSUME_STORAGE_ITEM_RES
        // 8: S2C_ITEM_MOVE_ITEM_RES
        // 0x24: changeCharacterEquip
        // 0x25: changePawnEquip
        // 0x26: changeCharacterStorageEquip
        // 0x27: changePawnStorageEquip
        // 0x28: Job Change
        // 0x29: Pawn Job Change
        // 0x2A ITEM_NOTICE_TYPE_ORB_DEVOTE_CONSUME
        // 0x32 ITEM_NOTICE_TYPE_RELEASE_TREE_ELEMENT
        // 0x37: S2C_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_RES
        // 0x38: S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_RES
        // 0x10a: S2C_SHOP_BUY_SHOP_GOODS_RES
        // 0x10b: S2C_ITEM_SELL_ITEM_RES
        // 0x121: Job Items
        public ushort UpdateType;
        public List<CDataItemUpdateResult> UpdateItemList;
        public List<CDataUpdateWalletPoint> UpdateWalletList;

        public class Serializer : PacketEntitySerializer<S2CItemUpdateCharacterItemNtc>
        {
            public override void Write(IBuffer buffer, S2CItemUpdateCharacterItemNtc obj)
            {
                WriteUInt16(buffer, obj.UpdateType);
                WriteEntityList<CDataItemUpdateResult>(buffer, obj.UpdateItemList);
                WriteEntityList<CDataUpdateWalletPoint>(buffer, obj.UpdateWalletList);
            }

            public override S2CItemUpdateCharacterItemNtc Read(IBuffer buffer)
            {
                S2CItemUpdateCharacterItemNtc obj = new S2CItemUpdateCharacterItemNtc();
                obj.UpdateType = ReadUInt16(buffer);
                obj.UpdateItemList = ReadEntityList<CDataItemUpdateResult>(buffer);
                obj.UpdateWalletList = ReadEntityList<CDataUpdateWalletPoint>(buffer);
                return obj;
            }
        }
    }
}
