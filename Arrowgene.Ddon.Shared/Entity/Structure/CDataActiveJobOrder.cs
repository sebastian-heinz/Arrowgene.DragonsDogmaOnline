using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataActiveJobOrder
    {
        public CDataActiveJobOrder()
        {
            JobOrderProgressList = new List<CDataJobOrderProgress>();
        }

        public ReleaseType ReleaseType { get; set; }
        public uint ReleaseId { get; set; }
        public byte ReleaseLv { get; set; }
        public List<CDataJobOrderProgress> JobOrderProgressList { get; set; }

        // This field is not part of the packet but used for some tracking
        // internally
        public bool OrderAccepted { get; set; }

        public class Serializer : EntitySerializer<CDataActiveJobOrder>
        {
            public override void Write(IBuffer buffer, CDataActiveJobOrder obj)
            {
                WriteByte(buffer, (byte) obj.ReleaseType);
                WriteUInt32(buffer, obj.ReleaseId);
                WriteByte(buffer, obj.ReleaseLv);
                WriteEntityList(buffer, obj.JobOrderProgressList);
            }

            public override CDataActiveJobOrder Read(IBuffer buffer)
            {
                CDataActiveJobOrder obj = new CDataActiveJobOrder();
                obj.ReleaseType = (ReleaseType) ReadByte(buffer);
                obj.ReleaseId = ReadUInt32(buffer);
                obj.ReleaseLv = ReadByte(buffer);
                obj.JobOrderProgressList = ReadEntityList<CDataJobOrderProgress>(buffer);
                return obj;
            }
        }
    }
}
