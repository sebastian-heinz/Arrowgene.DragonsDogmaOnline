using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetSetQuestListNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_GET_SET_QUEST_LIST_NTC;

        public S2CQuestGetSetQuestListNtc()
        {
            SelectCharacterId = 0;
            DistributeId = 0;
            SetQuestList = new List<CDataSetQuestList>();
        }

        public uint SelectCharacterId { get; set; }
        public QuestAreaId DistributeId {  get; set; }
        public List<CDataSetQuestList> SetQuestList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetSetQuestListNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestGetSetQuestListNtc obj)
            {
                WriteUInt32(buffer, obj.SelectCharacterId);
                WriteUInt32(buffer, (uint)obj.DistributeId);
                WriteEntityList<CDataSetQuestList>(buffer, obj.SetQuestList);
            }

            public override S2CQuestGetSetQuestListNtc Read(IBuffer buffer)
            {
                S2CQuestGetSetQuestListNtc obj = new S2CQuestGetSetQuestListNtc();
                obj.SelectCharacterId = ReadUInt32(buffer);
                obj.DistributeId = (QuestAreaId)ReadUInt32(buffer);
                obj.SetQuestList = ReadEntityList<CDataSetQuestList>(buffer);
                return obj;
            }
        }
    }
}
