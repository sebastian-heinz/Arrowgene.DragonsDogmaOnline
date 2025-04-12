using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnJoinPartyMyPawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_JOIN_PARTY_MYPAWN_REQ;
        
        public byte PawnNumber { get; set; }
        public uint Data1 { get; set; }
        public ushort Data2 { get; set; }
        
        public C2SPawnJoinPartyMyPawnReq()
        {
            PawnNumber = 0;
            Data1 = 0;
            Data2 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SPawnJoinPartyMyPawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnJoinPartyMyPawnReq obj)
            {
                WriteByte(buffer, obj.PawnNumber);
                WriteUInt32(buffer, obj.Data1);
                WriteUInt16(buffer, obj.Data2);
            }

            public override C2SPawnJoinPartyMyPawnReq Read(IBuffer buffer)
            {
                C2SPawnJoinPartyMyPawnReq obj = new C2SPawnJoinPartyMyPawnReq();
                obj.PawnNumber = ReadByte(buffer);
                obj.Data1 = ReadUInt32(buffer);
                obj.Data2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
