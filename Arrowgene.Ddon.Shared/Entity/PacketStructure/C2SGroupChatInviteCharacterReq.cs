using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGroupChatInviteCharacterReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_REQ;

        public ulong GroupId { get; set; }
        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SGroupChatInviteCharacterReq>
        {
            public override void Write(IBuffer buffer, C2SGroupChatInviteCharacterReq obj)
            {
                WriteUInt64(buffer, obj.GroupId);
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SGroupChatInviteCharacterReq Read(IBuffer buffer)
            {
                C2SGroupChatInviteCharacterReq obj = new C2SGroupChatInviteCharacterReq();
                obj.GroupId = ReadUInt64(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
