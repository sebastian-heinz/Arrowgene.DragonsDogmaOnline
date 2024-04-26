using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Xml;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SFriendRegisterFavoriteFriendReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_FRIEND_REGISTER_FAVORITE_FRIEND_REQ;
        
        // u32 m_unFriendNo;
        // b8 m_bIsFavorite;
        
        public UInt32 unFriendNo { get; set; }
        public bool isFavorite { get; set; }

        public class Serializer : PacketEntitySerializer<C2SFriendRegisterFavoriteFriendReq>
        {
            public override void Write(IBuffer buffer, C2SFriendRegisterFavoriteFriendReq obj)
            {
                WriteUInt32(buffer, obj.unFriendNo);
                WriteBool(buffer, obj.isFavorite);
            }

            public override C2SFriendRegisterFavoriteFriendReq Read(IBuffer buffer)
            {
                C2SFriendRegisterFavoriteFriendReq obj = new C2SFriendRegisterFavoriteFriendReq();
                obj.unFriendNo = ReadUInt32(buffer);
                obj.isFavorite = ReadBool(buffer);
                return obj;
            }
        }
    }
}
