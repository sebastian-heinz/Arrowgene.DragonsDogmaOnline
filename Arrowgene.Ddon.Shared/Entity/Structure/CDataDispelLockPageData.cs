using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLockPageData
    {
        public string PageName { get; set; } = string.Empty;
        public List<CDataCommonU32> SealIndexList { get; set; } = [];

        public class Serializer : EntitySerializer<CDataDispelLockPageData>
        {
            public override void Write(IBuffer buffer, CDataDispelLockPageData obj)
            {
                WriteMtString(buffer, obj.PageName);
                WriteEntityList(buffer, obj.SealIndexList);
            }

            public override CDataDispelLockPageData Read(IBuffer buffer)
            {
                CDataDispelLockPageData obj = new CDataDispelLockPageData();
                obj.PageName = ReadMtString(buffer);
                obj.SealIndexList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
