using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestProgressWorkSaveNtc : IPacketStructure
    {
        public S2CQuestQuestProgressWorkSaveNtc()
        {
            WorkList = new List<CDataQuestProgressWork>();
        }

        public PacketId Id => PacketId.S2C_QUEST_PROGRESS_WORK_SAVE_NTC;
        public UInt32 QuestScheduleId { get; set; }
        public UInt16 ProcessNo { get; set; }
        public UInt16 SequenceNo { get; set; }
        public UInt16 BlockNo { get; set; }
        public List<CDataQuestProgressWork> WorkList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestProgressWorkSaveNtc>
        {

            public override void Write(IBuffer buffer, S2CQuestQuestProgressWorkSaveNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
                WriteUInt16(buffer, obj.SequenceNo);
                WriteUInt16(buffer, obj.BlockNo);
                WriteEntityList<CDataQuestProgressWork>(buffer, obj.WorkList);
            }

            public override S2CQuestQuestProgressWorkSaveNtc Read(IBuffer buffer)
            {
                S2CQuestQuestProgressWorkSaveNtc obj = new S2CQuestQuestProgressWorkSaveNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                obj.SequenceNo = ReadUInt16(buffer);
                obj.BlockNo = ReadUInt16(buffer);
                obj.WorkList = ReadEntityList<CDataQuestProgressWork>(buffer);
                return obj;
            }
        }
    }
}
