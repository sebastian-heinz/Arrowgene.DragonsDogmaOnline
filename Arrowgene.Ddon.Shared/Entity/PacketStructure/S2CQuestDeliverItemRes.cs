using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestDeliverItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_DELIVER_ITEM_RES;

        public S2CQuestDeliverItemRes()
        {
        }

        public UInt32 QuestScheduleId {  get; set; }
        public UInt16 ProcessNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestDeliverItemRes>
        {
            public override void Write(IBuffer buffer, S2CQuestDeliverItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
            }

            public override S2CQuestDeliverItemRes Read(IBuffer buffer)
            {
                S2CQuestDeliverItemRes obj = new S2CQuestDeliverItemRes();
                ReadServerResponse(buffer, obj);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
