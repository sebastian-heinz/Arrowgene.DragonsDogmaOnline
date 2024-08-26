using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobOrbTreeStatus
    {
        public JobId JobId { get; set; }
        public bool IsReleased { get; set; }
        public float Rate { get; set; }
    
        public class Serializer : EntitySerializer<CDataJobOrbTreeStatus>
        {
            public override void Write(IBuffer buffer, CDataJobOrbTreeStatus obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteBool(buffer, obj.IsReleased);
                WriteFloat(buffer, obj.Rate);
            }
        
            public override CDataJobOrbTreeStatus Read(IBuffer buffer)
            {
                CDataJobOrbTreeStatus obj = new CDataJobOrbTreeStatus();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.IsReleased = ReadBool(buffer);
                obj.Rate = ReadFloat(buffer);
                return obj;
            }
        }
    }
}
