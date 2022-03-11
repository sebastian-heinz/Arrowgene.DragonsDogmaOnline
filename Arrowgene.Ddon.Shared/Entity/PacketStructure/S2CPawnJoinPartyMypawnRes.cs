using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnJoinPartyMypawnRes : IPacketStructure
    {



        public PacketId Id => PacketId.S2C_PAWN_JOIN_PARTY_MYPAWN_RES;

        public byte PawnNumber;
        public uint Data1;
        public ushort Data2;

        public class Serializer : PacketEntitySerializer<S2CPawnJoinPartyMypawnRes>
        {

            public override void Write(IBuffer buffer, S2CPawnJoinPartyMypawnRes obj)
            {
                WriteByte(buffer, obj.PawnNumber);
                WriteUInt32(buffer, obj.Data1);
                WriteUInt16(buffer, obj.Data2);
            }

            public override S2CPawnJoinPartyMypawnRes Read(IBuffer buffer)
            {
                S2CPawnJoinPartyMypawnRes obj = new S2CPawnJoinPartyMypawnRes();
                obj.PawnNumber = ReadByte(buffer);
                obj.Data1 = ReadUInt32(buffer);
                obj.Data2 = ReadUInt16(buffer);
                return obj;
            }
        }

    }

}
