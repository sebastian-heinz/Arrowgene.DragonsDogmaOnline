using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobPlayPoint
    {
        public CDataJobPlayPoint()
        {
            Job = 0;
            PlayPoint = new CDataPlayPointData();
        }

        public byte Job;
        public CDataPlayPointData PlayPoint;
    }

    public class CDataJobPlayPointSerializer : EntitySerializer<CDataJobPlayPoint>
    {
        public override void Write(IBuffer buffer, CDataJobPlayPoint obj)
        {
            WriteByte(buffer, obj.Job);
            WriteEntity(buffer, obj.PlayPoint);
        }

        public override CDataJobPlayPoint Read(IBuffer buffer)
        {
            CDataJobPlayPoint obj = new CDataJobPlayPoint();
            obj.Job = ReadByte(buffer);
            obj.PlayPoint = ReadEntity<CDataPlayPointData>(buffer);
            return obj;
        }
    }
}
