using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayInterruptRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_PLAY_INTERRUPT_RES;

        public S2CQuestPlayInterruptRes()
        {
        }
        public byte DeadlineSec { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayInterruptRes>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayInterruptRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.DeadlineSec);
            }

            public override S2CQuestPlayInterruptRes Read(IBuffer buffer)
            {
                S2CQuestPlayInterruptRes obj = new S2CQuestPlayInterruptRes();
                ReadServerResponse(buffer, obj);
                obj.DeadlineSec = ReadByte(buffer);
                return obj;
            }
        }
    }
}
