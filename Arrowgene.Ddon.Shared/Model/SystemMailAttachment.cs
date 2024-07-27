using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class SystemMailAttachment
    {
        public SystemMailAttachment()
        {
            Param0 = string.Empty;
        }

        public ulong MessageId { get; set; }
        public ulong AttachmentId { get; set; }
        public SystemMailAttachmentType AttachmentType { get; set; }
        public string Param0 { get; set; }
        public uint Param1 { get; set; }
        public uint Param2 { get; set; }
        public uint Param3 { get; set; }
        public bool IsReceived { get; set; }

        public CDataMailItemInfo ToCDataMailItemInfo()
        {
            return new CDataMailItemInfo()
            {
                AttachmentInfo = new CDataMailAttachmentInfo()
                {
                    AttachmentId = AttachmentId,
                    IsReceived = IsReceived,
                },
                ItemId = Param1,
                Num = (ushort) Param2,
            };
        }

        public CDataMailGPInfo ToCDataMailGPInfo()
        {
            return new CDataMailGPInfo()
            {
                AttachmentInfo = new CDataMailAttachmentInfo()
                {
                    AttachmentId = AttachmentId,
                    IsReceived = IsReceived,
                },
                Type = Param1,
                Num = Param2,
            };
        }

        public CDataMailOptionCourseInfo ToCDataMailOptionCourseInfo()
        {
            return new CDataMailOptionCourseInfo()
            {
                AttachmentInfo = new CDataMailAttachmentInfo()
                {
                    AttachmentId = AttachmentId,
                    IsReceived = IsReceived,
                },
                OptionCourseId = Param1,
                OptionCourseLineupId = Param2,
                Time = Param3
            };
        }

        public CDataMailLegendPawnInfo ToCDataMailLegendPawnInfo()
        {
            return new CDataMailLegendPawnInfo()
            {
                AttachmentInfo = new CDataMailAttachmentInfo()
                {
                    AttachmentId = AttachmentId,
                    IsReceived = IsReceived,
                },
                Name = Param0,
                PawnId = Param1
            };
        }
    }
}
