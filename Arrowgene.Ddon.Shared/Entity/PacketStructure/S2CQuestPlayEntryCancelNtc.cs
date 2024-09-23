using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayEntryCancelNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_ENTRY_CANCEL_NTC;

        public S2CQuestPlayEntryCancelNtc()
        {
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayEntryCancelNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayEntryCancelNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CQuestPlayEntryCancelNtc Read(IBuffer buffer)
            {
                S2CQuestPlayEntryCancelNtc obj = new S2CQuestPlayEntryCancelNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
