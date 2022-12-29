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
            UpdateWallet=new List<CDataUpdateWalletPoint>();
        }

        public ushort UpdateType;
        public List<CDataItemUpdateResult> UpdateItemList;
        public List<CDataUpdateWalletPoint> UpdateWallet;

        public class Serializer : PacketEntitySerializer<S2CItemUpdateCharacterItemNtc>
        {
            public override void Write(IBuffer buffer, S2CItemUpdateCharacterItemNtc obj)
            {
                WriteUInt16(buffer, obj.UpdateType);
                WriteEntityList<CDataItemUpdateResult>(buffer, obj.UpdateItemList);
                WriteEntityList<CDataUpdateWalletPoint>(buffer, obj.UpdateWallet);
            }

            public override S2CItemUpdateCharacterItemNtc Read(IBuffer buffer)
            {
                S2CItemUpdateCharacterItemNtc obj = new S2CItemUpdateCharacterItemNtc();
                obj.UpdateType = ReadUInt16(buffer);
                obj.UpdateItemList = ReadEntityList<CDataItemUpdateResult>(buffer);
                obj.UpdateWallet = ReadEntityList<CDataUpdateWalletPoint>(buffer);
                return obj;
            }
        }
    }
}
