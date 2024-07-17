using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailOptionCourseInfo
    {
        public CDataMailOptionCourseInfo()
        {
            AttachmentInfo = new CDataMailAttachmentInfo();
        }

        public CDataMailAttachmentInfo AttachmentInfo { get; set; }
        public uint OptionCourseId { get; set; }
        public uint OptionCourseLineupId { get; set; }
        public uint Time {  get; set; }

        public class Serializer : EntitySerializer<CDataMailOptionCourseInfo>
        {

            public override void Write(IBuffer buffer, CDataMailOptionCourseInfo obj)
            {
                WriteEntity(buffer, obj.AttachmentInfo);
                WriteUInt32(buffer, obj.OptionCourseId);
                WriteUInt32(buffer, obj.OptionCourseLineupId);
                WriteUInt32(buffer, obj.Time);
            }

            public override CDataMailOptionCourseInfo Read(IBuffer buffer)
            {
                CDataMailOptionCourseInfo obj = new CDataMailOptionCourseInfo();
                obj.AttachmentInfo = ReadEntity<CDataMailAttachmentInfo>(buffer);
                obj.OptionCourseId = ReadUInt32(buffer);
                obj.OptionCourseLineupId = ReadUInt32(buffer);
                obj.Time = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
