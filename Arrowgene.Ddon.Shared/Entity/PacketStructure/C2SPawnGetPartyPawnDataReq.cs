using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetPartyPawnDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_PARTY_PAWN_DATA_REQ;

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public bool IsPawnProfile { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnGetPartyPawnDataReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetPartyPawnDataReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteBool(buffer, obj.IsPawnProfile);
            }

            public override C2SPawnGetPartyPawnDataReq Read(IBuffer buffer)
            {
                C2SPawnGetPartyPawnDataReq obj = new C2SPawnGetPartyPawnDataReq();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.IsPawnProfile = ReadBool(buffer);
                return obj;
            }
        }

    }
}