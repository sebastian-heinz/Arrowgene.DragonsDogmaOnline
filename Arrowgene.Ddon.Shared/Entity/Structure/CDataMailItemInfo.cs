using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailItemInfo
    {
        public CDataMailItemInfo()
        {
            AttachmentInfo = new CDataMailAttachmentInfo();
        }

        public CDataMailAttachmentInfo AttachmentInfo {  get; set; }
        public uint ItemId { get; set; }
        public ushort Num {  get; set; }

        public class Serializer : EntitySerializer<CDataMailItemInfo>
        {

            public override void Write(IBuffer buffer, CDataMailItemInfo obj)
            {
                WriteEntity(buffer, obj.AttachmentInfo);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }

            public override CDataMailItemInfo Read(IBuffer buffer)
            {
                CDataMailItemInfo obj = new CDataMailItemInfo();
                obj.AttachmentInfo = ReadEntity<CDataMailAttachmentInfo>(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
