using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealItem
    {
        public CDataSoulOrdealItem()
        {
        }

        public uint ItemId { get; set; }
        public ushort Num { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealItem>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }

            public override CDataSoulOrdealItem Read(IBuffer buffer)
            {
                CDataSoulOrdealItem obj = new CDataSoulOrdealItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
