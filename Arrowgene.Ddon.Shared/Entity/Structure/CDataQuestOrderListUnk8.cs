using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestOrderListUnk8
    {    
        public uint Unk0 { get; set; }
        public ulong Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestOrderListUnk8>
        {
            public override void Write(IBuffer buffer, CDataQuestOrderListUnk8 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt64(buffer, obj.Unk1);
            }
        
            public override CDataQuestOrderListUnk8 Read(IBuffer buffer)
            {
                CDataQuestOrderListUnk8 obj = new CDataQuestOrderListUnk8();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}