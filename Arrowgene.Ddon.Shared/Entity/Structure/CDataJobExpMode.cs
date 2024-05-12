using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobExpMode
    {
        public JobId Job { get; set; }
        public byte ExpMode { get; set; }

        public class Serializer : EntitySerializer<CDataJobExpMode>
        {
            public override void Write(IBuffer buffer, CDataJobExpMode obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.ExpMode);
            }
        
            public override CDataJobExpMode Read(IBuffer buffer)
            {
                CDataJobExpMode obj = new CDataJobExpMode();
                obj.Job = (JobId) ReadByte(buffer);
                obj.ExpMode = ReadByte(buffer);
                return obj;
            }
        }
    }
}