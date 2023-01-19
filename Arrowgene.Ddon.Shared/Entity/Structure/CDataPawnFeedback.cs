using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnFeedback
    {
        public byte Type { get; set; }
        public byte Value { get; set; }
        public byte CommentNo { get; set; }

        public class Serializer : EntitySerializer<CDataPawnFeedback>
        {
            public override void Write(IBuffer buffer, CDataPawnFeedback obj)
            {
                WriteByte(buffer, obj.Type);
                WriteByte(buffer, obj.Value);
                WriteByte(buffer, obj.CommentNo);
            }
        
            public override CDataPawnFeedback Read(IBuffer buffer)
            {
                CDataPawnFeedback obj = new CDataPawnFeedback();
                obj.Type = ReadByte(buffer);
                obj.Value = ReadByte(buffer);
                obj.CommentNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}