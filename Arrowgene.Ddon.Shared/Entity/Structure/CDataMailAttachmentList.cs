using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMailAttachmentList
    {
        public CDataMailAttachmentList()
        {
            ItemList = new List<CDataMailItemInfo>();
            GPList = new List<CDataMailGPInfo>();
            OptionCourseList = new List<CDataMailOptionCourseInfo>();
            LegendPawnList = new List<CDataMailLegendPawnInfo>();
        }

        public List<CDataMailItemInfo> ItemList { get; set; }
        public List<CDataMailGPInfo> GPList { get; set; }
        public List<CDataMailOptionCourseInfo> OptionCourseList { get; set;}
        public List<CDataMailLegendPawnInfo> LegendPawnList {  get; set; }

        public class Serializer : EntitySerializer<CDataMailAttachmentList>
        {

            public override void Write(IBuffer buffer, CDataMailAttachmentList obj)
            {
                WriteEntityList(buffer, obj.ItemList);
                WriteEntityList(buffer, obj.GPList);
                WriteEntityList(buffer, obj.OptionCourseList);
                WriteEntityList(buffer, obj.LegendPawnList);
            }

            public override CDataMailAttachmentList Read(IBuffer buffer)
            {
                CDataMailAttachmentList obj = new CDataMailAttachmentList();
                obj.ItemList = ReadEntityList<CDataMailItemInfo>(buffer);
                obj.GPList = ReadEntityList<CDataMailGPInfo>(buffer);
                obj.OptionCourseList = ReadEntityList<CDataMailOptionCourseInfo>(buffer);
                obj.LegendPawnList = ReadEntityList<CDataMailLegendPawnInfo>(buffer);
                return obj;
            }
        }
    }
}

