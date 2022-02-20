using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // Fields in the PS4 binaries:
    //  hpMax (u32)
    public class C2SCharacterCharacterPenaltyReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_PENALTY_REVIVE_REQ;

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public C2SCharacterCharacterPenaltyReviveReq() {
            Unk0 = 0;
            Unk1 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterPenaltyReviveReq>
        {

            public override void Write(IBuffer buffer, C2SCharacterCharacterPenaltyReviveReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override C2SCharacterCharacterPenaltyReviveReq Read(IBuffer buffer)
            {
                C2SCharacterCharacterPenaltyReviveReq obj = new C2SCharacterCharacterPenaltyReviveReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
