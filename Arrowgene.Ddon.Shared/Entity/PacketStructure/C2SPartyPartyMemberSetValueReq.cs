using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyMemberSetValueReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_MEMBER_SET_VALUE_REQ;

        public byte Index { get; set; }
        public byte Value {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartyPartyMemberSetValueReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyMemberSetValueReq obj)
            {
                WriteByte(buffer, obj.Index);
                WriteByte(buffer, obj.Value);
            }

            public override C2SPartyPartyMemberSetValueReq Read(IBuffer buffer)
            {
                C2SPartyPartyMemberSetValueReq obj = new C2SPartyPartyMemberSetValueReq();
                obj.Index = ReadByte(buffer);
                obj.Value = ReadByte(buffer);
                return obj;
            }
        }
    }
}

