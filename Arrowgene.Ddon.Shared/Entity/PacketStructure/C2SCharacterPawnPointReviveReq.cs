using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterPawnPointReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_PAWN_POINT_REVIVE_REQ;
        
        public uint PawnId { get; set; }
        public uint HpMax { get; set; }
        
        public C2SCharacterPawnPointReviveReq()
        {
            PawnId = 0;
            HpMax = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterPawnPointReviveReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterPawnPointReviveReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.HpMax);
            }

            public override C2SCharacterPawnPointReviveReq Read(IBuffer buffer)
            {
                C2SCharacterPawnPointReviveReq obj = new C2SCharacterPawnPointReviveReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.HpMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
