using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestListUnk0
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestListUnk0>
        {
            public override void Write(IBuffer buffer, CDataQuestListUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }
        
            public override CDataQuestListUnk0 Read(IBuffer buffer)
            {
                CDataQuestListUnk0 obj = new CDataQuestListUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}