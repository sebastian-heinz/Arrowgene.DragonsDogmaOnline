using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendRemoveFriendNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_FRIEND_REMOVE_FRIEND_NTC;
        public UInt32 CharacterId { get; set; }


        public class Serializer : PacketEntitySerializer<S2CFriendRemoveFriendNtc>
        {
            public override void Write(IBuffer buffer, S2CFriendRemoveFriendNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                
            }

            public override S2CFriendRemoveFriendNtc Read(IBuffer buffer)
            {
                S2CFriendRemoveFriendNtc obj = new S2CFriendRemoveFriendNtc();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
            
        }
    }
}

