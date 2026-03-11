using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataValuableItem
    {

        public ItemId ItemId { get; set; }
        public WalletType WalletType { get; set; }
        public uint Price { get; set; }
        public ushort Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataValuableItem>
        {
            public override void Write(IBuffer buffer, CDataValuableItem obj)
            {
                WriteUInt32(buffer, (uint)obj.ItemId);
                WriteByte(buffer, (byte)obj.WalletType);
                WriteUInt32(buffer, obj.Price);
                WriteUInt16(buffer, obj.Unk3);
            }

            public override CDataValuableItem Read(IBuffer buffer)
            {
                CDataValuableItem obj = new CDataValuableItem();
                obj.ItemId = (ItemId)ReadUInt32(buffer);
                obj.WalletType = (WalletType)ReadByte(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
