using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsNewsDetail
    {
        public uint QuestId { get; set; }
        public uint BaseLevel { get; set; }
        public ushort ContentJoinItemrank { get; set; }
        public byte SituationLevel { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsNewsDetail>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsNewsDetail obj)
            {
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteUInt16(buffer, obj.ContentJoinItemrank);
                WriteByte(buffer, obj.SituationLevel);
            }

            public override CDataCycleContentsNewsDetail Read(IBuffer buffer)
            {
                CDataCycleContentsNewsDetail obj = new CDataCycleContentsNewsDetail();
                obj.QuestId = ReadUInt32(buffer);
                obj.BaseLevel = ReadUInt32(buffer);
                obj.ContentJoinItemrank = ReadUInt16(buffer);
                obj.SituationLevel = ReadByte(buffer);
                return obj;
            }
        }
    }
}
