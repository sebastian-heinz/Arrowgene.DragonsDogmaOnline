using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanQuestClearNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_QUEST_CLEAR_NTC;

        public uint QuestId;

        public class Serializer : PacketEntitySerializer<S2CClanClanQuestClearNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanQuestClearNtc obj)
            {
                WriteUInt32(buffer, obj.QuestId);
            }

            public override S2CClanClanQuestClearNtc Read(IBuffer buffer)
            {
                S2CClanClanQuestClearNtc obj = new S2CClanClanQuestClearNtc();

                obj.QuestId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
