using Arrowgene.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryItem
    {
        public CDataEntryItem()
        {
            Param = new CDataEntryItemParam();
            EntryMemberList = new List<CDataEntryMemberData>();
        }

        public uint Id { get; set; }
        public CDataEntryItemParam Param { get; set; }
        public List<CDataEntryMemberData> EntryMemberList {  get; set; }
        public ushort BoardRequiredAvgItemRank { get; set; }
        public ushort TimeOut {  get; set; }
        public uint PartyLeaderCharacterId { get; set; }
        public bool Unk0 {  get; set; }

        public class Serializer : EntitySerializer<CDataEntryItem>
        {
            public override void Write(IBuffer buffer, CDataEntryItem obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteEntity(buffer, obj.Param);
                WriteEntityList(buffer, obj.EntryMemberList);
                WriteUInt16(buffer, obj.BoardRequiredAvgItemRank);
                WriteUInt16(buffer, obj.TimeOut);
                WriteUInt32(buffer, obj.PartyLeaderCharacterId);
                WriteBool(buffer, obj.Unk0);
            }

            public override CDataEntryItem Read(IBuffer buffer)
            {
                CDataEntryItem obj = new CDataEntryItem();
                obj.Id = ReadUInt32(buffer);
                obj.Param = ReadEntity<CDataEntryItemParam>(buffer);
                obj.EntryMemberList = ReadEntityList<CDataEntryMemberData>(buffer);
                obj.BoardRequiredAvgItemRank = ReadUInt16(buffer);
                obj.TimeOut = ReadUInt16(buffer);
                obj.PartyLeaderCharacterId = ReadUInt32(buffer);
                obj.Unk0 = ReadBool(buffer);
                return obj;
            }
        }
    }
}

