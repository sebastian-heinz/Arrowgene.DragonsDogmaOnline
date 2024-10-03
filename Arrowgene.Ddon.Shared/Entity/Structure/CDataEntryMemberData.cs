using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryMemberData
    {
        public CDataEntryMemberData()
        {
            CharacterListElement = new CDataCharacterListElement();
        }

        public ushort Id { get; set; }
        public bool EntryFlag { get; set; }
        public uint Unk0 { get; set; }
        public CDataCharacterListElement CharacterListElement { get; set; }

        public class Serializer : EntitySerializer<CDataEntryMemberData>
        {
            public override void Write(IBuffer buffer, CDataEntryMemberData obj)
            {
                WriteUInt16(buffer, obj.Id);
                WriteBool(buffer, obj.EntryFlag);
                WriteUInt32(buffer, obj.Unk0);
                WriteEntity(buffer, obj.CharacterListElement);
            }

            public override CDataEntryMemberData Read(IBuffer buffer)
            {
                CDataEntryMemberData obj = new CDataEntryMemberData();
                obj.Id = ReadUInt16(buffer);
                obj.EntryFlag = ReadBool(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                return obj;
            }
        }
    }
}
