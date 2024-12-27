using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaSpotSet
    {
        public QuestAreaId AreaId { get; set; }
        public uint SpotId { get; set; }

        public class Serializer : EntitySerializer<CDataAreaSpotSet>
        {
            public override void Write(IBuffer buffer, CDataAreaSpotSet obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaId);
                WriteUInt32(buffer, obj.SpotId);
            }

            public override CDataAreaSpotSet Read(IBuffer buffer)
            {
                CDataAreaSpotSet obj = new CDataAreaSpotSet();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);
                obj.SpotId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
