using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPackageQuestDetail
    {
        public CDataPackageQuestDetail()
        {
            Unk3 = new();
            QuestList = new();
        }

        public uint SeqNo { get; set; }
        public int CurrentProgress { get; set; }
        public int MaxProgress { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; }
        public bool Unk4 { get; set; }
        public bool Unk5 { get; set; }
        public List<CDataQuestList> QuestList { get; set; }

        public class Serializer : EntitySerializer<CDataPackageQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataPackageQuestDetail obj)
            {
                WriteUInt32(buffer, obj.SeqNo);
                WriteInt32(buffer, obj.CurrentProgress);
                WriteInt32(buffer, obj.MaxProgress);
                WriteEntityList(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteBool(buffer, obj.Unk5);
                WriteEntityList(buffer, obj.QuestList);
            }

            public override CDataPackageQuestDetail Read(IBuffer buffer)
            {
                CDataPackageQuestDetail obj = new CDataPackageQuestDetail();
                obj.SeqNo = ReadUInt32(buffer);
                obj.CurrentProgress = ReadInt32(buffer);
                obj.MaxProgress = ReadInt32(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadBool(buffer);
                obj.QuestList = ReadEntityList<CDataQuestList>(buffer);
                return obj;
            }
        }
    }
}
