using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanUserParam
    {
        public CDataClanUserParam() {
            Name = string.Empty;
            ShortName = string.Empty;
            Comment = string.Empty;
            BoardMessage = string.Empty;
            Created = DateTimeOffset.MinValue;
        }
    
        public string Name { get; set; }
        public string ShortName { get; set; }
        public byte EmblemMarkType { get; set; }
        public byte EmblemBaseType { get; set; }
        public byte EmblemBaseMainColor { get; set; }
        public byte EmblemBaseSubColor { get; set; }
        public uint Motto { get; set; }
        public uint ActiveDays { get; set; }
        public uint ActiveTime { get; set; }
        public uint Characteristic { get; set; }
        public bool IsPublish { get; set; }
        public string Comment { get; set; }
        public string BoardMessage { get; set; }
        public DateTimeOffset Created { get; set; }
    
        public class Serializer : EntitySerializer<CDataClanUserParam>
        {
            public override void Write(IBuffer buffer, CDataClanUserParam obj)
            {
                WriteMtString(buffer, obj.Name);
                WriteMtString(buffer, obj.ShortName);
                WriteByte(buffer, obj.EmblemMarkType);
                WriteByte(buffer, obj.EmblemBaseType);
                WriteByte(buffer, obj.EmblemBaseMainColor);
                WriteByte(buffer, obj.EmblemBaseSubColor);
                WriteUInt32(buffer, obj.Motto);
                WriteUInt32(buffer, obj.ActiveDays);
                WriteUInt32(buffer, obj.ActiveTime);
                WriteUInt32(buffer, obj.Characteristic);
                WriteBool(buffer, obj.IsPublish);
                WriteMtString(buffer, obj.Comment);
                WriteMtString(buffer, obj.BoardMessage);
                WriteInt64(buffer, obj.Created.ToUnixTimeSeconds());
            }
        
            public override CDataClanUserParam Read(IBuffer buffer)
            {
                CDataClanUserParam obj = new CDataClanUserParam();
                obj.Name = ReadMtString(buffer);
                obj.ShortName = ReadMtString(buffer);
                obj.EmblemMarkType = ReadByte(buffer);
                obj.EmblemBaseType = ReadByte(buffer);
                obj.EmblemBaseMainColor = ReadByte(buffer);
                obj.EmblemBaseSubColor = ReadByte(buffer);
                obj.Motto = ReadUInt32(buffer);
                obj.ActiveDays = ReadUInt32(buffer);
                obj.ActiveTime = ReadUInt32(buffer);
                obj.Characteristic = ReadUInt32(buffer);
                obj.IsPublish = ReadBool(buffer);
                obj.Comment = ReadMtString(buffer);
                obj.BoardMessage = ReadMtString(buffer);
                obj.Created = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
