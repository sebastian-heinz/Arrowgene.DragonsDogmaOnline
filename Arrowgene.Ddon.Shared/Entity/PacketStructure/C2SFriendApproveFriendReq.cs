using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Xml;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SFriendApproveFriendReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_FRIEND_APPROVE_FRIEND_REQ;
        
        public UInt32 CharacterId { get; set; }
        public bool IsApproved { get; set; }

        public class Serializer : PacketEntitySerializer<C2SFriendApproveFriendReq>
        {
            public override void Write(IBuffer buffer, C2SFriendApproveFriendReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteBool(buffer, obj.IsApproved);
            }

            public override C2SFriendApproveFriendReq Read(IBuffer buffer)
            {
                C2SFriendApproveFriendReq obj = new C2SFriendApproveFriendReq();
                obj.CharacterId = ReadUInt32(buffer);
                obj.IsApproved = ReadBool(buffer);
                return obj;
            }
        }
    }
}
