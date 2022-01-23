using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // Fields in the PS4 binaries:
    //  hpMax (u32)
    public class C2SCharacterPenaltyReviveReq
    {
        public C2SCharacterPenaltyReviveReq() {
            unk0 = 0;
            unk1 = 0;
        }

        public uint unk0;
        public uint unk1;
    }

    public class C2SCharacterPenaltyReviveReqSerializer : EntitySerializer<C2SCharacterPenaltyReviveReq>
    {
        public override void Write(IBuffer buffer, C2SCharacterPenaltyReviveReq obj)
        {
            WriteUInt32(buffer, obj.unk0);
            WriteUInt32(buffer, obj.unk1);
        }

        public override C2SCharacterPenaltyReviveReq Read(IBuffer buffer)
        {
            C2SCharacterPenaltyReviveReq obj = new C2SCharacterPenaltyReviveReq();
            obj.unk0 = ReadUInt32(buffer);
            obj.unk1 = ReadUInt32(buffer);
            return obj;
        }
    }
}