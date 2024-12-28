using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOrbPageStatus
    {
        public CDataOrbPageStatus()
        {
            PageNo = 0;
            CategoryStatusList = new List<CDataOrbCategoryStatus>();
        }

        public byte PageNo;
        public List<CDataOrbCategoryStatus> CategoryStatusList;

        public class Serializer : EntitySerializer<CDataOrbPageStatus>
        {
            public override void Write(IBuffer buffer, CDataOrbPageStatus obj)
            {
                WriteByte(buffer, obj.PageNo);
                WriteEntityList(buffer, obj.CategoryStatusList);
            }

            public override CDataOrbPageStatus Read(IBuffer buffer)
            {
                CDataOrbPageStatus obj = new CDataOrbPageStatus();
                obj.PageNo = ReadByte(buffer);
                obj.CategoryStatusList = ReadEntityList<CDataOrbCategoryStatus>(buffer);
                return obj;
            }
        }
    }
}
