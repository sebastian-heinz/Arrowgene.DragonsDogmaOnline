using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealItemInfo
    {
        public CDataSoulOrdealItemInfo()
        {
            ItemUId = string.Empty;
        }

        public string ItemUId { get; set; }
        public uint Num { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealItemInfo>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealItemInfo obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.Num);
            }

            public override CDataSoulOrdealItemInfo Read(IBuffer buffer)
            {
                CDataSoulOrdealItemInfo obj = new CDataSoulOrdealItemInfo();
                obj.ItemUId = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
