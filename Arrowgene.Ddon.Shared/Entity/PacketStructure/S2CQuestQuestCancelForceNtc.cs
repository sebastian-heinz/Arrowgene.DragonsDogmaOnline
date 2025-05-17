using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    internal class S2CQuestQuestCancelForceNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_CANCEL_FORCE_NTC;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestCancelForceNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestCancelForceNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override S2CQuestQuestCancelForceNtc Read(IBuffer buffer)
            {
                S2CQuestQuestCancelForceNtc obj = new S2CQuestQuestCancelForceNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
