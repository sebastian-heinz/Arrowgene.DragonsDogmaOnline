using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestCancelSilentNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_CANCEL_SILENT_NTC;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestCancelSilentNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestCancelSilentNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestQuestCancelSilentNtc Read(IBuffer buffer)
            {
                S2CQuestQuestCancelSilentNtc obj = new S2CQuestQuestCancelSilentNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
