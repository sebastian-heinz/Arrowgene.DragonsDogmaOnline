using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterPawnGoldenReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_PAWN_GOLDEN_REVIVE_REQ;
        
        public uint PawnId { get; set; }
        public uint HpMax { get; set; }
        
        public C2SCharacterPawnGoldenReviveReq()
        {
            PawnId = 0;
            HpMax = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterPawnGoldenReviveReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterPawnGoldenReviveReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.HpMax);
            }

            public override C2SCharacterPawnGoldenReviveReq Read(IBuffer buffer)
            {
                C2SCharacterPawnGoldenReviveReq obj = new C2SCharacterPawnGoldenReviveReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.HpMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
