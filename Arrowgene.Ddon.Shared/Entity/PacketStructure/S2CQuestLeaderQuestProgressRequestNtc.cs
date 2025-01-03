using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{

    public class S2CQuestLeaderQuestProgressRequestNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_LEADER_QUEST_PROGRESS_REQUEST_NTC;

        public S2CQuestLeaderQuestProgressRequestNtc()
        {
        }

        public uint RequestCharacterId;
        public uint QuestScheduleId;
        public ushort ProcessNo;
        public ushort SequenceNo;
        public ushort BlockNo;

        public class Serializer : PacketEntitySerializer<S2CQuestLeaderQuestProgressRequestNtc>
        {

            public override void Write(IBuffer buffer, S2CQuestLeaderQuestProgressRequestNtc obj)
            {
                WriteUInt32(buffer, obj.RequestCharacterId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
                WriteUInt16(buffer, obj.SequenceNo);
                WriteUInt16(buffer, obj.BlockNo);
            }

            public override S2CQuestLeaderQuestProgressRequestNtc Read(IBuffer buffer)
            {
                S2CQuestLeaderQuestProgressRequestNtc obj = new S2CQuestLeaderQuestProgressRequestNtc();
                obj.RequestCharacterId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                obj.SequenceNo = ReadUInt16(buffer);
                obj.BlockNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
