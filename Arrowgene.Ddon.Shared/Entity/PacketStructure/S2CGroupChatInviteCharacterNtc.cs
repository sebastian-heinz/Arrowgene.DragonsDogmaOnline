using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatInviteCharacterNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_NTC;

        public ulong GroupId { get; set; }
        public CDataCommunityCharacterBaseInfo InviterInfo { get; set; } = new();
        public CDataCharacterListElement VisitorInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CGroupChatInviteCharacterNtc>
        {
            public override void Write(IBuffer buffer, S2CGroupChatInviteCharacterNtc obj)
            {
                WriteUInt64(buffer, obj.GroupId);
                WriteEntity(buffer, obj.InviterInfo);
                WriteEntity(buffer, obj.VisitorInfo);
            }

            public override S2CGroupChatInviteCharacterNtc Read(IBuffer buffer)
            {
                S2CGroupChatInviteCharacterNtc obj = new S2CGroupChatInviteCharacterNtc();
                obj.GroupId = ReadUInt64(buffer);
                obj.InviterInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.VisitorInfo = ReadEntity<CDataCharacterListElement>(buffer);
                return obj;
            }
        }
    }
}
