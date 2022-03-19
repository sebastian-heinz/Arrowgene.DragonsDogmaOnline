using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemUpdateCharacterItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ITEM_UPDATE_CHARACTER_ITEM_NOTICE;

        public S2CItemUpdateCharacterItemNtc()
        {
            Unk0=0;
            ItemUpdateResultList=new List<CDataItemUpdateResult>();
            UpdateWalletPointList=new List<CDataUpdateWalletPoint>();
        }

        public ushort Unk0;
        public List<CDataItemUpdateResult> ItemUpdateResultList;
        public List<CDataUpdateWalletPoint> UpdateWalletPointList;

        public class Serializer : PacketEntitySerializer<S2CItemUpdateCharacterItemNtc>
        {
            public override void Write(IBuffer buffer, S2CItemUpdateCharacterItemNtc obj)
            {
                WriteUInt16(buffer, obj.Unk0);
                WriteEntityList<CDataItemUpdateResult>(buffer, obj.ItemUpdateResultList);
                WriteEntityList<CDataUpdateWalletPoint>(buffer, obj.UpdateWalletPointList);
            }

            public override S2CItemUpdateCharacterItemNtc Read(IBuffer buffer)
            {
                S2CItemUpdateCharacterItemNtc obj = new S2CItemUpdateCharacterItemNtc();
                obj.Unk0 = ReadUInt16(buffer);
                obj.ItemUpdateResultList = ReadEntityList<CDataItemUpdateResult>(buffer);
                obj.UpdateWalletPointList = ReadEntityList<CDataUpdateWalletPoint>(buffer);
                return obj;
            }
        }
    }
}
