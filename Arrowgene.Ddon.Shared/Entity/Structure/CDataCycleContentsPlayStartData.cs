using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsPlayStartData
    {
        public CDataCycleContentsPlayStartData()
        {
            QuestProcessStateList = new List<CDataQuestProcessState>();
            QuestEnemyInfoList = new List<CDataQuestEnemyInfo>();
            QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>();
            Unk0List = new List<CDataCycleContentsUnk1>(); // Always two entries?
        }

        public uint CycleContentsScheduleId { get; set; }
        public uint KeyId { get; set; }
        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
        public uint BaseLevel { get; set; }
        public byte StartPos { get; set; }
        public List<CDataQuestProcessState> QuestProcessStateList { get; set; }
        public List<CDataQuestEnemyInfo> QuestEnemyInfoList { get; set; }
        public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList { get; set; }
        public List<CDataCycleContentsUnk1> Unk0List { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsPlayStartData>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsPlayStartData obj)
            {
                WriteUInt32(buffer, obj.CycleContentsScheduleId);
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteByte(buffer, obj.StartPos);
                WriteEntityList(buffer, obj.QuestProcessStateList);
                WriteEntityList(buffer, obj.QuestEnemyInfoList);
                WriteEntityList(buffer, obj.QuestLayoutFlagSetInfoList);
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override CDataCycleContentsPlayStartData Read(IBuffer buffer)
            {
                CDataCycleContentsPlayStartData obj = new CDataCycleContentsPlayStartData();
                obj.CycleContentsScheduleId = ReadUInt32(buffer);
                obj.KeyId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.BaseLevel = ReadUInt32(buffer);
                obj.StartPos = ReadByte(buffer);
                obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
                obj.QuestEnemyInfoList = ReadEntityList<CDataQuestEnemyInfo>(buffer);
                obj.QuestLayoutFlagSetInfoList = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
                obj.Unk0List = ReadEntityList<CDataCycleContentsUnk1>(buffer);
                return obj;
            }
        }
    }
}
