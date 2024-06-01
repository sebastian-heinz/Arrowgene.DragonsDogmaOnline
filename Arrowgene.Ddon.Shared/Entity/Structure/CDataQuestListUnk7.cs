using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestListUnk7
    {
        public uint Unk0 { get; set; }

        public class Serializer : EntitySerializer<CDataQuestListUnk7>
        {
            public override void Write(IBuffer buffer, CDataQuestListUnk7 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override CDataQuestListUnk7 Read(IBuffer buffer)
            {
                CDataQuestListUnk7 obj = new CDataQuestListUnk7();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
