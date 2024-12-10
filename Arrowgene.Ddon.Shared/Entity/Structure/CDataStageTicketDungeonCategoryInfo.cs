using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageTicketDungeonCategoryInfo
    {
        public CDataStageTicketDungeonCategoryInfo()
        {
            DungeonName = string.Empty;
            Unk1Str = string.Empty;
            EntryFeeList = new List<CDataStageDungeonItem>();
        }

        public uint DungeonId { get; set; }
        public string DungeonName { get; set; } // Gets passed to C2S_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_REQ

        public uint Unk0 { get; set; } // Gets passed to C2S_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_REQ UNK0
        public string Unk1Str { get; set; } 
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public bool Unk4 { get; set; }

        public List<CDataStageDungeonItem> EntryFeeList { get; set; } // Is this a reward list instead?

        public class Serializer : EntitySerializer<CDataStageTicketDungeonCategoryInfo>
        {
            public override void Write(IBuffer buffer, CDataStageTicketDungeonCategoryInfo obj)
            {
                WriteUInt32(buffer, obj.DungeonId);
                WriteMtString(buffer, obj.DungeonName);
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1Str);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.EntryFeeList);
            }

            public override CDataStageTicketDungeonCategoryInfo Read(IBuffer buffer)
            {
                CDataStageTicketDungeonCategoryInfo obj = new CDataStageTicketDungeonCategoryInfo();
                obj.DungeonId = ReadUInt32(buffer);
                obj.DungeonName = ReadMtString(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1Str = ReadMtString(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.EntryFeeList = ReadEntityList<CDataStageDungeonItem>(buffer);
                return obj;
            }
        }
    }
}
