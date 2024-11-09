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
        
        public uint FriendNo { get; set; }
        public bool IsFavorite { get; set; }

        public class Serializer : PacketEntitySerializer<C2SFriendRegisterFavoriteFriendReq>
        {
            public override void Write(IBuffer buffer, C2SFriendRegisterFavoriteFriendReq obj)
            {
                WriteUInt32(buffer, obj.FriendNo);
                WriteBool(buffer, obj.IsFavorite);
            }

            public override C2SFriendRegisterFavoriteFriendReq Read(IBuffer buffer)
            {
                C2SFriendRegisterFavoriteFriendReq obj = new C2SFriendRegisterFavoriteFriendReq();
                obj.FriendNo = ReadUInt32(buffer);
                obj.IsFavorite = ReadBool(buffer);
                return obj;
            }
        }
    }
}
