using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatLeaveCharacterNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_NTC;

        public ulong GroupId { get; set; }
        public uint LeaveCharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGroupChatLeaveCharacterNtc>
        {
            public override void Write(IBuffer buffer, S2CGroupChatLeaveCharacterNtc obj)
            {
                WriteUInt64(buffer, obj.GroupId);
                WriteUInt32(buffer, obj.LeaveCharacterId);
            }

            public override S2CGroupChatLeaveCharacterNtc Read(IBuffer buffer)
            {
                S2CGroupChatLeaveCharacterNtc obj = new S2CGroupChatLeaveCharacterNtc();
                obj.GroupId = ReadUInt64(buffer);
                obj.LeaveCharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
