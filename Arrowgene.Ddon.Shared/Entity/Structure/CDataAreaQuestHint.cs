using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaQuestHint
    {
        public uint ScheduleId { get; set; }
        public uint Price { get; set; }
        public bool IsSold { get; set; }

        public class Serializer : EntitySerializer<CDataAreaQuestHint>
        {
            public override void Write(IBuffer buffer, CDataAreaQuestHint obj)
            {
                WriteUInt32(buffer, obj.ScheduleId);
                WriteUInt32(buffer, obj.Price);
                WriteBool(buffer, obj.IsSold);
            }

            public override CDataAreaQuestHint Read(IBuffer buffer)
            {
                CDataAreaQuestHint obj = new CDataAreaQuestHint();
                obj.ScheduleId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.IsSold = ReadBool(buffer);
                return obj;
            }
        }
    }
}
