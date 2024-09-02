using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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

        // See ItemNoticeType
        public ItemNoticeType UpdateType;
        public List<CDataItemUpdateResult> UpdateItemList;
        public List<CDataUpdateWalletPoint> UpdateWalletList;

        public class Serializer : PacketEntitySerializer<S2CItemUpdateCharacterItemNtc>
        {
            public override void Write(IBuffer buffer, S2CItemUpdateCharacterItemNtc obj)
            {
                WriteUInt16(buffer, (ushort) obj.UpdateType);
                WriteEntityList<CDataItemUpdateResult>(buffer, obj.UpdateItemList);
                WriteEntityList<CDataUpdateWalletPoint>(buffer, obj.UpdateWalletList);
            }

            public override S2CItemUpdateCharacterItemNtc Read(IBuffer buffer)
            {
                S2CItemUpdateCharacterItemNtc obj = new S2CItemUpdateCharacterItemNtc();
                obj.UpdateType = (ItemNoticeType) ReadUInt16(buffer);
                obj.UpdateItemList = ReadEntityList<CDataItemUpdateResult>(buffer);
                obj.UpdateWalletList = ReadEntityList<CDataUpdateWalletPoint>(buffer);
                return obj;
            }
        }
    }
}
