using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Xml;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SFriendRemoveFriendReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_FRIEND_REMOVE_FRIEND_REQ;
        
        public UInt32 unFriendNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SFriendRemoveFriendReq>
        {
            public override void Write(IBuffer buffer, C2SFriendRemoveFriendReq obj)
            {
                WriteUInt32(buffer, obj.unFriendNo);
            }

            public override C2SFriendRemoveFriendReq Read(IBuffer buffer)
            {
                C2SFriendRemoveFriendReq obj = new C2SFriendRemoveFriendReq();
                obj.unFriendNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
