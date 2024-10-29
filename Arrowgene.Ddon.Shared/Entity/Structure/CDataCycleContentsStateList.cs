using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsStateList
    {
        public uint CycleContentsScheduleId { get; set; }
        public byte Category { get; set; }
        public uint CategoryType { get; set; }
        public byte State { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsStateList>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsStateList obj)
            {
                WriteUInt32(buffer, obj.CycleContentsScheduleId);
                WriteByte(buffer, obj.Category);
                WriteUInt32(buffer, obj.CategoryType);
                WriteByte(buffer, obj.State);
            }

            public override CDataCycleContentsStateList Read(IBuffer buffer)
            {
                CDataCycleContentsStateList obj = new CDataCycleContentsStateList();
                obj.CycleContentsScheduleId = ReadUInt32(buffer);
                obj.Category = ReadByte(buffer);
                obj.CategoryType = ReadUInt32(buffer);
                obj.State = ReadByte(buffer);
                return obj;
            }
        }
    }
}
