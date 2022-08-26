using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobChangeInfo
    {
        public CDataJobChangeInfo()
        {
            JobId=0;
            EquipItemList=new List<CDataEquipItemInfo>();
        }

        public JobId JobId { get; set; }
        public List<CDataEquipItemInfo> EquipItemList { get; set; }

        public class Serializer : EntitySerializer<CDataJobChangeInfo>
        {
            public override void Write(IBuffer buffer, CDataJobChangeInfo obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemList);
            }

            public override CDataJobChangeInfo Read(IBuffer buffer)
            {
                CDataJobChangeInfo obj = new CDataJobChangeInfo();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.EquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                return obj;                
            }
        }
    }
}
