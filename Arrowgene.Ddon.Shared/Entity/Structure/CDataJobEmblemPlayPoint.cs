using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemPlayPoint
    {
        public CDataJobEmblemPlayPoint()
        {
            JobPlayPoint = new();
        }

        public CDataJobPlayPoint JobPlayPoint { get; set; }
        public int Unk0 { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemPlayPoint>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemPlayPoint obj)
            {
                WriteEntity(buffer, obj.JobPlayPoint);
                WriteInt32(buffer, obj.Unk0);
            }

            public override CDataJobEmblemPlayPoint Read(IBuffer buffer)
            {
                CDataJobEmblemPlayPoint obj = new CDataJobEmblemPlayPoint();
                obj.JobPlayPoint = ReadEntity<CDataJobPlayPoint>(buffer);
                obj.Unk0 = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
