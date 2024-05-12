using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendCancelFriendApplicationNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_FRIEND_CANCEL_FRIEND_APPLICATION_NTC;
        public UInt32 CharacterId { get; set; }


        public class Serializer : PacketEntitySerializer<S2CFriendCancelFriendApplicationNtc>
        {
            public override void Write(IBuffer buffer, S2CFriendCancelFriendApplicationNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                
            }

            public override S2CFriendCancelFriendApplicationNtc Read(IBuffer buffer)
            {
                S2CFriendCancelFriendApplicationNtc obj = new S2CFriendCancelFriendApplicationNtc
                {
                    CharacterId = ReadUInt32(buffer)
                };
                return obj;
            }
            
        }
    }
}

