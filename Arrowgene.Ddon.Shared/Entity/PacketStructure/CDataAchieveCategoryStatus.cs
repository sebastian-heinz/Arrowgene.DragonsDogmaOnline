using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAchieveCategoryStatus
    {
        public byte CategoryID { get; set; }
        public ushort AchieveNum { get; set; }
        public ushort TargetNum { get; set; }
    
        public class Serializer : EntitySerializer<CDataAchieveCategoryStatus>
        {
            public override void Write(IBuffer buffer, CDataAchieveCategoryStatus obj)
            {
                WriteByte(buffer, obj.CategoryID);
                WriteUInt16(buffer, obj.AchieveNum);
                WriteUInt16(buffer, obj.TargetNum);
            }
        
            public override CDataAchieveCategoryStatus Read(IBuffer buffer)
            {
                CDataAchieveCategoryStatus obj = new CDataAchieveCategoryStatus();
                obj.CategoryID = ReadByte(buffer);
                obj.AchieveNum = ReadUInt16(buffer);
                obj.TargetNum = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}