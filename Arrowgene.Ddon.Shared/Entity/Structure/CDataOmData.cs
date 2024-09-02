using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataOmData
    {
        public CDataOmData()
        {
            Key = 0;
            Value = 0;
        }

        public CDataOmData(UInt64 key, UInt32 value)
        {
            Key = key;
            Value = value;
        }

        public UInt64 Key { get; set; }
        public UInt32 Value { get; set; }

        public class Serializer : EntitySerializer<CDataOmData>
        {
            public override void Write(IBuffer buffer, CDataOmData obj)
            {
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
            }
            public override CDataOmData Read(IBuffer buffer)
            {
                CDataOmData obj = new CDataOmData();
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
