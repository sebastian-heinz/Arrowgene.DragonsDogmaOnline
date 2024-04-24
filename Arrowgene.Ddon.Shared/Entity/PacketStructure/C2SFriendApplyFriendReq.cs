using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SFriendApplyFriendReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_FRIEND_APPLY_FRIEND_REQ;
        
        public UInt32 CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SFriendApplyFriendReq>
        {
            public override void Write(IBuffer buffer, C2SFriendApplyFriendReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SFriendApplyFriendReq Read(IBuffer buffer)
            {
                C2SFriendApplyFriendReq obj = new C2SFriendApplyFriendReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
