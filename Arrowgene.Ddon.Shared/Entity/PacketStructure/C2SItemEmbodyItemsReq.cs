using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemEmbodyItemsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_EMBODY_ITEMS_REQ;

        public C2SItemEmbodyItemsReq()
        {
            ItemList = new List<CDataItemEmbodyItem>();
        }

        public byte StorageType {  get; set; }
        public WalletType WalletType { get; set; } // Passed from the value of unk0 in price res
        public List<CDataItemEmbodyItem> ItemList {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemEmbodyItemsReq>
        {
            public override void Write(IBuffer buffer, C2SItemEmbodyItemsReq obj)
            {
                WriteByte(buffer, obj.StorageType);
                WriteUInt32(buffer, (uint) obj.WalletType);
                WriteEntityList(buffer, obj.ItemList);
            }

            public override C2SItemEmbodyItemsReq Read(IBuffer buffer)
            {
                C2SItemEmbodyItemsReq obj = new C2SItemEmbodyItemsReq();
                obj.StorageType = ReadByte(buffer);
                obj.WalletType = (WalletType) ReadUInt32(buffer);
                obj.ItemList = ReadEntityList<CDataItemEmbodyItem>(buffer);
                return obj;
            }
        }
    }
}

