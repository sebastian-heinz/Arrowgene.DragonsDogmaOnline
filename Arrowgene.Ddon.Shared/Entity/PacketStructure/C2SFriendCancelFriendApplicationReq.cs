using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Xml;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SFriendCancelFriendApplicationReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_FRIEND_CANCEL_FRIEND_APPLICATION_REQ;
        
        public UInt32 CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SFriendCancelFriendApplicationReq>
        {
            public override void Write(IBuffer buffer, C2SFriendCancelFriendApplicationReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SFriendCancelFriendApplicationReq Read(IBuffer buffer)
            {
                C2SFriendCancelFriendApplicationReq obj = new C2SFriendCancelFriendApplicationReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
