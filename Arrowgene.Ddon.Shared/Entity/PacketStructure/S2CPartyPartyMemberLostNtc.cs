using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyMemberLostNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_MEMBER_LOST_NTC;

        public byte MemberIndex { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyMemberLostNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyMemberLostNtc obj)
            {
                WriteByte(buffer, obj.MemberIndex);
            }

            public override S2CPartyPartyMemberLostNtc Read(IBuffer buffer)
            {
                S2CPartyPartyMemberLostNtc obj = new S2CPartyPartyMemberLostNtc();
                obj.MemberIndex = ReadByte(buffer);
                return obj;
            }
        }
    }
}
