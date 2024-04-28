using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Xml;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCommunityCharacterStatusGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_REQ;
        
        public UInt32 unType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterCommunityCharacterStatusGetReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterCommunityCharacterStatusGetReq obj)
            {
                WriteUInt32(buffer, obj.unType);
            }

            public override C2SCharacterCommunityCharacterStatusGetReq Read(IBuffer buffer)
            {
                C2SCharacterCommunityCharacterStatusGetReq obj = new C2SCharacterCommunityCharacterStatusGetReq();
                obj.unType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
