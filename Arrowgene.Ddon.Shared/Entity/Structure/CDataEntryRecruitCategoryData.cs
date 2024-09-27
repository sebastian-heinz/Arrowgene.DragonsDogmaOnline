using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryRecruitCategoryData
    {
        public CDataEntryRecruitCategoryData()
        {
            CategoryName = string.Empty;
        }

        public uint CategoryId { get; set; }
        public string CategoryName { get; set; }
        public uint NumGroups { get; set; }

        public class Serializer : EntitySerializer<CDataEntryRecruitCategoryData>
        {
            public override void Write(IBuffer buffer, CDataEntryRecruitCategoryData obj)
            {
                WriteUInt32(buffer, obj.CategoryId);
                WriteMtString(buffer, obj.CategoryName);
                WriteUInt32(buffer, obj.NumGroups);
            }

            public override CDataEntryRecruitCategoryData Read(IBuffer buffer)
            {
                CDataEntryRecruitCategoryData obj = new CDataEntryRecruitCategoryData();
                obj.CategoryId = ReadUInt32(buffer);
                obj.CategoryName = ReadMtString(buffer);
                obj.NumGroups = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
