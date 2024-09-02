using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailLegendPawnInfo
    {
        public CDataMailLegendPawnInfo()
        {
            AttachmentInfo = new CDataMailAttachmentInfo();
            Name = String.Empty;
        }

        public CDataMailAttachmentInfo AttachmentInfo { get; set; }
        public uint PawnId { get; set; }
        public string Name { get; set; }

        public class Serializer : EntitySerializer<CDataMailLegendPawnInfo>
        {

            public override void Write(IBuffer buffer, CDataMailLegendPawnInfo obj)
            {
                WriteEntity(buffer, obj.AttachmentInfo);
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.Name);
            }

            public override CDataMailLegendPawnInfo Read(IBuffer buffer)
            {
                CDataMailLegendPawnInfo obj = new CDataMailLegendPawnInfo();
                obj.AttachmentInfo = ReadEntity<CDataMailAttachmentInfo>(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
