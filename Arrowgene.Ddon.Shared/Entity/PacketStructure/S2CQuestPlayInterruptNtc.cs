using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayInterruptNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_INTERRUPT_NTC;

        public S2CQuestPlayInterruptNtc()
        {
        }

        public uint CharacterId { get; set; }
        public byte DeadlineSec { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayInterruptNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayInterruptNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.DeadlineSec);
            }

            public override S2CQuestPlayInterruptNtc Read(IBuffer buffer)
            {
                S2CQuestPlayInterruptNtc obj = new S2CQuestPlayInterruptNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.DeadlineSec = ReadByte(buffer);
                return obj;
            }
        }
    }
}
