using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayEntryNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_ENTRY_NTC;

        public S2CQuestPlayEntryNtc()
        {
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayEntryNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayEntryNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CQuestPlayEntryNtc Read(IBuffer buffer)
            {
                S2CQuestPlayEntryNtc obj = new S2CQuestPlayEntryNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
