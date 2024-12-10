using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContentsPlayStartData
    {
        public CDataContentsPlayStartData()
        {
            QuestProcessStateList = new List<CDataQuestProcessState>();
            QuestEnemyInfoList = new List<CDataQuestEnemyInfo>();
            QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>();
            QuestPhaseGroupIdList = new List<CDataCommonU32>();
            Unk3List = new List<CDataCommonU8>();
        }

        public uint KeyId { get; set; }
        public uint QuestScheudleId {  get; set; }
        public uint QuestId { get; set; }
        public uint BaseLevel { get; set; }
        public byte StartPos {  get; set; }
        public bool Unk0 {  get; set; }
        public bool Unk1 {  get; set; }
        public bool Unk2 {  get; set; }
        public List<CDataQuestProcessState> QuestProcessStateList {  get; set; }
        public List<CDataQuestEnemyInfo> QuestEnemyInfoList { get; set; }
        public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList {  get; set; }
        public List<CDataCommonU32> QuestPhaseGroupIdList { get; set; }
        public List<CDataCommonU8> Unk3List {  get; set; }

        public class Serializer : EntitySerializer<CDataContentsPlayStartData>
        {
            public override void Write(IBuffer buffer, CDataContentsPlayStartData obj)
            {
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheudleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteByte(buffer, obj.StartPos);
                WriteBool(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
                WriteBool(buffer, obj.Unk2);
                WriteEntityList(buffer, obj.QuestProcessStateList);
                WriteEntityList(buffer, obj.QuestEnemyInfoList);
                WriteEntityList(buffer, obj.QuestLayoutFlagSetInfoList);
                WriteEntityList(buffer, obj.QuestPhaseGroupIdList);
                WriteEntityList(buffer, obj.Unk3List);
            }

            public override CDataContentsPlayStartData Read(IBuffer buffer)
            {
                CDataContentsPlayStartData obj = new CDataContentsPlayStartData();
                obj.KeyId = ReadUInt32(buffer);
                obj.QuestScheudleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.BaseLevel = ReadUInt32(buffer);
                obj.StartPos = ReadByte(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.Unk2 = ReadBool(buffer);
                obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
                obj.QuestEnemyInfoList = ReadEntityList<CDataQuestEnemyInfo>(buffer);
                obj.QuestLayoutFlagSetInfoList = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
                obj.QuestPhaseGroupIdList = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk3List = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }
    }
}
