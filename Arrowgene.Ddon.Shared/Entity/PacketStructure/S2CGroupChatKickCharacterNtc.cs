using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatKickCharacterNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_NTC;

        public ulong GroupId { get; set; }
        public uint KickerId { get; set; }
        public uint KickedCharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGroupChatKickCharacterNtc>
        {
            public override void Write(IBuffer buffer, S2CGroupChatKickCharacterNtc obj)
            {
                WriteUInt64(buffer, obj.GroupId);
                WriteUInt32(buffer, obj.KickerId);
                WriteUInt32(buffer, obj.KickedCharacterId);
            }

            public override S2CGroupChatKickCharacterNtc Read(IBuffer buffer)
            {
                S2CGroupChatKickCharacterNtc obj = new S2CGroupChatKickCharacterNtc();
                obj.GroupId = ReadUInt64(buffer);
                obj.KickerId = ReadUInt32(buffer);
                obj.KickedCharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
