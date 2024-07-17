using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailTextInfo
    {
        public CDataMailTextInfo()
        {
            Text = string.Empty;
            MailAttachmentList = new CDataMailAttachmentList();
        }

        public string Text { get; set; }
        public CDataMailAttachmentList MailAttachmentList {get; set;}

        public class Serializer : EntitySerializer<CDataMailTextInfo>
        {

            public override void Write(IBuffer buffer, CDataMailTextInfo obj)
            {
                WriteMtString(buffer, obj.Text);
                WriteEntity(buffer, obj.MailAttachmentList);
            }

            public override CDataMailTextInfo Read(IBuffer buffer)
            {
                CDataMailTextInfo obj = new CDataMailTextInfo();
                obj.Text = ReadMtString(buffer);
                obj.MailAttachmentList = ReadEntity<CDataMailAttachmentList>(buffer);
                return obj;
            }
        }
    }
}
