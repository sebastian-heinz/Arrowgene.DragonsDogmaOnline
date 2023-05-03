using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestListUnk1
    {
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestListUnk1>
        {
            public override void Write(IBuffer buffer, CDataQuestListUnk1 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
            }
        
            public override CDataQuestListUnk1 Read(IBuffer buffer)
            {
                CDataQuestListUnk1 obj = new CDataQuestListUnk1();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}