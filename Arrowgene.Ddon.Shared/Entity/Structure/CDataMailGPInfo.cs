using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailGPInfo
    {
        public CDataMailGPInfo()
        {
            AttachmentInfo = new CDataMailAttachmentInfo();
        }

        public CDataMailAttachmentInfo AttachmentInfo { get; set; }
        public uint Num { get; set; }
        public uint Type { get; set; }

        public class Serializer : EntitySerializer<CDataMailGPInfo>
        {

            public override void Write(IBuffer buffer, CDataMailGPInfo obj)
            {
                WriteEntity(buffer, obj.AttachmentInfo);
                WriteUInt32(buffer, obj.Num);
                WriteUInt32(buffer, obj.Type);
            }

            public override CDataMailGPInfo Read(IBuffer buffer)
            {
                CDataMailGPInfo obj = new CDataMailGPInfo();
                obj.AttachmentInfo = ReadEntity<CDataMailAttachmentInfo>(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.Type = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
