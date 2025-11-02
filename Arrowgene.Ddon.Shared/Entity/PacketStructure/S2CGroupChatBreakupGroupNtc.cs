using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatBreakupGroupNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_BREAKUP_GROUP_NTC;

        public ulong GroupId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGroupChatBreakupGroupNtc>
        {
            public override void Write(IBuffer buffer, S2CGroupChatBreakupGroupNtc obj)
            {
                WriteUInt64(buffer, obj.GroupId);
            }

            public override S2CGroupChatBreakupGroupNtc Read(IBuffer buffer)
            {
                S2CGroupChatBreakupGroupNtc obj = new S2CGroupChatBreakupGroupNtc();
                obj.GroupId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
