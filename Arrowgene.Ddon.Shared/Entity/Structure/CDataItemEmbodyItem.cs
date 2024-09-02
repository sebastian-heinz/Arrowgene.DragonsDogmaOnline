using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemEmbodyItem
    {
        public CDataItemEmbodyItem()
        {
            UId = string.Empty;
        }

        public string UId { get; set; }
        public uint Num { get; set; }

        public class Serializer : EntitySerializer<CDataItemEmbodyItem>
        {
            public override void Write(IBuffer buffer, CDataItemEmbodyItem obj)
            {
                WriteMtString(buffer, obj.UId);
                WriteUInt32(buffer, obj.Num);
            }

            public override CDataItemEmbodyItem Read(IBuffer buffer)
            {
                CDataItemEmbodyItem obj = new CDataItemEmbodyItem();
                obj.UId = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }

}
