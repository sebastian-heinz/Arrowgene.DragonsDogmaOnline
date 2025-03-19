using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataHistoryElement
    {
        /// <summary>
        /// For Arisen Profiles, this is always 1?
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// For Arisen Profiles, this points at an Achievement ID.
        /// </summary>
        public uint TargetID { get; set; }
        public uint Param { get; set; }
        public DateTimeOffset DateTime { get; set; }
    
        public class Serializer : EntitySerializer<CDataHistoryElement>
        {
            public override void Write(IBuffer buffer, CDataHistoryElement obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.TargetID);
                WriteUInt32(buffer, obj.Param);
                WriteInt64(buffer, obj.DateTime.ToUnixTimeSeconds());
            }
        
            public override CDataHistoryElement Read(IBuffer buffer)
            {
                CDataHistoryElement obj = new CDataHistoryElement();
                obj.Type = ReadByte(buffer);
                obj.TargetID = ReadUInt32(buffer);
                obj.Param = ReadUInt32(buffer);
                obj.DateTime = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
