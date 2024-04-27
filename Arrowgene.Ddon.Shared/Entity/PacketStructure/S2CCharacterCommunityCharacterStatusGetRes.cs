using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCommunityCharacterStatusGetRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_RES;
        public Int32 Result { get; set; }


        public class Serializer : PacketEntitySerializer<S2CCharacterCommunityCharacterStatusGetRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterCommunityCharacterStatusGetRes obj)
            {
                WriteInt32(buffer, obj.Result);
                
            }

            public override S2CCharacterCommunityCharacterStatusGetRes Read(IBuffer buffer)
            {
                S2CCharacterCommunityCharacterStatusGetRes obj = new S2CCharacterCommunityCharacterStatusGetRes();
                obj.Result = ReadInt32(buffer);
                return obj;
            }
            
        }
    }
}

