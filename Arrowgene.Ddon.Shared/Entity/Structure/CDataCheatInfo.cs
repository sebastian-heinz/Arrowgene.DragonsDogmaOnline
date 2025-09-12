using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCheatInfo
    {
        public byte Id { get; set; }
        public byte Count { get; set; }
        public uint Param1 { get; set; }
        public uint Param2 { get; set; }
        public uint Param3 { get; set; }   

        public class Serializer : EntitySerializer<CDataCheatInfo>
        {
            public override void Write(IBuffer buffer, CDataCheatInfo obj)
            {
                WriteByte(buffer, obj.Id);
                WriteByte(buffer, obj.Count);
                WriteUInt32(buffer, obj.Param1);
                WriteUInt32(buffer, obj.Param2);
                WriteUInt32(buffer, obj.Param3);
            }

            public override CDataCheatInfo Read(IBuffer buffer)
            {
                CDataCheatInfo obj = new CDataCheatInfo();
                obj.Id = ReadByte(buffer);
                obj.Count = ReadByte(buffer);
                obj.Param1 = ReadUInt32(buffer);
                obj.Param2 = ReadUInt32(buffer);
                obj.Param3 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
