using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanApproveJoinReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_APPROVE_JOIN_REQ;

        public UInt32 CharacterId { get; set; }
        public bool IsApproved { get; set; }

        public class Serializer : PacketEntitySerializer<C2SClanClanApproveJoinReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanApproveJoinReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteBool(buffer, obj.IsApproved);
            }

            public override C2SClanClanApproveJoinReq Read(IBuffer buffer)
            {
                C2SClanClanApproveJoinReq obj = new();
                obj.CharacterId = ReadUInt32(buffer);
                obj.IsApproved = ReadBool(buffer);
                return obj;
            }
        }
    }
}
