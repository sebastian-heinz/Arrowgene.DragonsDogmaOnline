using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailAttachmentInfo
    {
        public CDataMailAttachmentInfo()
        {
        }

        public ulong AttachmentId { get; set; }
        public bool IsReceived { get; set; }

        public class Serializer : EntitySerializer<CDataMailAttachmentInfo>
        {

            public override void Write(IBuffer buffer, CDataMailAttachmentInfo obj)
            {
                WriteUInt64(buffer, obj.AttachmentId);
                WriteBool(buffer, obj.IsReceived);
            }

            public override CDataMailAttachmentInfo Read(IBuffer buffer)
            {
                CDataMailAttachmentInfo obj = new CDataMailAttachmentInfo();
                obj.AttachmentId = ReadUInt64(buffer);
                obj.IsReceived = ReadBool(buffer);
                return obj;
            }
        }
    }
}
