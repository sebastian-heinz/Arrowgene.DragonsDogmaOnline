using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanJoinRequest : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_REGISTER_JOIN_REQ;

        public UInt32 ClanId { get; set; }

        public C2SClanClanJoinRequest()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanJoinRequest>
        {
            public override void Write(IBuffer buffer, C2SClanClanJoinRequest obj)
            {
                WriteUInt32(buffer, obj.ClanId);
            }

            public override C2SClanClanJoinRequest Read(IBuffer buffer)
            {
                C2SClanClanJoinRequest obj = new C2SClanClanJoinRequest();
                obj.ClanId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
