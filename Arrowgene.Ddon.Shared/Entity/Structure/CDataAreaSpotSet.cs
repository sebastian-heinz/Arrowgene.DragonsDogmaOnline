using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaSpotSet
    {
        public uint AreaId { get; set; }
        public uint SpotId { get; set; }

        public class Serializer : EntitySerializer<CDataAreaSpotSet>
        {
            public override void Write(IBuffer buffer, CDataAreaSpotSet obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.SpotId);
            }

            public override CDataAreaSpotSet Read(IBuffer buffer)
            {
                CDataAreaSpotSet obj = new CDataAreaSpotSet();
                obj.AreaId = ReadUInt32(buffer);
                obj.SpotId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
