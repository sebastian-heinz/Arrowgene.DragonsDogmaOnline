using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobChangeInfo
    {
        public CDataJobChangeInfo()
        {
            JobId=0;
            EquipItemList=new List<CDataEquipItemInfo>();
        }

        public byte JobId { get; set; }
        public List<CDataEquipItemInfo> EquipItemList { get; set; }

        public class Serializer : EntitySerializer<CDataJobChangeInfo>
        {
            public override void Write(IBuffer buffer, CDataJobChangeInfo obj)
            {
                WriteByte(buffer, obj.JobId);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemList);
            }

            public override CDataJobChangeInfo Read(IBuffer buffer)
            {
                CDataJobChangeInfo obj = new CDataJobChangeInfo();
                obj.JobId = ReadByte(buffer);
                obj.EquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                return obj;                
            }
        }
    }
}
