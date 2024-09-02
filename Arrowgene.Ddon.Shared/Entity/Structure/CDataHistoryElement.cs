using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataHistoryElement
    {
        public byte Type { get; set; }
        public uint TargetID { get; set; }
        public uint Param { get; set; }
        public ulong DateTime { get; set; }
    
        public class Serializer : EntitySerializer<CDataHistoryElement>
        {
            public override void Write(IBuffer buffer, CDataHistoryElement obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.TargetID);
                WriteUInt32(buffer, obj.Param);
                WriteUInt64(buffer, obj.DateTime);
            }
        
            public override CDataHistoryElement Read(IBuffer buffer)
            {
                CDataHistoryElement obj = new CDataHistoryElement();
                obj.Type = ReadByte(buffer);
                obj.TargetID = ReadUInt32(buffer);
                obj.Param = ReadUInt32(buffer);
                obj.DateTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}