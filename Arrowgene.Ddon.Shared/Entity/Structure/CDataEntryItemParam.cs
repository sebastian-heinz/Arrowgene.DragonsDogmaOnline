using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryItemParam
    {
        public CDataEntryItemParam()
        {
            Comment = string.Empty;
            EntryRecruitList = new List<CDataEntryRecruitData>();
        }

        public bool PasswordOn { get; set; }
        public bool PawnOn { get; set; }

        public int BottomEntryJobLevel { get; set; } // Minimum Level?
        public int TopEntryJobLevel { get; set; } // Maximum Level?
        public ushort RequiredItemRank { get; set; }

        public byte ItemRankType {  get; set; }

        public uint ItemRankCheckRoleType { get; set; }
        public ushort MinEntryNum { get; set; }
        public ushort MaxEntryNum { get; set; }
        public string Comment {  get; set; }
        public List<CDataEntryRecruitData> EntryRecruitList {  get; set; }

        public class Serializer : EntitySerializer<CDataEntryItemParam>
        {
            public override void Write(IBuffer buffer, CDataEntryItemParam obj)
            {
                WriteBool(buffer, obj.PasswordOn);
                WriteBool(buffer, obj.PawnOn);
                WriteInt32(buffer, obj.BottomEntryJobLevel);
                WriteInt32(buffer, obj.TopEntryJobLevel);
                WriteUInt16(buffer, obj.RequiredItemRank);
                WriteByte(buffer, obj.ItemRankType);
                WriteUInt32(buffer, obj.ItemRankCheckRoleType);
                WriteUInt16(buffer, obj.MinEntryNum);
                WriteUInt16(buffer, obj.MaxEntryNum);
                WriteMtString(buffer, obj.Comment);
                WriteEntityList(buffer, obj.EntryRecruitList);
            }

            public override CDataEntryItemParam Read(IBuffer buffer)
            {
                CDataEntryItemParam obj = new CDataEntryItemParam();
                obj.PasswordOn = ReadBool(buffer);
                obj.PawnOn = ReadBool(buffer);
                obj.BottomEntryJobLevel = ReadInt32(buffer);
                obj.TopEntryJobLevel = ReadInt32(buffer);
                obj.RequiredItemRank = ReadUInt16(buffer);
                obj.ItemRankType = ReadByte(buffer);
                obj.ItemRankCheckRoleType = ReadUInt32(buffer);
                obj.MinEntryNum = ReadUInt16(buffer);
                obj.MaxEntryNum = ReadUInt16(buffer);
                obj.Comment = ReadMtString(buffer);
                obj.EntryRecruitList = ReadEntityList<CDataEntryRecruitData>(buffer);
                return obj;
            }
        }
    }
}
