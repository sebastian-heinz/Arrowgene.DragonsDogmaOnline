using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftProduct
    {
        public uint ItemID { get; set; }

        public uint ItemNum { get; set; }

        public byte PlusValue { get; set; }

        public class Serializer : EntitySerializer<CDataCraftProduct>
        {
            public override void Write(IBuffer buffer, CDataCraftProduct obj)
            {
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt32(buffer, obj.ItemNum);
                WriteByte(buffer, obj.PlusValue);
            }

            public override CDataCraftProduct Read(IBuffer buffer)
            {
                CDataCraftProduct obj = new CDataCraftProduct();
                obj.ItemID = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                obj.PlusValue = ReadByte(buffer);
                return obj;
            }
        }
    }
}
