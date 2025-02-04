using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobPlayPoint
    {
        public CDataJobPlayPoint()
        {
            Job = 0;
            PlayPoint = new CDataPlayPointData();
        }

        public JobId Job;
        public CDataPlayPointData PlayPoint;

        public class Serializer : EntitySerializer<CDataJobPlayPoint>
        {
            public override void Write(IBuffer buffer, CDataJobPlayPoint obj)
            {
                WriteByte(buffer, (byte)obj.Job);
                WriteEntity(buffer, obj.PlayPoint);
            }

            public override CDataJobPlayPoint Read(IBuffer buffer)
            {
                CDataJobPlayPoint obj = new CDataJobPlayPoint();
                obj.Job = (JobId)ReadByte(buffer);
                obj.PlayPoint = ReadEntity<CDataPlayPointData>(buffer);
                return obj;
            }
        }
    }
}
