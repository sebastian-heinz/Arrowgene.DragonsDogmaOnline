using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestOrderConditionParam
    {
        public uint Type { get; set; }
        public int Param01 { get; set; }
        public int Param02 { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestOrderConditionParam>
        {
            public override void Write(IBuffer buffer, CDataQuestOrderConditionParam obj)
            {
                WriteUInt32(buffer, obj.Type);
                WriteInt32(buffer, obj.Param01);
                WriteInt32(buffer, obj.Param02);
            }
        
            public override CDataQuestOrderConditionParam Read(IBuffer buffer)
            {
                CDataQuestOrderConditionParam obj = new CDataQuestOrderConditionParam();
                obj.Type = ReadUInt32(buffer);
                obj.Param01 = ReadInt32(buffer);
                obj.Param02 = ReadInt32(buffer);
                return obj;
            }
        }
    }
}