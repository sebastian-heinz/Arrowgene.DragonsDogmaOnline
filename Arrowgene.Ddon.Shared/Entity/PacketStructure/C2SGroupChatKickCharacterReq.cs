using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGroupChatKickCharacterReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_REQ;

        public ulong GroupId { get; set; }
        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SGroupChatKickCharacterReq>
        {
            public override void Write(IBuffer buffer, C2SGroupChatKickCharacterReq obj)
            {
                WriteUInt64(buffer, obj.GroupId);
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SGroupChatKickCharacterReq Read(IBuffer buffer)
            {
                C2SGroupChatKickCharacterReq obj = new C2SGroupChatKickCharacterReq();
                obj.GroupId = ReadUInt64(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
