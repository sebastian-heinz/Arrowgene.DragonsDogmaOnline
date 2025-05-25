using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanSearchResult
    {
        public CDataClanSearchResult()
        {
            Name = "";
            MasterCharacterName = new CDataCharacterName();
        }

        public uint ClanId { get; set; }
        public string Name { get; set; }
        public ushort Level { get; set; }
        public ushort MemberNum { get; set; }
        public uint Motto { get; set; }
        public byte EmblemMarkType { get; set; }
        public byte EmblemBaseType { get; set; }
        public byte EmblemMainColor { get; set; }
        public byte EmblemSubColor { get; set; }
        public uint MasterCharacterId { get; set; }
        public CDataCharacterName MasterCharacterName { get; set; }
        public DateTimeOffset Created { get; set; }

        public class Serializer : EntitySerializer<CDataClanSearchResult>
        {
            public override void Write(IBuffer buffer, CDataClanSearchResult obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteMtString(buffer, obj.Name);
                WriteUInt16(buffer, obj.Level);
                WriteUInt16(buffer, obj.MemberNum);
                WriteUInt32(buffer, obj.Motto);
                WriteByte(buffer, obj.EmblemMarkType);
                WriteByte(buffer, obj.EmblemBaseType);
                WriteByte(buffer, obj.EmblemMainColor);
                WriteByte(buffer, obj.EmblemSubColor);
                WriteUInt32(buffer, obj.MasterCharacterId);
                WriteEntity<CDataCharacterName>(buffer, obj.MasterCharacterName);
                WriteInt64(buffer, obj.Created.ToUnixTimeSeconds());
            }

            public override CDataClanSearchResult Read(IBuffer buffer)
            {
                CDataClanSearchResult obj = new CDataClanSearchResult();
                obj.ClanId = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Level = ReadUInt16(buffer);
                obj.MemberNum = ReadUInt16(buffer);
                obj.Motto = ReadUInt32(buffer);
                obj.EmblemMarkType = ReadByte(buffer);
                obj.EmblemBaseType = ReadByte(buffer);
                obj.EmblemMainColor = ReadByte(buffer);
                obj.EmblemSubColor = ReadByte(buffer);
                obj.MasterCharacterId = ReadUInt32(buffer);
                obj.MasterCharacterName = ReadEntity<CDataCharacterName>(buffer);
                obj.Created = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
