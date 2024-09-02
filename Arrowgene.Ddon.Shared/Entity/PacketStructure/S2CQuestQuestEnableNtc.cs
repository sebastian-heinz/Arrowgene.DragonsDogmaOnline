using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestEnableNtc : IPacketStructure
    {
        public PacketId Id => PacketId.UNKNOWN;

        public S2CQuestQuestEnableNtc()
        {
        }

        public UInt32 QuestScheduleId { get; set; }
        public bool IsEnable {  get; set; }


        public class Serializer : PacketEntitySerializer<S2CQuestQuestEnableNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestEnableNtc obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteBool(buffer, obj.IsEnable);
            }

            public override S2CQuestQuestEnableNtc Read(IBuffer buffer)
            {
                S2CQuestQuestEnableNtc obj = new S2CQuestQuestEnableNtc();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.IsEnable = ReadBool(buffer);
                return obj;
            }
        }
    }
}
